# demo-wallet

## To run in Docker

```shell
docker compose -f docker-compose.yml up --detach
docker compose -f docker-compose-api.yml up --detach
```

The command in the first line creates dependency services. For the moment this is only **PostgreSQL** server.

The command in the second creates application services - Api application.

## To see API Swagger UI

After docker containers are built and run, please follow http://localhost:2588/swagger/index.html. This will open Wallet API Swagger UI page with description of API.

## Create test data

The first time you run you will need some test data as described in the task document. Please use `/v1/Test/populateTestData` endpoint for this purpose.

All endpoints and schemas are described in Swagger UI, pretty straight forward.

## Architecture

Applied design principles: DDD, CQRS, DI, API versioning

The solution contains several app layers:

* `Domain` - contains business logic, nothing more. Each object encapsulates bare minimum data to be functional and operations, and also checks which makes the data consistent. An aggregate here - `Player` object + all his `Transactions`
* `Database` - contains means to store domain objects in the database. Based on **EntityFrameworkCore**. DB reads and writes are allowed only inside transactions which have SNAPSHOT isolation by default. `IWalletRepository` is designed to be accessed only inside a transaction created by `IWalletDatabase`
* `Application` - contains CQRS pattern and services which perform queries and commands
* `Api` - contains controllers and API models which are mapped to/from `Application` classes (queries, commands and results) with the means of `AutoMapper`. Note that this API is considered *internal* and should not be publicly visible.

## Points of extension

* `Domain` can be easily extended with any new objects and business logic, which doesn't hide between different kinds of complexity
* When new domain objects are introduced, or old ones' structure is changed, all these changes can be included into `WalletDbContext` and migration can be created. All new migrations are automatically applied to the DB when the `Api` application starts. New migrations can be created with the command:

  ```shell 
  dotnet ef migrations add [MigrationName] -p src/Database/Database.csproj --msbuildprojectextensionspath artifacts/obj/Database -- [ConnectionString (optional)]
  ```

  If `[ConnectionString]` can be missing. In this case `dotnet ef` command will use development database defined in `docker-compose.yml`.
* In `Application`, existing CQRS abstractions and models allow to perform most of routine operations and make implementation and registration of new handlers easier.
* `Api` project already contains versioning support, thus it's already easy to extend existing controllers as well as create new versions of the API. New version will automatically appear in Swagger Specs and UI. Thanks to `AutoMapper` making new models for new operations is also easy. UnitTests automatically check whether mapper configuration is valid or not.
* The solution is ready for adding new microservices like 'Api'. For example it can be `Worker` or `ExternalApi` for publicly visible API.

## Unit Tests

I used:
* **Xunit** framework
* **FluentAssertions**
* **AutoFixture**
* **Moq** (was not actually used as there isn't enough tests, but it's nice to have it in place for new tests)
