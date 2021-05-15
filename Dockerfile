
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Directory.Build.props .

COPY BLL/*.csproj ./BLL/
COPY DAL/*.csproj ./DAL/
COPY Domain/*.csproj ./Domain/
COPY Resources/*.csproj ./Resources/
COPY TestProject/*.csproj ./TestProject/

COPY WebApp/WebApp.csproj ./WebApp/

# restore all the nuget packages
RUN dotnet restore

# copy everything else and build app
COPY BLL/. ./BLL/
COPY DAL/. ./DAL/
COPY Domain/. ./Domain/
COPY Resources/. ./Resources/
COPY TestProject/. ./TestProject/

COPY WebApp/. ./WebApp/

WORKDIR /source/WebApp
RUN dotnet publish -c Release -o out


# create a new image from aspnet runtime (no compilers)
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime
WORKDIR /app
COPY --from=build /source/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]