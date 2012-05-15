using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Data.SqlClient;
using Dapper;

namespace Domain.Repository
{
    public class FoodRepository : IFoodRepository
    {
        private SqlConnection connection;
        private String connectionString;

        public FoodRepository(String connectionString)
        {
            this.connectionString = connectionString;
        }

        public IList<Food> RetrieveFoods()
        {
            IList<Food> results;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                results = this.connection.Query<Food>("SELECT a.NDB_No as Id, a.Long_Desc as Name, b.Fdgrp_desc as Category  FROM [TheChallenge].[DimitryUshakov].[Food_Des] a, [TheChallenge].[DimitryUshakov].[Fd_group] b WHERE a.fdgrp_cd = b.fdgrp_cd").ToList();
            }

            return results;
        }


        public Food RetrieveFoodById(int foodId)
        {
            Food result;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                result = this.connection.Query<Food>("SELECT a.NDB_No as Id, a.Long_Desc as Name, b.Fdgrp_desc as Category  FROM [TheChallenge].[DimitryUshakov].[Food_Des] a, [TheChallenge].[DimitryUshakov].[Fd_group] b WHERE a.fdgrp_cd = b.fdgrp_cd AND a.ndb_no = @foodId", new { foodId = foodId }).FirstOrDefault();

            }

            return result;
        }

        public IList<Nutrient> RetrieveAllNutrients()
        {
            IList<Nutrient> results;
            using (this.connection)
            {
                this.connection.Open();
                results = this.connection.Query<Nutrient>(@"select	a.ndb_no,b.units, b.nutrDesc, a.nutr_val, c.srccd_desc, d.deriv_desc, add_nutr_mark, addmod_date
                                                            from	thechallenge.dimitryushakov.nut_data a,
		                                                            thechallenge.dimitryushakov.nutr_def b,
		                                                            thechallenge.dimitryushakov.src_cd c,
		                                                            thechallenge.dimitryushakov.deriv_cd d
                                                            where	a.nutr_no = b.nutr_no
                                                            and		a.src_cd = c.src_cd
                                                            and		a.deriv_cd = d.deriv_cd").ToList();
            }

            return results;
        }

        public IList<Nutrient> RetrieveNutrientsForFoodId(int foodId)
        {
            IList<Nutrient> results;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                results = this.connection.Query<Nutrient>(@"select	CAST(a.ndb_no as int) as Id,b.units as Units, b.nutrDesc as Description, a.nutr_val as AmountIn100Grams, c.srccd_desc as SourceCode, d.deriv_desc as DerivCode, CASE WHEN add_nutr_mark = 'Y' THEN CAST('TRUE' as bit) ELSE CAST('FALSE' as bit) END as IsNutrientAdded, CASE WHEN addmod_date != 0 THEN CAST(addmod_date as date) else NULL END  as LastUpdated
                                                            from	thechallenge.dimitryushakov.nut_data a,
		                                                            thechallenge.dimitryushakov.nutr_def b,
		                                                            thechallenge.dimitryushakov.src_cd c,
		                                                            thechallenge.dimitryushakov.deriv_cd d
                                                            where	a.ndb_no = @foodId
                                                            and     a.nutr_no = b.nutr_no
                                                            and		a.src_cd = c.src_cd
                                                            and		a.deriv_cd = d.deriv_cd", new { foodId = foodId }).ToList();
            }

            return results;
        }

        public IList<Serving> RetrieveAllServings()
        {
            IList<Serving> results;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                results = this.connection.Query<Serving>(@"select	*
                                                            from	thechallenge.dimitryushakov.food_weight").ToList();
            }

            return results;
        }

        public IList<Serving> RetrieveServingsForFoodId(int foodId)
        {
            IList<Serving> results;
            using (this.connection = new SqlConnection(this.connectionString))
            {
                this.connection.Open();
                results = this.connection.Query<Serving>(@"select	CAST(nbd_no as int) as FoodId, CAST(seq as int) as Id, CAST(Amount as decimal) as Amount, msre_desc as Description, CAST(gm_wgt as decimal) as WeightInGrams
                                                            from	thechallenge.dimitryushakov.food_weight
                                                            where	nbd_no = @foodId", new { foodId = foodId }).ToList();
            }

            return results;
        }

        public Food RetrieveCompleteFood(int foodId)
        {
            Food food = RetrieveFoodById(foodId);
            if (food != null)
            {
                food.AvailableNutrients = RetrieveNutrientsForFoodId(foodId);
                food.AvailableServings = RetrieveServingsForFoodId(foodId);
            }
            return food;
        }
    }
}
