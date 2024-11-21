﻿// <auto-generated />
using System;
using Beat_backend_game.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Beat_backend_game.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241120115613_ModelBuilder8")]
    partial class ModelBuilder8
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Beat_backend_game.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Beat_backend_game.Models.EscolhaMultipla", b =>
                {
                    b.Property<int>("IdTipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTipo"));

                    b.Property<bool>("Correta")
                        .HasColumnType("boolean");

                    b.Property<int>("IdPergunta")
                        .HasColumnType("integer");

                    b.Property<string>("TextoOpcao")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdTipo");

                    b.HasIndex("IdPergunta");

                    b.ToTable("EscolhaMultiplas");
                });

            modelBuilder.Entity("Beat_backend_game.Models.OrdemPalavras", b =>
                {
                    b.Property<int>("IdTipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTipo"));

                    b.Property<int>("IdPergunta")
                        .HasColumnType("integer");

                    b.Property<string>("Palavra")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Posicao")
                        .HasColumnType("integer");

                    b.HasKey("IdTipo");

                    b.HasIndex("IdPergunta");

                    b.ToTable("OrdemPalavras");
                });

            modelBuilder.Entity("Beat_backend_game.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("NivelDificuldade")
                        .HasColumnType("integer");

                    b.Property<string>("TempoLimite")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TextoPergunta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TipoPergunta")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Beat_backend_game.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RefreshTokenExpiry")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Beat_backend_game.Models.VerdadeiroFalso", b =>
                {
                    b.Property<int>("IdTipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTipo"));

                    b.Property<bool>("Correta")
                        .HasColumnType("boolean");

                    b.Property<int>("IdPergunta")
                        .HasColumnType("integer");

                    b.HasKey("IdTipo");

                    b.HasIndex("IdPergunta");

                    b.ToTable("VerdadeiroFalsos");
                });

            modelBuilder.Entity("Beat_backend_game.Models.EscolhaMultipla", b =>
                {
                    b.HasOne("Beat_backend_game.Models.Question", "Pergunta")
                        .WithMany("EscolhaMultiplas")
                        .HasForeignKey("IdPergunta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pergunta");
                });

            modelBuilder.Entity("Beat_backend_game.Models.OrdemPalavras", b =>
                {
                    b.HasOne("Beat_backend_game.Models.Question", "Pergunta")
                        .WithMany("OrdemPalavras")
                        .HasForeignKey("IdPergunta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pergunta");
                });

            modelBuilder.Entity("Beat_backend_game.Models.VerdadeiroFalso", b =>
                {
                    b.HasOne("Beat_backend_game.Models.Question", "Pergunta")
                        .WithMany("VerdadeiroFalsos")
                        .HasForeignKey("IdPergunta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pergunta");
                });

            modelBuilder.Entity("Beat_backend_game.Models.Question", b =>
                {
                    b.Navigation("EscolhaMultiplas");

                    b.Navigation("OrdemPalavras");

                    b.Navigation("VerdadeiroFalsos");
                });
#pragma warning restore 612, 618
        }
    }
}
