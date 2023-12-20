# Chirp!

Chirp! is a social media web app developed for the "Analysis, Design and Software Architecture (Autumn 2023)" at the IT-University of Copenhagen. The web app can be accessed at: https://bdsagroup23chirprazor.azurewebsites.net/

## How to make Chirp! work locally

Prerequisites: Microsoft .Net 7.0 and Docker

To make Chirp! work locally, first you must clone the repository:

```
git clone https://github.com/ITU-BDSA23-GROUP23/Chirp.git
```

On windows or osx, make sure that the docker desktop application is running first.
On linux systems, ensure the Docker daemon is running. It can be started with:

```
sudo dockerd
```

From here, you must first start an MSSQL docker container using one of the following commands:

On Linux:

```
sudo docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DhE883cb" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest
```

On Windows or Mac:

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DhE883cb" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/mssql/server:2022-latest
```

On Mac-M1/M2:

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=DhE883cb" \
   -p 1433:1433 --name sql1 --hostname sql1 \
   -d \
   mcr.microsoft.com/azure-sql-edge
```

Next, from the root directory in /Chirp, run the following command:

```
dotnet run --project src/Chirp.Web
```

Alternatively, from the /Chirp.Web folder:

```
dotnet run
```

Finally, open your browser of choice and connect to `https://localhost:7040`
