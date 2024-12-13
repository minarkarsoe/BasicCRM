using System.ComponentModel.DataAnnotations;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object _dependentValue;

    public RequiredIfAttribute(string dependentProperty, object dependentValue)
    {
        _dependentProperty = dependentProperty;
        _dependentValue = dependentValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dependentProperty = validationContext.ObjectType.GetProperty(_dependentProperty);
        if (dependentProperty == null)
            return new ValidationResult($"Property '{_dependentProperty}' not found.");

        var dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance);

        if (dependentValue?.ToString() == _dependentValue?.ToString())
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
        }

        return ValidationResult.Success;
    }
}
