using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
     .AddSingleton(sp =>
     {
         return Configure(sp, builder.Configuration);
     })
     .AddCors(options =>
     {
         options.AddDefaultPolicy(
                           builder =>
                           {
                               builder
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowAnyOrigin();
                           });
     })
    .AddStackExchangeRedisCache(redisCacheConfig =>
    {
        var redisConfig = builder.Configuration.GetSection(nameof(RedisConfiguration))
        .Get<MongoDbConfiguration>();
        redisCacheConfig.ConfigurationOptions =
            ConfigurationOptions.Parse(redisConfig.ConnectionString);
    })
    .AddHttpContextAccessor()
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbProjections()
    .AddMongoDbPagingProviders();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "pid";
    options.IdleTimeout = TimeSpan.FromMinutes(60 * 24 * 14);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app
   .UseCors()
   .UseRouting()
   .UseSession()
   .UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();

IMongoDatabase Configure(IServiceProvider sp, ConfigurationManager configurationManager)
{
    var mongodbConfig = configurationManager.GetSection(nameof(MongoDbConfiguration))
        .Get<MongoDbConfiguration>();
    var mongoConnectionUrl = new MongoUrl(mongodbConfig.ConnectionString);
    var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
    mongoClientSettings.ClusterConfigurator = cb =>
    {
        // This will print the executed command to the console
        cb.Subscribe<CommandStartedEvent>(e =>
        {
            Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
        });
    };
    var client = new MongoClient(mongoClientSettings);
    var database = client.GetDatabase(mongodbConfig.Database);
    return database;
}