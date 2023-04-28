### DEL /User/Logout
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

> **DEL /User/Logout**

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
    "traceId": "909d32f7-7c43-4382-a5c2-6e8dfe71eacc",
    "dateTime": "2023-04-28T19:46:17.4003552+02:00",
    "message": "The logout was successful",
    "args": {
        "userName": "UserNames",
        "email": "email@example.com",
        "bearerToken": "*TOKEN*"
    }
}
```