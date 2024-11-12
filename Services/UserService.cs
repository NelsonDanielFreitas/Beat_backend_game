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
            };

            // Gera o Refresh Token
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = _jwtTokenService.GetRefreshTokenExpiry();

            // Salva o usuário no banco de dados
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (user, refreshToken);
        }
    }
}
