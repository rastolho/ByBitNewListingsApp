
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0-jammy AS base

WORKDIR /app

RUN mkdir -p /app/data

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ByBitNewListingsApp/ByBitNewListingsApp.csproj", "ByBitNewListingsApp/"]
RUN dotnet restore "./ByBitNewListingsApp/ByBitNewListingsApp.csproj"
COPY . .
WORKDIR "/src/ByBitNewListingsApp"
RUN dotnet build "./ByBitNewListingsApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ByBitNewListingsApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ByBitNewListingsApp.dll"]