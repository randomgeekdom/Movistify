using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movistify.Configuration;
using Movistify.Extensions;
using Movistify.MappingProfiles;
using Movistify.Models;
using System;

namespace Movistify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Add API Key implementation for swagger
            builder.Services.AddApiKeyForSwagger();

            // Adds API Key authentication
            builder.Services.AddAuthentication(Constants.ApiKey)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Constants.ApiKey, null);

            builder.Services.AddDbContextFactory<MovistifyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            },
    ServiceLifetime.Scoped);

            builder.Services.AddRepositories();

            builder.Services.AddAutoMapper(x => {
                x.AddProfile<MovieProfile>();
                x.AddProfile<ActorProfile>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}