using Microsoft.Azure.Cosmos;



// API "settings":----------------------------------------------------------------------------------->
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
     {
         options.AddPolicy("AllowAll",
             policy =>
             {
                 policy
                 .WithOrigins("http://localhost:3000") 
                 .AllowAnyMethod()
                 .AllowAnyHeader();
             });
     });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// end -- API "settings"------------------------------------------------------------------------------|

// Databasetilkobling: ---------------------------------------------------------------------------------->

//Dette må selvsagt fjernes før vi "leverer"
string _endpoint ="https://ciem.documents.azure.com:443/";
string _key ="B5SkUYL5BLm9RnT5kPgU12itHZFjxZ3Q1VtvqbPn3LPOIlWy5r6fiuXdDQCvlSFPlo8ME7OtdE99ACDbrF8wMg==";

CosmosClient client = new(_endpoint, _key); //initialiserer Cosmos Client

Database database = client.GetDatabase("ciem"); //kobler til riktig database

// end -- Databasetilkobling ---------------------------------------------------------------------------|

app.MapControllers();

app.Run();

