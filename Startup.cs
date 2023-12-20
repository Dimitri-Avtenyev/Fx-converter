
namespace Fx_converter
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();
            //services.AddScoped<IFileHandlerService, FileHandlerService>;
            services.AddSwaggerGen();
            var connectionString = "";
        }

        [Obsolete]
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
