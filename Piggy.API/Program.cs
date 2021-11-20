using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
     .AddSingleton(sp =>
     {
         return ConfigureMongoDB(sp);
     })
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

app.UseHttpsRedirection();

app
    .UseRouting()
    .UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.Run();

IMongoDatabase ConfigureMongoDB(IServiceProvider sp)
{
    const string connectionString = "mongodb://localhost:27017";
    var mongoConnectionUrl = new MongoUrl(connectionString);
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
    var database = client.GetDatabase("PiggyDB");
    return database;
}