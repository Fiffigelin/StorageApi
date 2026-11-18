
using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs;

/** varför har denna klass en massa valideringar men inte ProductDto.cs?
* Jo för detta är data som ska in i databasen.. 
* ProductDto.cs är den samlade datan som ska visas
* En dto eller viewmodel kan även vara data från flera klasser/entities som frontenden behöver ha
*/

public class CreateProductDto
{
    [Required(ErrorMessage = "Produktnamn måste anges.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Produktnamnet måste vara mellan 2 och 100 tecken.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 1000000,
        ErrorMessage = "Priset måste vara mellan 1 och 1 000 000.")]
    public int Price { get; set; }

    public string Category { get; set; } = string.Empty;

    public string Shelf { get; set; } = string.Empty;

    [Range(0, 100000,
        ErrorMessage = "Antalet måste vara mellan 0 och 100 000.")]
    public int Count { get; set; }

    [StringLength(500,
        ErrorMessage = "Beskrivningen får vara högst 500 tecken.")]
    public string Description { get; set; } = string.Empty;
}
