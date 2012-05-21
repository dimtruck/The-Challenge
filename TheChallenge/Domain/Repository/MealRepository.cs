using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Domain.Entities;
using Dapper;

namespace Domain.Repository
{
    public class MealRepository : IMealRepository
    {
        private SqlConnection connection;
        private String connectionString;

        public MealRepository(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool SaveMeals(IList<Meal> meals)
        {
            int numberOfEntries = 0;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                //enter meal
                foreach (Meal meal in meals)
                {
                    numberOfEntries = connection.Execute(@"insert into [TheChallenge].[DimitryUshakov].[Meal] values(@Date,@FoodId, @ServingSize,@ServingId)", meal.Foods);
                }
            }

            return numberOfEntries > 0;
        }


        public IList<Meal> GetMeals(DateTime entryDate)
        {
            Workout workout = new Workout()
            {
                ExerciseEntries = new List<ExerciseEntry>(),
                WorkoutDate = entryDate
            };
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                //add foodentry with all data that is also populated from food_repository
                var results = this.connection.Query(@"select MealDate as [Date], FoodId, ServingSize, ServingType, msre_desc as ServingDesc from thechallenge.dimitryushakov.[meal] a,
                                                        thechallenge.dimitryushakov.food_des b,
                                                        thechallenge.dimitryushakov.food_weight c
                                                        where cast(mealdate as date) = @EntryDate
                                                        and cast(b.ndb_no as int) = a.foodid
                                                        and b.ndb_no = c.nbd_no
                                                        and cast(c.seq as int) = a.servingtype", new { EntryDate = entryDate }).ToList();

                foreach (var result in results)
                {
                    workout.ExerciseEntries.Add(new ExerciseEntry()
                    {
                        Time = TimeSpan.FromMilliseconds(result.EntryTime),
                        Distance = result.Distance,
                        ExerciseId = result.EventId,
                        Reps = result.Reps,
                        Weight = result.Weight,
                        Name = result.EventName,
                    });
                }

            }

            return null;
        }


        public IList<FoodEntry> GetFoodEntries(DateTime entryDate)
        {
            IList<FoodEntry> results = null;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                //add foodentry with all data that is also populated from food_repository
                results = this.connection.Query<FoodEntry>(@"select MealDate as [Date], FoodId, ServingSize, ServingType as ServingId, Long_desc as [Name] from thechallenge.dimitryushakov.[meal] a,
                                                            thechallenge.dimitryushakov.food_des b
                                                            where cast(mealdate as date) =  @EntryDate
                                                            and cast(b.ndb_no as int) = a.foodid", new { EntryDate = entryDate }).ToList();

                String[] foodIds = new String[results.Count];
                String[] servingIds = new String[results.Count];
                int i = 0;
                foreach (FoodEntry result in results)
                {
                    //get food name
                    foodIds[i] = result.FoodId.PadLeft(5,'0');
                    servingIds[i] = result.ServingId;
                    i++;
                }

                IList<Nutrient> nutrients = this.connection.Query<Nutrient>(@"select CAST(a.ndb_no as int) as Id,b.units as Units, b.nutrDesc as Description, a.nutr_val as AmountIn100Grams, c.srccd_desc as SourceCode, d.deriv_desc as DerivCode, CASE WHEN add_nutr_mark = 'Y' THEN CAST('TRUE' as bit) ELSE CAST('FALSE' as bit) END as IsNutrientAdded, CASE WHEN addmod_date != 0 THEN CAST(addmod_date as date) else NULL END  as LastUpdated
                                                            from	thechallenge.dimitryushakov.nut_data a,
		                                                            thechallenge.dimitryushakov.nutr_def b,
		                                                            thechallenge.dimitryushakov.src_cd c,
		                                                            thechallenge.dimitryushakov.deriv_cd d
                                                            where	a.ndb_no in @foodIds
                                                            and     a.nutr_no = b.nutr_no
                                                            and		a.src_cd = c.src_cd
                                                            and		a.deriv_cd = d.deriv_cd", new { foodIds = foodIds }).ToList();

                IList<Serving> servings = this.connection.Query<Serving>(@"select	CAST(nbd_no as int) as FoodId, CAST(seq as int) as Id, CAST(Amount as decimal) as Amount, msre_desc as Description, CAST(gm_wgt as decimal) as WeightInGrams
                                                            from	thechallenge.dimitryushakov.food_weight
                                                            where	nbd_no in @foodIds
                                                            and     seq in @servingIds", new { foodIds = foodIds, servingIds = servingIds }).ToList();

                foreach (FoodEntry entry in results)
                {
                    entry.SelectedServing = servings.Where(t => t.FoodId == Int32.Parse(entry.FoodId) && t.Id == Int32.Parse(entry.ServingId)).FirstOrDefault();
                    entry.CalculatedNutrients = new List<Nutrient>();
                    if (entry.SelectedServing != null)
                    {
                        //calculate 
                        foreach (Nutrient nutrient in nutrients.Where(t => t.Id == Int32.Parse(entry.FoodId)))
                        {
                            entry.CalculatedNutrients.Add(new Nutrient()
                            {
                                AmountIn100Grams = nutrient.AmountIn100Grams * (decimal)entry.ServingSize * entry.SelectedServing.WeightInGrams / 100,
                                DerivCode = nutrient.DerivCode,
                                Description = nutrient.Description,
                                Id = nutrient.Id,
                                IsNutrientAdded = nutrient.IsNutrientAdded,
                                LastUpdated = nutrient.LastUpdated,
                                SourceCode = nutrient.SourceCode,
                                Units = nutrient.Units
                            });
                        }
                    }
                }
            }

            return results;
        }
    }
}
