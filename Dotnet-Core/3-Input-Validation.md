# Input Validation
In a **.NET Core Web API**, input validation is essential to ensure that the data received from clients meets the required standards. There are several ways to implement input validation in .NET Core, including **model validation**, **data annotations**, **custom validators**, and **middleware for global error handling**.

### **1. Use Data Annotations for Model Validation**
The simplest and most common approach is using data annotations on your **DTOs (Data Transfer Objects)**.

#### **Step 1: Create a Model with Validation Attributes**
```csharp
using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required]
    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    public int Age { get; set; }
}
```

---

#### **Step 2: Validate Input in the Controller**
```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Process valid input (e.g., save to database)
        return Ok(new { Message = "User created successfully" });
    }
}
```

---

### **2. Automatic Model State Validation with Filter**
Instead of manually checking `ModelState` in every action, use a **global filter**.

#### **In `Program.cs` (or `Startup.cs` for older versions):**
```csharp
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            Errors = errors
        });
    };
});
```

---

### **3. Create Custom Validation Attributes**
For complex validation logic, you can create custom attributes.

#### **Custom Validator Example:**
```csharp
using System.ComponentModel.DataAnnotations;

public class MinimumYearAttribute : ValidationAttribute
{
    private readonly int _year;
    public MinimumYearAttribute(int year)
    {
        _year = year;
        ErrorMessage = $"Year must be greater than or equal to {_year}";
    }

    public override bool IsValid(object value)
    {
        if (value is int year)
        {
            return year >= _year;
        }
        return false;
    }
}
```

---

#### **Apply Custom Attribute to DTO:**
```csharp
public class BookDto
{
    [Required]
    [MinimumYear(2000)]
    public int PublicationYear { get; set; }
}
```

---

### **4. Fluent Validation (Alternative Approach)**
For more advanced validation logic, use the **`FluentValidation`** library.

#### **Install FluentValidation:**
```bash
dotnet add package FluentValidation.AspNetCore
```

#### **Create a Validator Class:**
```csharp
using FluentValidation;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(u => u.Username)
            .NotEmpty().WithMessage("Username is required")
            .Length(3, 20).WithMessage("Username must be between 3 and 20 characters");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(u => u.Age)
            .InclusiveBetween(18, 100).WithMessage("Age must be between 18 and 100");
    }
}
```

---

#### **Register FluentValidation in `Program.cs`:**
```csharp
using FluentValidation;
using FluentValidation.AspNetCore;

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
```

---

### **5. Return Consistent Error Responses**
Handle all exceptions globally for consistent error responses.

#### **Global Error Handling Middleware:**
```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "An unexpected error occurred",
                Details = ex.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

---

#### **Register Middleware in `Program.cs`:**
```csharp
app.UseMiddleware<ErrorHandlingMiddleware>();
```
