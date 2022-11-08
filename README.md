# IdentityServerAspNetIdentity
### Overall Picture

In this repository, you will see that how to secure microservices with using Identity Server and backing with **API Gateway**.
We’re going to protect our Web and API applications with using **OAuth 2 and OpenID Connect** in IdentityServer. Securing
your web application and API with tokens, working with claims, authentication and authorization middlewares and applying policies, and so on.

### Client
A client is a piece of software that requests tokens from your IdentityServer - either for authenticating a user (requesting an identity token) 
or for accessing a resource (requesting an access token). A client must be first registered with your IdentityServer before it can request tokens.
While there are many different client types, e.g. web applications, native mobile or desktop applications, SPAs, server processes etc., they can all be put into two high-level categories.

### WebClientSample (Interactive Applications)

This is the most common type of client scenario: web applications, SPAs or native/mobile apps with interactive users. This scenario typically involves 
a browser for user interaction (e.g. for authentication or consent)

After that, we are going to develop WebClientSample Asp.Net project for Interactive Client of our application. This Interactive WebClientSample Client
application will be secured with OpenID Connect in IdentityServer4. Our client application pass credentials with logging to an Identity Server
and receive back a JSON Web Token (JWT).

### Client Applications (Machine to Machine Communication)
In this scenario two machines talk to each other (e.g. background processes, batch jobs, server daemons), and there is no interactive user present.
To authorize this communication, your IdentityServer issues a token to the caller.

Client Applications are console applications that want to access secure data sources (Web Api) that users use. A client must be first registered with IdentityServer before it can request tokens.
Examples for clients are web applications, native mobile or desktop applications, SPAs, server processes etc.

### Identity Server
Also, we are going to develop centralized standalone **Authentication Server** and **Identity Provider** with implementing IdentityServer4 package
and the name of microservice is Identity Server.
Identity Server4 is an open source framework which implements **OpenId Connect and OAuth2 protocols** for .Net Core.
With Identity Server, we can provide authentication and access control for our web applications or Web APIs from a single point between applications
or on a user basis.

### Resources
Resources are something you want to protect with your IdentityServer - either identity data of your users, or APIs.

Every resource has a unique name - and clients use this name to specify to which resources they want to get access to.

Identity data Identity information (aka claims) about a user, e.g. name or email address.

APIs APIs resources represent functionality a client wants to invoke - typically modelled as Web APIs, but not necessarily.
### APISample
First of all, we are going to develop **APISample** project and protect this API resources with **IdentityServer4 OAuth 2.0 implementation**. 
Generate **JWT Token** with client_credentials from IdentityServer4 and will use this token for securing APISample protected resources.

## Installation
Follow these steps to get your development environment set up:
1. Check All projects run profiles. One by one Right Click the project file, open Properties window and check the debug section. Launch Profile should
be the "Project" and App URLs should be the same as big picture.
2. For all projects, one by one, Set a Startup project and see the Run profile on the Run button. Change the default running profile to IIS Express to Project name.
3. Multiple startup projects. Right click the solution, open Properties, and set Multiple startup project and Start all 4 application click apply and ok.
4. Now you can run the overall application with Click Start button or F5.
You will see 4 project console window and 1 chrome window for client application.

* **WebClientSample -> https://localhost:5002/**

Check the application with logging the system with below credentials;

* **username - password 1 : alice - a1**
* **username - password 2 : bob - b1**



Further Reading 
# What is Identity Server ?
Identity Server4 is an open source framework which implements OpenId Connect and OAuth2 protocols for .Net Core.
With IdentityServer, we can provide authentication and access control for our web applications or Web APIs from a single point between applications or on a user basis.
IdentityServer determines how is your web or native client applications that want to access Web Api or Api (Resource) in corporate applications or modern web applications can be accessed using authentication and authorization. So for this operations, there is no need to write identification specific to the client application on the Web API side.

You can call this centralized security system as a security token service, identity provider, authorization server, IP-STS and more.
As a summary IdentityServer4 provides that issues security tokens to clients.

IdentityServer has a number of jobs and features — including:

** Protect your resources
** Authenticate users using a local account store or via an external identity provider
** Provide session management and single sign-on
** Manage and authenticate clients
** Issue identity and access tokens to clients
** validate tokens

# Identity Server  Terminologies
Basically, we can think of our structure as client applications (web, native, mobile), data source applications (web api, service) and IdentityServer application. 
You can see these system into image.


# Client Applications
Client Applications are applications that want to access secure data sources (Web Api) that users use. A client must be first registered with IdentityServer before it can request tokens.
Examples for clients are web applications, native mobile or desktop applications, SPAs, server processes etc.

# Resources
Data Resources are the data we want to be protected by IdentityServer. Data sources must have a unique name and the client that will use this resource must access the resource with this name.

#IdentityServer
IdentityServer that we can say OpenId Connect provider over the OAuth2 and OpenId protocols. In short, it provides security tokens to client applications. IdentityServer protects data sources, ensures authentication of users, provides single sign-on and session management, verifies client applications, provides identity and access tokens to client applications and checks the authenticity of these tokens.

#Identity Token
Identity Token represents to the result of the authentication process. It contains a sub-identifier for the user and information about how and when the user will be authenticated. It also contains identity information.

#Access Token
Access Token provides access to the data source (API). The client application can access the data by sending a request to the data source with this token. Access token contains client and user information. The API uses this information to authorize and allow access to data.

#Identity Server  in Microservices World
The security of the application we have developed that the protection and management of the data used by the application are very important. With the developing technology, the access of applications or devices in different technologies to our data may have some security and software problems or additional loads for us.

For example, let’s assume that the users of a SPA (Single Page App) Client application that will use a Web API we have developed log into the system with their username and password. Then, when another client web application to our same Web API wants to verify with windows authentication and enter, a native mobile client application wants to access or another web API wants to access the Web API that we have developed, the security supported by each client application technology, we may need to develop or configure the security part of our application.


