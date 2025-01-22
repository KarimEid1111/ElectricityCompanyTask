using Bogus;
using ConsoleApp.Domain.Context;
using ConsoleApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Service;

public static class Helpers
{
    public static async Task ResetDatabase(MyDbContext dbContext)
    {
        try
        {
            Console.WriteLine("Resetting database...");

            // Step 1: Disable all foreign key constraints
            Console.WriteLine("Disabling all foreign key constraints...");
            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';");
            Console.WriteLine("Foreign key constraints disabled.");

            // Step 2: Delete data from all tables
            Console.WriteLine("Deleting data from all tables...");
            await dbContext.Database.ExecuteSqlRawAsync("EXEC sp_msforeachtable 'DELETE FROM ?';");
            Console.WriteLine("Data deleted from all tables.");

            // Step 3: Reset identity values for tables with identity columns
            Console.WriteLine("Resetting identity values...");
            var resetIdentitySQL = @"
            DECLARE @table NVARCHAR(MAX);

            -- Use a cursor to iterate through tables with identity columns
            DECLARE identity_cursor CURSOR FOR
            SELECT TABLE_SCHEMA + '.' + TABLE_NAME
            FROM INFORMATION_SCHEMA.TABLES T
            WHERE EXISTS (
                SELECT 1
                FROM sys.columns C
                INNER JOIN sys.objects O ON C.object_id = O.object_id
                WHERE O.object_id = OBJECT_ID(T.TABLE_SCHEMA + '.' + T.TABLE_NAME)
                AND C.is_identity = 1
            );

            OPEN identity_cursor;
            FETCH NEXT FROM identity_cursor INTO @table;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                DECLARE @resetSQL NVARCHAR(MAX);
                SET @resetSQL = 'DBCC CHECKIDENT (''' + @table + ''', RESEED, 0);';
                EXEC sp_executesql @resetSQL;

                FETCH NEXT FROM identity_cursor INTO @table;
            END;

            CLOSE identity_cursor;
            DEALLOCATE identity_cursor;
        ";
            await dbContext.Database.ExecuteSqlRawAsync(resetIdentitySQL);
            Console.WriteLine("Identity values reset.");

            // Step 4: Re-enable all foreign key constraints
            Console.WriteLine("Re-enabling all foreign key constraints...");
            await dbContext.Database.ExecuteSqlRawAsync(
                "EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';");
            Console.WriteLine("Foreign key constraints re-enabled.");

            Console.WriteLine("Database reset completed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while resetting the database: " + ex.Message);
        }
    }
    
    public static async Task SyncNetworkElements(MyDbContext dbContext)
    {
        try
        {
            Console.WriteLine("Executing SP_Sync_Network_Elements...");

            // Call the stored procedure to sync network elements
            await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.SP_Sync_Network_Elements");

            Console.WriteLine("SP_Sync_Network_Elements executed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while executing SP_Sync_Network_Elements: " + ex.Message);
        }
    }

    public static async Task InsertCuttingDown(MyDbContext dbContext)
    {
        try
        {
            Console.WriteLine("Executing SP_Insert_Cutting_Down_Closed_Cases...");

            // Call the stored procedure to insert cutting down tables 
            await dbContext.Database.ExecuteSqlRawAsync("EXEC SP_Insert_Cutting_Down_Closed_Cases");

            Console.WriteLine("SP_Insert_Cutting_Down_Closed_Cases executed successfully.");

            Console.WriteLine("Executing SP_Insert_Cutting_Down...");

            // Call the stored procedure to insert cutting down tables 
            await dbContext.Database.ExecuteSqlRawAsync("EXEC SP_Insert_Cutting_Down");

            Console.WriteLine("SP_Insert_Cutting_Down executed successfully.");

            Console.WriteLine("Executing SP_Insert_Cutting_Down_Ignored...");

            // Call the stored procedure to insert cutting down tables 
            await dbContext.Database.ExecuteSqlRawAsync("EXEC SP_Insert_Cutting_Down_Ignored");

            Console.WriteLine("SP_Insert_Cutting_Down_Ignored executed successfully.");


            await Task.Delay(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while executing SP: " + ex.Message);
        }
    }

    public static async Task SeedData(MyDbContext dbContext)
    {
        var faker = new Faker();

        // Temporary collections to store data before saving
        var governorates = new List<Governorate>();
        var sectors = new List<Sector>();
        var zones = new List<Zone>();
        var cities = new List<City>();
        var stations = new List<Station>();
        var towers = new List<Tower>();
        var cabins = new List<Cabin>(); // Renamed to avoid conflicts
        var cables = new List<Cable>(); // Renamed to avoid conflicts
        var blocks = new List<Block>();
        var buildings = new List<Building>();
        var flats = new List<Flat>();
        var subscriptions = new List<Subscription>();
        var hierarchyPaths = new List<NetworkElementHierarchyPath>();
        var networkElementTypes = new List<NetworkElementType>();


        var users = new List<User>();
        var channels = new List<Channel>();

        // Step 1: Populate `Governorates`, `Sectors`, etc.
        governorates = Enumerable.Range(1, 2).Select(i => new Governorate
        {
            GovernrateName = $"Gov-{faker.Random.Number(1000, 9999)}"
        }).ToList();

        // Generate Sectors with unique names
        sectors = governorates.SelectMany(g => Enumerable.Range(1, 2).Select(i => new Sector
        {
            SectorName = $"Sec-{faker.Random.Number(1000, 9999)}",
            GovernrateKeyNavigation = g
        })).ToList();

        // Generate Zones with unique names
        zones = sectors.SelectMany(s => Enumerable.Range(1, 2).Select(i => new Zone
        {
            ZoneName = $"Zon-{faker.Random.Number(1000, 9999)}",
            SectorKeyNavigation = s
        })).ToList();

        // Generate Cities with unique names
        cities = zones.SelectMany(z => Enumerable.Range(1, 2).Select(i => new City
        {
            CityName = $"Cit-{faker.Random.Number(1000, 9999)}",
            ZoneKeyNavigation = z
        })).ToList();

        // Generate Stations with unique names
        stations = cities.SelectMany(c => Enumerable.Range(1, 2).Select(i => new Station
        {
            StationName = $"Sta-{faker.Random.Number(1000, 9999)}",
            CityKeyNavigation = c
        })).ToList();

        // Generate Towers with unique names
        towers = stations.SelectMany(st => Enumerable.Range(1, 2).Select(i => new Tower
        {
            TowerName = $"Tow-{faker.Random.Number(1000, 9999)}",
            StationKeyNavigation = st
        })).ToList();

        // Generate Cabins with unique names
        cabins = towers.SelectMany(t => Enumerable.Range(1, 2).Select(i => new Cabin
        {
            CabinName = $"Cab-{faker.Random.Number(1000, 9999)}",
            TowerKeyNavigation = t
        })).ToList();

        // Generate Cables with unique names
        cables = cabins.SelectMany(c => Enumerable.Range(1, 2).Select(i => new Cable
        {
            CableName = $"Cabl-{faker.Random.Number(1000, 9999)}",
            CabinKeyNavigation = c
        })).ToList();

        // Generate Blocks with unique names
        blocks = cables.SelectMany(cb => Enumerable.Range(1, 2).Select(i => new Block
        {
            BlockName = $"Blk-{faker.Random.Number(1000, 9999)}",
            CableKeyNavigation = cb
        })).ToList();

        // Generate Buildings with unique names
        buildings = blocks.SelectMany(b => Enumerable.Range(1, 2).Select(i => new Building
        {
            BuildingName = $"Bld-{faker.Random.Number(1000, 9999)}",
            BlockKeyNavigation = b
        })).ToList();

        // Generate Flats with unique names
        flats = buildings.SelectMany(b => Enumerable.Range(1, 2).Select(i => new Flat
        {
            FlatName = $"Flt-{faker.Random.Number(1000, 9999)}",
            BuildingKeyNavigation = b
        })).ToList();

        // Generate Subscriptions with unique meter and pallet keys
        subscriptions = flats.SelectMany(f => Enumerable.Range(1, 2).Select(i => new Subscription
        {
            FlatKeyNavigation = faker.Random.Bool() ? f : null, // Randomly assign FlatKey or leave it null
            BuildingKeyNavigation = f.BuildingKeyNavigation,
            MeterKey = faker.Random.Int(1000, 9999),
            PaletKey = faker.Random.Int(1000, 9999)
        })).ToList();


        // Step 2: Populate `Network_Element_Hierarchy_Path`
        if (!dbContext.NetworkElementHierarchyPaths.Any())
        {
            hierarchyPaths = new[]
            {
                new NetworkElementHierarchyPath
                {
                    Abbreviation = "Governorate -> Individual Subscription",
                    NetwrokElementHierarchyPathName =
                        "Governrate, Sector, Zone, City, Station, Tower, Cabin, Cable, Buidling, Flat, Individual Subscription"
                },
                new NetworkElementHierarchyPath
                {
                    Abbreviation = "Governorate -> Corporate Subscription",
                    NetwrokElementHierarchyPathName =
                        "Governrate, Sector, Zone, City, Station, Tower, Cabin, Cable, Buidling, Corporate Subscription"
                }
            }.ToList();
            dbContext.NetworkElementHierarchyPaths.AddRange(hierarchyPaths);
            await dbContext.SaveChangesAsync();
        }

        // Step 3: Populate `Network_Element_Type`
        if (!dbContext.NetworkElementTypes.Any())
        {
            // First create the root element (Governrate)
            var governorateType = new NetworkElementType
            {
                NetworkElementTypeName = "Governrate",
                ParentNetworkElementTypeKey = null, // Root element, no parent
                NetworkElementHierarchyPathKey = 1
            };

            // Add the root element to the list
            dbContext.NetworkElementTypes.Add(governorateType);
            await dbContext.SaveChangesAsync();

            // After saving, we get the generated NetworkElementTypeKey for the root element
            var governorateTypeId = governorateType.NetworkElementTypeKey;

            // Create the Sector type, which has the root element (Governrate) as parent
            var sectorType = new NetworkElementType
            {
                NetworkElementTypeName = "Sector",
                ParentNetworkElementTypeKey = governorateTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(sectorType);
            await dbContext.SaveChangesAsync();

            var sectorTypeId = sectorType.NetworkElementTypeKey;

            // Now create the children under Sector (Zone, City, etc.)
            var zoneType = new NetworkElementType
            {
                NetworkElementTypeName = "Zone",
                ParentNetworkElementTypeKey = sectorTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(zoneType);
            await dbContext.SaveChangesAsync();

            var zoneTypeId = zoneType.NetworkElementTypeKey;

            var cityType = new NetworkElementType
            {
                NetworkElementTypeName = "City",
                ParentNetworkElementTypeKey = zoneTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(cityType);
            await dbContext.SaveChangesAsync();

            var cityTypeId = cityType.NetworkElementTypeKey;

            var stationType = new NetworkElementType
            {
                NetworkElementTypeName = "Station",
                ParentNetworkElementTypeKey = cityTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(stationType);
            await dbContext.SaveChangesAsync();

            var stationTypeId = stationType.NetworkElementTypeKey;

            var towerType = new NetworkElementType
            {
                NetworkElementTypeName = "Tower",
                ParentNetworkElementTypeKey = stationTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(towerType);
            await dbContext.SaveChangesAsync();

            var towerTypeId = towerType.NetworkElementTypeKey;

            var cabinType = new NetworkElementType
            {
                NetworkElementTypeName = "Cabin",
                ParentNetworkElementTypeKey = towerTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(cabinType);
            await dbContext.SaveChangesAsync();

            var cabinTypeId = cabinType.NetworkElementTypeKey;

            var cableType = new NetworkElementType
            {
                NetworkElementTypeName = "Cable",
                ParentNetworkElementTypeKey = cabinTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(cableType);
            await dbContext.SaveChangesAsync();

            var cableTypeId = cableType.NetworkElementTypeKey;

            var blockType = new NetworkElementType
            {
                NetworkElementTypeName = "Block",
                ParentNetworkElementTypeKey = cableTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(blockType);
            await dbContext.SaveChangesAsync();

            var blockTypeId = blockType.NetworkElementTypeKey;

            var buildingType = new NetworkElementType
            {
                NetworkElementTypeName = "Building",
                ParentNetworkElementTypeKey = blockTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(buildingType);
            await dbContext.SaveChangesAsync();

            var buildingTypeId = buildingType.NetworkElementTypeKey;

            var flatType = new NetworkElementType
            {
                NetworkElementTypeName = "Flat",
                ParentNetworkElementTypeKey = buildingTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(flatType);
            await dbContext.SaveChangesAsync();

            var flatTypeId = flatType.NetworkElementTypeKey;

            var individualSubscriptionType = new NetworkElementType
            {
                NetworkElementTypeName = "Individual Subscription",
                ParentNetworkElementTypeKey = flatTypeId,
                NetworkElementHierarchyPathKey = 1
            };

            dbContext.NetworkElementTypes.Add(individualSubscriptionType);
            await dbContext.SaveChangesAsync();


            var corporateSubscriptionType = new NetworkElementType
            {
                NetworkElementTypeName = "Corporate Subscription",
                ParentNetworkElementTypeKey = buildingTypeId,
                NetworkElementHierarchyPathKey = 2
            };

            dbContext.NetworkElementTypes.Add(corporateSubscriptionType);
            await dbContext.SaveChangesAsync();
        }


        // Step 6: Populate Users
        if (!dbContext.Users.Any())
        {
            users = new[]
            {
                new User { Name = "admin", Password = "admin" },
                new User { Name = "test", Password = "test" },
                new User { Name = "SourceA", Password = "Source_A" },
                new User { Name = "SourceB", Password = "Source_B" }
            }.ToList();
        }


        if (!dbContext.ProblemTypes.Any())
        {
            var problemTypes = new List<FTAProblemType>
            {
                new FTAProblemType { ProblemTypeName = "حريق" },
                new FTAProblemType { ProblemTypeName = "ضغط عالى" },
                new FTAProblemType { ProblemTypeName = "استهلاك عالى" },
                new FTAProblemType { ProblemTypeName = "مديونيه" },
                new FTAProblemType { ProblemTypeName = "تلف عداد" },
                new FTAProblemType { ProblemTypeName = "سرقة تيار" },
                new FTAProblemType { ProblemTypeName = "امطار" },
                new FTAProblemType { ProblemTypeName = "كسر ماسورة مياه" },
                new FTAProblemType { ProblemTypeName = "كسر ماسورة غاز" },
                new FTAProblemType { ProblemTypeName = "تحديث واحلال" },
                new FTAProblemType { ProblemTypeName = "صيانه" },
                new FTAProblemType { ProblemTypeName = "كابل مقطوع" },
                new FTAProblemType { ProblemTypeName = "توصيل كابل" }
            };
            await dbContext.ProblemTypes.AddRangeAsync(problemTypes);
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.ProblemTypes1.Any())
        {
            var problemTypes1 = new List<STAProblemType>
            {
                new() { ProblemTypeName = "حريق" },
                new() { ProblemTypeName = "ضغط عالى" },
                new() { ProblemTypeName = "استهلاك عالى" },
                new() { ProblemTypeName = "مديونيه" },
                new() { ProblemTypeName = "تلف عداد" },
                new() { ProblemTypeName = "سرقة تيار" },
                new() { ProblemTypeName = "امطار" },
                new() { ProblemTypeName = "كسر ماسورة مياه" },
                new() { ProblemTypeName = "كسر ماسورة غاز" },
                new() { ProblemTypeName = "تحديث واحلال" },
                new() { ProblemTypeName = "صيانه" },
                new() { ProblemTypeName = "كابل مقطوع" },
                new() { ProblemTypeName = "توصيل كابل" }
            };
            await dbContext.ProblemTypes1.AddRangeAsync(problemTypes1);
            await dbContext.SaveChangesAsync();
        }

        // Step 7: Populate Channels
        if (!dbContext.Channels.Any())
        {
            channels = new[]
            {
                new Channel { ChannelName = "Source A" },
                new Channel { ChannelName = "Source B" }
            }.ToList();
        }


        // Save all data at the end
        dbContext.Governorates.AddRange(governorates);
        dbContext.Sectors.AddRange(sectors);
        dbContext.Zones.AddRange(zones);
        dbContext.Cities.AddRange(cities);
        dbContext.Stations.AddRange(stations);
        dbContext.Towers.AddRange(towers);
        dbContext.Cabins.AddRange(cabins);
        dbContext.Cables.AddRange(cables);
        dbContext.Blocks.AddRange(blocks);
        dbContext.Buildings.AddRange(buildings);
        dbContext.Flats.AddRange(flats);
        dbContext.Subscriptions.AddRange(subscriptions);

        dbContext.NetworkElementTypes.AddRange(networkElementTypes);


        dbContext.Users.AddRange(users);
        dbContext.Channels.AddRange(channels);


        await dbContext.SaveChangesAsync();
    }
}