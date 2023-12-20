using Fx_converter;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webbuilder =>
    {
        webbuilder.UseStartup<Startup>();
    })
    .Build()
    .RunAsync();
