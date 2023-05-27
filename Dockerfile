FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
WORKDIR /build
COPY Directory.Build.* Directory.Packages.props .editorconfig *.sln ./
COPY src/Directory.Build.* src/
COPY tests/Directory.Build.* tests/

COPY src/Api/Api.csproj src/Api/
COPY src/Application/Application.csproj src/Application/
COPY src/Database/Database.csproj src/Database/
COPY src/Domain/Domain.csproj src/Domain/
COPY tests/Domain.UnitTests/Domain.UnitTests.csproj tests/Domain.UnitTests/
COPY tests/Api.UnitTests/Api.UnitTests.csproj tests/Api.UnitTests/
COPY tests/Application.UnitTests/Application.UnitTests.csproj tests/Application.UnitTests/
COPY tests/Database.UnitTests/Database.UnitTests.csproj tests/Database.UnitTests/

RUN dotnet restore --nologo

FROM restore AS build
COPY . .
RUN dotnet build --no-restore --nologo -c Release

FROM build AS api-publish
RUN dotnet publish --no-build --nologo -c Release -o /app ./src/Api/Api.csproj

FROM base AS api-runtime
COPY --from=api-publish /app .
ENTRYPOINT ["dotnet", "Greentube.DemoWallet.Api.dll"]
