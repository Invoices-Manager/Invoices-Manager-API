# Invoices Manager - API (written in C#   DOTNET6.0)

## Important Info!
The program may contain errors, if any are found, please report them!  

The server / api does not encrypt the data. 
It manages the data as it receives it, so you should encrypt the data before sending it to the api.

## Application description:
Are you also tired of having all your invoices (and other documents)
only on one PC? Although nowadays everything has a cloud. <br/>
Now it's over, so you can use your Invoices Manager as usual, which now  
has a cloud function, and this is the API you need to host it.

## Setup:
### Needed to start the API and manage the database afterwards.
1.) Install the latest version of [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

### Needed to manage the database.
2.) Install the `dotnet ef` tool via `dotnet tool install --global dotnet-ef`

### Just a test if everything is installed correctly.
3.) Run `dotnet ef` in your terminal, if you see an unicorn, everything is installed correctly.

### Is the source from the api
4.) Clone the repository `(from the main branch)`  (Recommended way if you want to simply update the API via Git)  
    or  
    download the source code in the release section  
    
### Is needed if you downloaded it from the release section
5.) Extract the archive and delete the archive

### Configure the Conf for the API
6.) Open the `appsettings.json` file

7.) Change the `DefaultConnection` to your database connection string

8.) Change the `SymmetricSecurityKey` to your own SymmetricSecurityKey

9.) Change the `Issuer` to your own Issuer

10.) Change the `Audience` to your own Audience

11.) Change the `Expiration` to your own Expiration (in minutes)  

12.) Save and close the file  

### Configure / Generate the database | This will generate all tables and columns in your database on the latest version
13.) Run `dotnet ef migrations add firstInit` in your terminal. (of course in the project folder)  
  
14.) Run `dotnet ef database update` in your terminal. (of course in the project folder)

### Start the API | This will start the api on port 5000 for all ip's
#### Change the port if you want to
#### --configuration <configuration mode>: Sets the configuration mode. Possible values are `Debug` or `Release`.
#### --verbosity <level>: Sets the level of detail of the console output. Possible values are `quiet`, `minimal`, `normal`, `detailed` and `diagnostic`.
15.) Run the `dotnet run --urls=http://0.0.0.0:5000 --configuration Release --verbosity minimal` to start the api.

## Features:
✔️ 100% free and open source  
✔️ Easy to use  
✔️ JWT Authentication  
✔️ Cloud function  
✔️ Easy to host  
✔️ Easy Documentation  
✔️ Postman Collection  
✔️ BackUp, Invoice, Note & User function  

## API documentation:
Press [here](https://github.com/Invoices-Manager/Invoices-Manager-API/blob/master/Resources/ApiDoc_V01/APIDOC_V01.md) to see the documentation


# CHANGELOG
## Version structure (X.Y.Z.W)
### X = Major version
### Y = Minor version (big updates)
### Z = Minor version (small updates)
### W = Revision version (bug fixes)

## v1.0.3.1
- The Salt will be send at the who am i query

## v1.0.3.0
- Ram usage improved
- Response time improved

## v1.0.2.1
- The Endpoint "GET /api/v01/User" (create user) now is an POST endpoint (C# does not allow a body in a get request)

## v1.0.2.0
- The API will now be started from the source code and not from the compiled version
- FIX: The user controller tried to find a hashed token with an unhashed one when logging off

    
## v1.0.1.0
- JWT are now saved hashed in the database

## v1.0.0.0
- Set Up the whole project
- Add JWT Authentication
- Add 3 Controllers (User, Invoice, Note)
- Add 17 endpoints
- Add Postman Collection
