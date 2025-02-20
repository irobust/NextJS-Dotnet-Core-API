# Entity Framework Core

## **1. Install SQLite Dependencies**  
Run the following commands in the terminal or **Package Manager Console**:

```powershell
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

These packages provide SQLite support and migration tools.

---

## **2. Create a Model**  
Define your data model class. This will represent a table in the SQLite database.

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

---

## **3. Create a DbContext**  
The `DbContext` class manages database operations. Configure it to use SQLite.

```csharp
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db");
    }
}
```
- `"Data Source=app.db"` → This specifies the SQLite database file.

---

## **4. Apply Migrations & Create the Database**  
To create the database and apply the schema:

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- `migrations add InitialCreate` → Generates migration files.
- `database update` → Creates the database (`app.db`) and tables.

---

## **5. Perform CRUD Operations**  
You can now interact with the SQLite database.

### **Add Data**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = new Product { Name = "Smartphone", Price = 499.99M };
    context.Products.Add(product);
    context.SaveChanges();
}
```

### **Retrieve Data**
```csharp
using (var context = new ApplicationDbContext())
{
    var products = context.Products.ToList();
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Id} - {product.Name} - ${product.Price}");
    }
}
```

### **Update Data**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = context.Products.FirstOrDefault(p => p.Id == 1);
    if (product != null)
    {
        product.Price = 549.99M;
        context.SaveChanges();
    }
}
```

### **Delete Data**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = context.Products.FirstOrDefault(p => p.Id == 1);
    if (product != null)
    {
        context.Products.Remove(product);
        context.SaveChanges();
    }
}
```

---

## **6. Use Dependency Injection (ASP.NET Core)**
Instead of `OnConfiguring`, use **dependency injection** in `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
```

And in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=app.db"
  }
}
```

---

## **7. Querying with LINQ**
You can use LINQ to query the SQLite database.

- **Find a product by ID**  
  ```csharp
  var product = context.Products.Find(1);
  ```
- **Filter by price**  
  ```csharp
  var expensiveProducts = context.Products.Where(p => p.Price > 500).ToList();
  ```
- **Sort products**  
  ```csharp
  var sortedProducts = context.Products.OrderBy(p => p.Name).ToList();
  ```

---

## **8. Handling Relationships (One-to-Many Example)**
To add relationships, define a new model.

### **Category Model**
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
```

### **Update Product Model**
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
```

### **Update `ApplicationDbContext`**
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db");
    }
}
```

### **Apply Migrations Again**
Run:

```powershell
dotnet ef migrations add AddCategory
dotnet ef database update
```

Now, products will have categories!

---

## **9. Transactions (Ensuring Data Consistency)**
You can use transactions in EF Core with SQLite:

```csharp
using var context = new ApplicationDbContext();
using var transaction = context.Database.BeginTransaction();
try
{
    var category = new Category { Name = "Electronics" };
    context.Categories.Add(category);
    context.SaveChanges();

    var product = new Product { Name = "Tablet", Price = 299.99M, CategoryId = category.Id };
    context.Products.Add(product);
    context.SaveChanges();

    transaction.Commit();  // Commit the transaction
}
catch
{
    transaction.Rollback();  // Rollback on error
}
```

---

## **10. Additional Configurations**
### **Enable Lazy Loading**
Lazy loading allows navigation properties to be loaded on demand.

1. Install the proxies package:

```powershell
dotnet add package Microsoft.EntityFrameworkCore.Proxies
```

2. Enable it in `DbContext`:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    options.UseSqlite("Data Source=app.db")
           .UseLazyLoadingProxies();
}
```

### **Logging SQL Queries**
To debug, log SQL queries:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    options.UseSqlite("Data Source=app.db")
           .LogTo(Console.WriteLine, LogLevel.Information);
}
```

## Query filter and Paging
### **Querying, Filtering, and Pagination with Entity Framework Core**

Entity Framework Core (EF Core) makes it easy to **query**, **filter**, and **paginate** data using LINQ.

---

## **1. Querying Data**
To fetch data from the database, use `DbSet<T>`, which represents a table.

### **Retrieve All Records**
```csharp
using (var context = new ApplicationDbContext())
{
    var products = context.Products.ToList();
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Id} - {product.Name} - ${product.Price}");
    }
}
```

### **Find a Record by Primary Key**
```csharp
var product = context.Products.Find(1);
Console.WriteLine(product.Name);
```
> `Find(id)` is optimized and uses the in-memory cache before hitting the database.

### **Find a Record with a Condition**
```csharp
var product = context.Products.FirstOrDefault(p => p.Name == "Laptop");
```

### **Include Related Data (Eager Loading)**
```csharp
var productsWithCategories = context.Products.Include(p => p.Category).ToList();
```

---

## **2. Filtering Data**
EF Core allows filtering using **LINQ queries**.

### **Filter by a Single Condition**
```csharp
var cheapProducts = context.Products.Where(p => p.Price < 500).ToList();
```

