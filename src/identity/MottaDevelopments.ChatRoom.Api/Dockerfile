#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MottaDevelopments.ChatRoom/MottaDevelopments.ChatRoom.Api.csproj", "src/MottaDevelopments.ChatRoom/"]
COPY ["src/MottaDevelopments.ChatRoom.Application/MottaDevelopments.ChatRoom.Application.csproj", "src/MottaDevelopments.ChatRoom.Application/"]
COPY ["src/MottaDevelopments.ChatRoom.Infrastructure/MottaDevelopments.ChatRoom.Infrastructure.csproj", "src/MottaDevelopments.ChatRoom.Infrastructure/"]
COPY ["src/MottaDevelopments.ChatRoom.Domain/MottaDevelopments.ChatRoom.Domain.csproj", "src/MottaDevelopments.ChatRoom.Domain/"]
RUN dotnet restore "src/MottaDevelopments.ChatRoom/MottaDevelopments.ChatRoom.Api.csproj"
COPY . .
WORKDIR "/src/src/MottaDevelopments.ChatRoom"
RUN dotnet build "MottaDevelopments.ChatRoom.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MottaDevelopments.ChatRoom.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MottaDevelopments.ChatRoom.Api.dll"]
