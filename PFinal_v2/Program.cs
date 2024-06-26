﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PFinal_v2.Data;
using PFinal_v2.Models;
using System.Security.Claims;

namespace PFinal_v2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<PFinal_v2Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PFinal_v2Context") ?? throw new InvalidOperationException("Connection string 'PFinal_v2Context' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Código acrescentado
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Conta/Login";
                    options.AccessDeniedPath = "/Conta/AccessDenied";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("ColaboradorPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Colaborador"));

            });

            // Injeção de dependência para o serviço LoginService
            builder.Services.AddScoped<LoginService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                WbsData.Initialize(services);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedData.Initialize(services);
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                UsuarioData.Initialize(services);
            }



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Middleware para tratar status codes (404, 500....) acrescentado 05/06/2024 para rodar nova page de erro
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Conta}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
