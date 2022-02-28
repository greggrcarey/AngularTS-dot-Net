using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddRazorRuntimeCompilation();

////using System.Text.Json.Serialization setup 
//builder.Services.AddControllersWithViews(options =>
//{   
//    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
//    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
//    {
//        ReferenceHandler = ReferenceHandler.IgnoreCycles
//    }));
//}).AddRazorRuntimeCompilation();
builder.Services.AddTransient<IMailService, NullMailService>();


//Add DB Context for EF and prepare to seed data
var connectionString = builder.Configuration.GetConnectionString("DutchContextDb");
builder.Services.AddDbContext<DutchContext>(cfg =>
{
    cfg.UseSqlServer(connectionString);
});


builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<DutchContext>();

builder.Services.AddAuthentication().AddIdentityServerJwt();

builder.Services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = builder.Configuration.GetValue<string>("Tokens:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("Tokens:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Tokens:Key")))
                    };
                });

builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddScoped<IDutchRepository, DutchRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

if (args.Length > 0 && args[0].ToLower() == "/seed")
{
    SeedDb(app);
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
}

static void SeedDb(WebApplication app)
{
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
    if (scopeFactory is not null)
    {
        using var scope = scopeFactory.CreateScope();
        var seederService = scope.ServiceProvider.GetService<DutchSeeder>();
        if (seederService is not null)
        {
            //I know .Wait() breaks async, but we are not learning that now.
            seederService.SeedAsync().Wait();
        }
    }
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();//Must be 1st
app.UseAuthorization();//Must be 2nd


app.UseEndpoints(cfg =>
{
    cfg.MapRazorPages();

    cfg.MapControllerRoute("Default",
        "/{controller}/{action}/{id?}",
        new { controller = "App", action = "Index" });
});

//app.MapGet("/", () => "Hello World!");
app.Run();
