FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloSES/HelloSES.csproj", "HelloSES/"]
RUN dotnet restore "HelloSES/HelloSES.csproj"
COPY . .
WORKDIR "/src/HelloSES"
RUN dotnet build "HelloSES.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloSES.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloSES.dll"]
