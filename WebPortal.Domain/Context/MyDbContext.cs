using Microsoft.EntityFrameworkCore;
using WebPortalDomain.Entities;

namespace WebPortalDomain.Context;

public partial class MyDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Cabin> Cabins { get; set; }

    public virtual DbSet<Cable> Cables { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CuttingDownA> CuttingDownAs { get; set; }

    public virtual DbSet<CuttingDownB> CuttingDownBs { get; set; }

    public virtual DbSet<CuttingDownDetail> CuttingDownDetails { get; set; }

    public virtual DbSet<CuttingDownHeader> CuttingDownHeaders { get; set; }

    public virtual DbSet<CuttingDownIgnored> CuttingDownIgnoreds { get; set; }

    public virtual DbSet<Flat> Flats { get; set; }

    public virtual DbSet<Governorate> Governorates { get; set; }

    public virtual DbSet<NetworkElement> NetworkElements { get; set; }

    public virtual DbSet<NetworkElementHierarchyPath> NetworkElementHierarchyPaths { get; set; }

    public virtual DbSet<NetworkElementType> NetworkElementTypes { get; set; }

    public virtual DbSet<FTAProblemType> FTAProblemTypes { get; set; }

    public virtual DbSet<STAProblemType> STAProblemTypes { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tower> Towers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(
            "Server=.;Database=Electricity Company Task;Integrated Security=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.BlockKey).HasName("PK__Block__DF1963F11B56E877");

            entity.ToTable("Block", "STA");

            entity.HasIndex(e => e.CableKey, "idx_Block_CableKey");

            entity.Property(e => e.BlockKey).HasColumnName("Block_Key");
            entity.Property(e => e.BlockName)
                .HasMaxLength(100)
                .HasColumnName("Block_Name");
            entity.Property(e => e.CableKey).HasColumnName("Cable_Key");

            entity.HasOne(d => d.CableKeyNavigation).WithMany(p => p.Blocks)
                .HasForeignKey(d => d.CableKey)
                .HasConstraintName("FK_Block_Cable");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingKey).HasName("PK__Building__03305901CB205A00");

            entity.ToTable("Building", "STA");

            entity.HasIndex(e => e.BlockKey, "idx_Building_BlockKey");

            entity.Property(e => e.BuildingKey).HasColumnName("Building_Key");
            entity.Property(e => e.BlockKey).HasColumnName("Block_Key");
            entity.Property(e => e.BuildingName)
                .HasMaxLength(100)
                .HasColumnName("Building_Name");

            entity.HasOne(d => d.BlockKeyNavigation).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.BlockKey)
                .HasConstraintName("FK_Building_Block");
        });

        modelBuilder.Entity<Cabin>(entity =>
        {
            entity.HasKey(e => e.CabinKey).HasName("PK__Cabin__341FCAA062FC1C85");

            entity.ToTable("Cabin", "STA");

            entity.HasIndex(e => e.TowerKey, "idx_Cabin_TowerKey");

            entity.Property(e => e.CabinKey).HasColumnName("Cabin_Key");
            entity.Property(e => e.CabinName)
                .HasMaxLength(100)
                .HasColumnName("Cabin_Name");
            entity.Property(e => e.TowerKey).HasColumnName("Tower_Key");

            entity.HasOne(d => d.TowerKeyNavigation).WithMany(p => p.Cabins)
                .HasForeignKey(d => d.TowerKey)
                .HasConstraintName("FK_Cabin_Tower");
        });

        modelBuilder.Entity<Cable>(entity =>
        {
            entity.HasKey(e => e.CableKey).HasName("PK__Cable__5420959C23C4B9B7");

            entity.ToTable("Cable", "STA");

            entity.HasIndex(e => e.CabinKey, "idx_Cable_CabinKey");

            entity.Property(e => e.CableKey).HasColumnName("Cable_Key");
            entity.Property(e => e.CabinKey).HasColumnName("Cabin_Key");
            entity.Property(e => e.CableName)
                .HasMaxLength(100)
                .HasColumnName("Cable_Name");

            entity.HasOne(d => d.CabinKeyNavigation).WithMany(p => p.Cables)
                .HasForeignKey(d => d.CabinKey)
                .HasConstraintName("FK_Cable_Cabin");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.ChannelKey).HasName("PK__Channel__011B50F442581D1D");

            entity.ToTable("Channel", "FTA");

            entity.Property(e => e.ChannelKey).HasColumnName("Channel_Key");
            entity.Property(e => e.ChannelName)
                .HasMaxLength(100)
                .HasColumnName("Channel_Name");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityKey).HasName("PK__City__2D42E4414BC818AE");

            entity.ToTable("City", "STA");

            entity.HasIndex(e => e.ZoneKey, "idx_City_ZoneKey");

            entity.Property(e => e.CityKey).HasColumnName("City_Key");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("City_Name");
            entity.Property(e => e.ZoneKey).HasColumnName("Zone_Key");

            entity.HasOne(d => d.ZoneKeyNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ZoneKey)
                .HasConstraintName("FK_City_Zone");
        });

        modelBuilder.Entity<CuttingDownA>(entity =>
        {
            entity.HasKey(e => e.CuttingDownAIncidentId).HasName("PK__Cutting___DB73CADEA6AD29EA");

            entity.ToTable("Cutting_Down_A", "STA");

            entity.Property(e => e.CuttingDownAIncidentId).HasColumnName("Cutting_Down_A_Incident_ID");
            entity.Property(e => e.CuttingDownCabinName)
                .HasMaxLength(100)
                .HasColumnName("Cutting_Down_Cabin_Name");
            entity.Property(e => e.PlannedEndDts)
                .HasColumnName("PlannedEndDTS");
            entity.Property(e => e.PlannedStartDts)
                .HasColumnName("PlannedStartDTS");
            entity.Property(e => e.ProblemTypeKey).HasColumnName("Problem_Type_Key");

            entity.HasOne(d => d.ProblemTypeKeyNavigation).WithMany(p => p.CuttingDownAs)
                .HasForeignKey(d => d.ProblemTypeKey)
                .HasConstraintName("FK_CuttingDownA_ProblemType");
        });

        modelBuilder.Entity<CuttingDownB>(entity =>
        {
            entity.HasKey(e => e.CuttingDownBIncidentId).HasName("PK__Cutting___C48B399FBEAC19D1");

            entity.ToTable("Cutting_Down_B", "STA");

            entity.Property(e => e.CuttingDownBIncidentId).HasColumnName("Cutting_Down_B_Incident_ID");
            entity.Property(e => e.CuttingDownCableName)
                .HasMaxLength(100)
                .HasColumnName("Cutting_Down_Cable_Name");
            entity.Property(e => e.PlannedEndDts)
                .HasColumnName("PlannedEndDTS");
            entity.Property(e => e.PlannedStartDts)
                .HasColumnName("PlannedStartDTS");
            entity.Property(e => e.ProblemTypeKey).HasColumnName("Problem_Type_Key");

            entity.HasOne(d => d.ProblemTypeKeyNavigation).WithMany(p => p.CuttingDownBs)
                .HasForeignKey(d => d.ProblemTypeKey)
                .HasConstraintName("FK_CuttingDownB_ProblemType");
        });

        modelBuilder.Entity<CuttingDownDetail>(entity =>
        {
            entity.HasKey(e => e.CuttingDownDetailKey).HasName("PK__Cutting___DB17612BA5A92787");

            entity.ToTable("Cutting_Down_Detail", "FTA");

            entity.HasIndex(e => e.CuttingDownKey, "idx_Cutting_Down_Detail_HeaderKey");

            entity.HasIndex(e => e.NetworkElementKey, "idx_Cutting_Down_Detail_Network_ElementKey");

            entity.Property(e => e.CuttingDownDetailKey).HasColumnName("Cutting_Down_Detail_Key");
            entity.Property(e => e.CuttingDownKey).HasColumnName("Cutting_Down_Key");
            entity.Property(e => e.NetworkElementKey).HasColumnName("Network_Element_Key");

            entity.HasOne(d => d.CuttingDownKeyNavigation).WithMany(p => p.CuttingDownDetails)
                .HasForeignKey(d => d.CuttingDownKey)
                .HasConstraintName("FK_Cutting_Down_Detail_Header");

            entity.HasOne(d => d.NetworkElementKeyNavigation).WithMany(p => p.CuttingDownDetails)
                .HasForeignKey(d => d.NetworkElementKey)
                .HasConstraintName("FK_Cutting_Down_Detail_Network_Element");
        });

        modelBuilder.Entity<CuttingDownHeader>(entity =>
        {
            entity.HasKey(e => e.CuttingDownKey).HasName("PK__Cutting___5629E0B09DF13C0B");

            entity.ToTable("Cutting_Down_Header", "FTA");

            entity.HasIndex(e => e.ChannelKey, "idx_Cutting_Down_Header_ChannelKey");

            entity.HasIndex(e => e.CuttingDownProblemTypeKey, "idx_Cutting_Down_Header_Problem_TypeKey");

            entity.Property(e => e.CuttingDownKey).HasColumnName("Cutting_Down_Key");
            entity.Property(e => e.ChannelKey).HasColumnName("Channel_Key");
            entity.Property(e => e.CreateSystemUserId).HasColumnName("CreateSystemUserID");
            entity.Property(e => e.CuttingDownIncidentId).HasColumnName("Cutting_Down_Incident_ID");
            entity.Property(e => e.CuttingDownProblemTypeKey).HasColumnName("Cutting_Down_Problem_Type_Key");
            entity.Property(e => e.PlannedEndDts)
                .HasColumnName("PlannedEndDTS");
            entity.Property(e => e.PlannedStartDts)
                .HasColumnName("PlannedStartDTS");
            entity.Property(e => e.UpdateSystemUserId).HasColumnName("UpdateSystemUserID");

            entity.HasOne(d => d.ChannelKeyNavigation).WithMany(p => p.CuttingDownHeaders)
                .HasForeignKey(d => d.ChannelKey)
                .HasConstraintName("FK_Cutting_Down_Header_Channel");

            entity.HasOne(d => d.CreateSystemUser).WithMany(p => p.CuttingDownHeaderCreateSystemUsers)
                .HasForeignKey(d => d.CreateSystemUserId)
                .HasConstraintName("FK_Cutting_Down_Header_Users");

            entity.HasOne(d => d.CuttingDownProblemTypeKeyNavigation).WithMany(p => p.CuttingDownHeaders)
                .HasForeignKey(d => d.CuttingDownProblemTypeKey)
                .HasConstraintName("FK_Cutting_Down_Header_Problem_Type");

            entity.HasOne(d => d.UpdateSystemUser).WithMany(p => p.CuttingDownHeaderUpdateSystemUsers)
                .HasForeignKey(d => d.UpdateSystemUserId)
                .HasConstraintName("FK_Cutting_Down_Header_Users1");
        });

        modelBuilder.Entity<CuttingDownIgnored>(entity =>
        {
            entity.HasKey(e => e.Id); // Set Id as the primary key

            entity.ToTable("Cutting_Down_Ignored", "FTA");

            entity.Property(e => e.CabinName)
                .HasMaxLength(100)
                .HasColumnName("Cabin_Name");
            entity.Property(e => e.CableName)
                .HasMaxLength(100)
                .HasColumnName("Cable_Name");
            entity.Property(e => e.CuttingDownIncidentId).HasColumnName("Cutting_Down_Incident_ID");

            entity.HasOne(d => d.CreatedUserNavigation).WithMany()
                .HasForeignKey(d => d.CreatedUser)
                .HasConstraintName("FK_Cutting_Down_Ignored_Users");
        });


        modelBuilder.Entity<Flat>(entity =>
        {
            entity.HasKey(e => e.FlatKey).HasName("PK__Flat__B7E8B7F70F3CA75F");

            entity.ToTable("Flat", "STA");

            entity.HasIndex(e => e.BuildingKey, "idx_Flat_BuildingKey");

            entity.Property(e => e.FlatKey).HasColumnName("Flat_Key");
            entity.Property(e => e.BuildingKey).HasColumnName("Building_Key");

            entity.HasOne(d => d.BuildingKeyNavigation).WithMany(p => p.Flats)
                .HasForeignKey(d => d.BuildingKey)
                .HasConstraintName("FK_Flat_Building");
        });

        modelBuilder.Entity<Governorate>(entity =>
        {
            entity.HasKey(e => e.GovernrateKey).HasName("PK__Governor__7EE78ED56E13B025");

            entity.ToTable("Governorate", "STA");

            entity.Property(e => e.GovernrateKey).HasColumnName("Governrate_Key");
            entity.Property(e => e.GovernrateName)
                .HasMaxLength(100)
                .HasColumnName("Governrate_Name");
        });

        modelBuilder.Entity<NetworkElement>(entity =>
        {
            entity.HasKey(e => e.NetworkElementKey).HasName("PK__Network___C7B956967B8DBE63");

            entity.ToTable("Network_Element", "FTA");

            entity.HasIndex(e => e.ParentNetworkElementKey, "idx_Network_Element_ParentKey");

            entity.HasIndex(e => e.NetworkElementTypeKey, "idx_Network_Element_TypeKey");

            entity.Property(e => e.NetworkElementKey).HasColumnName("Network_Element_Key");
            entity.Property(e => e.NetworkElementName)
                .HasMaxLength(255)
                .HasColumnName("Network_Element_Name");
            entity.Property(e => e.NetworkElementTypeKey).HasColumnName("Network_Element_Type_Key");
            entity.Property(e => e.ParentNetworkElementKey).HasColumnName("Parent_Network_Element_Key");

            entity.HasOne(d => d.NetworkElementTypeKeyNavigation).WithMany(p => p.NetworkElements)
                .HasForeignKey(d => d.NetworkElementTypeKey)
                .HasConstraintName("FK_Network_Element_Type");

            entity.HasOne(d => d.ParentNetworkElementKeyNavigation)
                .WithMany(p => p.InverseParentNetworkElementKeyNavigation)
                .HasForeignKey(d => d.ParentNetworkElementKey)
                .HasConstraintName("FK_Network_Element_Parent");
        });

        modelBuilder.Entity<NetworkElementHierarchyPath>(entity =>
        {
            entity.HasKey(e => e.NetworkElementHierarchyPathKey).HasName("PK__Network___EC59B2AB292D5B9A");

            entity.ToTable("Network_Element_Hierarchy_Path", "FTA");

            entity.Property(e => e.NetworkElementHierarchyPathKey).HasColumnName("Network_Element_Hierarchy_Path_Key");
            entity.Property(e => e.Abbreviation).HasMaxLength(100);
            entity.Property(e => e.NetwrokElementHierarchyPathName)
                .HasMaxLength(255)
                .HasColumnName("Netwrok_Element_Hierarchy_Path_Name");
        });

        modelBuilder.Entity<NetworkElementType>(entity =>
        {
            entity.HasKey(e => e.NetworkElementTypeKey).HasName("PK__Network___DD02B8BB50F4F3DF");

            entity.ToTable("Network_Element_Type", "FTA");

            entity.HasIndex(e => e.ParentNetworkElementTypeKey, "idx_Network_Element_Type_ParentKey");

            entity.Property(e => e.NetworkElementTypeKey).HasColumnName("Network_Element_Type_Key");
            entity.Property(e => e.NetworkElementHierarchyPathKey).HasColumnName("Network_Element_Hierarchy_Path_Key");
            entity.Property(e => e.NetworkElementTypeName)
                .HasMaxLength(100)
                .HasColumnName("Network_Element_Type_Name");
            entity.Property(e => e.ParentNetworkElementTypeKey).HasColumnName("Parent_Network_Element_Type_Key");

            entity.HasOne(d => d.NetworkElementHierarchyPathKeyNavigation).WithMany(p => p.NetworkElementTypes)
                .HasForeignKey(d => d.NetworkElementHierarchyPathKey)
                .HasConstraintName("FK_Network_Element_Type_Network_Element_Hierarchy_Path");

            entity.HasOne(d => d.ParentNetworkElementTypeKeyNavigation)
                .WithMany(p => p.InverseParentNetworkElementTypeKeyNavigation)
                .HasForeignKey(d => d.ParentNetworkElementTypeKey)
                .HasConstraintName("FK_Network_Element_Type_Parent");
        });

        modelBuilder.Entity<FTAProblemType>(entity =>
        {
            entity.HasKey(e => e.ProblemTypeKey).HasName("PK__Problem___E6DB25E9E4566EA1");

            entity.ToTable("Problem_Type", "FTA");

            entity.Property(e => e.ProblemTypeKey).HasColumnName("Problem_Type_Key");
            entity.Property(e => e.ProblemTypeName)
                .HasMaxLength(100)
                .HasColumnName("Problem_Type_Name");
        });

        modelBuilder.Entity<STAProblemType>(entity =>
        {
            entity.HasKey(e => e.ProblemTypeKey).HasName("PK__Problem___E6DB25E95ABD8009");

            entity.ToTable("Problem_Type", "STA");

            entity.Property(e => e.ProblemTypeKey).HasColumnName("Problem_Type_Key");
            entity.Property(e => e.ProblemTypeName)
                .HasMaxLength(100)
                .HasColumnName("Problem_Type_Name");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.SectorKey).HasName("PK__Sector__4D01A17EE6436EBD");

            entity.ToTable("Sector", "STA");

            entity.HasIndex(e => e.GovernrateKey, "idx_Sector_GovernrateKey");

            entity.Property(e => e.SectorKey).HasColumnName("Sector_Key");
            entity.Property(e => e.GovernrateKey).HasColumnName("Governrate_Key");
            entity.Property(e => e.SectorName)
                .HasMaxLength(100)
                .HasColumnName("Sector_Name");

            entity.HasOne(d => d.GovernrateKeyNavigation).WithMany(p => p.Sectors)
                .HasForeignKey(d => d.GovernrateKey)
                .HasConstraintName("FK_Sector_Governorate");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.StationKey).HasName("PK__Station__32A17CF0024F2ADA");

            entity.ToTable("Station", "STA");

            entity.HasIndex(e => e.CityKey, "idx_Station_CityKey");

            entity.Property(e => e.StationKey).HasColumnName("Station_Key");
            entity.Property(e => e.CityKey).HasColumnName("City_Key");
            entity.Property(e => e.StationName)
                .HasMaxLength(100)
                .HasColumnName("Station_Name");

            entity.HasOne(d => d.CityKeyNavigation).WithMany(p => p.Stations)
                .HasForeignKey(d => d.CityKey)
                .HasConstraintName("FK_Station_City");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionKey).HasName("PK__Subscrip__283EE706A4594C50");

            entity.ToTable("Subscription", "STA");

            entity.HasIndex(e => e.BuildingKey, "idx_Subscription_BuildingKey");

            entity.HasIndex(e => e.FlatKey, "idx_Subscription_FlatKey");

            entity.Property(e => e.SubscriptionKey).HasColumnName("Subscription_Key");
            entity.Property(e => e.BuildingKey).HasColumnName("Building_Key");
            entity.Property(e => e.FlatKey).HasColumnName("Flat_Key");
            entity.Property(e => e.MeterKey).HasColumnName("Meter_Key");
            entity.Property(e => e.PaletKey).HasColumnName("Palet_Key");

            entity.HasOne(d => d.BuildingKeyNavigation).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.BuildingKey)
                .HasConstraintName("FK_Subscription_Building");

            entity.HasOne(d => d.FlatKeyNavigation).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.FlatKey)
                .HasConstraintName("FK_Subscription_Flat");
        });

        modelBuilder.Entity<Tower>(entity =>
        {
            entity.HasKey(e => e.TowerKey).HasName("PK__Tower__6CF1B014B417D0C9");

            entity.ToTable("Tower", "STA");

            entity.HasIndex(e => e.StationKey, "idx_Tower_StationKey");

            entity.Property(e => e.TowerKey).HasColumnName("Tower_Key");
            entity.Property(e => e.StationKey).HasColumnName("Station_Key");
            entity.Property(e => e.TowerName)
                .HasMaxLength(100)
                .HasColumnName("Tower_Name");

            entity.HasOne(d => d.StationKeyNavigation).WithMany(p => p.Towers)
                .HasForeignKey(d => d.StationKey)
                .HasConstraintName("FK_Tower_Station");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserKey).HasName("PK__Users__A2B887F2CAA2D633");

            entity.ToTable("Users", "FTA");

            entity.Property(e => e.UserKey).HasColumnName("User_Key");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.ZoneKey).HasName("PK__Zone__702254989059BBDF");

            entity.ToTable("Zone", "STA");

            entity.HasIndex(e => e.SectorKey, "idx_Zone_SectorKey");

            entity.Property(e => e.ZoneKey).HasColumnName("Zone_Key");
            entity.Property(e => e.SectorKey).HasColumnName("Sector_Key");
            entity.Property(e => e.ZoneName)
                .HasMaxLength(100)
                .HasColumnName("Zone_Name");

            entity.HasOne(d => d.SectorKeyNavigation).WithMany(p => p.Zones)
                .HasForeignKey(d => d.SectorKey)
                .HasConstraintName("FK_Zone_Sector");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    // Call the stored procedure to fetch child network elements till Cable
    public async Task<List<int>> GetChildNetworkElementsTillCableAsync(int networkElementKey)
    {
        var childElements = new List<int>();

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "SP_Get_Child_Network_Elements_Till_Cable";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var param = command.CreateParameter();
            param.ParameterName = "@NetworkElementKey";
            param.Value = networkElementKey;
            command.Parameters.Add(param);

            await Database.OpenConnectionAsync();

            using (var result = await command.ExecuteReaderAsync())
            {
                while (await result.ReadAsync())
                {
                    childElements.Add(result.GetInt32(0)); // Assuming DescendantKey is returned
                }
            }

            await Database.CloseConnectionAsync();
        }

        return childElements;
    }

    // Call the stored procedure to fetch higher network elements till Cabin
    public async Task<List<int>> GetHigherNetworkElementsTillCabinAsync(int networkElementKey)
    {
        var parentElements = new List<int>();

        using (var command = Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "SP_Get_Higher_Network_Elements_Till_Cabin";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var param = command.CreateParameter();
            param.ParameterName = "@NetworkElementKey";
            param.Value = networkElementKey;
            command.Parameters.Add(param);

            await Database.OpenConnectionAsync();

            using (var result = await command.ExecuteReaderAsync())
            {
                while (await result.ReadAsync())
                {
                    parentElements.Add(result.GetInt32(0)); // Assuming AncestorKey is returned
                }
            }

            await Database.CloseConnectionAsync();
        }

        return parentElements;
    }
}