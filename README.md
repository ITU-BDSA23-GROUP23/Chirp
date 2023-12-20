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

## How to run the test suite locally

To run the test suites locally, first you will have to start your docker container, as described above in the "How to make Chirp! work locally" section.

Next, open up a terminal in the project. Assuming you are in the root of the repository Chirp, direct to either:

```bash
cd Test/Chirp.Razor.Tests
```

or

```bash
cd Test/UITest/PlaywrightTests
```

In both the Chirp.Razor.Tests and PlaywrightTests folder, to run the tests:

```bash
dotnet test
```

The project contains two test suites, Chirp.Razor.Tests and UITest.
The first test suite contains unit tests and integration tests.
The unit tests are testing the functionality of the isolated components in our application, that is testing methods within our application of core, infrastructure and web components. <!-- Tror ikke vi har unit tests af web components. -->
The integration tests are testing the interactions of different components in our application, that is testing when using logic from e.g. the infrastructure layer in our web components.

The second test suite contains our UI tests. These are UI automation tests, using Playwright to simulate a users interactions with the user interface. These are implemented such that we can ensure that the UI behaves as expected, performing actions and receiving expected output, when doing all types of interactions with our application from the UI. Before being able to run the test the program has to be running on the same local machine.