### **Filter by Multiple Conditions**
```csharp
var filteredProducts = context.Products
    .Where(p => p.Price > 100 && p.Name.Contains("Phone"))
    .ToList();
```

### **Get the First or Default Item**
```csharp
var firstProduct = context.Products.FirstOrDefault(p => p.Name.StartsWith("Lap"));
```

### **Check if a Record Exists**
```csharp
bool exists = context.Products.Any(p => p.Name == "Tablet");
```

---

## **3. Sorting Data**
### **Sort in Ascending Order**
```csharp
var products = context.Products.OrderBy(p => p.Price).ToList();
```

### **Sort in Descending Order**
```csharp
var products = context.Products.OrderByDescending(p => p.Price).ToList();
```

### **Sort by Multiple Columns**
```csharp
var products = context.Products
    .OrderBy(p => p.Name)
    .ThenByDescending(p => p.Price)
    .ToList();
```

---

## **4. Pagination (Skip & Take)**
Pagination allows you to fetch data in **chunks** rather than loading everything at once.

### **Example: Paginating Products**
```csharp
int pageSize = 5;
int pageNumber = 2; // 1-based index

var pagedProducts = context.Products
    .OrderBy(p => p.Id) // Always order before paginating
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```
- `Skip(n)` → Skips `n` records.
- `Take(n)` → Takes `n` records.

---

## **5. Pagination with Filtering and Sorting**
A common scenario is **filtering, sorting, and paginating** at the same time.

### **Example: Search "Laptop", Sort by Price, and Paginate**
```csharp
int pageSize = 5;
int pageNumber = 1;
string searchTerm = "Laptop";

var query = context.Products
    .Where(p => p.Name.Contains(searchTerm))
    .OrderBy(p => p.Price)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

---

## **6. Getting Total Records for Pagination**
When paginating, it's common to fetch the total record count.

```csharp
int totalRecords = context.Products.Count();
```
If filtering is applied:
```csharp
int filteredRecords = context.Products.Where(p => p.Name.Contains("Laptop")).Count();
```

---

## **7. Asynchronous Queries (Recommended for Web APIs)**
Use `async` methods to prevent blocking operations in **ASP.NET Core** applications.

### **Paginated Query (Async)**
```csharp
public async Task<List<Product>> GetPagedProductsAsync(int pageNumber, int pageSize)
{
    using var context = new ApplicationDbContext();
    return await context.Products
        .OrderBy(p => p.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
```
> `.ToListAsync()` requires `using Microsoft.EntityFrameworkCore;`

### **Get Total Count (Async)**
```csharp
int totalRecords = await context.Products.CountAsync();
```

---

## **8. Dynamic Filtering & Pagination in ASP.NET Core API**
Here's how you can implement **searching, sorting, and pagination** in an API.

### **Controller Method**
```csharp
[HttpGet("products")]
public async Task<IActionResult> GetProducts(
    string? search = "", 
    string? sortColumn = "Price", 
    string sortDirection = "asc", 
    int page = 1, 
    int pageSize = 5)
{
    using var context = new ApplicationDbContext();

    var query = context.Products.AsQueryable();

    // Filtering
    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(p => p.Name.Contains(search));
    }

    // Sorting
    if (sortDirection == "asc")
        query = query.OrderBy(p => EF.Property<object>(p, sortColumn));
    else
        query = query.OrderByDescending(p => EF.Property<object>(p, sortColumn));

    // Pagination
    int totalRecords = await query.CountAsync();
    var products = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(new { totalRecords, products });
}
```

### **Example API Calls**
- Get products with default sorting & pagination:  
  **`GET /products`**
- Get products with search:  
  **`GET /products?search=Phone`**
- Get products sorted by Name descending:  
  **`GET /products?sortColumn=Name&sortDirection=desc`**
- Get paginated products (Page 2, 5 per page):  
  **`GET /products?page=2&pageSize=5`**

---

## **9. Performance Optimization**
### **Use Projections to Select Only Needed Fields**
Instead of `ToList()`, use `.Select()` to fetch only required fields:

```csharp
var products = context.Products
    .Select(p => new { p.Name, p.Price })
    .ToList();
```

### **Use `.AsNoTracking()` for Read-Only Queries**
This improves performance by disabling EF Core’s change tracking.

```csharp
var products = context.Products.AsNoTracking().ToList();
```

### **Use Indexed Columns for Filtering**
For better performance, create indexes on frequently filtered columns in your database.

```csharp
[Index(nameof(Name))]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

## Insert, Update and Delete

### **Insert, Update, and Delete Data with Entity Framework Core (EF Core)**  

EF Core provides a simple and efficient way to perform **CRUD (Create, Read, Update, Delete) operations** on a database. Below is a step-by-step guide to inserting, updating, and deleting data using EF Core.

---

## **1. Setting Up the DbContext and Model**  
If you haven't set up your database context, define it as follows:

```csharp
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=app.db"); // Using SQLite
    }
}
```

Define a **Product** model:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

---

## **2. Insert Data (Create)**
To insert data into the database, use the `Add()` method followed by `SaveChanges()`.

### **Example: Insert a Single Record**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = new Product { Name = "Laptop", Price = 1200.99M };

    context.Products.Add(product);  // Add new record
    context.SaveChanges();          // Save changes to the database
}
```

### **Insert Multiple Records**
```csharp
using (var context = new ApplicationDbContext())
{
    var products = new List<Product>
    {
        new Product { Name = "Tablet", Price = 500.99M },
        new Product { Name = "Smartphone", Price = 899.99M }
    };

    context.Products.AddRange(products); // Add multiple records
    context.SaveChanges();
}
```

---

## **3. Update Data**
To update data, retrieve the entity, modify it, and call `SaveChanges()`.

### **Example: Update a Record**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = context.Products.FirstOrDefault(p => p.Id == 1); // Find product with ID 1

    if (product != null)
    {
        product.Price = 1300.00M; // Modify the price
        context.SaveChanges();    // Save changes
    }
}
```

