using System.Globalization;
using System.Windows.Controls;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace ClusterizerGui.Utils.ValidationRules
{
    /// <summary>
    /// Règle de validation pour un entier borné entre MinValue et MaxValue
    /// </summary>
    public class InRangeIntegerValidationRule : ValidationRule
    {
        /// <summary>
        /// La valeur minimale autorisé pour la validation
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// La valeur maximale autorisé pour la validation
        /// </summary>
        public int MaxValue { get; set; }

        /// <summary>
        /// Valide la donnée si elle est comprise entre la valeur min et la valeur max
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value as string, out var i))
            {
                return new ValidationResult(false, "should be use with a int");
            }

            if (i < MinValue)
            {
                return new ValidationResult(false, $"value should be greater than {MinValue} (or equal)");
            }

            return i > MaxValue
                ? new ValidationResult(false, $"value should be lower than {MaxValue} (or equal)")
                : new ValidationResult(true, null);
        }
    }
}
