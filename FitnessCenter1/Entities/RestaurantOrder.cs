namespace FitnessCenter1.Entities
{
    public class RestaurantOrder
    {
        public int RestaurantOrderId { get; set; }
        public int UserId { get; set; }
        public int RestaurantMenuId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual User? User { get; set; }
        public virtual RestaurantMenu? RestaurantMenu { get; set; }
    }
}