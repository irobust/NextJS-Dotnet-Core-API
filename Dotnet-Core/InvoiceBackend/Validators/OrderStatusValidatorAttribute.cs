using System.ComponentModel.DataAnnotations;

namespace InvoiceBackend.Validators{
    public class OrderStatusValidatorAttribute: ValidationAttribute{
        public OrderStatusValidatorAttribute(){
            ErrorMessage = "Invalid order status";
        }

        public override bool IsValid(object? value){
            var allowedStatuses = new []{ "created", "paid", "delivered", "completed" };
            bool result = allowedStatuses.Any(allowedStatus => value.ToString().Contains(allowedStatus));
            return result;
        }
    }
}