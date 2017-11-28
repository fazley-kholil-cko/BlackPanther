# BlackPanther
## Starting docker using docker-compose.yml
``docker-compose up``


## Running the App
Build and run API client using the following:
URL: ``http://localhost:5000/api/charge``


Method: POST


Request:
```
{
  "email": "{{Email}}",
  "value": 700,
  "currency": "usd",
  "trackId": "Your tracker",  
    "name": "{{Name}}",
    "number": "4242424242424242",
    "expiryMonth": "06",
    "expiryYear": "2018",
    "cvv": "100"
    
  
}
```

Stream: ``in_api_requests``




## Viewing stream output
Build and run ``Kafka.Consumer`` to view in  ``Kafka.Consumer``
