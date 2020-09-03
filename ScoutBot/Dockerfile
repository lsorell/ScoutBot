# ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
# BUILD IMAGE
# We build in a unique image, since the sdk package size
# is much larger than the runtime.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
# Files in .dockerignore will be ignored
COPY . ./
RUN dotnet publish -c Release -o out

# ---- ---- ---- ---- ---- ---- ---- ---- ---- ----
# RUNTIME IMAGE
# Creating a new image just for the runtime will
# drastically reduce the size of our container image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

# Copy build files from build image
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ScoutBot.dll"]