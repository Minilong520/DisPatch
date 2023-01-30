var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 解决跨域
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

//解决json对象反序列化之后，大写字母被转成小写的问题
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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
