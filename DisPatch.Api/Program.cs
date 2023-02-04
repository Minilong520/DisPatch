using DisPatch.Common.Authentication;
using DisPatch.Common.Helpers;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new AppsettingHelper(builder.Configuration));

// 解决跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowCors",
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
builder.Services.AddMvc().AddJsonOptions(
    options =>
    {
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

// 自定义鉴权
builder.Services.AddAuthentication(options =>
{
    //options.DefaultScheme = DefaultAuthHandler.SchemeName;//不要指定默认授权方案，否则所有请求都会进行验证
    options.AddScheme<DisPatchAuthHandler>(DisPatchAuthOptions.Scheme, DisPatchAuthOptions.Scheme);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
