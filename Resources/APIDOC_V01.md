#  Invoices-Manager-API-v01 Documentation
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