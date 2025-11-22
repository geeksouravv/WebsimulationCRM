using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using WebsimulationCRM.CORE.Domain.IdentityEntities;
using WebsimulationCRM.CORE.RepositoryContracts;
using WebsimulationCRM.CORE.ServiceContracts;
using WebsimulationCRM.CORE.Services;
using WebsimulationCRM.Infrastructure.DBContext;
using WebsimulationCRM.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser , ApplicationRole,ApplicationDbContext,int>>()
    .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,int>>();

builder.Services.AddAuthorization(option =>
{
    option.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/login";
});
// Lead Repository + Service DI
builder.Services.AddScoped<ILeadRepository, LeadsRepository>();
builder.Services.AddScoped<ILeadsAddService, LeadsAddService>();
builder.Services.AddScoped<ILeadsGetService, LeadsGetService>();
builder.Services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<INotificationService, NotificationService>();
// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();


app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
