using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using WebClientSample.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// For changing to Sqlite also add UseSqlite library 
// builder.Services.AddDbContext<IdpContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// For changing to SQLServer also add sql library 
builder.Services.AddDbContextPool<IdpContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = "Cookies";
		options.DefaultChallengeScheme = "oidc";
	})
	.AddCookie("Cookies")
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = "https://localhost:5001";

		options.ClientId = "web";
		options.ClientSecret = "secret";
		options.ResponseType = "code";

		options.SaveTokens = true;

		options.Scope.Clear();
		options.Scope.Add("openid");
		options.Scope.Add("profile");
		options.Scope.Add("offline_access");
		options.Scope.Add("api1");
		//options.Scope.Add("driver");
		options.Scope.Add("myDriver");

		options.GetClaimsFromUserInfoEndpoint = true;
		//options.ClaimActions.MapUniqueJsonKey("driver", "driver");
		options.ClaimActions.MapUniqueJsonKey("myDriverClaim", "myDriverClaim");
		options.ClaimActions.MapUniqueJsonKey("Username", "Username");
		options.ClaimActions.MapUniqueJsonKey("UserId", "UserId");

		options.Events = new OpenIdConnectEvents
		{
			OnTicketReceived = context =>
			{
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				return Task.CompletedTask;
			},
			OnMessageReceived = context =>
			{
				return Task.CompletedTask;
			},
			OnUserInformationReceived = context =>
			{
				return Task.CompletedTask;
			},
			OnAuthorizationCodeReceived = context =>
			{
				return Task.CompletedTask;
			},
			OnTokenResponseReceived = context =>
			{
				return Task.CompletedTask;
			},
			OnRemoteFailure = context =>
			{
				return Task.CompletedTask;
			},
		};

	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
