### POST /Invoice
---

#### Query parameters:

| Parameter | Type | Description | Required |
| --- | --- | --- | --- |

#### Header:

| Parameter | Description | Required |
| --- | --- | --- |
| `bearerToken` | Your Bearer Token is needed here so that you can authenticate yourself. You received it when you logged in, it is a JWT. | Yes |

##### sample-request:

> **POST /Invoice**

##### Body:

```json
{
  "NewInvoice": {
    "CaptureDate": "2022-01-01T00:00:00Z",
    "ExhibitionDate": "2022-01-01T00:00:00Z",
    "Reference": "Ref-123",
    "DocumentType": "PDF",
    "Organization": "ABC Inc.",
    "InvoiceNumber": "INV-123",
    "Tags": [
      "Tag1",
      "Tag2"
    ],
    "ImportanceState": 1,
    "MoneyState": 1,
    "PaidState": 1,
    "MoneyTotal": 100.00
  },
  "InvoiceFileBase64": "SGFsbG8="
}
```

#### Successful answer:

*HTTP/1.1 201 CREATED
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 201,
    "traceId": "a7b6601d-dd55-4e7b-81ba-f2322ad0fcd1",
    "dateTime": "2023-04-28T19:34:13.6248898+02:00",
    "message": "Invoice created successfully",
    "args": {
        "invoice": {
            "id": 11,
            "fileID": "d1bf93299de1b68e6d382c893bf1215f",
            "captureDate": "2023-04-28T19:34:13.5620468+02:00",
            "exhibitionDate": "2022-01-01T00:00:00Z",
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
            "moneyTotal": 100
        }
    }
}
```