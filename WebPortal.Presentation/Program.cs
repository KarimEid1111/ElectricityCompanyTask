using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebPortal.Service.Common;
using WebPortal.Service.Repositories;
using WebPortal.Service.Services;
using WebPortalDomain.Context;
using WebPortalDomain.Interfaces.Common;
using WebPortalDomain.Interfaces.Repositories;
using WebPortalDomain.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBlockRepository, BlockRepository>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<ICabinRepository, CabineRepository>();
builder.Services.AddScoped<ICableRepository, CableRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IFlatRepository, FlatRepository>();
builder.Services.AddScoped<IGovernrateRepository, GovernrateRepository>();
builder.Services.AddScoped<INetworkElementHierarchyPathRepository, NetworkElementHierarchyPathRepository>();
builder.Services.AddScoped<INetworkElementRepository, NetworkElementRepository>();
builder.Services.AddScoped<INetworkElementTypeRepository, NetworkElementTypeRepository>();
builder.Services.AddScoped<IFtaProblemTypeRepository, FtaProblemTypeRepository>();
builder.Services.AddScoped<IStaProblemTypeRepository, StaProblemTypeRepository>();
builder.Services.AddScoped<ISectorRepository, SectorRepository>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<ISubscribtionRepository, SubscribtionRepository>();
builder.Services.AddScoped<ITowerRepository, TowerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<ICuttingDownHeaderRepository, CuttingDownHeaderRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<ICuttingDownIgnoredRepository, CuttingDownIgnoredRepository>();
builder.Services.AddScoped<ICuttingDetailRepository, CuttingDetailRepository>();



builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";  // Path to your login page
        options.AccessDeniedPath = "/Login/Login";  // Path for access denied
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Cookie expiration time
        options.SlidingExpiration = true;  // Renew cookie before expiration
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CuttingDownSearch}/{action=Search}/{id?}");

app.Run();