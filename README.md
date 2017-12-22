# Movie-API
MovieAPI lets one to search through movie catalog, signin and signup accounts, update their profile and allows to rent movies on demand.

## API Documentation URL: [Documentation](https://documenter.getpostman.com/view/3412451/movieapi/7Ln9hd1#)

## Getting Started
Once Movie-API is downloaded from github onto local-machine, be sure to change to replace the base url for API as per your localhost URL (when run in Visual Studio locally).

On deployment to any other environment, either staging or production base url needs to be updated.
```
For instance, on my local machine it was set to http://localhost:56020/
```

## Login Credentials
#### User Role 
* email: testuser1@gmail.com
* password: Password1$

#### Admin Role
* email: admin@admin.com
* password: Password1

## Database connection string in MovieAPI web.config file needs to be updated accordingly.
* movies.bak is the database backup file added to the project.
```
Connection string MovieEntities needs to replaced with SQL server credentials.
```

## Technical Writeup - Architecture

Movie API project is created as Layered architecture. It has layers as follows:

* MovieAPI: This is main api project which uses the below layers(MovieAPI.IBLL,MovieAPI.BLL,MovieAPI.Data,MovieAPI.Data) as dependencies.This project contains 
   ⋅⋅* Controllers
   ⋅⋅* Custom Authorize Attributes(Validate Authorize Key)
   ⋅⋅* Custom Validators
   ⋅⋅* Ninject configuration
   ⋅⋅* Request Logger
* MovieAPI.IBLL Layer: This acts as a intermediate/mediator layer between controller and Data Access layer,hence plays an important role in creating loosly coupled application.
This layer mainly contains interfaces  where the abstract methods are declared.

* MovieAPI.BLL Layer:This layer contains classes which is mainly used to implement all the interface methods in MovieAPI.IBLL.
This layer can be considered as the Data Access layer of the project.This contains all the data quering techniques to interact directly with database.

* MovieAPIAPI.Data: This layer mainly contains edmx file. In this Layer entity framework DLL is added.

* MovieAPIAPI.Model: This contain all the models.
 

 
## Tools/Packages/Patterns used to build API

* Visual Studio - ASP.NET webapi
* [Ninject](http://www.ninject.org/) -  Dependency Management - constructor injection
* [Postman](https://www.getpostman.com/) - Used in API development
* SQL Server (SSMS) for data storage.
* JWT (JSON Web Token) Library for generating authkeys.
* Fluent Validation Library for validating purposes.
* Entity framework ORM (Database First)
* Repository design pattern
* I am pretty much familiar with asp.net identity for membership layer but for this project made custom membership layer.

