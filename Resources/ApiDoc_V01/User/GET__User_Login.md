### GET /User/Login
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
|  |  |  |  |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
|  |  |  |

##### sample-request:

> **GET /User/Login**

##### Body:

```json
{
    "username": "UserName",
    "password": "UserPassword"
}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 200,
    "traceId": "cc0a4a65-19e8-46f3-883a-448ea9bc7865",
    "dateTime": "2023-04-28T19:46:40.8815406+02:00",
    "message": "The login was successful",
    "args": {
        "token": "*TOKEN*",
        "creationDate": "2023-04-28T19:46:40.7806409+02:00",
        "userName": "UserName"
    }
}
```