FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
COPY *.sln ./
COPY /DNCEcommerceApi /DNCEcommerceApi
COPY /DNCEcommerceApi.Tests /DNCEcommerceApi.Tests
RUN dotnet restore /DNCEcommerceApi/DNCEcommerceApi.csproj
RUN dotnet restore /DNCEcommerceApi.Tests/DNCEcommerceApi.Tests.csproj
COPY . ./
RUN dotnet restore

FROM base AS tests
RUN dotnet test

FROM base as publish
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/out .

ENTRYPOINT [ "dotnet", "DNCEcommerceApi.dll" ]