FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloSQS-Fulfill/HelloSQS-Fulfill.csproj", "HelloSQS-Fulfill/"]
RUN dotnet restore "HelloSQS-Fulfill/HelloSQS-Fulfill.csproj"
COPY . .
WORKDIR "/src/HelloSQS-Fulfill"
RUN dotnet build "HelloSQS-Fulfill.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloSQS-Fulfill.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloSQS-Fulfill.dll"]
