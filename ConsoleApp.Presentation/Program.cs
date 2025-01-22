using ConsoleApp.Domain.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ConsoleApp.Service;

class Program
{
    public async static Task Main(string[] args)
    {
        // Set up the DI container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // Build the service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        await Helpers.ResetDatabase(dbContext);

        // Step 1: Generate Data
        await Helpers.SeedData(dbContext);

        // Step 2: Execute Stored Procedure to Sync Network Elements
        await Helpers.SyncNetworkElements(dbContext);

        await Run(serviceProvider);
        await Helpers.InsertCuttingDown(dbContext);
        while (true)
        {
            Console.WriteLine("Service is running...");
            Thread.Sleep(1000000000);
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddDbContext<MyDbContext>(x =>
            x.UseSqlServer(
                "Server=.;Database=Electricity Company Task;Integrated Security=True;encrypt=false;"));
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
                    Console.WriteLine(
                        $"API response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
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
                    Console.WriteLine(
                        $"API response: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred during API call to generate cable cuttings: " + e.Message);
            }
        }
    }
}