
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
RUN mkdir -p /app/data

# Copy pre-published files from the publish folder
COPY . .

ENTRYPOINT ["dotnet", "ByBitNewListingsApp.dll"]