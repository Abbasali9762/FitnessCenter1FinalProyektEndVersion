using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;
using FitnessCenter1.Services;
using FitnessCenter1.Configurations;
using System.Runtime.InteropServices;


class Program
{
    static async Task Main(string[] args)
    {
        using (var context = new FitnessCenterDbContext())
        {
            await Datas(context);

            var emailService = new EmailService();
            var notificationService = new NotificationService(context);
            var userService = new UserService(context, emailService);
            var parkingService = new ParkingService(context);
            var restaurantService = new RestaurantService(context, emailService);
            var fitnessService = new FitnessService(context, emailService, notificationService);
            var trainerService = new TrainerService(context);
            var membershipService = new MembershipService(context, emailService);
            var paymentService = new PaymentService(context, emailService);
            var equipmentService = new EquipmentService(context);
            var equipmentUsageService = new EquipmentUsageService(context);

            await RunApplication(userService, parkingService, restaurantService, fitnessService,
                               trainerService, membershipService, paymentService, equipmentService,
                               equipmentUsageService, notificationService);
        }
    }

    static async Task Datas(FitnessCenterDbContext context)
    {
        if (!await context.FitnessPrograms.AnyAsync(fp => fp.GenderTarget == "Male"))
        {
            context.FitnessPrograms.AddRange(new[]
            {
                new FitnessProgram { Name = "Sauna", Description = "Relaxing sauna session", Price = 60, GenderTarget = "Male" },
                new FitnessProgram { Name = "Jacuzzi", Description = "Relaxation with Jacuzzi", Price = 40, GenderTarget = "Male" },
                new FitnessProgram { Name = "Fitness", Description = "Workout in fitness area", Price = 30, GenderTarget = "Male" },
                new FitnessProgram { Name = "Massage room", Description = "Professional massage", Price = 80, GenderTarget = "Male" }
            });
        }

        if (!await context.FitnessPrograms.AnyAsync(fp => fp.GenderTarget == "Female"))
        {
            context.FitnessPrograms.AddRange(new[]
            {
                new FitnessProgram { Name = "Pilates", Description = "Pilates class", Price = 35, GenderTarget = "Female" },
                new FitnessProgram { Name = "Yoga", Description = "Yoga session", Price = 30, GenderTarget = "Female" },
                new FitnessProgram { Name = "Gym", Description = "Main gym access", Price = 25, GenderTarget = "Female" },
                new FitnessProgram { Name = "Jacuzzi", Description = "Relaxation with Jacuzzi", Price = 40, GenderTarget = "Female" },
                new FitnessProgram { Name = "Swimming pool", Description = "Swimming pool access", Price = 20, GenderTarget = "Female" }
            });
        }

        if (!await context.RestaurantMenus.AnyAsync())
        {
            context.RestaurantMenus.AddRange(new[]
            {
                new RestaurantMenu { Name = "Steak", Description = "Grilled steak with vegetables", Price = 25, Category = "Male" },
                new RestaurantMenu { Name = "Protein drink", Description = "High protein drink", Price = 8, Category = "Male" },
                new RestaurantMenu { Name = "Salad", Description = "Fresh garden salad", Price = 12, Category = "Female" },
                new RestaurantMenu { Name = "Smoothie", Description = "Fruit smoothie", Price = 7, Category = "Female" },
                new RestaurantMenu { Name = "Coffee", Description = "Premium coffee", Price = 5, Category = "Both" },
                new RestaurantMenu { Name = "Tea", Description = "Herbal tea", Price = 3, Category = "Both" },
                new RestaurantMenu { Name = "Pure", Description = "Boiled and smashed potatoes with salt and pepper.", Price = 8, Category = "Both", IsSpecialOffer = true, SpecialOfferPrice = 4 },
                new RestaurantMenu { Name = "Fish", Description = "Khal khal style grilled", Price = 30, Category = "Both", IsSpecialOffer = true, SpecialOfferPrice = 4 },
                new RestaurantMenu { Name = "Jam", Description = "Anjir jam", Price = 5, Category = "Both", IsSpecialOffer = true, SpecialOfferPrice = 4 }
            });
        }

        if (!await context.ParkingAreas.AnyAsync())
        {
            context.ParkingAreas.AddRange(new[]
            {
                new ParkingArea { Name = "Main Parking", TotalSpots = 50, AvailableSpots = 50 },
                new ParkingArea { Name = "VIP Parking", TotalSpots = 10, AvailableSpots = 10 }
            });
        }

        if (!await context.Trainers.AnyAsync())
        {
            context.Trainers.AddRange(new[]
            {
                new Trainer { Name = "John", Surname = "Smith", Specialization = "Bodybuilding", HourlyRate = 50, IsAvailable = true },
                new Trainer { Name = "Emma", Surname = "Johnson", Specialization = "Yoga", HourlyRate = 40, IsAvailable = true }
            });
        }

        if (!await context.Equipment.AnyAsync())
        {
            context.Equipment.AddRange(new[]
            {
                new Equipment { Name = "Treadmill", Description = "Professional treadmill", Quantity = 10, AvailableQuantity = 10, Category = "Cardio", PurchaseDate = DateTime.Now.AddMonths(-6) },
                new Equipment { Name = "Dumbbell set", Description = "5-50kg dumbbell set", Quantity = 5, AvailableQuantity = 5, Category = "Strength", PurchaseDate = DateTime.Now.AddMonths(-3) },
                new Equipment { Name = "Yoga mat", Description = "Premium yoga mat", Quantity = 20, AvailableQuantity = 20, Category = "Yoga", PurchaseDate = DateTime.Now.AddMonths(-1) }
            });
        }

        await context.SaveChangesAsync();
    }

