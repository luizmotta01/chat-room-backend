#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/contacts/MottaDevelopments.ChatRoom.Contacts.Api/MottaDevelopments.ChatRoom.Contacts.Api.csproj", "src/contacts/MottaDevelopments.ChatRoom.Contacts.Api/"]
COPY ["src/contacts/MottaDevelopments.ChatRoom.Contacts.Application/MottaDevelopments.ChatRoom.Contacts.Application.csproj", "src/contacts/MottaDevelopments.ChatRoom.Contacts.Application/"]
COPY ["src/common/MottaDevelopments.MicroServices.Application/MottaDevelopments.MicroServices.Application.csproj", "src/common/MottaDevelopments.MicroServices.Application/"]
COPY ["src/common/MottaDevelopments.MicroServices.Infrastructure/MottaDevelopments.MicroServices.Infrastructure.csproj", "src/common/MottaDevelopments.MicroServices.Infrastructure/"]
COPY ["src/common/MottaDevelopments.MicroServices.Domain/MottaDevelopments.MicroServices.Domain.csproj", "src/common/MottaDevelopments.MicroServices.Domain/"]
COPY ["src/contacts/MottaDevelopments.ChatRoom.Contacts.Infrastructure/MottaDevelopments.ChatRoom.Contacts.Infrastructure.csproj", "src/contacts/MottaDevelopments.ChatRoom.Contacts.Infrastructure/"]
COPY ["src/contacts/MottaDevelopments.ChatRoom.Contacts.Domain/MottaDevelopments.ChatRoom.Contacts.Domain.csproj", "src/contacts/MottaDevelopments.ChatRoom.Contacts.Domain/"]
RUN dotnet restore "src/contacts/MottaDevelopments.ChatRoom.Contacts.Api/MottaDevelopments.ChatRoom.Contacts.Api.csproj"
COPY . .
WORKDIR "/src/src/contacts/MottaDevelopments.ChatRoom.Contacts.Api"
RUN dotnet build "MottaDevelopments.ChatRoom.Contacts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MottaDevelopments.ChatRoom.Contacts.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MottaDevelopments.ChatRoom.Contacts.Api.dll"]
