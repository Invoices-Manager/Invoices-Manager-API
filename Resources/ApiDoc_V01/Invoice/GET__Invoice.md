### GET /Invoice
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |
| `Id` | `int` | This is the ID of the Invoice, you get it from "GetAll" | Yes |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **GET /Invoice?id=11**

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
    "traceId": "c902883a-2f08-46c4-b880-f73172fc9b94",
    "dateTime": "2023-04-28T19:38:37.5707361+02:00",
    "message": "The invoice",
    "args": {
        "result": {
            "invoice": {
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
            },
            "base64": "SGFsbG8="
        }
    }
}
```