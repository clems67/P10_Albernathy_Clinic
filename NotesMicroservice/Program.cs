
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using NotesMicroservice.Models;

namespace NotesMicroservice
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB");

            //IMongoClient mongoClient = null;
            //IMongoDatabase database = null;
            //const int maxRetryCount = 5;
            //int currentRetry = 0;
            //IAsyncCursor<string> collections = null;

            //while (collections == null && currentRetry < maxRetryCount)
            //{
            //    try
            //    {
            //        mongoClient = new MongoClient(mongoConnectionString);
            //        // Attempt to connect to the database
            //        database = mongoClient.GetDatabase("Notes");
            //        collections = database.ListCollectionNames(); // A quick call to verify the connection
            //        Console.WriteLine("Connected to MongoDB.");
            //    }
            //    catch (Exception ex)
            //    {
            //        currentRetry++;
            //        Console.WriteLine($"MongoDB not ready, retrying ({currentRetry}/{maxRetryCount})...");
            //    }
            //}

            //if (mongoClient == null)
            //{
            //    throw new Exception("Failed to connect to MongoDB after retries.");
            //}

            var mongoClient = new MongoClient(mongoConnectionString);
            //var notesCollection = database.GetCollection<NoteDBModel>("notes");
            //Console.WriteLine(database.ListCollectionNames().ToString());
            builder.Services.AddSingleton<IMongoClient>(mongoClient);

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"));
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
