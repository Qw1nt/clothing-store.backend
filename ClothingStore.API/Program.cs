using ClothingStore.API.Extensions;
using ClothingStore.Configurations;
using ClothingStore.Services;
using VKWebApi.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var authenticationConfig = new AuthenticationConfiguration();
configuration.GetSection(AuthenticationConfiguration.SectionKey).Bind(authenticationConfig);

services.AddJwtAuthentication(authenticationConfig);
services.AddAuthorizationWithPolicy();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddAuthenticationSwaggerGen();

services.AddMemoryCache();
services.AddDataContext(configuration);

services.AddScoped<IHashSaltService, ComputeHashSaltService>();
services.AddScoped<JwtGenerationService>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddSingleton(authenticationConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();