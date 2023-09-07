### GET /Invoice/GetAll
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

> **GET /Invoice/GetAll**

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
    "traceId": "8adfa7ca-3a9d-4c12-bc97-7129ef7c089a",
    "dateTime": "2023-04-28T19:35:02.8899099+02:00",
    "message": "All invoices",
    "args": {
        "invoices": [
            {
                "id": 11,
                "fileID": "d1bf93299de1b68e6d382c893bf1215f",
                "captureDate": "2023-04-28T19:34:13.562046",
                "exhibitionDate": "2022-01-01T00:00:00",
                "reference": "Ref-123",
                "documentType": "PDF",
                "organization": "ABC Inc.",
                "invoiceNumber": "INV-123",
                "tags": [
                    "Tag1",
                    "Tag2"
                ],
                "tagsAsString": "Tag1;Tag2",
                "importanceState": 1,
                "moneyState": 1,
                "paidState": 1,
                "moneyTotal": "100"
            }
        ]
    }
}
```