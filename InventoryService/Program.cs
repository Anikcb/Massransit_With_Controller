using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMassTransit(ConfigurationBinder =>
{
    ConfigurationBinder.AddConsumer<OrderConsumer>();
    ConfigurationBinder.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("order-queue", c =>
        {
            c.ConfigureConsumer<OrderConsumer>(cxt);
        });
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