### **Alternative: Update Without Fetching the Record**
If you already know the ID and don't want to fetch the record first:

```csharp
using (var context = new ApplicationDbContext())
{
    var product = new Product { Id = 1, Name = "Updated Laptop", Price = 1400.00M };

    context.Products.Update(product); // Mark entity as modified
    context.SaveChanges();
}
```
> **Note:** This updates all properties, even if only one is changed.

### **Update Specific Fields Without Affecting Other Columns**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = new Product { Id = 1 };
    context.Attach(product);
    product.Price = 1500.00M;  // Only update the Price column

    context.Entry(product).Property(p => p.Price).IsModified = true;
    context.SaveChanges();
}
```

---

## **4. Delete Data**
To delete data, retrieve the entity, call `Remove()`, and save the changes.

### **Example: Delete a Record**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = context.Products.FirstOrDefault(p => p.Id == 1); // Find product

    if (product != null)
    {
        context.Products.Remove(product); // Mark for deletion
        context.SaveChanges(); // Save changes
    }
}
```

### **Delete Without Fetching the Entity**
```csharp
using (var context = new ApplicationDbContext())
{
    var product = new Product { Id = 1 }; // Only provide the ID
    context.Products.Remove(product);
    context.SaveChanges();
}
```
> **Note:** This works if the entity is already tracked or if you attach it manually.

### **Delete Multiple Records**
```csharp
using (var context = new ApplicationDbContext())
{
    var productsToDelete = context.Products.Where(p => p.Price < 500).ToList();
    
    context.Products.RemoveRange(productsToDelete); // Delete multiple records
    context.SaveChanges();
}
```

---

## **5. Bulk Insert, Update, and Delete (EF Core Extensions)**
For **high-performance bulk operations**, use the **EF Core Extensions** package:

```powershell
dotnet add package Z.EntityFramework.Extensions.EFCore
```

### **Bulk Insert**
```csharp
context.BulkInsert(new List<Product>
{
    new Product { Name = "Monitor", Price = 300 },
    new Product { Name = "Keyboard", Price = 50 }
});
```

### **Bulk Update**
```csharp
context.BulkUpdate(products);
```

### **Bulk Delete**
```csharp
context.BulkDelete(products);
```

---

## **6. Using Transactions for Multiple Changes**
To ensure **data consistency**, use a **transaction** when performing multiple operations.

```csharp
using (var context = new ApplicationDbContext())
{
    using var transaction = context.Database.BeginTransaction();
    try
    {
        var product1 = new Product { Name = "Mouse", Price = 30 };
        var product2 = new Product { Name = "Speaker", Price = 100 };

        context.Products.Add(product1);
        context.Products.Add(product2);
        context.SaveChanges();

        transaction.Commit(); // Commit changes if successful
    }
    catch
    {
        transaction.Rollback(); // Undo changes if an error occurs
    }
}
```

---

## **7. Asynchronous Insert, Update, and Delete**
For **web applications**, it's best to use async methods.

### **Insert (Async)**
```csharp
public async Task AddProductAsync(Product product)
{
    using var context = new ApplicationDbContext();
    await context.Products.AddAsync(product);
    await context.SaveChangesAsync();
}
```

### **Update (Async)**
```csharp
public async Task UpdateProductAsync(int id, decimal newPrice)
{
    using var context = new ApplicationDbContext();
    var product = await context.Products.FindAsync(id);

    if (product != null)
    {
        product.Price = newPrice;
        await context.SaveChangesAsync();
    }
}
```

### **Delete (Async)**
```csharp
public async Task DeleteProductAsync(int id)
{
    using var context = new ApplicationDbContext();
    var product = await context.Products.FindAsync(id);

    if (product != null)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}
```