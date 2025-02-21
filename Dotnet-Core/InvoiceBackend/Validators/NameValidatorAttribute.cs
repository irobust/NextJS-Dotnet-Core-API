using System.ComponentModel.DataAnnotations;

namespace InvoiceBackend.Validators{
    public class NameValidatorAttribute: RegularExpressionAttribute{
        public NameValidatorAttribute(): base(@"^[a-z0-9-]+$"){
            ErrorMessage = "Name must only be alpha, numeric and dash";
        }
    }
}