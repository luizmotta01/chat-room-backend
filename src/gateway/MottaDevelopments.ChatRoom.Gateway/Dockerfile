#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MottaDevelopments.ChatRoom.Gateway/MottaDevelopments.ChatRoom.Gateway.csproj", "src/MottaDevelopments.ChatRoom.Gateway/"]
RUN dotnet restore "src/MottaDevelopments.ChatRoom.Gateway/MottaDevelopments.ChatRoom.Gateway.csproj"
COPY . .
WORKDIR "/src/src/MottaDevelopments.ChatRoom.Gateway"
RUN dotnet build "MottaDevelopments.ChatRoom.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MottaDevelopments.ChatRoom.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MottaDevelopments.ChatRoom.Gateway.dll"]