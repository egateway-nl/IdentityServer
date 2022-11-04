using Duende.IdentityServer;
using IdentityServer;
using IdentityServerAspNetIdentity.Data;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


        // For changing to SQLServer also add sql library 
        // builder.Services.AddDbContextPool<DataSpacePORContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")))


        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<CustomProfileService>();
        //.AddProfileService<CustomProfileService>();

        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            })
            .AddFacebook("Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
            });
        //payman remove Demo IdentityServer
        //.AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
        //{
        //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        //    options.SaveTokens = true;

        //    options.Authority = "https://demo.duendesoftware.com";
        //    options.ClientId = "interactive.confidential";
        //    options.ClientSecret = "secret";
        //    options.ResponseType = "code";

        //    ///Payman
        //    ///options.ClaimActions.Add
        //    //options.ClaimActions.DeleteClaim("sid");
        //    options.ClaimActions.MapUniqueJsonKey("custom_company_id", "custom_company_id");
        //    //options.ClaimActions.Add(new ClaimAction ("custom_company_id", "custom_company_id"))

        //    options.ClaimActions.Add(new MapAllClaimsAction());

        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        //NameClaimType = "name",
        //        //RoleClaimType = "role"
        //        //or
        //        NameClaimType = JwtClaimTypes.GivenName,
        //        RoleClaimType = JwtClaimTypes.Role
        //    };
        //});

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}