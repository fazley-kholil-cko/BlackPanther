# BlackPanther
## Starting docker using docker-compose.yml
``docker-compose up``


## Running the App
Build and run API client using the following:
URL: ``http://localhost:5000/api/charge``


Method: POST


Request:
```json
{
  "email": "{{Email}}",
  "value": 700,
  "currency": "usd",
  "trackId": "Your tracker",  
  "name": "John Doe",
  "number": "4242424242424242",
  "expiryMonth": "06",
  "expiryYear": "2018",
  "cvv": "100"
}
```

TransactionRequest :

```csharp
 public class TransactionRequest
    {
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "trackId")]
        public string TrackId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "expiryMonth")]
        public string ExpiryMonth { get; set; }

        [JsonProperty(PropertyName = "expiryYear")]
        public string ExpiryYear { get; set; }

        [JsonProperty(PropertyName = "cvv")]
        public string Cvv { get; set; }
    }
```

IN Topic: ``in_api_requests``

## Request Validator
Validate incoming request from ``in_api_requests``
OUT Topic: ``out_risk``
OUT ERROR Topic: ``out_response``

Request:
```
{
  "email": "{{Email}}",
  "value": 700,
  "currency": "usd",
  "trackId": "Your tracker",  
  "name": "John Doe",
  "number": "4242424242424242",
  "expiryMonth": "06",
  "expiryYear": "2018",
  "cvv": "100"
}
```

Response:
```
{
  "email": "{{Email}}",
  "value": 700,
  "currency": "usd",
  "trackId": "Your tracker",  
  "name": "John Doe",
  "number": "4242424242424242",
  "expiryMonth": "06",
  "expiryYear": "2018",
  "cvv": "100"
}
```

OUT Error Response:

```
{
	"errorMessage": "System Error"
}
```


## Viewing stream output
Build and run ``Kafka.Consumer`` to view in console.
