using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using API.MyNotes.Dependencies;
using API.Middlewares;
using Newtonsoft.Json;

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
//ver esto luego. y ver si dejo el paquete o no.
//builder.Services.AddControllers().AddNewtonsoftJson(o =>
//{
//    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
//});

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
builder.Services.AddLogicServices();
builder.Services.AddAPIServices();

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

//app.Use(async (context, next) => {
//    var serviceScope = app.Services.CreateScope();
//    var userSecurityService = serviceScope.ServiceProvider.GetRequiredService<IUserSecurityService>();
//    var requestAuthorizationMiddleware = new RequestAuthorizationMiddleware(userSecurityService);
//    await requestAuthorizationMiddleware.ValidateRequestAutorizathion(context);
//    await next();
//});

//app.UseHttpsRedirection();

app.UseCors(AllowWebUIOrigin);

app.MapControllers();

app.UseRequestAuthorizationMiddleware();

app.Run();