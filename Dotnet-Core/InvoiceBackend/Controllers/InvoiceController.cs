using Microsoft.AspNetCore.Mvc;
using InvoiceBackend.DataStore;
using Asp.Versioning;
using InvoiceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvoiceBackend.Controllers;

[ApiVersion(1.0)]
[ApiVersion(2.0)]
[ApiController]
[Route("/api/[controller]")]
public class InvoiceController : ControllerBase
{
    private static ProductDataStore _products = new ProductDataStore();
    private ApplicationDbContext _context;

    public InvoiceController(ApplicationDbContext context){
        _context = context;
    }

    [HttpGet("{id:int}/products")]
    public IActionResult GetProductsV1(int id){
        return Ok(_products.getProducts());
    }

    [HttpGet("{id:int}/products"), MapToApiVersion(2.0)]
    public IActionResult GetProductsV2(int id){
        return Ok(_products.getProducts()[0]);
    }

    [HttpGet("{id:int}/products/{name:length(5,10)}")]
    public IActionResult GetProductsByName(int id){
        return Ok(_products.getProducts());
    }

    [HttpGet("{id:int}/products/{productId:int=1}")]
    public IActionResult GetEachProducts(int id, int productId){
        var products = _products.getProducts(); 
        var index = productId-1;
        if(index < 0 || index >= products.Count){
            return NotFound();
        }
        return Ok(products[index]);
    }

    [HttpPost]
    public IActionResult AddNewInvoice([FromBody] Invoice invoice){
        // using(var context = new ApplicationDbContext()){
        //     // var invoice = new Invoice(){
        //     //     Name = "INV001",
        //     //     Email = "john@example.com",
        //     //     ImageUrl ="sample.png",
        //     //     Amount = 1000.00M
        //     // };

        //     context.Invoices.Add(invoice);
        //     context.SaveChanges();
        // }

        _context.Invoices.Add(invoice);
        _context.SaveChanges();
        return Ok(invoice);
    }

    [HttpGet]
    public IActionResult GetInvoices(){
        List<Invoice> invoices;
        invoices = _context.Invoices.Include(i => i.Products).ToList();
        if(invoices.Count == 0) NotFound();
        return Ok(invoices);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetInvoiceById(int Id){
        Invoice invoice;
        invoice = _context.Invoices.FirstOrDefault(invoice => invoice.Id == Id);
        if(invoice == null) return NotFound();
        return Ok(invoice);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteInvoice(int Id){
        // var invoice = context.Invoices.FirstOrDefault(invoice => invoice.Id == Id);

        var invoice = new Invoice(){ Id = Id };
        if(invoice != null){
            _context.Invoices.Remove(invoice);
            _context.SaveChanges();
        }
        return Ok(null);
    }

    [HttpGet("amount")]
    public IActionResult GetHighValue([FromQuery] int _value){
        List<Invoice> invoices;
        invoices = _context.Invoices.Where(invoice => invoice.Amount > _value).ToList();
        return Ok(invoices);
    }
}