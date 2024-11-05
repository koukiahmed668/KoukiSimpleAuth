
# KoukiSimpleAuth


KoukiSimpleAuth is an open-source ASP.NET Core library designed to simplify user authentication and authorization in web applications. This library offers a robust solution for handling common authentication needs, such as user registration, login, and JWT-based security, making it easy to integrate a reliable authentication system into any ASP.NET Core project.



## Features

- User Registration and Login: Quickly add secure user registration and login functionality.
- JWT Authentication: Secure API endpoints with JSON Web Tokens, making user authentication scalable and efficient.
- Role-Based Authorization: Easily manage user roles and permissions.
- Social Media Authentication: Extendable to allow authentication through social media platforms (e.g., Google, Facebook).
- Modular Services: Lightweight and customizable, allowing for easy integration with other services.

## Getting Started
To start using KoukiSimpleAuth, follow these steps to set up the library in your ASP.NET Core project.


## Prerequisites

- .NET Core 6.0 or later
- ASP.NET Core Web API

## Installation

1. install it via NuGet:


```bash
  install-package KoukiSimpleAuth
```


## Setup

1. Configure appsettings.json
Add your database connection string and JWT settings in the appsettings.json file of your ASP.NET Core project:
```csharp
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionStringHere"
  },
  "Jwt": {
    "Key": "YourSecretKeyHere",
  }
}
```

2. Register Services
In Program.cs, configure your authentication library by adding the required services:


```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add controllers
        builder.Services.AddControllers();

        // Register database context
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        // Register JWT authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        // Register your library's AuthService
        builder.Services.AddScoped<IAuthService, AuthService<ApplicationDbContext>>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

```

3. Create API Endpoints
In your ASP.NET Core project, create controllers to handle registration and login requests using the KoukiSimpleAuth services.

Example of a UsersController:
```csharp
using Microsoft.AspNetCore.Mvc;
using SimpleAuth.Models;
using SimpleAuth.Services;

namespace YourProjectName.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(userDTO);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.LoginAsync(loginRequest.Username, loginRequest.Password);
            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

```

## Contributing

If you'd like to contribute to KoukiSimpleAuth, feel free to fork the repository and submit a pull request. We welcome all contributions to improve the functionality and usability of the library.
## License

MIT License

Copyright (c) 2024 Kouki Ahmed

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
