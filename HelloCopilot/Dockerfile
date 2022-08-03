FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloCopilot/HelloCopilot.csproj", "HelloCopilot/"]
RUN dotnet restore "HelloCopilot/HelloCopilot.csproj"
COPY . .
WORKDIR "/src/HelloCopilot"
RUN dotnet build "HelloCopilot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloCopilot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloCopilot.dll"]
