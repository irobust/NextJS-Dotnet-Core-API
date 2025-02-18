# Routing API Resources
In ASP.NET Core Web API, resource routing is based on **RESTful principles**, where URLs represent resources, and HTTP methods (`GET`, `POST`, `PUT`, `DELETE`, etc.) represent actions on those resources. Here's how to set up resource-based routing effectively in a .NET Core Web API.

---

## **1. Resource Routing Basics in .NET Core**
A **resource** typically corresponds to a **model** (e.g., `Product`, `Order`, `Customer`), and routes are organized to interact with these resources.

### 🚀 **Example Routes for a Product Resource:**
| HTTP Method | URL                     | Action                |
|-------------|-------------------------|-----------------------|
| **GET**     | `/api/products`         | Get all products     |
| **GET**     | `/api/products/{id}`    | Get product by ID    |
| **POST**    | `/api/products`         | Create a product     |
| **PUT**     | `/api/products/{id}`    | Update a product     |
| **DELETE**  | `/api/products/{id}`    | Delete a product     |

---

## **2. Setting Up Resource Routing in `.NET Core`**

### **Step 1: Create a Model**
Create a `Product` model:
```csharp
namespace RoutingApiDemo.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
```

---

### **Step 2: Create a Controller**
Create a `ProductsController` under the `Controllers` folder:

```csharp
using Microsoft.AspNetCore.Mvc;
using RoutingApiDemo.Models;

namespace RoutingApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Mock in-memory data (for demo purposes)
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200.00M },
            new Product { Id = 2, Name = "Mouse", Price = 25.99M },
            new Product { Id = 3, Name = "Keyboard", Price = 45.50M }
        };

        // GET: api/products
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(_products);
        }

        // GET: api/products/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null) return BadRequest();
            product.Id = _products.Count + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            _products.Remove(product);
            return NoContent();
        }
    }
}
```

#### **Setting up Routing in a .NET Web API**

1. **Attribute Routing (Recommended)**
Attribute routing allows you to define routes directly on the controller and action methods.

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route: api/products
    public class ProductsController : ControllerBase
    {
        [HttpGet] // Route: api/products
        public IActionResult GetAll()
        {
            return Ok(new[] { "Product1", "Product2" });
        }

        [HttpGet("{id}")] // Route: api/products/5
        public IActionResult GetById(int id)
        {
            return Ok($"Product {id}");
        }

        [HttpPost] // Route: api/products
        public IActionResult Create([FromBody] string product)
        {
            return Created("", product);
        }

        [HttpPut("{id}")] // Route: api/products/5
        public IActionResult Update(int id, [FromBody] string product)
        {
            return NoContent();
        }

        [HttpDelete("{id}")] // Route: api/products/5
        public IActionResult Delete(int id)
        {
            return NoContent();
        }
    }
}
```

- `[Route("api/[controller]")]`: Automatically maps to `api/products` (controller name).
- `[HttpGet("{id}")]`: Maps to `api/products/{id}`.

---

1. **Conventional Routing (In Program.cs)**

Conventional routing is defined centrally and applies to multiple controllers.

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapControllers(); // Maps controllers automatically

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/products", () =>
{
    return Results.Ok(new[] { "Product1", "Product2" });
});

app.MapGet("/api/products/{id:int}", (int id) =>
{
    return Results.Ok($"Product {id}");
});

app.Run();
```

---

1. **Customizing Routes with Route Tokens**
You can customize routes using tokens like `[controller]`, `[action]`, and `[area]`.

```csharp
[ApiController]
[Route("api/[controller]/[action]")]
public class OrdersController : ControllerBase
{
    [HttpGet]
    public IActionResult List() // Route: api/orders/list
    {
        return Ok(new[] { "Order1", "Order2" });
    }

    [HttpGet("{id}")]
    public IActionResult Details(int id) // Route: api/orders/details/5
    {
        return Ok($"Order {id}");
    }
}
```

---

1. **Route Constraints**
Route constraints restrict the format of parameters.

```csharp
[HttpGet("{id:int}")] // Only matches if 'id' is an integer
public IActionResult GetById(int id)
{
    return Ok($"Product {id}");
}

[HttpGet("{name:alpha}")] // Only matches if 'name' is alphabetical
public IActionResult GetByName(string name)
{
    return Ok($"Product: {name}");
}
```

