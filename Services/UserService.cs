using Beat_backend_game.Data;
using Beat_backend_game.Models;
using Microsoft.EntityFrameworkCore;

namespace Beat_backend_game.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public UserService(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<(User user, string refreshToken)> RegisterUserAsync(string username, string email, string password)
        {
            // Verifica se o nome de usuário ou email já existe
            if (await _context.Users.AnyAsync(u => u.Username == username || u.Email == email))
            {
                return (null, null); // Você pode retornar uma mensagem personalizada aqui
            }

            // Cria o hash da senha
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Cria um novo usuário
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                IsAdmin = true,
            };

            // Gera o Refresh Token
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiry();
            var userSend = new User
            {
                Username = username,
                Email = email,
                IsAdmin = true,
            };
            // Salva o usuário no banco de dados
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (userSend, refreshToken);
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            // Atualiza o refresh token se ele estiver expirado
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                user.RefreshToken = _jwtTokenService.GenerateRefreshToken();
                user.RefreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiry();
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
