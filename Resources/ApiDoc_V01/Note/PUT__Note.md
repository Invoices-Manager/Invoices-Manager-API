### PUT /Note
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **PUT /Note**

##### Body:

```json
{
    "id": 37,
    "name": "MyNote",
    "value": "Hello"
}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 200,
    "traceId": "0f552354-931b-4bed-83d3-9fcdab3be1d1",
    "dateTime": "2023-04-28T19:45:36.674221+02:00",
    "message": "The note was updated",
    "args": {
        "note": {
            "id": 37,
            "name": "MyNote",
            "value": "Hello",
            "creationDate": "2023-04-28T19:45:17.216055",
            "lastEditDate": "2023-04-28T19:45:36.6318609+02:00"
        }
    }
}
```