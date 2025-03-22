using Projects;

var builder = DistributedApplication.CreateBuilder(args);
//Add backend services
var postgres = builder
        .AddPostgres("postgres")
        .WithPgAdmin()
       // .WithDataVolume()
        .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

var cache = builder
    .AddRedis("cache")
    .WithRedisInsight()
  //  .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var rabbitmq = builder
    .AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
   // .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var keycloak = builder
    .AddKeycloak("keycloak", 8080)
   // .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

if (builder.ExecutionContext.IsRunMode)
{
    // Data volumes don't work on ACA for linux (esp Postres) so only add when running
    postgres.WithDataVolume(); //Can use Azure Cloud Postgres
    rabbitmq.WithDataVolume(); //can use Azure Service Bus for prod
    keycloak.WithDataVolume(); //can use Azure Identity Services for prod
}

var ollama = builder
    .AddOllama("ollama", 11434)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithOpenWebUI();

var llama = ollama.AddModel("llama3.2");

//Add Projects
var catalog = builder.AddProject<Catalog>("catalog")
     .WithReference(catalogDb)
     .WithReference(rabbitmq)
     .WithReference(llama)
     .WaitFor(catalogDb)
     .WaitFor(rabbitmq)
     .WaitFor(llama);

var basket = builder.AddProject<Basket>("basket")
    .WithReference(cache)
    .WithReference(catalog)
    .WithReference(rabbitmq)
    .WithReference(keycloak)
    .WaitFor(cache)
    .WaitFor(rabbitmq)
    .WaitFor(keycloak);



var webapp = builder
    .AddProject<Projects.WebApp>("webapp")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(catalog)
    .WithReference(basket)
    .WaitFor(catalog)
    .WaitFor(basket);



builder.Build().Run();
