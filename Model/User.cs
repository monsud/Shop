using System;
using System.Configuration;

namespace Shop.Model
{
    public class User
    {
        private string _username;
        private string _pass;
        private string _name;
        private string _surname;
        private GenderType _sex;
        private string _country;
        private DateTime _birth;
        private int _age;
        private Cart _myCart;

        public enum GenderType
        {
            Male,
            Female,
            Other
        }
        public User() {}
        public string Username
        {
            get => _username;
            set => _username = value;
        }
        public string Password
        {
            get => _pass;
            set => _pass = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Surname
        {
            get => _surname;
            set => _surname = value;
        }
        public GenderType Sex
        {
            get => _sex;
            set => _sex = value;
        }
        public string Country
        {
            get => _country;
            set => _country = value;
        }
        public DateTime Birth
        {
            get => _birth;
            set => _birth = value;
        }
        public Cart myCart
        {
            get => _myCart;
            set => _myCart = value;
        }
        public int Age
        {
            get => _age;
            set => _age = value;
        }
        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age--;
            return age;
        }

        public bool RemoveSingleProductFromCart (string cod)
        {
            if (_myCart != null)
                return _myCart.RemoveProduct(cod);
            else
                throw new Exception("Cart is empty.");
        }

        public bool ResetCart()
        {
            if (_myCart != null)
                return _myCart.RemoveAll();
            else
                throw new Exception("Cart is empty.");
        }
        public bool AddProductInCart()
        {
            if (_myCart != null)
            {
                switch (_username)
                {
                    case "pippo":
                        _myCart.DeserializeListOfProducts(ConfigurationManager.AppSettings["pippo"]);
                        break;
                    case "minnie":
                        _myCart.DeserializeListOfProducts(ConfigurationManager.AppSettings["minnie"]);
                        break;
                    case "ndonio":
                        _myCart.DeserializeListOfProducts(ConfigurationManager.AppSettings["ndonio"]);
                        break;
                    case "lollo":
                        _myCart.DeserializeListOfProducts(ConfigurationManager.AppSettings["lollo"]);
                        break;
                    case "livestar":
                        _myCart.DeserializeListOfProducts(ConfigurationManager.AppSettings["livestar"]);
                        break;
                }
                return true;
            }
            else
                throw new Exception("Cart is empty.");
        }

        public double TotalExpense()
        {
            if (_myCart != null)
            {
                return _myCart.TotalCart();
            }
            else
                throw new Exception("Cart is empty.");
        }

        public bool IsProductExistInTheCart(string cod)
        {
            if (_myCart != null)
            {
                return _myCart.IsProductIntoList(cod);
            }
            else
                throw new Exception("Cart is empty.");
        }


        public override string ToString()
        {
            return " USER == " + " User: " + Username + " Password: " + Password + " Name: " + Name + " Surname: " + Surname + " Country: " + Country + " Sex: " + Sex + " Birth: " + Birth;
        }
    }
}
