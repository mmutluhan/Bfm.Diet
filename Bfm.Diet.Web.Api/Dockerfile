#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5500

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Bfm.Diet.Web.Api/Bfm.Diet.Web.Api.csproj", "Bfm.Diet.Web.Api/"]
COPY ["Bfm.Diet.Dto/Bfm.Diet.Dto.csproj", "Bfm.Diet.Dto/"]
COPY ["Bfm.Diet.Authorization/Bfm.Diet.Authorization.csproj", "Bfm.Diet.Authorization/"]
COPY ["Bfm.Diet.Core/Bfm.Diet.Core.csproj", "Bfm.Diet.Core/"]
COPY ["Bfm.Diet.Service/Bfm.Diet.Service.csproj", "Bfm.Diet.Service/"]
COPY ["Bfm.Diet.Model/Bfm.Diet.Model.csproj", "Bfm.Diet.Model/"]
RUN dotnet restore "Bfm.Diet.Web.Api/Bfm.Diet.Web.Api.csproj"
COPY . .
WORKDIR "/src/Bfm.Diet.Web.Api"
RUN dotnet build "Bfm.Diet.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bfm.Diet.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 5500
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bfm.Diet.Web.Api.dll"]