# DemoEFCoreConsole

It's a demonstation projet using Entity Framework Core in a console project, with configuration.

It uses SQLite to be portable. Below some notes about what's required and how to build a similar projet *from scratch*.

The projet does nothing interesting by itself. It's a kind of benchmark program, but without all the rigor required to do a serious one. It's juste a pretext.

There is no dependency injection like in an ASP NET Core project. It's not useful here, neither for the demonstration nor to execute anything.

The current version uses .NET Core and EF Core 2.1 RC1.


## Required package for a new projet

For EF Core (with SQLite) add two packages. The `Microsoft.EntityFrameworkCore.Design` is only useful if you need to use EF Core CLI :

```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite -v 2.1.0-rc1-final
dotnet add package Microsoft.EntityFrameworkCore.Design -v 2.1.0-rc1-final
```

For this example I also use a configuration file. It's not mandatory for EF Core, but as it will often be used in real projet, and does not really add complexity, I prefer include it in this demo.

The package `Microsoft.Extensions.Configuration.EnvironmentVariables` allows the use of `AddEnvironmentVariables` in the configuration builder (see `ConfigurationFactory` class). Both could be removed as they're not really used in this projet, but this feature is here just to have a production similar builder.

```
dotnet add package Microsoft.Extensions.Configuration -v 2.1.0-rc1-final
dotnet add package Microsoft.Extensions.Configuration.Json -v 2.1.0-rc1-final
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables -v 2.1.0-rc1-final
dotnet add package Microsoft.Extensions.Configuration.Binder -v 2.1.0-rc1-final
```


## Change in the project file

This content was manually added to the `DemoEFCoreConsole.csproj` file. Unless this configuration files will not be copied into the build directory.

```
  <ItemGroup>
    <None Update="appsettings.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>

  <ItemGroup Condition="'$(Configuration)' == 'Debug'">
    <None Update="appsettings.*.json" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
```


## Interesting code parts

In the `DataLayer` folder you have three EF related classes :

* `MyContext` is the `DbContext`, although it's the heart of any EF Core usage, there is nothing special here for anybody already knowing EF bits.

* `MyTable` is the single model.

* `MyContextFactory` is more interesting. It implements the `IDesignTimeDbContextFactory` allowing EF Core CLI to create the `MyContext` instance when needed. In this example it fetches the configuration like a normal program execution : when you manage migration from the CLI, the database connection string is automatically retrieved.


## Migrations

The `Migration` folder and its content was created with the EF Core CLI tool after writing the `DbContext` part and the model `MyTable`. You can remove all its content and recreate it with :


```
dotnet ef migrations add CreateMyTable
```

The program will create the database and apply migrations on startup, but you can also do it manually with this command (the DB information will be read from the configuration file) :

```
dotnet ef database update
```
