
using Fx_converter.Entities;
using Fx_converter.Services.DataCollector;
using Microsoft.EntityFrameworkCore;

namespace Fx_converter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            //services.AddScoped<IFileHandler, FileHandler>;
            //services.Addscoped<iFxData, FxData>;
            services.AddSwaggerGen();
           
            var USER = Environment.GetEnvironmentVariable("USER");
            var PSW = Environment.GetEnvironmentVariable("PSW");
            var connectionString = $"Server=tcp:fxconverterdb-server.database.windows.net,1433;Initial Catalog=FxConverterDB;Persist Security Info=False;User ID={USER};Password={PSW};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<FxDbContext>(options => {
                options.UseSqlServer(connectionString);
            });
       
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {

                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("./swagger/v1/swagger.json", "FX converter API v1");
                    c.RoutePrefix = String.Empty;
                });
            } else {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = async context => await context.Response.WriteAsJsonAsync(new { error = "Something went wrong." })
                });
                app.UseRouting();
                app.UseEndpoints(endpoints => {
                    endpoints.MapControllers();
                });
            }
        }


    }
}
