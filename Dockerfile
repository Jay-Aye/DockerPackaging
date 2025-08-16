FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DockerPackaging.csproj", "./"]
RUN dotnet restore "DockerPackaging.csproj"
COPY . .
RUN dotnet build "DockerPackaging.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerPackaging.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerPackaging.dll"]
