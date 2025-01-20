using ConsoleApp.Domain.Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

class Program
{
    public async static  Task Main(string[] args)
    {
        // Set up the DI container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);


        // Build the service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        dbContext.Database.ExecuteSqlRaw("SP_Insert_Cutting_Down_Closed_Cases");

        // Run the application
        await Run(serviceProvider);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register HttpClient
        services.AddHttpClient();
        services.AddDbContext<MyDbContext>(x =>
            x.UseSqlServer(
                "Server=DESKTOP-FC654RV\\KARIM;Database=Electricity Company Task;Integrated Security=True;encrypt=false;"));

    }

    public static async Task Run(IServiceProvider serviceProvider)
    {
        // Base API URL
        var baseUrl = "http://localhost:5094/api/CuttingDown";

        using (var scope1 = serviceProvider.CreateScope())
        {
            try
            {
                Console.WriteLine("Calling API to generate cabin cuttings...");
                var httpClient = scope1.ServiceProvider.GetRequiredService<HttpClient>();
                var response = await httpClient.GetAsync($"{baseUrl}/generate-cabin-cuttings");
            
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cabin cuttings generated successfully.");
                }
                else
                {
                    Console.WriteLine($"API response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred during API call to generate cabin cuttings: " + e.Message);
            }
        }

        using (var scope2 = serviceProvider.CreateScope())
        {
            try
            {
                Console.WriteLine("Calling API to generate cable cuttings...");
                var httpClient = scope2.ServiceProvider.GetRequiredService<HttpClient>();
                var response = await httpClient.GetAsync($"{baseUrl}/generate-cable-cuttings");
            
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cable cuttings generated successfully.");
                }
                else
                {
                    Console.WriteLine($"API response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred during API call to generate cable cuttings: " + e.Message);
            }
        }
        
        // // Run stored procedures
        // await ExecuteStoredProcedureAsync(connectionString, "SP_Insert_Cutting_Down");
        // await ExecuteStoredProcedureAsync(connectionString, "SP_Insert_Cutting_Down_Closed_Cases");


        // Keep the console application running
        while (true)
        {
            Console.WriteLine("Service is running...");
            Thread.Sleep(100000);
        }
    }

    // private static async Task ExecuteStoredProcedureAsync(string connectionString, string storedProcedureName)
    // {
    //     try
    //     {
    //         Console.WriteLine($"Executing stored procedure: {storedProcedureName}");
    //
    //         using (var connection = new SqlConnection(connectionString))
    //         {
    //             await connection.OpenAsync();
    //
    //             using (var command = new SqlCommand(storedProcedureName, connection))
    //             {
    //                 command.CommandType = System.Data.CommandType.StoredProcedure;
    //
    //                 // Execute the stored procedure
    //                 await command.ExecuteNonQueryAsync();
    //             }
    //         }
    //
    //         Console.WriteLine($"Stored procedure {storedProcedureName} executed successfully.");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error executing stored procedure {storedProcedureName}: {ex.Message}");
    //     }
    // }
}