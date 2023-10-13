using Logic.ILogic;
using Logic.Logic;

namespace API.MyNotes.Dependencies
{
    public static class LogicDependencyCollection
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUserSecurityLogic, UserSecurityLogic>();
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<INoteLogic, NoteLogic>();
            services.AddScoped<ITagLogic, TagLogic>();

            return services;
        }
    }
}