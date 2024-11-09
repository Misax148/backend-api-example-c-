FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MovieApi.API/MovieApi.API.csproj", "MovieApi.API/"]
COPY ["MovieApi.Core/MovieApi.Core.csproj", "MovieApi.Core/"]
COPY ["MovieApi.Infraestructure/MovieApi.Infraestructure.csproj", "MovieApi.Infraestructure/"]
RUN dotnet restore "MovieApi.API/MovieApi.API.csproj"
COPY . .
WORKDIR "/src/MovieApi.API"
RUN dotnet build "MovieApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovieApi.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieApi.API.dll"]

