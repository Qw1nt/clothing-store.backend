FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./ClothingStore.API/ClothingStore.API.csproj", "ClothingStore.API/"]
COPY ["./ClothingStore.Configurations/ClothingStore.Configurations.csproj", "ClothingStore.Configurations/"]
COPY ["./ClothingStore.Data.Context/ClothingStore.Data.Context.csproj", "ClothingStore.Data.Context/"]
COPY ["./ClothingStore.Data.Requests/ClothingStore.Data.Requests.csproj", "ClothingStore.Data.Requests/"]
COPY ["./ClothingStore.Data.Responses/ClothingStore.Data.Responses.csproj", "ClothingStore.Data.Responses/"]
COPY ["./ClothingStore.Data.Validators/ClothingStore.Data.Validators.csproj", "ClothingStore.Data.Validators/"]
COPY ["./ClothingStore.Services/ClothingStore.Services.csproj", "ClothingStore.Services/"]
RUN dotnet restore "ClothingStore.API/ClothingStore.API.csproj"
COPY . .
WORKDIR "/src/ClothingStore.API"
RUN dotnet build "ClothingStore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClothingStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClothingStore.API.dll"]
