FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FleetApi.Api/FleetApi.Api.csproj", "src/FleetApi.Api/"]
COPY ["src/FleetApi.Application/FleetApi.Application.csproj", "src/FleetApi.Application/"]
COPY ["src/FleetApi.Domain/FleetApi.Domain.csproj", "src/FleetApi.Domain/"]
COPY ["src/FleetApi.Infrastructure/FleetApi.Infrastructure.csproj", "src/FleetApi.Infrastructure/"]
RUN dotnet restore "src/FleetApi.Api/FleetApi.Api.csproj"
COPY . .
WORKDIR "/src/src/FleetApi.Api"
RUN dotnet build "FleetApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FleetApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FleetApi.Api.dll"]
