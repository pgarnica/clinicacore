#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClinicaHumaita/ClinicaHumaita.csproj", "ClinicaHumaita/"]
COPY ["ClinicaHumaitaBusiness/ClinicaHumaita.Business.csproj", "ClinicaHumaitaBusiness/"]
COPY ["ClinicaHumaita.Repository/ClinicaHumaita.Data.csproj", "ClinicaHumaita.Repository/"]
COPY ["ClinicaHumaita.Shared/ClinicaHumaita.Shared.csproj", "ClinicaHumaita.Shared/"]
RUN dotnet restore "ClinicaHumaita/ClinicaHumaita.csproj"
COPY . .
WORKDIR "/src/ClinicaHumaita"
RUN dotnet build "ClinicaHumaita.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClinicaHumaita.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClinicaHumaita.dll"]