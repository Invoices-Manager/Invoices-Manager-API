### GET /Note
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

> **GET /Note?id=38**

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
    "traceId": "01908bce-3aee-464e-bbd4-594747161263",
    "dateTime": "2023-04-28T19:45:25.179225+02:00",
    "message": "the note with the id: 38 ",
    "args": {
        "note": {
            "id": 38,
            "name": "MyNote",
            "value": "Hello",
            "creationDate": "2023-04-28T19:45:17.216055",
            "lastEditDate": "2023-04-28T19:45:17.216162"
        }
    }
}
```
