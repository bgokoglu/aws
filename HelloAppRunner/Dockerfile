FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloAppRunner.csproj", "HelloAppRunner/"]
RUN dotnet restore "HelloAppRunner/HelloAppRunner.csproj"

WORKDIR "/src/HelloAppRunner"
COPY . .
RUN dotnet build "HelloAppRunner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloAppRunner.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloAppRunner.dll"]