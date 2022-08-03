FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloCloudWatch.csproj", "HelloCloudWatch/"]
RUN dotnet restore "HelloCloudWatch/HelloCloudWatch.csproj"

WORKDIR "/src/HelloCloudWatch"
COPY . .
RUN dotnet build "HelloCloudWatch.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloCloudWatch.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloCloudWatch.dll"]