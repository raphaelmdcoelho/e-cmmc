# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["E.CMMC.Inventory.csproj", "./"]
RUN dotnet restore "./E.CMMC.Inventory.csproj.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "E.CMMC.Inventory.csproj.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E.CMMC.Inventory.csproj.csproj" -c Release -o /app/publish

# Build the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E.CMMC.Inventory.csproj.dll"]