### GET /Invoice/GetFile
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
| `Id` | `int` | This is the ID of the Invoice, you get it from "GetAll" | Yes |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **GET /Invoice/GetFile?id=11**

##### Body:

```json
{

}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 200,
    "traceId": "6c1290ad-a942-454c-b6f3-303cc8797312",
    "dateTime": "2023-04-28T19:38:49.4919188+02:00",
    "message": "The invoice",
    "args": {
        "base64": "SGFsbG8="
    }
}
```