FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app/swp391

COPY ["PregnaCare.csproj", "./"]
RUN dotnet restore "./PregnaCare.csproj"

COPY . .
RUN dotnet build "PregnaCare.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PregnaCare.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5167
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 5167

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PregnaCare.dll"]