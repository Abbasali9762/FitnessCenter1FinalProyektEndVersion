using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class RestaurantService : BaseService, IRestaurantService
    {
        private readonly IEmailService _emailService;

        public RestaurantService(FitnessCenterDbContext context, IEmailService emailService) : base(context)
        {
            _emailService = emailService;
        }

        public RestaurantService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<RestaurantOrder> PlaceOrder(int userId, int menuItemId, int quantity)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var menuItem = await _context.RestaurantMenus.FindAsync(menuItemId);
            if (menuItem == null) throw new Exception("Menu item not found");

            decimal totalPrice = menuItem.IsSpecialOffer ?
                menuItem.SpecialOfferPrice * quantity :
                menuItem.Price * quantity;

            if (user.Money < totalPrice)
                throw new Exception("Insufficient funds");

            var order = new RestaurantOrder
            {
                UserId = userId,
                RestaurantMenuId = menuItemId,
                Quantity = quantity,
                TotalPrice = totalPrice,
                OrderTime = DateTime.Now
            };

            user.Money -= totalPrice;

            _context.RestaurantOrders.Add(order);
            await _context.SaveChangesAsync();

            string orderDetails = $"Order ID: {order.RestaurantOrderId}\n" +
                                $"Item: {menuItem.Name}\n" +
                                $"Quantity: {quantity}\n" +
                                $"Total: {totalPrice:C}";

            await _emailService.SendOrderConfirmationEmail(user.Email, orderDetails);

            return order;
        }

        public async Task<List<RestaurantMenu>> GetMenuByGender(string gender)
        {
            return await _context.RestaurantMenus
                .Where(rm => rm.Category == gender || rm.Category == "Both")
                .ToListAsync();
        }

        public async Task<List<RestaurantMenu>> GetSpecialOffers()
        {
            return await _context.RestaurantMenus
                .Where(rm => rm.IsSpecialOffer)
                .ToListAsync();
        }

        public async Task<RestaurantOrder> GetOrderById(int orderId)
        {
            return await _context.RestaurantOrders
                .Include(ro => ro.User)
                .Include(ro => ro.RestaurantMenu)
                .FirstOrDefaultAsync(ro => ro.RestaurantOrderId == orderId);
        }

        public async Task<List<RestaurantOrder>> GetAllOrders()
        {
            return await _context.RestaurantOrders
                .Include(ro => ro.User)
                .Include(ro => ro.RestaurantMenu)
                .ToListAsync();
        }
    }
}