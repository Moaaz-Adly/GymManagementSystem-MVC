using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public class GymDbContextSeed
    {
        



        public static bool SeedData ( GYMDbContext dbContext )
        {
            try
            {
                var HasPlan = dbContext.Plans.Any();
                var HasCategory = dbContext.Categorys.Any();
                if (HasPlan && HasCategory) return false;

                if (!HasPlan)
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Plans.Any())
                        dbContext.AddRange(Plans);

                }

                if (!HasCategory)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                        dbContext.AddRange(Categories);

                }

                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
                    


        }



        private static List<T> LoadDataFromJsonFile<T>(string FileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//Files", FileName);
            if (!File.Exists(FilePath)) throw new FileNotFoundException();
            string Data = File.ReadAllText(FilePath);
            var Option = new JsonSerializerOptions()
            {

                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Option) ?? new List<T>();

        }



    }
}
