###Features
- Industry Standard Authentication Based On JWT hash token
- Aggegated Log Statistics Based On Elasticsearch
- Kibana Dashboard
- Registration And Login
- Alert Notice, Error and Exception
- Pagination
- CRUD Book
- Local Keyword Search
- API Versioning for API Gateway Service
- CQRS Architect Pattern For Scalablilty

### CQRS Architect
![image](https://drive.google.com/uc?export=view&id=1tnToN4C3DzzjWAOVI4WA26qM-fVWi-FO)


### Technical Points
- .Net Core & Visual Studio Code
>This is sort of technical trends. 
>Even though they have a bit incomvenience for debugging, unit test and so on, those are light and cheap. 
>This gives us and company a good oppotunity to have cost effective projects. 
>Furthermore technically, thesy are actually pretty fancy and rapidly glowing. 
>For example, you as a .Net developer can make some server modules running on IoT device based on Linux.

- Structure Of Module & Projects
>It was divided into as many as possible according to the role of each module. 
>In my experiences through several projects, seperation of concerns is the most important factor for maintenance with many different developers. Especially, I think controllers should not contain many logic in itself.

- Rest API
>So I used some readable words and names on them unlike Rest API CRUD conventions but it works well. :) 

- Elasticsearch
>When I received this assignment email, I decided to use Elasticsearch immediately. 
>As you may know, it has a lot of benefits for flexible search and aggregation and of course scalability. 

- Kibana Dashboard
>If someone choosed Elasticsearch, Kibana is next. No doubt. 
>Of course I considered using some other dashboard directly. But I had to select Kibana for practical use.

- Docker
>As a backend guy, I love Docker and Kubernetes.
>But for this project I need only docker to deploy ELK handy.

- MSSQL Linux
>A bit heavy and slow than other DBMS

- Angular 7
>One of fancy SPA front-end stacks

### Read Me
- Prerequisite
> - .Net Core 2.2.103
> - Docker 18.09.1
> - NodeJs 10.15.1
> - Npm 6.4.1
> - Angular CLI 7.2.3 

- How To Build
> On Angular project, run npm -i to install all dependencies.
> After that, you will be able to build.

- How To Run
> - Make sure the docker service running on your machine 
> - After above, Run the docker image that is in the root in branch following steps
>> - docker network create esnetwork --driver=bridge
>> - docker-compose up 'on the same folder of branch root'
>> - Elasticsearch and Kinbana will be downloaded and run.
> - After running Docker,Kibana,Elasticsearch, mssql linux rabbitMQ, you can run the main service module 'Web', 'Command.API' and 'Query.API' by running 'dotnet run'
> - After that, you can go to 'SPA' and run 'ng serve' and open 'http://localhost:4200'
> - Thats it!

- How To Use
> - SPA : http://localhost:4200/
> - Kibana Dashboard : http://localhost:5601/
>> - To use Kibana at first, you have to import one file which is in the root branch named 'kibana_dashboard_indexpattern.json'
	You can easily import it. Go to 'Management' menu and Open 'Save Objects' link and you can see 'Import' link button upper side.
	Then you can import the json file. And then you should make the default index pattern. Go to the 'Management' menu again.  
	Click 'Index Pattern' link button. you can see 'logstash' and 'route' which came from the json file already.
	Select 'logstash' and click 'Start' button. That's it. You will be able to one Dashboad.
>> - Of course you have to make sure the range of date for data and you can set it top side menu.
>> You can compare the dashboard data and 'Statistic' json page data which you can see on the 'SPA'.

###End