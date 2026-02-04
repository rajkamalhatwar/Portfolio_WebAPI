 # -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /out

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy published output
COPY --from=build /out .

# Render provides PORT automatically
ENV ASPNETCORE_URLS=http://+:$PORT

# 👇 CHANGE DLL NAME IF NEEDED
ENTRYPOINT ["dotnet", "Portfolio_APIs.dll"]
