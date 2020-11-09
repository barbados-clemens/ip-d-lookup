FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY *.sln .
COPY IpDLookUp.Core/*.csproj ./IpDLookUp.Core/
COPY IpDLookUp.Services/*.csproj ./IpDLookUp.Services/
COPY IpDLookUp.Tests/*.csproj ./IpDLookUp.Tests/
COPY IpDLookUp.Worker/*.csproj ./IpDLookUp.Worker/
RUN dotnet restore

COPY IpDLookUp.Core/. ./IpDLookUp.Core/
COPY IpDLookUp.Services/. ./IpDLookUp.Services/
COPY IpDLookUp.Tests/. ./IpDLookUp.Tests/
COPY IpDLookUp.Worker/. ./IpDLookUp.Worker/
WORKDIR /app
CMD ls
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/dist ./
ENTRYPOINT [ "dotnet", "IpDLookUp.Core.dll" ]