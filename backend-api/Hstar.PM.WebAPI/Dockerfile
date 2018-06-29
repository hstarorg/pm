FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Hstar.PM.WebAPI/Hstar.PM.WebAPI.csproj Hstar.PM.WebAPI/
RUN dotnet restore Hstar.PM.WebAPI/Hstar.PM.WebAPI.csproj
COPY . .
WORKDIR /src/Hstar.PM.WebAPI
RUN dotnet build Hstar.PM.WebAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Hstar.PM.WebAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hstar.PM.WebAPI.dll"]
