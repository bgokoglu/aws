FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloS3/HelloS3.csproj", "HelloS3/"]
RUN dotnet restore "HelloS3/HelloS3.csproj"
COPY . .
WORKDIR "/src/HelloS3"
RUN dotnet build "HelloS3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloS3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloS3.dll"]
