FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MyPortforlioWebsite.csproj .
RUN dotnet restore MyPortforlioWebsite.csproj

COPY . .
RUN dotnet publish MyPortforlioWebsite.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MyPortforlioWebsite.dll"]