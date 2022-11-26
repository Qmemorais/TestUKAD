#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestUrl.MvcApp/TestUrl.MvcApp.csproj", "TestUrl.MvcApp/"]
COPY ["TestUrls.BusinessLayer/TestUrls.TestResultLogic.csproj", "TestUrls.BusinessLayer/"]
COPY ["TestURLS.UrlLogic/TestURLS.UrlLogic.csproj", "TestURLS.UrlLogic/"]
COPY ["TestUrls.EntityFramework/TestUrls.EntityFramework.csproj", "TestUrls.EntityFramework/"]
RUN dotnet restore "TestUrl.MvcApp/TestUrl.MvcApp.csproj"
COPY . .
WORKDIR "/src/TestUrl.MvcApp"
RUN dotnet build "TestUrl.MvcApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestUrl.MvcApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestUrl.MvcApp.dll"]