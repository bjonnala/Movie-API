# Movie-API

MovieAPI lets one to search through movie catalog, signin and signup accounts, update their profile and allows to rent movies on demand.

## API Documentation URL: [Documentation](https://documenter.getpostman.com/view/3412451/movieapi/7Ln9hd1#93f569de-2ba8-ccd2-dde5-3726fcafb525)

## Getting Started

Once Movie-API is downloaded from github onto local-machine, be sure to change to replace the base url for API as per your localhost URL (when run in Visual Studio locally).
On deployment to any other environment, either staging or production base url needs to be updated.

## Database connection string in MovieAPI web.config file needs to be updated accordingly.

```
Connection string MovieEntities needs to replaced with SQL server credentials.
```

```
For instance, on my local machine it was set to http://localhost:56020/ when the solution ran in my local machine Visual Studio.
```

## Technical Writeup - Architecture

Movie API project is created as Layered architecture. It has layers as follows:

* MovieAPI: This is main api project which uses the below layers(WebAPI.IBLL,WebAPI.BLL,WebAPI.Data,WebAPI.Data) as dependencies.This project contains 
   ⋅⋅* Controllers
   ⋅⋅* Custom Authorize Attributes(Validate Authorize Key)
   ⋅⋅* Custom Validators
   ⋅⋅* Ninject configuration
   ⋅⋅* Request Logger
* WebAPI.IBLL Layer: This acts as a intermediate/mediator layer between controller and Data Access layer,hence plays an important role in creating loosly coupled application.
This layer mainly contains interfaces  where the abstract methods are declared.

* WebAPI.BLL Layer:This layer contains classes which is mainly used to implement all the interface methods in WebAPI.IBLL.
This layer can be considered as the Data Access layer of the project.This contains all the data quering techniques to interact directly with database.

* WebAPI.Data: This layer mainly contains Edmx files. In this Layer entity framework DLL is added.

* WebAPI.Model: This contain all the models.
 


## Tools/Packages/Patterns used to build API

* Visual Studio - ASP.NET webapi
* [Ninject](http://www.ninject.org/) -  Dependency Management - constructor injection
* [Postman](https://www.getpostman.com/) - Used in API development
* SQL Server (SSMS) for data storage.
* JWT (JSON Web Token) Library for generating authkeys.
* Fluent Validation Library for validating purposes.
* Entity framework ORM (Database First)
* Repository design pattern



