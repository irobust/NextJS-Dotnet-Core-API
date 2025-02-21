using InvoiceBackend.Models;
namespace InvoiceBackend.DataStore{
    public class ProductDataStore{
        public List<Product> products {get; set;}

        public List<Product> getProducts(){
            products = new List<Product>(){
                new Product(){ Id=1, Name="Product A", Price=1200.00M},
                new Product(){ Id=2, Name="Product B", Price=1300.00M},
                new Product(){ Id=3, Name="Product C", Price=1400.00M}
            };
            return products;
        }
    }

}