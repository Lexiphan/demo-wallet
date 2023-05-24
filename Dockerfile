FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
WORKDIR /build
COPY Directory.Build.* Directory.Packages.props .editorconfig *.sln ./
COPY src/Directory.Build.* src/
COPY src/Api/Api.csproj src/Api/
RUN dotnet restore --nologo

FROM restore AS build
COPY . .
RUN dotnet build --no-restore --nologo -c Release

FROM build AS api-publish
RUN dotnet publish --no-build --nologo -c Release -o /app ./src/Api/Api.csproj

FROM base AS api-runtime
COPY --from=api-publish /app .
ENTRYPOINT ["dotnet", "Greentube.DemoWallet.Api.dll"]
