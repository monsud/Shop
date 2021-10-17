using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Shop.Model
{
    public class Product
    {   
        private string _cod;
        private string _descr;
        private double _price;
        private int _quantity;
        private double _discountedprice;
        public Product() { }
        public Product(string cod, string desc, double price, int quantity, double discountedprice)
        {
            _cod = cod;
            _descr = desc;
            _price = price;
            _quantity = quantity;
            _discountedprice = discountedprice;
        }
        public string Cod
        {
            get => _cod;
            set => _cod = value;
        }
        public string Description
        {
            get => _descr;
            set => _descr = value;
        }
        public double Price
        {
            get => _price;
            set => _price = value;
        }
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
        public double DiscountedPrice
        {
            get => _discountedprice;
            set => _discountedprice = value;
        }

        public double GetDiscountPrice(int percentage)
        {
            double myDiscount = 0;
            myDiscount = _price - (_price / 100 * percentage);
            return myDiscount;
        }
        public override string ToString()
        {
            return " PRODUCT == " + " Cod: " + Cod + " Description: " + Description + " Price: " + Price + " Quantity: " + Quantity + " Discount: " + DiscountedPrice;
        }
    }
}
