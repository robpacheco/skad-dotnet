# Surviving Kubernetes as an Application Developer

This is the source code for the Manning liveVideo: Surviving Kubernetes as an Application Developer

## Project Layout

This section describes the layout of this repository.

* `Common` - A .NET project for all common classes shared across services
* `k8s` - Kubernetes YAML files
* `postgres` - SKAD PostgreSQL Docker files and scripts
* `VulnerabiltiyFeed` - The Vulnerability Feed service source and docker files
* `Subscription` - The Subscription service source and docker files
* `VulnerabilityFeedLoader` - Utility to load CVE data into the Vulnerability Feed

More services will be added as the project continues.

# Container Images

There are two container images that can be build: Postgres and Vulnerability Feed. In addition, it is often useful to run Postgres locally as a container in Docker while developing locally.

## Building the PostgreSQL Container Image

To build the Postgres container image: (starting from the root of the repo)

```
cd postgres/docker
docker build -f Dockerfile -t skad-postgres:latest .
```

## Removing Existing PostgreSQL Docker container

```
docker rm -f skad-pg
```

## Running PostgreSQL in Docker

Once the Postgres container image is built, it can be run in Docker and exposed locally on port 5432:

```
docker run --name skad-pg -d -p 5432:5432 -e POSTGRES_PASSWORD=secret skad-postgres:latest
```

Note that in this repository the password `secret` is often used. This is not a realistic password. Passwords should not be committed to repositories.

## Building the Vulnerability Feed Container Image

To build the Vulnerability Feed container image: (starting from the root of the repository)

```
docker build -f VulnerabilityFeed/docker/Dockerfile -t skad-vulnfeed:latest .
```

## Building the Subscription Container Image

To build the Subscription container image: (starting from the root of the repository)

```
docker build -f Subscription/docker/Dockerfile -t skad-subscription:latest .
```

## Running in Kubernetes

### Deploying from Kustomize

```
cd k8s/manifests
kubectl apply -k .
```

### Ensure pods are running

```
kubectl -n skad get pods
```

### Port-forward services to localhost

```
kubectl -n skad get svc
kubectl -n skad port-forward svc/vulnfeed 5000:80
kubectl -n skad port-forward svc/subscription 5002:80
```

### Port-Forward Ingress

```
sudo kubectl -n ingress-nginx port-forward svc/ingress-nginx-controller 80:80 
```