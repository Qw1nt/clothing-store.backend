using ClothingStore.API.Extensions;
using ClothingStore.Configurations;
using ClothingStore.Services.Authentication;
using ClothingStore.Services.FileSaveService;
using ClothingStore.Services.HashSalt;
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

services.AddFileSaveService<FileSaveService>(builder);

services.AddSingleton(authenticationConfig);
services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(configure =>
{
    configure
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseClaimsDetermination();

app.MapControllers();

app.Run();