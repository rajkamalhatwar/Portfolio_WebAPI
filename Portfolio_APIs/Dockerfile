# -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files
COPY Portfolio_APIs/Portfolio_APIs.csproj Portfolio_APIs/
COPY DataAccess/DataAccess.csproj DataAccess/

# Restore dependencies
RUN dotnet restore Portfolio_APIs/Portfolio_APIs.csproj

# Copy everything else
COPY . .

# Publish
RUN dotnet publish Portfolio_APIs/Portfolio_APIs.csproj -c Release -o /out

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "Portfolio_APIs.dll"]
