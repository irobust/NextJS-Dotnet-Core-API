# Authentication
To create a login system using **Next.js** (frontend) and **.NET Core Web API** (backend), follow these steps:

---

## **1. Setup .NET Core Web API (Backend)**

### Install Required Packages
```sh
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqLite
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Configure Authentication and JWT in `appsettings.json`
Modify `appsettings.json`:
```json
{
  "JwtSettings": {
    "Key": "your_secret_key_here",
    "Issuer": "your_api",
    "Audience": "your_clients"
  },
  "ConnectionStrings": {
    "Default": "invoice.db"
  }
}
```

### Setup User Model and Authentication
Modify `Models/ApplicationUser.cs`:
```csharp
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
}
```

Modify `Data/ApplicationDbContext.cs`:
```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
```

Modify `Program.cs`:
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"]
    };
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### **e. Create Login and Register Endpoints**
Modify `Controllers/AuthController.cs`:
```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }
        return Unauthorized();
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

Define `Models/RegisterModel.cs`:
```csharp
public class RegisterModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

Define `Models/LoginModel.cs`:
```csharp
public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

Run migrations:
```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Run the API:
```sh
dotnet run
```

---

## **2. Setup Next.js Frontend**
### **a. Create a Next.js App**
```sh
npx create-next-app@latest next-auth-app
cd next-auth-app
```

### **b. Install Axios for API Calls**
```sh
npm install axios
```

### **c. Create a Login Page (`pages/login.js`)**
```jsx
import { useState } from "react";
import axios from "axios";
import { useRouter } from "next/router";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const router = useRouter();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post("http://localhost:5000/api/auth/login", { email, password });
            localStorage.setItem("token", response.data.token);
            router.push("/dashboard");
        } catch (error) {
            console.error("Login failed", error);
        }
    };

    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <input type="email" placeholder="Email" onChange={(e) => setEmail(e.target.value)} required />
                <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} required />
                <button type="submit">Login</button>
            </form>
        </div>
    );
}
```

### **d. Create a Protected Dashboard (`pages/dashboard.js`)**
```jsx
import { useEffect, useState } from "react";
import { useRouter } from "next/router";

export default function Dashboard() {
    const [authenticated, setAuthenticated] = useState(false);
    const router = useRouter();

    useEffect(() => {
        const token = localStorage.getItem("token");
        if (!token) {
            router.push("/login");
        } else {
            setAuthenticated(true);
        }
    }, []);

    return authenticated ? <h1>Welcome to Dashboard</h1> : null;
}
```

---

## **3. Running the Application**
- **Start the .NET Core API:** `dotnet run`
- **Start the Next.js app:** `npm run dev`