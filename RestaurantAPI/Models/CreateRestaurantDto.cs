using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI;
public class CreateRestaurantDto
{
    //za pomocą atrybutów można określić warunki dla poszczególnych pól
    //muszą być spójne z tym co określiliśmy w DbContext
    [Required]
    [MaxLength(25)]
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public bool HasDelivery { get; set; }
    public string ContactEmail { get; set; }
    public string ContactNumber { get; set; }
    [Required]
    [MaxLength(50)]
    public string City { get; set; }
    [Required]
    [MaxLength(50)]
    public string Street { get; set; }
    public string PostalCode { get; set; }
}

/* pozostałe wbudowane atrubuty walidacji
[CreditCard] - potwierdza że właściwość ma format karty kredytowej
[Compare("otherProperty")] - sprawdza, czy dwie właściwości w modelu są zgodne
[EmailAddress] - sprawdza czy właściwość ma format wiadomości e-mail
[Phone] - sprawdza czy właściwość ma format telefonu
[Range(minValue, maxValue)] - sprawdza czy właściwość mieści się w określonym przedziale
[RegularExpression(pattern)] - sprawdza czy właściwość pasuje do określonego wyrażenia regularnego
*/