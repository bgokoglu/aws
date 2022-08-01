FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HelloBeanstalk/HelloBeanstalk.csproj", "HelloBeanstalk/"]
RUN dotnet restore "HelloBeanstalk/HelloBeanstalk.csproj"
COPY . .
WORKDIR "/src/HelloBeanstalk"
RUN dotnet build "HelloBeanstalk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloBeanstalk.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloBeanstalk.dll"]
