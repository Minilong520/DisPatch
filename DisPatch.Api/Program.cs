using DisPatch.Common.Authentication;
using DisPatch.Common.Helpers;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new AppsettingHelper(builder.Configuration));

// �������
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

//���json�������л�֮�󣬴�д��ĸ��ת��Сд������
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

// �Զ����Ȩ
builder.Services.AddAuthentication(options =>
{
    //options.DefaultScheme = DefaultAuthHandler.SchemeName;//��Ҫָ��Ĭ����Ȩ�����������������󶼻������֤
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
