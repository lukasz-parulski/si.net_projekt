FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["171701_api/171701_api.csproj", "171701_api/"]
RUN dotnet restore "171701_api/171701_api.csproj"
COPY . .
WORKDIR "/src/171701_api"
RUN dotnet build "171701_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "171701_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "171701_api.dll"]
