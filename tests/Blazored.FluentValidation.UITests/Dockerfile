FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tests/Blazored.FluentValidation.Tests/Blazored.FluentValidation.Tests.csproj", "tests/Blazored.FluentValidation.Tests/"]
COPY ["src/Blazored.FluentValidation/Blazored.FluentValidation.csproj", "src/Blazored.FluentValidation/"]
COPY ["samples/BlazorServer/BlazorServer.csproj", "samples/BlazorServer/"]
COPY ["samples/Shared/SharedModels/SharedModels.csproj", "samples/Shared/SharedModels/"]
RUN dotnet restore "tests/Blazored.FluentValidation.Tests/Blazored.FluentValidation.Tests.csproj"
COPY . .
WORKDIR "/src/tests/Blazored.FluentValidation.Tests"
RUN dotnet build "Blazored.FluentValidation.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Blazored.FluentValidation.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blazored.FluentValidation.Tests.dll"]
