using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �������
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "myCors",
        builde =>
        {
            //builde.WithOrigins(builder.Configuration.GetValue<string>("CorsUrls").Split(","))
            builde.WithOrigins("*")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

//���json�������л�֮�󣬴�д��ĸ��ת��Сд������
builder.Services.AddMvc().AddJsonOptions(    
    options => {        
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    }
);

//builder.Services.Configure<JsonOptions>(options =>
//{
//    options.SerializerOptions.Converters.Add(new TimeOnly);
//});

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
