FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloSNS/HelloSNS.csproj", "HelloSNS/"]
RUN dotnet restore "HelloSNS/HelloSNS.csproj"
COPY . .
WORKDIR "/src/HelloSNS"
RUN dotnet build "HelloSNS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloSNS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloSNS.dll"]
