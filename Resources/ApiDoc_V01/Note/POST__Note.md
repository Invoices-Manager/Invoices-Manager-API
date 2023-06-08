### POST /Note
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
|  |  |  |  |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **POST /Note

##### Body:

```json
{
    "Name": "MyNote",
    "Value": "Hello"
}
```

#### Successful answer:

*HTTP/1.1 201 CREATED
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 201,
    "traceId": "3ddcdec3-45b0-4149-adac-98b5e1118c31",
    "dateTime": "2023-04-28T19:45:45.3278068+02:00",
    "message": "Note created successfully",
    "args": {
        "note": {
            "id": 38,
            "name": "MyNote",
            "value": "Hello",
            "creationDate": "2023-04-28T19:45:45.2101015+02:00",
            "lastEditDate": "2023-04-28T19:45:45.2101039+02:00"
        }
    }
}
```
