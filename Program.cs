using ChainStoreApi.Data;
using ChainStoreApi.Services;
using ChainStoreApi.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>

{

options.AddPolicy(

name: "AllowOrigin",

builder => {

builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

});

});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSetting>(
    builder.Configuration.GetSection("ChainStoreDatabase"));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));


var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew=TimeSpan.Zero
    };
});
builder.Services.AddSingleton<StaffService>();
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<ProfileStaffService>();
builder.Services.AddSingleton<StoreService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<PartnerService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<TimeWorkService>();
builder.Services.AddSingleton<CustomerService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<RevenueService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
