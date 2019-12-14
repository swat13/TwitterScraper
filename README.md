# TwitterScraper

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them

```
Instal MongoDB and run its service.
```

### Installing

Download the project and go to the following directory 
```
TwitterScraper\Host\bin\Debug
```
And run the Host.exe file as Admin 

## Using the api

As long as the .exe file is open you can send request to the following url
```
http://localhost:8080/twitter/GetHotTopicsCount -Get method
```
And the sample result would be like this
```
[
    {
        "dailyCountList": [
            {
                "count": 0,
                "date": "2019-12-14"
            },
            {
                "count": 100,
                "date": "2019-12-13"
            }
        ],
        "id": "3f8e94c0-8167-4264-a40c-7c8bfaebbc49",
        "nextResultUrl": "?max_id=1205619958346125313&q=Trump&count=100&include_entities=1&result_type=recent",
        "topicType": 0
    },
    {
        "dailyCountList": [
            {
                "count": 0,
                "date": "2019-12-14"
            }
        ],
        "id": "07649b8d-8edc-408d-afe1-cb6cd98e2fa0",
        "nextResultUrl": "?q=ISIS&count=100&include_entities=1&result_type=recent",
        "topicType": 2
    },
    {
        "dailyCountList": [
            {
                "count": 0,
                "date": "2019-12-14"
            }
        ],
        "id": "e72fa4be-10e5-41e4-bdf5-6ca10bf9c7e3",
        "nextResultUrl": "?q=Lay%20Gaga&count=100&include_entities=1&result_type=recent",
        "topicType": 1
    },
    {
        "dailyCountList": [
            {
                "count": 0,
                "date": "2019-12-14"
            }
        ],
        "id": "18ae35a4-9e01-4849-80a0-d9f392877436",
        "nextResultUrl": "?q=Esports&count=100&include_entities=1&result_type=recent",
        "topicType": 3
    }
]
```

