# Creating and running Web API
Creating and running a Web API with .NET Core using the CLI involves several steps. Here's a detailed guide:

### **Step 1: Install .NET SDK**
Ensure you have the .NET SDK installed. You can download it from [.NET Official Site](https://dotnet.microsoft.com/).

Check the installation:
```bash
dotnet --version
```

---

### **Step 2: Create a New Web API Project**
Open your terminal and run:
```bash
dotnet new webapi -n MyWebApi
```
- `dotnet new webapi`: Creates a new Web API project.
- `-n MyWebApi`: Names the project "MyWebApi".

---

### **Step 3: Navigate to the Project Directory**
```bash
cd MyWebApi
```

---

### **Step 4: Run the Web API**
```bash
dotnet run
```
- You should see output indicating that the application is running, usually at `https://localhost:5001` and `http://localhost:5000`.

---

### **Step 5: Test the API**
By default, .NET Core generates a weather forecast endpoint. You can test it using:

- **Browser:** Visit `https://localhost:5001/weatherforecast`
- **curl:**
```bash
curl https://localhost:5001/weatherforecast
```

---

### **Step 6: Modify the API**
- Open the project in your favorite editor (e.g., Visual Studio Code):
```bash
code .
```

- Locate `Controllers/WeatherForecastController.cs` to see the auto-generated endpoint.
- Add a new controller, e.g., `HelloController.cs`:

**Create `Controllers/HelloController.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello, World!";
        }
    }
}
```

---

### **Step 7: Run and Test the New Endpoint**
1. **Start the API:**
   ```bash
   dotnet run
   ```
2. **Test the Endpoint:**
   ```bash
   curl https://localhost:5001/hello
   ```
   You should get:
   ```
   Hello, World!
   ```

---

### **Step 8: Build and Publish**
- **Build the project:**
  ```bash
  dotnet build
  ```
- **Publish the project:**
  ```bash
  dotnet publish -c Release -o ./publish
  ```
  The output will be in the `publish` directory.

---

### **Step 9: Hosting (Optional)**
You can use **Kestrel**, **IIS**, **Nginx**, or **Docker** to host the Web API.

To run using Kestrel:
```bash
dotnet MyWebApi.dll
```

---

### **Summary of Commands**
```bash
dotnet new webapi -n MyWebApi
cd MyWebApi
dotnet run
code .
dotnet build
dotnet publish -c Release -o ./publish
```