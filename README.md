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
1. Download the latest release (for linux or windows)
2. Extract the archive  

3. Open the `appsettings.json` file
4. Change the `DefaultConnection` to your database connection string
5. Change the `SymmetricSecurityKey` to your own SymmetricSecurityKey
6. Change the `Issuer` to your own Issuer
7. Change the `Audience` to your own Audience
8. Change the `Expiration` to your own Expiration (in minutes)  
9. Save and close the file  

10. Connect via shell to you Database-Server
11. Create a Database
12. use {databaseName}
13. Open the `SqlDb.sql` file and copy the content
14. Paste the content into your shell and execute it

12. Run the `InvoicesManagerAPI` file

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

## v1.0.1.0
- JWT are now saved hashed in the database

## v1.0.0.0
- Set Up the whole project
- Add JWT Authentication
- Add 3 Controllers (User, Invoice, Note)
- Add 17 endpoints
- Add Postman Collection
