#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BLOG.Api/BLOG.Api.csproj", "BLOG.Api/"]
COPY ["BLOG.Application/BLOG.Application.csproj", "BLOG.Application/"]
COPY ["BLOG.Domain/BLOG.Domain.csproj", "BLOG.Domain/"]
COPY ["BLOG.Infrastructure/BLOG.Infrastructure.csproj", "BLOG.Infrastructure/"]
RUN dotnet restore "./BLOG.Api/./BLOG.Api.csproj"
COPY . .
WORKDIR "/src/BLOG.Api"
RUN dotnet build "./BLOG.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BLOG.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BLOG.Api.dll"]