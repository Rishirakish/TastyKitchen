using System;
using System.Collections.Generic;
using System.Text;

namespace Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen
{
    public class MenuCategoryWiseSaleReport : Entity 
    {
        public List<MenuCategoryWiseSaleData> MenuCategoryWiseSales { get; set; } = new List<MenuCategoryWiseSaleData>();
        public double TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
