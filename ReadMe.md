
# DDD Sample Project

This project has been prepared to set an example for the project structure to be developed with the DDD concept.

You can read more information about structure here:
https://medium.com/@ademcatamak/layers-in-ddd-projects-bd492aa2b8aa

## __RUN__

__Way 1__

The project could be executed via _docker-compose_. If you have an IDE which is capable of debugging _docker-compose file_, _docker-compose.yml_ which is located at the main directory would  be useful for you.

In case of choosing this way to run the project, you can reach swagger screen via _http://localhost:9999.

Note: Because the sql server needs more time to be ready compared to UserManagement-Api, it might take a while for you to reach the endpoints after `docker-compose up` command execution.

__Way 2__

If you want to execute the project without using docker, it is required that you set the connection strings inside the UserManagement/appsettings.json file.

Changes to be made are:
1. DbConfig -> DbOptions -> ConnectionStr value should be changed with the Sql Server connection string that you have.
2. MassTransitConfig -> MassTransitOptions -> HostName, VirtualHost, Username, Password values should be changed with the RabbitMq platform information that you have.

## __Projects in Solution__

You can reach the article about the responsibilities of the layers via [this link](https://medium.com/@ademcatamak/layers-in-ddd-projects-bd492aa2b8aa "Layers in DDD Projects").
