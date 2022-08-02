FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloSQS/HelloSQS.csproj", "HelloSQS/"]
RUN dotnet restore "HelloSQS/HelloSQS.csproj"
COPY . .
WORKDIR "/src/HelloSQS"
RUN dotnet build "HelloSQS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloSQS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloSQS.dll"]