**Common Route Constraints:**
- `int`: Integer only
- `alpha`: Alphabetical only
- `datetime`: Valid datetime
- `length`: String length (`{name:length(5,10)}`)
- `regex`: Regular expression match (`{code:regex(^[A-Z]{3}$)}`)

---

1. **Default Route Parameters**
Set default values in routes.

```csharp
[HttpGet("{id:int=1}")] // Default id is 1 if not provided
public IActionResult GetById(int id)
{
    return Ok($"Product {id}");
}
```

---

1. **Route Prefixes**
Use route prefixes to reduce redundancy.

```csharp
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpGet("all")] // Route: api/products/all
    public IActionResult GetAll()
    {
        return Ok(new[] { "Product1", "Product2" });
    }

    [HttpGet("{id}")] // Route: api/products/{id}
    public IActionResult GetById(int id)
    {
        return Ok($"Product {id}");
    }
}
```

---

1. **API Versioning**
In a real-world API, versioning is crucial for backward compatibility.

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/products")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class ProductsController : ControllerBase
{
    [HttpGet, MapToApiVersion("1.0")] // Route: api/v1/products
    public IActionResult GetV1()
    {
        return Ok("Products from API v1");
    }

    [HttpGet, MapToApiVersion("2.0")] // Route: api/v2/products
    public IActionResult GetV2()
    {
        return Ok("Products from API v2");
    }
}
```

---

1. Global Routing Configuration**
You can globally configure routing behavior in `Program.cs`.

```csharp
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // Use lowercase URLs
    options.AppendTrailingSlash = false; // No trailing slash
});
```

---

## 🧵 **3. Configure Routing in `Program.cs`**
Make sure routing is configured properly in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## **4. Test API Routes**
### **Run the Application**
```bash
dotnet run
```

### **Test with Swagger**:  
Open [https://localhost:5001/swagger](https://localhost:5001/swagger)

Or use **curl** commands:

#### ✅ Get All Products:
```bash
curl -X GET https://localhost:5001/api/products
```

#### ✅ Get Product by ID:
```bash
curl -X GET https://localhost:5001/api/products/1
```

#### ✅ Create a Product:
```bash
curl -X POST https://localhost:5001/api/products \
-H "Content-Type: application/json" \
-d '{"name":"Tablet","price":299.99}'
```

#### ✅ Update a Product:
```bash
curl -X PUT https://localhost:5001/api/products/1 \
-H "Content-Type: application/json" \
-d '{"name":"Gaming Laptop","price":1500.00}'
```

#### ✅ Delete a Product:
```bash
curl -X DELETE https://localhost:5001/api/products/1
```

---

## **5. Advanced Resource Routing Techniques**
### **Custom Routes with Child Resources**
For nested resources (e.g., Products and Reviews):
```csharp
// Route: GET api/products/{productId}/reviews
[HttpGet("{productId:int}/reviews")]
public IActionResult GetReviewsForProduct(int productId)
{
    return Ok($"Reviews for product {productId}");
}
```

### **Multiple Route Templates**
You can assign multiple routes to a single action:
```csharp
[HttpGet("all")]
[HttpGet("list")]
public IActionResult ListProducts()
{
    return Ok(_products);
}
```

---

## 🛡️ **6. Route Constraints**
You can enforce constraints on route parameters:
```csharp
// Route: api/products/{id}
// Only allows integers greater than 0
[HttpGet("{id:int:min(1)}")]
public IActionResult GetProductWithConstraint(int id)
{
    return Ok($"Product with ID: {id}");
}
```

---

## 📌 **7. Route Naming for `CreatedAtAction`**
Use route names for `CreatedAtAction` or `CreatedAtRoute`:
```csharp
// POST: api/products
[HttpPost(Name = "CreateProduct")]
public IActionResult CreateProduct([FromBody] Product product)
{
    product.Id = _products.Count + 1;
    _products.Add(product);

    return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
}

// GET: api/products/{id}
[HttpGet("{id:int}", Name = "GetProductById")]
public IActionResult GetProductById(int id)
{
    var product = _products.FirstOrDefault(p => p.Id == id);
    return product == null ? NotFound() : Ok(product);
}
```

---

## 💡 **8. Versioning the API**
To version your resources:
```bash
dotnet add package Microsoft.AspNetCore.Mvc.Versioning
```

In `Program.cs`:
```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
```

In `ProductsController`:
```csharp
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    // Example: api/v1/products
    [HttpGet]
    public IActionResult GetAllProducts() => Ok(_products);
}
```