### DEL /Note/GetAll
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **GET /Note/GetAll**

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
    "traceId": "f13dd95c-4ac3-4ff6-8e25-386204bfc8be",
    "dateTime": "2023-04-28T19:45:40.9458068+02:00",
    "message": "All notes",
    "args": {
        "notes": [
            {
                "id": 37,
                "name": "Name",
                "value": "Value",
                "creationDate": "2023-04-28T19:45:17.216055",
                "lastEditDate": "2023-04-28T19:45:36.63186"
            }
        ]
    }
}
```