### DEL /Note
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
| `Id` | `int` | This is the ID of the Note, you get it from "GetAll" | Yes |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **DEL /Note?id=11**

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
    "traceId": "63085970-5b6c-4d9a-b4f8-b01dcf7c614d",
    "dateTime": "2023-04-28T19:45:50.4854454+02:00",
    "message": "The note was deleted",
    "args": {
        "note": {
            "id": 38,
            "name": "Name",
            "value": "Value",
            "creationDate": "2023-04-28T19:45:45.210101",
            "lastEditDate": "2023-04-28T19:45:45.210103"
        }
    }
}
```