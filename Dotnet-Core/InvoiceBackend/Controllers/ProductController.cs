using Microsoft.AspNetCore.Mvc;
using InvoiceBackend.DataStore;
using InvoiceBackend.Models;
using Asp.Versioning;

namespace InvoiceBackend.Controllers;

[ApiController]
[Route("/api/[controller]")]
[ApiVersion(1.0)]
public class ProductController : ControllerBase
{
    private ApplicationDbContext _context;
    public ProductController(ApplicationDbContext context){
        _context = context;
    }
    // private static ProductDataStore _productDataStore = new ProductDataStore();

    // [HttpGet]
    // public IActionResult GetAllProducts(){
    //     return Ok(_productDataStore.getProducts());
    // }

    // [HttpGet("{id}")]
    // public IActionResult GetProductById(int id){
    //     var products = _productDataStore.getProducts(); 
    //     var index = id-1;
    //     if(index < 0 || index >= products.Count){
    //         return NotFound();
    //     }
    //     return Ok(products[index]);
    // }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product){
        // if(!ModelState.IsValid){
        //     return BadRequest();
        // }
        
        _context.Products.Add(product);
        _context.SaveChanges();

        return Ok(product);
    }

}