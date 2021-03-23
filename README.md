# dotnet-core-seed-mssql
A jumping off point for building a Web Api using dotnet core 3.1 along with Microsoft SQL Server

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
- Do not forget to insert a record into table Authorization. Choose any loginname and password
- Open Visual Studio 2019, open Solution and build
- Otherwise you can run: dotnet restore
- Followed by: dotnet run
- If running in Development mode you need to edit appsettings.Development.json Make sure you update connectionString accordingly

#### Get login working with your backend
- Once the site is running, it will load up Swagger endpoint, from here you can test login feature.
- Once logged in, grab the token and modify Swagger to use given token.
- Start adding/editing products.

#### Features
- Fully async for better performance
- Using Dapper instead of Entity Framework
- Token Protection / Route Protection using security
- Pagination using dapper
