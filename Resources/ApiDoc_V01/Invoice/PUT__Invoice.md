### PUT /Invoice
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

> **PUT /Invoice**

##### Body:

```json
{
    "id": 11,
    "captureDate": "2023-04-28T19:21:14.4365823+02:00",
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
    "moneyTotal": "10000"
}
```

#### Successful answer:

*HTTP/1.1 200 OK
Content-Type: application/json*

##### Response:

```json
{
    "statusCode": 200,
    "traceId": "b132d025-f9dc-4802-a7d4-cc62b1353a28",
    "dateTime": "2023-04-28T19:44:57.3687253+02:00",
    "message": "The invoice was successfully edited",
    "args": {
        "invoice": {
            "id": 11,
            "fileID": "d1bf93299de1b68e6d382c893bf1215f",
            "captureDate": "2023-04-28T19:34:13.562046",
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
            "moneyTotal": "10000"
        }
    }
}
```