### DEL /Invoice
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

> **DEL /Invoice?Id=10**

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
    "traceId": "6c831c7f-6241-4c27-9597-6f51d2da0473",
    "dateTime": "2023-04-28T19:28:47.6065129+02:00",
    "message": "The invoice was successfully deleted",
    "args": {
        "invoices": {
            "id": 10,
            "fileID": "d1bf93299de1b68e6d382c893bf1215f",
            "captureDate": "2023-04-28T19:21:14.436582",
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
            "moneyTotal": 10000
        }
    }
}
```
