using System.ComponentModel.DataAnnotations;
namespace Libtech.Api.DTOs;

// Updated: implements IValidatableObject to support cross-field validation
public class BookRequestDTO : IValidatableObject {
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Author { get; set; } = string.Empty;
    [Required] public string ISBN { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0.")]
    public int TotalCopies { get; set; }
    // Added: AvailableCopies field with >= 0 validation
    [Range(0, int.MaxValue, ErrorMessage = "AvailableCopies must be greater than or equal to 0.")]
    public int AvailableCopies { get; set; }

    // Added: ensures AvailableCopies does not exceed TotalCopies
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        if (AvailableCopies > TotalCopies)
            yield return new ValidationResult(
                "AvailableCopies must not exceed TotalCopies.",
                new[] { nameof(AvailableCopies) });
    }
}
