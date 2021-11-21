using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
     .AddSingleton(sp =>
     {
         return ConfigureMongoDb(sp, builder.Configuration);
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
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbProjections()
    .AddMongoDbPagingProviders();

var app = builder.Build();
app.UseCors();

app
    .UseRouting()
    .UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});



app.Run();

IMongoDatabase ConfigureMongoDb(IServiceProvider sp, ConfigurationManager configurationManager)
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