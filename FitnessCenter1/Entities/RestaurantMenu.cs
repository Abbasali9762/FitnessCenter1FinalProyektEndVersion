namespace FitnessCenter1.Entities
{
    public class RestaurantMenu
    {
        public int RestaurantMenuId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public bool IsSpecialOffer { get; set; }
        public decimal SpecialOfferPrice { get; set; }

        public virtual ICollection<RestaurantOrder>? RestaurantOrders { get; set; }
    }
}