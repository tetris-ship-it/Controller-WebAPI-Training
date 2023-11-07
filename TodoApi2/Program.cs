using Microsoft.EntityFrameworkCore;

using TodoApi2.Models;

var builder = WebApplication.CreateBuilder(args);//the WebApplication.CreateBuilder() method is used for creating an instance of the webapplicationbuilder class responsible for making web applications.

// Add services to the container.

builder.Services.AddControllers();//adds mvc controllers to the container
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));//adds the TodoContext class as a database context for the application using an in memory database named TodoList.
builder.Services.AddEndpointsApiExplorer();//adds api explorer services to the container, which are used by swagger for generating API documentation.
builder.Services.AddSwaggerGen();//this adds the swagger generation services which generate the swagger JSON and UI.

var app = builder.Build();//creating the web application instance using the build() method.

// Configure the HTTP request pipeline with the middleware components...
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//adds the swagger middleware to serve the swagger JSON documents.
    app.UseSwaggerUI();//adds the swagger UI middleware to serve the swagger UI.
}

app.UseAuthorization();//middleware for authorization purposes.

app.MapControllers();//maps the controller to routes.

app.Run();//starts the application and listens for http requests.
