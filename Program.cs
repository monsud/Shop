using System;
using System.Globalization;
using Shop.Model;
using System.Collections.Generic;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Cart.SerializeListOfProducts();
            CShop.SerializeListOfUser();
            CShop myShop = new CShop();
            myShop.LoadUsers();
            bool res1 = myShop.AddProductInPersonalCart("pippo", "abc123");
            bool res2 = myShop.AddProductInPersonalCart("minnie", "pass123");
            bool res3 = myShop.AddProductInPersonalCart("ndonio", "saluta21");
            bool res4 = myShop.AddProductInPersonalCart("lollo", "giuoco3");
            bool res5 = myShop.AddProductInPersonalCart("livestar", "12345");
            bool res6 = myShop.RemoveSingleProductFromPersonalCart("pippo", "abc123", "004a");
            double res7 = myShop.GetTotalPriceFromCart("pippo", "abc123");
            Dictionary <int, double> myGenderDic = myShop.GetGenderPercentageByProduct("004a");
            foreach (KeyValuePair<int, double> kvp in myGenderDic)
            {
                Console.WriteLine("Gender = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            Dictionary<string, double> myCountryDic = myShop.GetCountryPercentageByProduct("001a");
            foreach (KeyValuePair<string, double> kvp in myCountryDic)
            {
                Console.WriteLine("Country = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            Dictionary<string, double> myAgeDic = myShop.GetAgePercentageByProduct("002a");
            foreach (KeyValuePair<string, double> kvp in myAgeDic)
            {
                Console.WriteLine("Age = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
