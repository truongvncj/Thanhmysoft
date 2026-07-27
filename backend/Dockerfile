FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["SaleManagerApi/SaleManagerApi.csproj", "SaleManagerApi/"]
RUN dotnet restore "SaleManagerApi/SaleManagerApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/SaleManagerApi"
RUN dotnet publish "SaleManagerApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SaleManagerApi.dll"]
