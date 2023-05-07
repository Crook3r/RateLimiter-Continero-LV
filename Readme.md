# Functionality

`RateLimitter` library represents **middleware**, which exist in middleware pipeline before *request-specific* middleware library that require business processing of the request header (Auth, CORS, etc.). 
The basic criteria how requests will be restricted is the **incoming IP address**, in another words all access limits will be applied based on IP adress from which request comes.

# Initialization and usage

For using `RateLimiter`, it must be first Initialized with `Initialize` method on `RateLimiter` class. For initialization is used `RateLimiterConfig` class, described below.

`CheckRequestIsAllowed` method of `RateLimiter` is used for checking, if the request is allowed. Its parameteres are `Endpoint`, `Ip Address` and `Timestamp`. `Endpoint` represents path to endpoint, which is subject for checking. `IpAddress` is 

# Configuration

For configuration, class `RateLimiterConfig` is used for limiting request to all endpoints:
|Parameter|	Description|	Value|
|---------| -----------|----------|
|RequestLimiterEnabled|	Includes rate limiter functionality|	boolean|
|DefaultRequestLimitMs| Default time frame on the number of requests for all endpoints|integer|
|DefaultRequestLimitCount|Limit on the consecutive number of requests for all endpoints|integer|	
| EndpointLimits|	Limit list for specific endpoint | List&lt;EndpointLimiterConfig&gt;?|

Class `EndpointLimiterConfig` represents configuration for specific endpoint, which is used in `EndpointLimits` property of `RateLimiterConfig`:
|Parameter|	Description|	Value|
|---------| -----------|----------|
Endpoint|	Specific endpoint line trace|	string
RequestLimitMs|	Default timeframe to the number of endpoint requests| integer|
RequestLimitCount|Limit on the number of consecutive requests in a time frame for an endpoint | integer|
  
**Configuration example:**

```
"RateLimiterConfig": {
  "RequestLimiterEnabled": true,
  "DefaultRequestLimitMs": 1000,
  "DefaultRequestLimitCount": 10,
  "EndpointLimiterConfig": [```
    {
      "Endpoint": "/api/products/books",
      "RequestLimitMs": 1000,
      "RequestLimitCount": 1
    },
    {
      "Endpoint": "/api/products/pencils",
      "RequestLimitMs": 500,
      "RequestLimitCount": 2
    }
  ]
} 
```

