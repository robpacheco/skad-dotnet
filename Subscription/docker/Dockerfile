FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 AS build-env
WORKDIR /app

COPY Common ./Common
COPY Subscription ./Subscription

RUN dotnet restore Subscription/Subscription.csproj
RUN dotnet publish Subscription/Subscription.csproj -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.14

WORKDIR /opt/skad/
RUN mkdir app 
    
COPY --from=build-env /app/published app/
COPY Subscription/docker/docker-entrypoint.sh bin/docker-entrypoint.sh

# Run dos2unix for builds on Windows where the line endings might not match unix line endings.
# This will cause the entry points to fail.
RUN cd bin && \
    chmod 700 docker-entrypoint.sh && \
    dos2unix docker-entrypoint.sh

EXPOSE 80
    
ENTRYPOINT ["bin/docker-entrypoint.sh"]

