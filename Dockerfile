#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_URLS http://+:80;https://+:8443;http://+:8000
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN chgrp -R 0 . && chmod -R g=u .
    
COPY ["EmployeeRegistrationService.csproj", "."]
RUN dotnet restore "./EmployeeRegistrationService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EmployeeRegistrationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeRegistrationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeRegistrationService.dll"]
