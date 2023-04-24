### GET /endpoint
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
| `parameter_name` | `string` | Description of parameter | No |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **GET /endpoint?parameter_name=example**

##### Body:

```json
{
	"response_key": "response_value"
}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
	"response_key": "response_value"
}
```