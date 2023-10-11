using API.IServices;
using Data;
using Microsoft.EntityFrameworkCore;
using API.Middlewares;
using Microsoft.OpenApi.Models;
using Logic.ILogic;
using Logic.Logic;
using API.MyNotes.Services;
using API.MyNotes.IServices;

var AllowWebUIOrigin = "_allowWebUIOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    var webUIHostURL = builder.Configuration.GetValue<string>("WebUIHost");
    options.AddPolicy(AllowWebUIOrigin,
        policy =>
        {
            policy.WithOrigins(webUIHostURL)
            .AllowAnyMethod().AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter authorization",
        Name = "EndpointAuthorize",
        Type = SecuritySchemeType.Http,
        BearerFormat = "Bearer",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ServiceContext>(
        options => options.UseNpgsql("name=ConnectionStrings:ServiceContext"));

builder.Services.AddScoped<IUserSecurityLogic, UserSecurityLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<INoteLogic, NoteLogic>();
builder.Services.AddScoped<ITagLogic, TagLogic>();

builder.Services.AddScoped<IUserSecurityService, UserSecurityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ITagService, TagService>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT");
    app.Urls.Add($"http://*:{port}");
}

app.Use(async (context, next) => {
    var serviceScope = app.Services.CreateScope();
    var userSecurityService = serviceScope.ServiceProvider.GetRequiredService<IUserSecurityService>();
    var requestAuthorizationMiddleware = new RequestAuthorizationMiddleware(userSecurityService);
    await requestAuthorizationMiddleware.ValidateRequestAutorizathion(context);
    await next();
});

//app.UseHttpsRedirection();

app.UseCors(AllowWebUIOrigin);

app.MapControllers();

app.Run();