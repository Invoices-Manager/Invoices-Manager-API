### POST /User
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |

##### sample-request:

> **POST /User**

##### Body:

```json
{
  "username": "UserName",
  "password": "UserPassword",
  "firstName": "FirstName",
  "lastName": "LastName",
  "email": "email@example.com"
}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 200,
    "traceId": "63a737bd-3934-4e23-ab8e-591423e2ff1b",
    "dateTime": "2023-04-28T19:33:41.5622968+02:00",
    "message": "The user was created successfully",
    "args": {
        "user": {
            "id": 9,
            "username": "UserNames",
            "password": "41f0SGc2P4vQAE8bG5nRX8FwBcY0lbHoBKZ5CG+fUTUKQpHu+PUOLCd/TEDl4NJjlV3vA6Aafw2XxtXXxSMKDQ==",
            "salt": "41f0SGc2P4vQAE8bG5nRX8FwBcY0lbHoBKZ5CG+fUTU=",
            "firstName": "FirstName",
            "lastName": "LastName",
            "email": "email@example.com",
            "isBlocked": false,
            "incorrectLoginAttempts": 0,
            "notebook": [],
            "backUpInfos": [],
            "invoices": [],
            "logins": []
        }
    }
}
```