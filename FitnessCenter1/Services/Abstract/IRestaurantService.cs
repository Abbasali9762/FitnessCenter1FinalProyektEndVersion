using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IRestaurantService
    {
        Task<RestaurantOrder> PlaceOrder(int userId, int menuItemId, int quantity);
        Task<List<RestaurantMenu>> GetMenuByGender(string gender);
        Task<List<RestaurantMenu>> GetSpecialOffers();
        Task<RestaurantOrder> GetOrderById(int orderId);
        Task<List<RestaurantOrder>> GetAllOrders();
    }
}