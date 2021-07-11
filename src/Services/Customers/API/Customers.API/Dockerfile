#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Customers/API/Customers.API/Customers.API.csproj", "src/Services/Customers/API/Customers.API/"]
COPY ["src/Services/Customers/Data/Customers.Data/Customers.Data.csproj", "src/Services/Customers/Data/Customers.Data/"]
COPY ["src/Services/Customers/Domain/Customers.Domain/Customers.Domain.csproj", "src/Services/Customers/Domain/Customers.Domain/"]
COPY ["src/BuildingBlocks/SharedBase/Common.Base.Shared/Common.Base.Shared.csproj", "src/BuildingBlocks/SharedBase/Common.Base.Shared/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Common.Shared/EventBus.Common.Shared.csproj", "src/BuildingBlocks/EventBus/EventBus.Common.Shared/"]
COPY ["src/Services/Customers/Services/Customers.Services/Customers.Services.csproj", "src/Services/Customers/Services/Customers.Services/"]
COPY ["src/BuildingBlocks/ViewsAndDtos/Customers.ViewsAndDtos/Customers.ViewsAndDtos.csproj", "src/BuildingBlocks/ViewsAndDtos/Customers.ViewsAndDtos/"]
COPY ["src/BuildingBlocks/SharedBase/Common.Shared.Services/Common.Shared.Services.csproj", "src/BuildingBlocks/SharedBase/Common.Shared.Services/"]
RUN dotnet restore "src/Services/Customers/API/Customers.API/Customers.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Customers/API/Customers.API"
RUN dotnet build "Customers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.API.dll"]
