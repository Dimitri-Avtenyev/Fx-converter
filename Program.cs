using Fx_converter;

// get cd and load env vars
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
Env.Load(dotenv);

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webbuilder =>
    {
        webbuilder.UseStartup<Startup>();
    })
    .Build()
    .RunAsync();
