using API.IServices;
using API.MyNotes.IServices;
using API.MyNotes.Services;

namespace API.MyNotes.Dependencies
{
    public static class ServiceDependencyCollection
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services)
        {
            services.AddScoped<IUserSecurityService, UserSecurityService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }
    }
}