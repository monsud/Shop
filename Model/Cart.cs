using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Shop.Model
{

    public class Cart
    {
        private List<Product> _products;
        public Cart()
        {
            _products = new List<Product>();
        }

        public bool IsAlreadyExist (string cod)
        {
            return _products.Count > 0 && _products.Exists(x => x.Cod == cod);
        }
        public Product GetProduct(string cod)
        {
            if (_products.Count > 0)
            {
                var prod = _products.SingleOrDefault(x => x.Cod == cod && x.Quantity > 0);
                if (prod != null)
                {
                    return prod;
                }
                else
                    throw new Exception("This product doesn't exist.");
            }
            else
                throw new Exception("Product list it's empty.");

        }
        public bool RemoveProduct(string cod)
        {
            if (_products != null)
                return _products.RemoveAll(x => x.Cod == cod) > 0;
            else
                throw new Exception("Cannot remove.");
        }
        public bool RemoveAll()
        {
            if (_products != null)
            {
                foreach (Product prod in _products)
                {
                    _products.Remove(prod);
                }
                return true;
            }
            else
                throw new Exception("Cannot remove.");
        }
        public double TotalCart()
        {
            double tot = 0;
            if (_products != null)
            {
                foreach (Product prod in _products)
                {
                    tot += prod.Price * prod.Quantity;
                }
                return tot;
            }
            else
                throw new Exception("List is empty.");
        }
        public bool IsProductIntoList(string cod)
        {
            if (_products != null)
                return _products.Any(x => x.Cod == cod);
            else
                throw new Exception("Cannot remove.");
        }
        public static void SerializeListOfProducts()
        {
            List<Product> products = new List<Product>();
            Product p1 = new Product();
            p1.Cod = "001a";
            p1.Description = "Pizza";
            p1.Quantity = 1;
            p1.Price = 5;
            p1.DiscountedPrice = 5;
            Product p2 = new Product();
            p2.Cod = "002a";
            p2.Description = "Pasta";
            p2.Quantity = 1;
            p2.Price = 0.5;
            p2.DiscountedPrice = 0.5;
            Product p3 = new Product();
            p3.Cod = "003a";
            p3.Description = "Oil";
            p3.Quantity = 1;
            p3.Price = 4;
            p3.DiscountedPrice = 3;
            Product p4 = new Product();
            p4.Cod = "004a";
            p4.Description = "Water";
            p4.Quantity = 1;
            p4.Price = 5;
            p4.DiscountedPrice = 5;
            Product p5 = new Product();
            p5.Cod = "005a";
            p5.Description = "Ham";
            p5.Quantity = 1;
            p5.Price = 2.3;
            p5.DiscountedPrice = 1.8;
            Product p6 = new Product();
            p6.Cod = "006a";
            p6.Description = "Egg";
            p6.Quantity = 6;
            p6.Price = 3.2;
            p6.DiscountedPrice = 3.2;
            Product p7 = new Product();
            p7.Cod = "007a";
            p7.Description = "Salad";
            p7.Quantity = 1;
            p7.Price = 0.8;
            p7.DiscountedPrice = 0.8;
            Product p8 = new Product();
            p8.Cod = "007a";
            p8.Description = "Salad";
            p8.Quantity = 1;
            p8.Price = 0.8;
            p8.DiscountedPrice = 0.8;
            products.Add(p1);
            products.Add(p2);
            products.Add(p3);
            products.Add(p4);
            products.Add(p5);
            products.Add(p6);
            products.Add(p7);
            products.Add(p8);

            ListOfProducts myProducts = new ListOfProducts();
            myProducts.Products = products.ToArray();
            string sourcePath = ConfigurationManager.AppSettings["XMLFile"];
            XmlSerializer serializer = new XmlSerializer(typeof(ListOfProducts));
            TextWriter writer = new StreamWriter(sourcePath);
            serializer.Serialize(writer, myProducts);
            writer.Close();
        }
        public void DeserializeListOfProducts(string sourcePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ListOfProducts));
            StreamReader reader = new StreamReader(sourcePath);
            ListOfProducts myProducts = new ListOfProducts();
            myProducts = (ListOfProducts)serializer.Deserialize(reader);
            reader.Close();

            if (myProducts != null)
            {
                for (int i = 0; i < myProducts.Products.Length; i++)
                {
                    Product p = myProducts.Products[i];
                    _products.Add(p);
                }
            } else
                throw new Exception("Cart is empty.");
        }
    }
}
