### DEL /User/LogoutEverywhere
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

> **DEL /User/LogoutEverywhere**

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
    "traceId": "f3b13c0c-551c-401c-b08d-6ce59508c241",
    "dateTime": "2023-04-28T19:46:34.659284+02:00",
    "message": "All logins were deleted successfully",
    "args": {
        "loginCounts": 11,
        "userName": "UserNames",
        "email": "email@example.com"
    }
}
```