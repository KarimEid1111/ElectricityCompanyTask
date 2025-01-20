using ConsoleApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Domain.Context;

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

    public virtual DbSet<Governrate> Governrates { get; set; }

    public virtual DbSet<NetworkElement> NetworkElements { get; set; }

    public virtual DbSet<NetworkElementHierarchyPath> NetworkElementHierarchyPaths { get; set; }

    public virtual DbSet<NetworkElementType> NetworkElementTypes { get; set; }

    public virtual DbSet<FtaProblemType> ProblemTypes { get; set; }

    public virtual DbSet<StaProblemType> ProblemTypes1 { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Subscribtion> Subscribtions { get; set; }

    public virtual DbSet<Tower> Towers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FC654RV\\KARIM;Database=Electricity Company Task;Integrated Security=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Block__3214EC27B831AB71");

            entity.ToTable("Block", "sta");

            entity.HasIndex(e => e.CableId, "IX_Cable_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CableId).HasColumnName("Cable_ID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Cable).WithMany(p => p.Blocks)
                .HasForeignKey(d => d.CableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Block_Cable");
        });

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Building__3214EC27CBE7E718");

            entity.ToTable("Building", "sta");

            entity.HasIndex(e => e.BlockId, "IX_Block_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BlockId).HasColumnName("Block_ID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Block).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Building_Block");
        });

        modelBuilder.Entity<Cabin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cabin__3214EC2717FE7759");

            entity.ToTable("Cabin", "sta");

            entity.HasIndex(e => e.TowerId, "IX_Tower_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TowerId).HasColumnName("Tower_ID");

            entity.HasOne(d => d.Tower).WithMany(p => p.Cabins)
                .HasForeignKey(d => d.TowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cabin_Tower");
        });

        modelBuilder.Entity<Cable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cable__3214EC27D5089817");

            entity.ToTable("Cable", "sta");

            entity.HasIndex(e => e.CabinId, "IX_Cabin_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CabinId).HasColumnName("Cabin_ID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Cabin).WithMany(p => p.Cables)
                .HasForeignKey(d => d.CabinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cable_Cabin");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__channel__3213E83F742E29AA");

            entity.ToTable("Channel", "fta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC2785C9176D");

            entity.ToTable("City", "sta");

            entity.HasIndex(e => e.ZoneId, "IX_Zone_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ZoneId).HasColumnName("Zone_ID");

            entity.HasOne(d => d.Zone).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Zone");
        });

        modelBuilder.Entity<CuttingDownA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cutting___3214EC2711B78E30");

            entity.ToTable("Cutting_Down_A", "sta");

            entity.HasIndex(e => e.ProblemTypeId, "IX_Problem_Type_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnName("Create_Date");
            entity.Property(e => e.CreatedUserName)
                .HasMaxLength(100)
                .HasColumnName("Created_User_Name");
            entity.Property(e => e.CuttingDownCabinName)
                .HasMaxLength(100)
                .HasColumnName("Cutting_Down_Cabin_Name");
            entity.Property(e => e.EndDate).HasColumnName("End_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsGlobal).HasColumnName("Is_Global");
            entity.Property(e => e.IsPlanned).HasColumnName("Is_Planned");
            entity.Property(e => e.PlannedEndDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_End_Datetime");
            entity.Property(e => e.PlannedStartDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_Start_Datetime");
            entity.Property(e => e.ProblemTypeId).HasColumnName("Problem_Type_ID");
            entity.Property(e => e.UpdatedUserName)
                .HasMaxLength(100)
                .HasColumnName("Updated_User_Name");

            entity.HasOne(d => d.StaProblemType).WithMany(p => p.CuttingDownAs)
                .HasForeignKey(d => d.ProblemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_A_Problem_Type");
        });

        modelBuilder.Entity<CuttingDownB>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cutting___3214EC2783597760");

            entity.ToTable("Cutting_Down_B", "sta");

            entity.HasIndex(e => e.ProblemTypeId, "IX_Problem_Type_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnName("Create_Date");
            entity.Property(e => e.CreatedUserName)
                .HasMaxLength(100)
                .HasColumnName("Created_User_Name");
            entity.Property(e => e.CuttingDownCableName)
                .HasMaxLength(100)
                .HasColumnName("Cutting_Down_Cable_Name");
            entity.Property(e => e.EndDate).HasColumnName("End_Date");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsGlobal).HasColumnName("Is_Global");
            entity.Property(e => e.IsPlanned).HasColumnName("Is_Planned");
            entity.Property(e => e.PlannedEndDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_End_Datetime");
            entity.Property(e => e.PlannedStartDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_Start_Datetime");
            entity.Property(e => e.ProblemTypeId).HasColumnName("Problem_Type_ID");
            entity.Property(e => e.UpdatedUserName)
                .HasMaxLength(100)
                .HasColumnName("Updated_User_Name");

            entity.HasOne(d => d.StaProblemType).WithMany(p => p.CuttingDownBs)
                .HasForeignKey(d => d.ProblemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_B_Problem_Type");
        });

        modelBuilder.Entity<CuttingDownDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cutting___3214EC27920A4AF8");

            entity.ToTable("Cutting_Down_Detail", "fta");

            entity.HasIndex(e => e.CuttingDownHeaderId, "IX_Cutting_Down_Header_ID");

            entity.HasIndex(e => e.NetworkElementId, "IX_Network_Element_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualCreateDate).HasColumnName("Actual_Create_Date");
            entity.Property(e => e.ActualEndDate).HasColumnName("Actual_End_Date");
            entity.Property(e => e.CuttingDownHeaderId).HasColumnName("Cutting_Down_Header_ID");
            entity.Property(e => e.ImpactedCustomers).HasColumnName("Impacted_Customers");
            entity.Property(e => e.NetworkElementId).HasColumnName("Network_Element_ID");

            entity.HasOne(d => d.CuttingDownHeader).WithMany(p => p.CuttingDownDetails)
                .HasForeignKey(d => d.CuttingDownHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_Detail_Cutting_Down_Header");

            entity.HasOne(d => d.NetworkElement).WithMany(p => p.CuttingDownDetails)
                .HasForeignKey(d => d.NetworkElementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_Detail_Network_Element");
        });

        modelBuilder.Entity<CuttingDownHeader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cutting___3214EC272ED484D6");

            entity.ToTable("Cutting_Down_Header", "fta");

            entity.HasIndex(e => e.ChannelId, "IX_Channel_ID");

            entity.HasIndex(e => e.CreatedSystemUserId, "IX_Created_System_User_ID");

            entity.HasIndex(e => e.CuttingDownIncidentId, "IX_Cutting_Down_Incident_ID");

            entity.HasIndex(e => e.CuttingDownProblemId, "IX_Cutting_Down_Problem_ID");

            entity.HasIndex(e => e.UpdatedSystemUserId, "IX_Updated_System_User_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualCreateDate).HasColumnName("Actual_Create_Date");
            entity.Property(e => e.ActualEndDate).HasColumnName("Actual_End_Date");
            entity.Property(e => e.ChannelId).HasColumnName("Channel_ID");
            entity.Property(e => e.CreatedSystemUserId).HasColumnName("Created_System_User_ID");
            entity.Property(e => e.CuttingDownIncidentId).HasColumnName("Cutting_Down_Incident_ID");
            entity.Property(e => e.CuttingDownProblemId).HasColumnName("Cutting_Down_Problem_ID");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsGlobal).HasColumnName("Is_Global");
            entity.Property(e => e.IsPlanned).HasColumnName("Is_Planned");
            entity.Property(e => e.PlannedEndDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_End_Datetime");
            entity.Property(e => e.PlannedStartDateyime)
                .HasColumnType("datetime")
                .HasColumnName("Planned_Start_Dateyime");
            entity.Property(e => e.SynchCreateDate).HasColumnName("Synch_Create_Date");
            entity.Property(e => e.SynchUpdateDate).HasColumnName("Synch_Update_Date");
            entity.Property(e => e.UpdatedSystemUserId).HasColumnName("Updated_System_User_ID");

            entity.HasOne(d => d.Channel).WithMany(p => p.CuttingDownHeaders)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_Header_Channel");

            entity.HasOne(d => d.CreatedSystemUser).WithMany(p => p.CuttingDownHeaderCreatedSystemUsers)
                .HasForeignKey(d => d.CreatedSystemUserId)
                .HasConstraintName("FK_Cutting_Down_Header_User3");

            entity.HasOne(d => d.CuttingDownFtaProblem).WithMany(p => p.CuttingDownHeaders)
                .HasForeignKey(d => d.CuttingDownProblemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cutting_Down_Header_Problem_Type");

            entity.HasOne(d => d.UpdatedSystemUser).WithMany(p => p.CuttingDownHeaderUpdatedSystemUsers)
                .HasForeignKey(d => d.UpdatedSystemUserId)
                .HasConstraintName("FK_Cutting_Down_Header_User2");
        });

        modelBuilder.Entity<CuttingDownIgnored>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cutting___3214EC27853328E0");

            entity.ToTable("Cutting_Down_Ignored", "fta");

            entity.HasIndex(e => e.CreatedUserId, "IX_Created_User_Id");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActualCreateDate).HasColumnName("Actual_Create_Date");
            entity.Property(e => e.CabinName)
                .HasMaxLength(100)
                .HasColumnName("Cabin_Name");
            entity.Property(e => e.CableName)
                .HasMaxLength(100)
                .HasColumnName("Cable_Name");
            entity.Property(e => e.CreatedUserId).HasColumnName("Created_User_Id");
            entity.Property(e => e.SynchDate).HasColumnName("Synch_Date");

            entity.HasOne(d => d.CreatedUser).WithMany(p => p.CuttingDownIgnoreds)
                .HasForeignKey(d => d.CreatedUserId)
                .HasConstraintName("FK_Cutting_Down_Ignored_User");
        });

        modelBuilder.Entity<Flat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flat__3214EC27705E4CC3");

            entity.ToTable("Flat", "sta");

            entity.HasIndex(e => e.BuildingId, "IX_Building_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BuildingId).HasColumnName("Building_ID");

            entity.HasOne(d => d.Building).WithMany(p => p.Flats)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flat_Building");
        });

        modelBuilder.Entity<Governrate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Governra__3214EC27C7BF0174");

            entity.ToTable("Governrate", "sta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<NetworkElement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Network___3214EC27114E61DB");

            entity.ToTable("Network_Element", "fta");

            entity.HasIndex(e => e.NetworkElementTypeId, "IX_Network_Element_Type_ID");

            entity.HasIndex(e => e.ParentNetworkElementId, "IX_Parent_Network_Element_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NetworkElementTypeId).HasColumnName("Network_Element_Type_ID");
            entity.Property(e => e.ParentNetworkElementId).HasColumnName("Parent_Network_Element_ID");

            entity.HasOne(d => d.NetworkElementType).WithMany(p => p.NetworkElements)
                .HasForeignKey(d => d.NetworkElementTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Network_Element_Network_Element_Type");

            entity.HasOne(d => d.ParentNetworkElement).WithMany(p => p.InverseParentNetworkElement)
                .HasForeignKey(d => d.ParentNetworkElementId)
                .HasConstraintName("FK_Network_Element_Network_Element");
        });

        modelBuilder.Entity<NetworkElementHierarchyPath>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Network___3214EC279E35EDF0");

            entity.ToTable("Network_Element_Hierarchy_Path", "fta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Abbreviation)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<NetworkElementType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__network___3214EC2714360033");

            entity.ToTable("Network_Element_Type", "fta");

            entity.HasIndex(e => e.NetworkElementHierarchyPathId, "IX_Network_Element_Hierarchy_Path_ID");

            entity.HasIndex(e => e.ParentNetworkElementId, "IX_Parent_Network_Element_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NetworkElementHierarchyPathId).HasColumnName("Network_Element_Hierarchy_Path_ID");
            entity.Property(e => e.ParentNetworkElementId).HasColumnName("Parent_Network_Element_ID");

            entity.HasOne(d => d.NetworkElementHierarchyPath).WithMany(p => p.NetworkElementTypes)
                .HasForeignKey(d => d.NetworkElementHierarchyPathId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Network_Element_Type_Network_Element_Hierarchy_Path");

            entity.HasOne(d => d.ParentNetworkElement).WithMany(p => p.InverseParentNetworkElement)
                .HasForeignKey(d => d.ParentNetworkElementId)
                .HasConstraintName("FK_Network_Element_Type_Network_Element_Type");
        });

        modelBuilder.Entity<FtaProblemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Problem___3214EC27FCB5161C");

            entity.ToTable("Problem_Type", "fta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<StaProblemType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Problem___3214EC27437D720B");

            entity.ToTable("Problem_Type", "sta");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sector__3214EC27BA3D05DC");

            entity.ToTable("Sector", "sta");

            entity.HasIndex(e => e.GovernrateId, "IX_Governrate_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GovernrateId).HasColumnName("Governrate_ID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Governrate).WithMany(p => p.Sectors)
                .HasForeignKey(d => d.GovernrateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sector_Governrate");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Station__3214EC27374B3ED9");

            entity.ToTable("Station", "sta");

            entity.HasIndex(e => e.CityId, "IX_City_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityId).HasColumnName("City_ID");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Stations)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_City");
        });

        modelBuilder.Entity<Subscribtion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrib__3214EC2786556170");

            entity.ToTable("Subscribtion", "sta");

            entity.HasIndex(e => e.BuildingId, "IX_Building_ID");

            entity.HasIndex(e => e.FlatId, "IX_Flat_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BuildingId).HasColumnName("Building_ID");
            entity.Property(e => e.FlatId).HasColumnName("Flat_ID");
            entity.Property(e => e.MeterId).HasColumnName("Meter_ID");
            entity.Property(e => e.PaletId).HasColumnName("Palet_ID");

            entity.HasOne(d => d.Building).WithMany(p => p.Subscribtions)
                .HasForeignKey(d => d.BuildingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscribtion_Building");

            entity.HasOne(d => d.Flat).WithMany(p => p.Subscribtions)
                .HasForeignKey(d => d.FlatId)
                .HasConstraintName("FK_Subscribtion_Flat");
        });

        modelBuilder.Entity<Tower>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tower__3214EC2733E8A859");

            entity.ToTable("Tower", "sta");

            entity.HasIndex(e => e.StationId, "IX_Station_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StationId).HasColumnName("Station_ID");

            entity.HasOne(d => d.Station).WithMany(p => p.Towers)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tower_Station");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user__3213E83FC7BB03C9");

            entity.ToTable("User", "fta");

            entity.Property(e => e.UserId).HasColumnName("User_Key");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("Password");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Zone__3214EC272CCECF78");

            entity.ToTable("Zone", "sta");

            entity.HasIndex(e => e.SectorId, "IX_Sector_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SectorId).HasColumnName("Sector_ID");

            entity.HasOne(d => d.Sector).WithMany(p => p.Zones)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Zone_Sector");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
