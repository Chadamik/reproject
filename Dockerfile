FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY *.sln ./
COPY REproject3_1/REproject3_1.csproj ./REproject3_1/
COPY JSONLibrary/JSONLibrary.csproj ./JSONLibrary/
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "REproject3_1.dll"]