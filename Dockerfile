FROM node:16.15.1 AS ui
RUN npm g --no-package-lock @angular/cli@14.0.1
WORKDIR /app
COPY UI/public-ui/ ./
RUN npm install --no-package-lock --force && $(npm bin)/ng build --configuration production

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY ./ ./
RUN dotnet restore

WORKDIR /app/Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/Api/out ./
COPY --from=ui /app/dist/public-ui ./wwwroot
ENTRYPOINT ["dotnet", "Api.dll"]
