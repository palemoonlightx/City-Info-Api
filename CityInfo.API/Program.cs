using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CityInfo.API {
    public class Program {
        public static void Main(string[] args) {

            // Using 3rd party logger (Serilog) for more features and to log to textfile
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day) // A log file will be created everyday
                .CreateLogger();


            // CreateBuilder automatically has Logging 
            var builder = WebApplication.CreateBuilder(args);


            // Clears all configured providers in appsettings json file
            //builder.Logging.ClearProviders();

            // Manually adding console logger
            //builder.Logging.AddConsole();


            // Enabling Serilog
            builder.Host.UseSerilog();




            // Add services to the container.
            builder.Services.AddControllers(options => {
                options.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();


            // Injecting Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Injecting FileExtensionContentTypeProvider
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();


            // Registering a custom service made (LocalMailService)
            // Lifetimes-------------------------------------------------------------------------------
            // Transient - each time they are requested (works best for lightweight stateless services)
            // Scoped - created once per request
            // Singleton - created the first time they are requested 

            // Compiler directive (just to test the mail service implementation)
#if DEBUG
            builder.Services.AddTransient<IMailService, LocalMailService>();
#else
            builder.Services.AddTransient<IMailService, CloudMailService>();
#endif


            // Registering data store class
            builder.Services.AddSingleton<DataStore>();

            // Registering dbcontext
            builder.Services.AddDbContext<Context>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            // Registering repository (best lifetime is a scope lifetime)
            builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();


            // Registering auto mapper 
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            var app = builder.Build();

            // Adding swagger middleware
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.Run();
        }
    }
}