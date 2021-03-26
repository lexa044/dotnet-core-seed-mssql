# dotnet-core-seed-mssql
Build a fully functional, cloud-ready, and professional Web Api using the latest features in the .NET Core 3.1 and Microsoft SQL Server as your database management system.

#### Features
- Fully async for better performance
- Using Dapper instead of Entity Framework
- Token Protection / Route Protection using security
- Pagination using dapper

### Why .NET Core 3.1 and not .NET Core 5.0 ?
Good question! There is a simple answer to that. At the time of writing .NET Core 3.1 holds the LTS (Long-term support) title. However, if you want to switch to .NET Core 5.0 just change your project settings.

### Getting started

#### Helpful links if you're new to Dotnet core
- [.NetCore](https://dotnet.microsoft.com/download)
- [Dapper](https://dapper-tutorial.net/dapper)
- [Swagger](https://swagger.io/)
- [Automapper](https://automapper.org/)

#### Get project running
- cd into cloned repo
- Run the script located at Scripts/database_scripts.sql
- Once database is created and schema has been generated you need to add a SQL Login and make sure it is granted permissions for read/write to DNSeed database.
- Open Visual Studio 2019, open Solution and build
- Otherwise you can run: dotnet restore
- Followed by: dotnet run
- If running in Development mode you need to edit appsettings.Development.json Make sure you update connectionString accordingly => SQL Login
- If running in Production mode you need to edit appsettings.Production.json Make sure you update connectionString accordingly => SQL Login

#### Get login working with your backend
- Once the site is running, it will load up Swagger endpoint, from here you can test login feature.
- The provided Scripts/database_scripts.sql file, adds a user: demo2 / demo2_Sup3rPwd
- Once logged in, grab the token and modify Swagger to use given token.
- Start adding/editing products.

