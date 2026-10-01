# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Directory.Build.props CoreBlog.sln ./
COPY BE/BE.csproj BE/
COPY DAL/DAL.csproj DAL/
COPY BLL/BLL.csproj BLL/
COPY CoreBlog/CoreBlog.csproj CoreBlog/
RUN dotnet restore CoreBlog/CoreBlog.csproj
COPY . .
RUN dotnet publish CoreBlog/CoreBlog.csproj -c Release -o /app --no-restore

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Production \
    Demo__Enabled=true \
    Demo__ResetOnStartup=true
EXPOSE 8080
# Render setzt $PORT – lokal wird 8080 verwendet
CMD ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet CoreBlog.dll"]
