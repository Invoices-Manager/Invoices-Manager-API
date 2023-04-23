# Invoices Manager - API (written in C#   DOTNET6.0)

## Important Info!
The program may contain errors, if any are found, please report them!


## Application description:
Are you also tired of having all your invoices (and other documents)
only on one PC? Although nowadays everything has a cloud. <br/>
Now it's over, so you can use your Invoices Manager as usual, which now  
has a cloud function, and this is the API you need to host it.


## Features:
✔️ 100% free and open source  
✔️ Easy to use
✔️ JWT Authentication
✔️ Cloud function
✔️ Easy to host
✔️ Easy Documentation
✔️ Postman Collection
✔️ BackUp, Invoice, Note & User function


##  Invoices-Manager-API-v01 Documentation
This is the documentation for the REST API of the Invoices Manager, here you can see all the important information you need about this API.  
#### Your main endpoint looks like this: ```{ip-address:port}/api/v01```

## The following endpoints are listed here:
- Invoice
    - GET /Invoice/GetAll
    - GET /Invoice
    - POST /Invoice
    - PUT /Invoice
    - DEL /Invoice
- Note
    - GET /Note/GetAll
    - GET /Note
    - POST /Note
    - PUT /Note
    - DEL /Note
- User
    - GET /User/WhoAmI
    - POST /User
    - DEL /User
    - GET /User/Login
    - DEL /User/Logout
    - DEL /User/LogoutEverywhere

### GET /endpoint

#### Query parameters:
| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
| `parameter_name` | `string` | Description of parameter | No |

#### Header:
| Parameter | Description | Required |
| --- | --- | --- |
| `parameter_name` | Description of parameter | No |

##### sample-request:
GET /endpoint?parameter_name=example

##### Body:
```json
{
    "response_key": "response_value"
}
```

#### Successful answer:
HTTP/1.1 200 OK
Content-Type: application/json

##### Response:
```json
{
    "response_key": "response_value"
}
```

#### Error Responses:
| Status code | Description |
| --- | --- |
| `400` | Description of error |
| `401` | Description of error |
| `404` | Description of the error |




# CHANGELOG
## Version structure (X.Y.Z.W)
### X = Major version
### Y = Minor version (big updates)
### Z = Minor version (small updates)
### W = Revision version (bug fixes)


## v1.0.0.0
- Set Up the whole project
- Add JWT Authentication
- Add BackUp function
- Add over 20 endpoints
- Add Postman Collection
