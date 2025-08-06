# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Skop�ruj csproj a obnov z�vislosti
COPY financeBE/financeBE.csproj financeBE/
RUN dotnet restore financeBE/financeBE.csproj

# Skop�ruj cel� projekt a buildni
COPY . .
WORKDIR /src/financeBE
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "financeBE.dll"]
