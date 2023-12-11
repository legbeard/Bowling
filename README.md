# Bowling
This repository contains a simple implementation of 10-pin bowling.

## Project structure
The repository contains a single solution, containing 3 projects.

- Bowling - Class library containing the primary logic
- Bowling.Console - Console Application allowing you to score a game of 10-pin bowling
- Bowling.Test - Test suite

## Running Locally
The project should run entirely out of any .NET IDE, provided you already have at least the .NET 6 SDK installed. From here, you can either run `Bowling.Console` or run the tests in `Bowling.Test`.

Alternatively, if you do not have an IDE, you can run the console app in a terminal from the `/Bowling.Console` subdirectory using:
```bash
dotnet run
```

or run the tests fom the `/Bowling.Test` subdirectory using:
```bash
dotnet test
```