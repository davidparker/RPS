namespace API
{
    using Interfaces.Games;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Games;

    public static class GamesRegistration
    {
        /// <summary>
        /// Register all the games into a service collection so that we may
        /// loop through this and we are able to extend with multiple games
        /// without much changes.
        /// </summary>
        /// <param name="services">Current services collection, usually from startup</param>
        /// <returns>service collection with games all DI</returns>
        public static IServiceCollection RegisterGames(this IServiceCollection services)
        {
            return services
                .AddTransient<IGame, RockPaperScissorService>();
        }
    }
}
