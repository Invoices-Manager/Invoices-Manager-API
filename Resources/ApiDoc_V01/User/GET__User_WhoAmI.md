### GET /User/WhoAmI
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **GET /User/WhoAmI**

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
    "traceId": "c5dfaea5-b7c3-430f-a27c-e22df6019329",
    "dateTime": "2023-04-28T19:45:58.5235598+02:00",
    "message": "The user was found successfully",
    "args": {
        "userName": "UserNames",
        "email": "email@example.com",
        "firstName": "FirstName",
        "lastName": "LastName"
    }
}
```