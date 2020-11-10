# IP and Domain Name Look Up (IpDLookUp)

This is a sample project that takes an address (Domain Name or IPv4) 
and queries various services to find information about the address and 
exposes that information via a Swagger documented REST API. 

## Tech Stack
1. .NET Core 3.1 
    - Web API and Class Library
    - NUnit for Testing
1. Docker and Docker compose
    - Nginx load balancer
1. Digital Ocean/Cloudflare

## Services Used
1. GeoIP via [FreeGeoIP](https://freegeoip.app/)
1. RDAP via [OpenRDAP](https://openrdap.org/api)
1. ReverseDNS
1. SSL Labs Report via [SSL Labs](https://github.com/ssllabs/ssllabs-scan/blob/master/ssllabs-api-docs-v3.md)
    - Note these reports can take some time to generate
1. Ping

## Demo
![Video of API](https://media.calebukle.com/uploads/2020/11/wk-5rbp1wNP2Y.gif)
 
[Try it yourself](https://ipd.calebukle.com/swagger)

## Design

There are two apis in this solution.
1. [IpDLookUp.Core](IpDLookUp.Core) (aka Core)
1. [IpDLookUp.Worker](IpDLookUp.Worker) (aka Worker(s))

These two APIs share a class library [IpDLookUp.Services](IpDLookUp.Services) (aka Service Library)

The Service library contains all the logic querying the services. It also exposes the Types/Models needed for each API.

Core is a wrapper API for the most part. It takes a list of services and for each service sends a request to the Worker API(s). Core is also responsible for taking the `IServiceResult<T>` and translating into an `IAppResult` for the client to consume.

The Worker API takes in a single `ServiceType` and performs the query and returns that data. Essentially figures out what service model to use and calls the Service Library. The Worker API is meant to be separated out to allow scalability/Background processing. You can trace which worker does which request via the `WorkerId` property on each queried service

### Deployment/Hosting
I have deployed this project via Digital Ocean across 2 _droplets_. 1 for the Core API. 1 for the Worker API. Both running Docker. I have set up pipeline via github actions to test the app, then build inside a docker image and push it to dockerhub. I then have docker compose files on each machine that I use to run the applications. You can see the `docker-compose.yml` files below

The Core API is a single container `barbadosclemens/ipd-core:latest` that listens on port 80

The Worker API is 4 containers, 
- nginx load balancer/proxy
- Worker API 1-3 `barabdosclemens/ipd-worker:latest`

```yml
# core api docker-compose.yml
version: '3'
services:
    core:
        image: barbadosclemens/ipd-core:latest
        ports:
            - 80:80
        environment:
            WORKER_ADDRESS: https://ipd-workers-pool.calebukle.com/api/worker
        restart: unless-stopped
```

```yml
# worker api docker-compose.yml
version: '3'
services:
    worker:
        image: barbadosclemens/ipd-worker:latest
        networks:
            - ipd-workers
            # deploy: replicas no longer works
            # use docker-compose up --scale worker=3 -d
        restart: unless-stopped

    nginx:
        image: nginx:latest
        networks:
            - ipd-workers
        ports:
            - 80:80
        volumes:
            - ./nginx.conf:/etc/nginx/nginx.conf
        restart: unless-stopped

networks:
    ipd-workers:
```

```conf
# nginx.conf
events {}
http {
    upstream ipd-worker-pool {
        server ipd-workers_worker_1;
        server ipd-workers_worker_2;
        server ipd-workers_worker_3;
    }

    server {
        listen 80;
        listen [::]:80;
        
        gzip on;
        gzip_types text/plain text/xml text/css
                          text/comma-separated-values
                          text/javascript application/x-javascript
                          application/atom+xml application/json;
        
        location / {
            proxy_pass http://ipd-worker-pool;
        }
    }
}
```

You'll notice the configs only listen on port 80, that's because I didn't include an cert to use. It felt slightly beyond the scope of what I was trying to accomplish initially. But I did proxy the requests via Cloudflare, so the end user still see https to cloudflare. 

## Local Development
1. git clone
1. update [IpDLookUp.Core launchSettings.json](IpDLookUp.Core/Properties/launchSettings.json) to point to https://localhost:6001/api/worker in the WORKER_ADDRESS environment variable (or whatever address you're going to run the worker(s) on)
1. Run the Worker Project
1. Run the Core project
1. Interact via the swagger page from the Core Project

## Challenges
The hardest part about this project is making the workers distributed or in their own process.

Initially I built it all into one solution as a Proof of Concept once that worked I refactored out into a worker api and service class library. But when doing that causes all kinds of headaches with deserializing data. Which required me to use Generic and switch on the type of service being queried. Before I was just using  `List<IServiceResult>` where `Data` was type `object`. 

That generic change cascaded through out the code which makes certain parts of the code not as clean as I'd like.

## Ways to Improve
Try to reduce the pieces of code that need to know about the generic model, which would clean up a lot of areas of the code. 

Stream the results back over websockets otherwise everyone will be waiting on SSL Labs report. 

Better error handling to provide user friendly error messages instead of stack traces.

Add HTTPS on the apis themselves, right now I'm just proxying via cloudflare


## Time Spent

The time tracking software used didn't seem to track initial work done on a different machine. (Those pesky firewalls). With that time added in, I spent roughly 16-17 hours total on this project. Including Live testing/Deployment

![WakaTime Report](https://media.calebukle.com/uploads/2020/11/wk-9djhbT289S.png)
