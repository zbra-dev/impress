namespace Impress.Validation
{
    public class PropertyValidator<T> : CompositeValidator<T>
    {

        private string propertyName;
        private string typeName;

        public PropertyValidator(string typeName, string propertyName)
        {
            this.propertyName = propertyName;
            this.typeName = typeName;
        }

        public override ValidationResult Validate(T candidate)
        {
            var result = new ValidationResult();

            var valueResult = base.Validate(candidate);

            foreach (PropertyInvalidationReason reason in valueResult.Reasons().Select(r => new PropertyInvalidationReason(typeName, propertyName, r)))
            {
                result.AddReason(reason);
            }

            return result;
        }
    }
}
