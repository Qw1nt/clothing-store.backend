using API.Extensions;
using Application;
using Domain.Common.Configurations;
using Infrastructure;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.FileSaveService;
using Infrastructure.Services.HashSalt;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthorizationWithPolicy();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddAuthenticationSwaggerGen();

services.AddMemoryCache();
services.AddDataContext(configuration);

services.AddInfrastructureServices(configuration, builder.Environment);

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