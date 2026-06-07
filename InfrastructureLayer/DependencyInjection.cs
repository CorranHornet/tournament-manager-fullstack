
using ApplicationLayer.Interfaces;
using InfrastructureLayer.Database;
using InfrastructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TournamentApi.Infrastructure.Repositories;


namespace InfrastructureLayer
{
    public static class DependencyInjection
    {
        //Här registeras alla beroende / Paket /Services från Applikation 
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Registrera alla Services, beroenden, paket osv...
            //DB och Repositories
            services.AddDbContext<AppDbContext>(options =>
            {
            options.UseSqlServer(configuration.GetConnectionString("NemoCleanArchitectureDbString"));
            });
                

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            return services;
        }
    }
}
