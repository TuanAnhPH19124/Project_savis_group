using GlobalApi.Models;
using Microsoft.AspNetCore.Identity;

namespace GlobalApi.Data
{
    public static class SeedRecords
    {
        public static async Task SeedUserAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var _context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!await roleManager.RoleExistsAsync("User"))
                    await roleManager.CreateAsync(new IdentityRole("User"));

                if (!_context.Rooms.Any()){
                    var newListRoom = new List<Room>(){
                        new Room
                        { 
                            Name = "LUCK APART 8 - Hanoi Westlake Balcony Studio", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Long Biên",
                            Ward = "Phường Thượng Thanh",
                            AvailableUnits = 4,
                            Wifi = true,
                            Laundry = false,
                            Price = 200,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Madelise Palace Hotel & Spa", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Cầu Giấy",
                            Ward = "Phường Nghĩa Tân",
                            AvailableUnits = 7,
                            Wifi = true,
                            Laundry = true,
                            Price = 200,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Golden Sail Hotel & Spa", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Hoàn Kiếm",
                            Ward = "Phường Dịch Vọng Hậu",
                            AvailableUnits = 3,
                            Wifi = true,
                            Laundry = false,
                            Price = 300,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Ruby Serviced Apartment", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Tây Hồ",
                            Ward = "Phường Quảng An",
                            AvailableUnits = 3,
                            Wifi = false,
                            Laundry = true,
                            Price = 100,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Amira Hotel HanoiMở trong cửa sổ mới", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Ba Đình",
                            Ward = "Phường Phúc Xá",
                            AvailableUnits = 4,
                            Wifi = true,
                            Laundry = true,
                            Price = 200,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "The Flower Boutique Hotel & TravelMở trong cửa sổ mới", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Hai Bà Trưng",
                            Ward = "Phường Cống Vị",
                            AvailableUnits = 1,
                            Wifi = true,
                            Laundry = true,
                            Price = 300,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "AN HOMESTAY", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Hai Bà Trưng",
                            Ward = "Phường Liễu Giai",
                            AvailableUnits = 2,
                            Wifi = false,
                            Laundry = false,
                            Price = 150,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Gloud Hotel", 
                            City = "Thành phố Hà Nội",
                            District = "Huyện Đông Anh",
                            Ward = "Xã Xuân Nộn",
                            AvailableUnits = 5,
                            Wifi = false,
                            Laundry = false,
                            Price = 250,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Hà Nội Memoir Residences", 
                            City = "Thành phố Hà Nội",
                            District = "Quận Nam Từ Liêm",
                            Ward = "Phường Cầu Diễn",
                            AvailableUnits = 3,
                            Wifi = true,
                            Laundry = false,
                            Price = 400,
                            Photo = "https://loremflickr.com/640/480/business"
                        },
                        new Room
                        { 
                            Name = "Thien Phu Garden HotelMở trong cửa sổ mới", 
                            City = "Thành phố Hà Nội",
                            District = "Huyện Mê Linh",
                            Ward = "Xã Thạch Đà",
                            AvailableUnits = 4,
                            Wifi = true,
                            Laundry = true,
                            Price = 300,
                            Photo = "https://loremflickr.com/640/480/business"
                        }
                    };
                    await _context.AddRangeAsync(newListRoom);
                }

                await _context.SaveChangesAsync();         
            }
        }  
    }
    
}