    static async Task RunApplication(IUserService userService, IParkingService parkingService,
                               IRestaurantService restaurantService, IFitnessService fitnessService,
                               ITrainerService trainerService, IMembershipService membershipService,
                               IPaymentService paymentService, IEquipmentService equipmentService,
                               IEquipmentUsageService equipmentUsageService, INotificationService notificationService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Fitness Center Management System");
            Console.WriteLine("=================================");
            Console.WriteLine("\n1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Forgot Password");
            Console.WriteLine("4. Exit");
            Console.Write("Select: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await HandleLogin(userService, parkingService, restaurantService, fitnessService,
                                    trainerService, membershipService, paymentService, equipmentService,
                                    equipmentUsageService, notificationService);
                    break;
                case "2":
                    await HandleRegistration(userService);
                    break;
                case "3":
                    await HandleForgotPassword(userService);
                    break;
                case "4":
                    Console.WriteLine("Exiting system...");
                    Thread.Sleep(1000);
                    return;
                default:
                    Console.WriteLine("Wrong choice. Try again.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    static async Task HandleForgotPassword(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("=== Forgot Password ===");
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();

        try
        {
            var success = await userService.ForgotPassword(email);
            if (success)
            {
                Console.WriteLine("\nOTP code sent to your email.");

                Console.Write("Enter OTP: ");
                string enteredOtp = Console.ReadLine();

                if (await userService.VerifyOTP(email, enteredOtp))
                {
                    Console.Write("Enter new password: ");
                    string newPassword = Console.ReadLine();

                    var resetSuccess = await userService.ResetPassword(email, newPassword);
                    if (resetSuccess)
                    {
                        Console.WriteLine("\nPassword reset successfully!");
                    }
                    else
                    {
                        Console.WriteLine("\nPassword reset failed.");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid OTP. Password reset failed.");
                }
            }
            else
            {
                Console.WriteLine("\nEmail not found or password reset failed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        Thread.Sleep(2500);
    }

    static async Task HandleLogin(IUserService userService, IParkingService parkingService,
                                IRestaurantService restaurantService, IFitnessService fitnessService,
                                ITrainerService trainerService, IMembershipService membershipService,
                                IPaymentService paymentService, IEquipmentService equipmentService,
                                IEquipmentUsageService equipmentUsageService, INotificationService notificationService)
    {
        Console.Clear();
        Console.WriteLine("=== Login ===");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        try
        {
            var user = await userService.LoginUser(username, password);
            Console.WriteLine($"\nWelcome, {user.Name} {user.Surname}!");
            Thread.Sleep(1500);
            Console.Clear();

            if (user.Gender == "Male")
            {
                await ShowMaleServices(user, fitnessService, restaurantService, parkingService,
                                     trainerService, membershipService, paymentService, equipmentService,
                                     equipmentUsageService, notificationService);
            }
            else
            {
                await ShowFemaleServices(user, fitnessService, restaurantService, parkingService,
                                       trainerService, membershipService, paymentService, equipmentService,
                                       equipmentUsageService, notificationService);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nLogin failed: {ex.Message}");
            Thread.Sleep(2000);
        }
    }

    static async Task HandleRegistration(IUserService userService)
    {
        Console.Clear();
        Console.WriteLine("=== Registration ===");
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Surname: ");
        var surname = Console.ReadLine();
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();
        Console.Write("Email: ");
        var email = Console.ReadLine();
        Console.Write("Gender (Male/Female): ");
        var gender = Console.ReadLine();
        Console.Write("Do you have a car? (yes/no): ");
        bool isCar = Console.ReadLine().ToLower() == "yes";
        Console.Write("Initial balance: ");
        decimal initialMoney = decimal.Parse(Console.ReadLine());

        try
        {
            var user = await userService.RegisterUser(name, surname, username, password, email, gender, isCar, initialMoney);
            Console.WriteLine($"\nOTP code has benn sent succesfully to your email.");

            Console.Write("Enter OTP verification code : ");
            string enteredOtp = Console.ReadLine();

            if (await userService.VerifyOTP(email, enteredOtp))
            {
                Console.WriteLine("\nRegistration successful!");
            }
            else
            {
                Console.WriteLine("\nOTP is incorrect! Registration cancelled.");
                await userService.DeleteUser(user.UserId);
            }

            Thread.Sleep(2500);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nRegistration error: {ex.Message}");
            Thread.Sleep(2000);
        }
    }

    static async Task ShowMaleServices(User user, IFitnessService fitnessService, IRestaurantService restaurantService,
                                     IParkingService parkingService, ITrainerService trainerService,
                                     IMembershipService membershipService, IPaymentService paymentService,
                                     IEquipmentService equipmentService, IEquipmentUsageService equipmentUsageService,
                                     INotificationService notificationService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Male Services - Balance: {user.Money:C} ===");
            Console.WriteLine("1. Sauna (60)");
            Console.WriteLine("2. Jacuzzi (40)");
            Console.WriteLine("3. Fitness (30)");
            Console.WriteLine("4. Massage room (80)");
            Console.WriteLine("5. Restaurant");
            Console.WriteLine("6. Parking");
            Console.WriteLine("7. Trainer session");
            Console.WriteLine("8. Special offers");
            Console.WriteLine("9. Membership");
            Console.WriteLine("10. Payment");
            Console.WriteLine("11. Equipment");
            Console.WriteLine("12. Notifications");
            Console.WriteLine("13. Exit");
            Console.Write("Ente your choice :");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Sauna using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nSauna used successfully!");
                    await UseFitnessProgram(user, 1, fitnessService);
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("Jacuzzi using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nJacuzzi used successfully!");
                    await UseFitnessProgram(user, 2, fitnessService);
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Fitness using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nFitness used successfully!");
                    await UseFitnessProgram(user, 3, fitnessService);
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Massage room using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nMassage room used successfully!");
                    await UseFitnessProgram(user, 4, fitnessService);
                    break;
                case "5":
                    await ShowRestaurantMenu(user, restaurantService, "Male");
                    break;
                case "6":
                    await HandleParking(user, parkingService);
                    break;
                case "7":
                    await HandleTrainerSession(user, trainerService);
                    break;
                case "8":
                    await ShowSpecialOffers(user, fitnessService, restaurantService);
                    break;
                case "9":
                    await HandleMembership(user, membershipService);
                    break;
                case "10":
                    await HandlePayment(user, paymentService);
                    break;
                case "11":
                    await HandleEquipment(user, equipmentService, equipmentUsageService);
                    break;
                case "12":
                    await HandleNotifications(user, notificationService);
                    break;
                case "13":
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(1000);
                    return;
                default:
                    Console.WriteLine("Wrong choice.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    static async Task ShowFemaleServices(User user, IFitnessService fitnessService, IRestaurantService restaurantService,
                                       IParkingService parkingService, ITrainerService trainerService,
                                       IMembershipService membershipService, IPaymentService paymentService,
                                       IEquipmentService equipmentService, IEquipmentUsageService equipmentUsageService,
                                       INotificationService notificationService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Female Services - Balance: {user.Money:C} ===");
            Console.WriteLine("1. Pilates (35)");
            Console.WriteLine("2. Yoga (30)");
            Console.WriteLine("3. Gym (25)");
            Console.WriteLine("4. Jacuzzi (40)");
            Console.WriteLine("5. Swimming pool (20)");
            Console.WriteLine("6. Restaurant");
            Console.WriteLine("7. Parking");
            Console.WriteLine("8. Trainer session");
            Console.WriteLine("9. Special offers");
            Console.WriteLine("10. Membership");
            Console.WriteLine("11. Payment");
            Console.WriteLine("12. Equipment");
            Console.WriteLine("13. Notifications");
            Console.WriteLine("14. Exit");
            Console.Write("Ente your choice :");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Pilates using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nPilates used successfully!");
                    await UseFitnessProgram(user, 1, fitnessService);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Yoga using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nYoga used successfully!");
                    await UseFitnessProgram(user, 2, fitnessService);
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("Gym using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nGym used successfully!");
                    await UseFitnessProgram(user, 3, fitnessService);
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Swimming pool using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nJacuzzi used successfully!");
                    await UseFitnessProgram(user, 4, fitnessService);
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Swimming pool using...");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("\nSwimming pool used successfully!");
                    await UseFitnessProgram(user, 5, fitnessService);
                    break;
                case "6":
                    await ShowRestaurantMenu(user, restaurantService, "Female");
                    break;
                case "7":
                    await HandleParking(user, parkingService);
                    break;
                case "8":
                    await HandleTrainerSession(user, trainerService);
                    break;
                case "9":
                    await ShowSpecialOffers(user, fitnessService, restaurantService);
                    break;
                case "10":
                    await HandleMembership(user, membershipService);
                    break;
                case "11":
                    await HandlePayment(user, paymentService);
                    break;
                case "12":
                    await HandleEquipment(user, equipmentService, equipmentUsageService);
                    break;
                case "13":
                    await HandleNotifications(user, notificationService);
                    break;
                case "14":
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(1000);
                    return;
                default:
                    Console.WriteLine("Wrong choice.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    static async Task UseFitnessProgram(User user, int programId, IFitnessService fitnessService)
    {
        try
        {
            var usage = await fitnessService.UseFitnessService(user.UserId, programId);
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("");
            Thread.Sleep(2000);
        }
    }

    static async Task ShowRestaurantMenu(User user, IRestaurantService restaurantService, string gender)
    {
        Console.Clear();
        Console.WriteLine("=== Restaurant Menu ===");

        var menu = await restaurantService.GetMenuByGender(gender);
        Console.WriteLine("\nRestaurant Menu:");
        for (int i = 0; i < menu.Count; i++)
        {
            var item = menu[i];
            string priceInfo = item.IsSpecialOffer ?
                $"{item.SpecialOfferPrice:C} (Special offer!)" :
                $"{item.Price:C}";
            Console.WriteLine($"{i + 1}. {item.Name} - {item.Description} - {priceInfo}");
        }

        Console.Write("\nSelect item (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= menu.Count)
        {
            Console.Write("Quantity: ");
            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                try
                {
                    var order = await restaurantService.PlaceOrder(user.UserId, menu[choice - 1].RestaurantMenuId, quantity);
                    Console.WriteLine($"\nOrder placed successfully. Remaining balance: {user.Money:C}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nCould not place order: {ex.Message}");
                }
            }
        }
        Thread.Sleep(2000);
    }

    static async Task HandleParking(User user, IParkingService parkingService)
    {
        if (!user.IsCar)
        {
            Console.WriteLine("\nYou don't have a registered car. Parking reservation is only available for users with cars.");
            Thread.Sleep(2000);
            return;
        }

        Console.Clear();
        Console.WriteLine("=== Parking Reservation ===");

        var availableAreas = await parkingService.GetAvailableParkingAreas();
        if (!availableAreas.Any())
        {
            Console.WriteLine("No parking areas available.");
            Thread.Sleep(1500);
            return;
        }

        Console.WriteLine("\nAvailable Parking Areas:");
        for (int i = 0; i < availableAreas.Count; i++)
        {
            var area = availableAreas[i];
            Console.WriteLine($"{i + 1}. {area.Name} - {area.AvailableSpots}/{area.TotalSpots} spots available");
        }

        Console.Write("\nSelect parking area (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= availableAreas.Count)
        {
            Console.Write("Enter spot location: ");
            var spotLocation = Console.ReadLine();

            try
            {
                var reservation = await parkingService.ReserveParkingSpot(user.UserId, availableAreas[choice - 1].ParkingAreaId, spotLocation);
                Console.WriteLine($"\nParking reserved successfully: {spotLocation}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nCould not reserve parking: {ex.Message}");
            }
        }
        Thread.Sleep(2000);
    }
    static async Task HandleTrainerSession(User user, ITrainerService trainerService)
    {
        Console.Clear();
        Console.WriteLine("=== Trainer Session ===");

        var availableTrainers = await trainerService.GetAvailableTrainers();
        if (!availableTrainers.Any())
        {
            Console.WriteLine("No trainers available.");
            Thread.Sleep(1500);
            return;
        }

        Console.WriteLine("\nAvailable Trainers:");
        for (int i = 0; i < availableTrainers.Count; i++)
        {
            var trainer = availableTrainers[i];
            Console.WriteLine($"{i + 1}. {trainer.Name} {trainer.Surname} - {trainer.Specialization} - {trainer.HourlyRate:C}/hour");
        }

        Console.Write("\nSelect trainer (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= availableTrainers.Count)
        {
            Console.Write("Session date and time (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime sessionTime))
            {
                Console.Write("Duration (minutes): ");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    try
                    {
                        var session = await trainerService.BookTrainingSession(user.UserId, availableTrainers[choice - 1].TrainerId, sessionTime, duration);
                        Console.WriteLine($"\nTrainer session reserved successfully. Remaining balance: {user.Money:C}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nCould not reserve session: {ex.Message}");
                    }
                }
            }
        }
        Thread.Sleep(2000);
    }

    static async Task ShowSpecialOffers(User user, IFitnessService fitnessService, IRestaurantService restaurantService)
    {
        Console.Clear();
        Console.WriteLine("=== Special Offers ===");

        var fitnessOffers = await fitnessService.GetSpecialOffers();
        var restaurantOffers = await restaurantService.GetSpecialOffers();

        Console.WriteLine("\nFitness Programs:");
        for (int i = 0; i < fitnessOffers.Count; i++)
        {
            var offer = fitnessOffers[i];
            Console.WriteLine($"{i + 1}. {offer.Name} - {offer.Description} - {offer.SpecialOfferPrice:C} (was {offer.Price:C})");
        }

        Console.WriteLine("\nRestaurant Items:");
        for (int i = 0; i < restaurantOffers.Count; i++)
        {
            var offer = restaurantOffers[i];
            Console.WriteLine($"{i + fitnessOffers.Count + 1}. {offer.Name} - {offer.Description} - {offer.SpecialOfferPrice:C} (was {offer.Price:C})");
        }

        Console.Write("\nSelect offer (0 to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0)
        {
            if (choice <= fitnessOffers.Count)
            {
                Console.WriteLine($"\n{fitnessOffers[choice - 1].Name} succesfully used!");
                await UseFitnessProgram(user, fitnessOffers[choice - 1].FitnessProgramId, fitnessService);
            }
            else
            {
                int restaurantChoice = choice - fitnessOffers.Count - 1;
                if (restaurantChoice < restaurantOffers.Count)
                {
                    Console.Write("Quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        try
                        {
                            var order = await restaurantService.PlaceOrder(user.UserId, restaurantOffers[restaurantChoice].RestaurantMenuId, quantity);
                            Console.WriteLine($"\nOrder placed successfully. Remaining balance: {user.Money:C}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nCould not place order: {ex.Message}");
                        }
                    }
                }
            }
        }
        Thread.Sleep(2000);
    }

    static async Task HandleMembership(User user, IMembershipService membershipService)
    {
        Console.Clear();
        Console.WriteLine("=== Membership Management ===");
        Console.WriteLine("1. New membership");
        Console.WriteLine("2. Current memberships");
        Console.WriteLine("3. Cancel membership");
        Console.Write("Select: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Membership type (Basic/Premium/VIP): ");
                var type = Console.ReadLine();
                Console.Write("Duration (days): ");
                if (int.TryParse(Console.ReadLine(), out int days))
                {
                    var startDate = DateTime.Now;
                    var endDate = startDate.AddDays(days);
                    var price = days * 10;

                    try
                    {
                        var membership = await membershipService.CreateMembership(user.UserId, type, startDate, endDate, price);
                        Console.WriteLine($"\nMembership created successfully. Remaining balance: {user.Money:C}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nCould not create membership: {ex.Message}");
                    }
                }
                break;
            case "2":
                var memberships = await membershipService.GetUserMemberships(user.UserId);
                Console.WriteLine("\nCurrent Memberships:");
                foreach (var membership in memberships)
                {
                    Console.WriteLine($"{membership.Type} - {membership.StartDate:yyyy-MM-dd} to {membership.EndDate:yyyy-MM-dd} - {membership.Price:C} - {(membership.IsActive ? "Active" : "Inactive")}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            case "3":
                var activeMemberships = await membershipService.GetUserMemberships(user.UserId);
                var active = activeMemberships.FirstOrDefault(m => m.IsActive);
                if (active != null)
                {
                    try
                    {
                        var success = await membershipService.CancelMembership(active.MembershipId);
                        if (success) Console.WriteLine("\nMembership cancelled successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nCould not cancel membership: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo active membership found.");
                }
                break;
        }
        Thread.Sleep(2000);
    }

    static async Task HandlePayment(User user, IPaymentService paymentService)
    {
        Console.Clear();
        Console.WriteLine("=== Payment Management ===");
        Console.WriteLine("1. New payment");
        Console.WriteLine("2. Payment history");
        Console.Write("Select: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.Write("Description: ");
                    var description = Console.ReadLine();

                    try
                    {
                        var payment = await paymentService.CreatePayment(user.UserId, "Card", amount, description);
                        var processed = await paymentService.ProcessPayment(payment.PaymentId);
                        if (processed) Console.WriteLine($"\nPayment completed successfully. Remaining balance: {user.Money:C}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nCould not process payment: {ex.Message}");
                    }
                }
                break;
            case "2":
                var payments = await paymentService.GetUserPayments(user.UserId);
                Console.WriteLine("\nPayment History:");
                foreach (var payment in payments)
                {
                    Console.WriteLine($"{payment.PaymentDate:yyyy-MM-dd HH:mm} - {payment.Amount:C} - {payment.Status} - {payment.Description}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
        }
        Thread.Sleep(2000);
    }

    static async Task HandleEquipment(User user, IEquipmentService equipmentService, IEquipmentUsageService equipmentUsageService)
    {
        Console.Clear();
        Console.WriteLine("=== Equipment Management ===");
        Console.WriteLine("1. Available equipment");
        Console.WriteLine("2. Use equipment");
        Console.WriteLine("3. Current usages");
        Console.Write("Select: ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                var equipment = await equipmentService.GetAllEquipment();
                Console.WriteLine("\nAvailable Equipment:");
                foreach (var item in equipment)
                {
                    Console.WriteLine($"{item.Name} - {item.Category} - {item.AvailableQuantity}/{item.Quantity} available - {(item.NeedsMaintenance ? "Needs maintenance" : "Good condition")}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
            case "2":
                var availableEquipment = await equipmentService.GetAllEquipment();
                var usableEquipment = availableEquipment.Where(e => e.IsAvailable).ToList();

                Console.WriteLine("\nUsable Equipment:");
                for (int i = 0; i < usableEquipment.Count; i++)
                {
                    var item = usableEquipment[i];
                    Console.WriteLine($"{i + 1}. {item.Name} - {item.Category}");
                }

                Console.Write("\nSelect equipment (0 to cancel): ");
                if (int.TryParse(Console.ReadLine(), out int eqChoice) && eqChoice > 0 && eqChoice <= usableEquipment.Count)
                {
                    try
                    {
                        var usage = await equipmentUsageService.StartUsage(user.UserId, usableEquipment[eqChoice - 1].EquipmentId);
                        Console.WriteLine($"\nEquipment usage started: {usableEquipment[eqChoice - 1].Name}");
                        Console.WriteLine("Type 'finish' when done.");

                        var input = Console.ReadLine();
                        if (input?.ToLower() == "finish")
                        {
                            var endedUsage = await equipmentUsageService.EndUsage(usage.EquipmentUsageId);
                            Console.WriteLine($"\nEquipment usage finished. Duration: {endedUsage.DurationMinutes} minutes");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nCould not use equipment: {ex.Message}");
                    }
                }
                break;
            case "3":
                var currentUsages = await equipmentUsageService.GetCurrentUsages();
                Console.WriteLine("\nCurrent Usages:");
                foreach (var usage in currentUsages)
                {
                    Console.WriteLine($"{usage.User.Name} - {usage.Equipment.Name} - started at {usage.StartTime:HH:mm}");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                break;
        }
        Thread.Sleep(1500);
    }

    static async Task HandleNotifications(User user, INotificationService notificationService)
    {
        Console.Clear();
        Console.WriteLine("=== Notifications ===");

        var notifications = await notificationService.GetUserNotifications(user.UserId);
        var unreadCount = await notificationService.GetUnreadNotificationCount(user.UserId);

        Console.WriteLine($"\nNotifications ({unreadCount} unread)");

        for (int i = 0; i < notifications.Count; i++)
        {
            var notification = notifications[i];
            Console.WriteLine($"{i + 1}. [{(notification.IsRead ? " " : "X")}] {notification.Title} - {notification.CreatedDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"   {notification.Message}");
            Console.WriteLine();
        }

        if (notifications.Any())
        {
            Console.Write("Select notification number to mark as read (0 to cancel): ");
            if (int.TryParse(Console.ReadLine(), out int notifChoice) && notifChoice > 0 && notifChoice <= notifications.Count)
            {
                await notificationService.MarkAsRead(notifications[notifChoice - 1].NotificationId);
                Console.WriteLine("\nNotification marked as read.");
            }
        }
        else
        {
            Console.WriteLine("No notifications.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}