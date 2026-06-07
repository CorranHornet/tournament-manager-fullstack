using ApplicationLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using ApplicationLayer.Services;
using TournamentApi.Services;

namespace ApplicationLayer
{
    public static class DependencyInjection
    {
        //Här registeras alla beroende / Paket /Services från Applikation 
        public static IServiceCollection AddApplication (this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITournamentService, TournamentService>();
            return services;
        }
    }
}
