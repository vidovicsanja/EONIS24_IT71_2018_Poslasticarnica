namespace Poslasticarnica.Model.dto
{
    public class CreateCheckoutSessionDTO
    {
        public long KorisnikId { get; set; }
        public List<CheckoutDto> Items { get; set; }
    }
}
