
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

#microsoft official .net sdk build 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

WORKDIR /src/app/apigateway
COPY . .
RUN dotnet restore "JK.csproj"
RUN dotnet build "JK.csproj" -c Release -o /app

#publish build from sdk build 
FROM build AS publish
RUN dotnet publish "JK.csproj" -c Release -o /app

#run build published output
FROM base AS final
#no need to update dou unix because patch has been included in 3.1 image

WORKDIR /app

COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JK.dll"]