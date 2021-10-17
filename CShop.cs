using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Shop.Model;
using static Shop.Model.User;

namespace Shop
{
    public class CShop
    {
        const int percentage = 100;
        private List<User> _userList;
        public CShop()
        {
            _userList = new List<User>();
        }

        public double GetPercentage (double sum, double tot)
        {
            return sum / tot * percentage;
        }

        public User CreateUser(string usr, string pass, string name, string surname, GenderType sex, string country, DateTime birth)
        {
            try
            {
                User user = new User();
                user.Username = usr;
                user.Password = pass;
                user.Name = name;
                user.Surname = surname;
                user.Sex = sex;
                user.Country = country;
                user.Birth = birth;
                user.Age = User.CalculateAge(birth);
                user.myCart = new Cart();
                _userList.Add(user);
                return user;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public User GetUser(string? username, string? pass)
        {
            try
            {
                var user = _userList.Single(x => x.Username == username && x.Password == pass);
                if (user != null)
                {
                    return user;
                }
                else
                    throw new Exception("This user doesn't exist.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool AddProductInPersonalCart(string? user, string? pass)
        {
            try
            {
                User personalAccount = GetUser(user, pass);
                if (personalAccount != null)
                    return personalAccount.AddProductInCart();
                else
                    throw new Exception("Cannot update cart.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ResetPersonalCart(string? user, string? pass)
        {
            try
            {
                User personalAccount = GetUser(user, pass);
                if (personalAccount != null)
                    return personalAccount.ResetCart();
                else
                    throw new Exception("Cannot reset cart.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool RemoveSingleProductFromPersonalCart(string? user, string? pass, string? cod)
        {
            try
            {
                User personalAccount = GetUser(user, pass);
                if (personalAccount != null)
                    return personalAccount.RemoveSingleProductFromCart(cod);
                else
                    throw new Exception("Cannot remove this item from cart.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public double GetTotalPriceFromCart(string? user, string? pass)
        {
            try
            {
                User personalAccount = GetUser(user, pass);
                if (personalAccount != null)
                    return personalAccount.TotalExpense();
                else
                    throw new Exception("Cannot total expense from your cart.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public Dictionary<int, double> GetGenderPercentageByProduct(string cod)
        {
            try
            {
                Dictionary<int, double> GenderPercentageDic = new Dictionary<int, double>();
                double maleCounter = 0, femaleCounter = 0, otherCounter = 0, tot = 0;
                if (_userList != null)
                {
                    foreach (User user in _userList)
                    {
                        if (user.IsProductExistInTheCart(cod))
                        {
                            switch (user.Sex)
                            {
                                case GenderType.Male:
                                    maleCounter++;
                                    break;
                                case GenderType.Female:
                                    femaleCounter++;
                                    break;
                                case GenderType.Other:
                                    otherCounter++;
                                    break;
                            }
                        }
                    }
                    tot = maleCounter + femaleCounter + otherCounter;
                    if (tot >= 0)
                    {
                        GenderPercentageDic.Add((int)GenderType.Male, GetPercentage(maleCounter,tot));
                        GenderPercentageDic.Add((int)GenderType.Female, GetPercentage(femaleCounter,tot));
                        GenderPercentageDic.Add((int)GenderType.Other, GetPercentage(otherCounter, tot));
                        return GenderPercentageDic;
                    }
                    else
                        throw new Exception("Total of users is 0.");
                }
                else
                    throw new Exception("User list is empty.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Dictionary<string, double> GetCountryPercentageByProduct(string cod)
        {
            try
            {
                Dictionary<string, double> CountryPercentageDic = new Dictionary<string, double>();
                Dictionary<string, double> CountryCounterDic = new Dictionary<string, double>();
                double tot = 0;
                if (_userList != null)
                {
                    foreach (User user in _userList)
                    {
                        if (user.IsProductExistInTheCart(cod))
                        {
                            if (!CountryCounterDic.ContainsKey(user.Country))
                                CountryCounterDic.Add(user.Country, 1);
                            else
                                CountryCounterDic[user.Country] = CountryCounterDic[user.Country] + 1;
                        }
                    }
                    tot = CountryCounterDic.Sum(x => x.Value);
                    if (tot >= 0)
                    {
                        foreach (KeyValuePair<string, double> kvp in CountryCounterDic)
                        {
                            CountryPercentageDic.Add(kvp.Key, GetPercentage(kvp.Value, tot));
                        }
                        return CountryPercentageDic;
                    }
                    else
                        throw new Exception("Total of users is 0.");
                }
                else
                    throw new Exception("User list is empty.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public Dictionary<string, double> GetAgePercentageByProduct(string cod)
        {
            List<AgeConfig> myAges = new List<AgeConfig>();
            AgeConfig a1 = new AgeConfig("18-28", null, 28);
            AgeConfig a2 = new AgeConfig("29-38", 29, 38);
            AgeConfig a3 = new AgeConfig("39-48", 39, 48);
            AgeConfig a4 = new AgeConfig("49-58", 49, 58);
            AgeConfig a5 = new AgeConfig("59-65", 59, 65);
            AgeConfig a6 = new AgeConfig("66+", 66, null);
            myAges.Add(a1);
            myAges.Add(a2);
            myAges.Add(a3);
            myAges.Add(a4);
            myAges.Add(a5);
            myAges.Add(a6);
            try
            {
                Dictionary<string, double> AgePercentageDic = new Dictionary<string, double>();
                Dictionary<string, double> AgeCounterDic = new Dictionary<string, double>();
                double tot = 0;
                if (_userList != null)
                {
                    foreach (User user in _userList)
                    {
                        if (user.IsProductExistInTheCart(cod))
                        {
                            foreach (AgeConfig age in myAges)
                            {
                                bool isMinDefined = age.Min.HasValue;
                                bool isMaxDefined = age.Max.HasValue;
                                if (isMinDefined && isMaxDefined && age.Max.Value > age.Min.Value && user.Age >= age.Min.Value && user.Age <= age.Max.Value)
                                {
                                    if (!AgeCounterDic.ContainsKey(age.Key))
                                        AgeCounterDic.Add(age.Key, 1);
                                    else
                                        AgeCounterDic[age.Key] = AgeCounterDic[age.Key] + 1;
                                }
                            }
                        }
                    }
                    tot = AgeCounterDic.Sum(x => x.Value);
                    foreach (KeyValuePair<string, double> kvp in AgeCounterDic)
                    {
                        if (tot != 0)
                        {
                            AgePercentageDic.Add(kvp.Key, kvp.Value / tot * percentage);
                        }
                        else
                            throw new Exception("Total of users is 0.");
                    }
                    return AgePercentageDic;
                }
                else
                    throw new Exception("User list is empty.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public static void SerializeListOfUser()
        {
            List<User> users = new List<User>();

            User user1 = new User();
            user1.Username = "pippo";
            user1.Password = "abc123";
            user1.Name = "Pippo";
            user1.Surname = "Rossi";
            user1.Sex = GenderType.Male;
            user1.Country = "Italy";
            user1.Birth = DateTime.Parse("16/11/1985", CultureInfo.CurrentCulture);
            user1.Age = User.CalculateAge(user1.Birth);
            user1.myCart = new Cart();

            User user2 = new User();
            user2.Username = "minnie";
            user2.Password = "pass123";
            user2.Name = "Pina";
            user2.Surname = "Rella";
            user2.Sex = GenderType.Female;
            user2.Country = "France";
            user2.Birth = DateTime.Parse("08/05/1977", CultureInfo.CurrentCulture);
            user2.Age = User.CalculateAge(user2.Birth);
            user2.myCart = new Cart();

            User user3 = new User();
            user3.Username = "ndonio";
            user3.Password = "saluta21";
            user3.Name = "Antonio";
            user3.Surname = "Conte";
            user3.Sex = GenderType.Male;
            user3.Country = "Italy";
            user3.Birth = DateTime.Parse("16/11/1988", CultureInfo.CurrentCulture);
            user3.Age = User.CalculateAge(user3.Birth);
            user3.myCart = new Cart();

            User user4 = new User();
            user4.Username = "lollo";
            user4.Password = "giuoco3";
            user4.Name = "Lorenzo";
            user4.Surname = "Damadou";
            user4.Sex = GenderType.Male;
            user4.Country = "Ghana";
            user4.Birth = DateTime.Parse("16/11/1992", CultureInfo.CurrentCulture);
            user4.myCart = new Cart();

            User user5 = new User();
            user5.Username = "livestar";
            user5.Password = "12345";
            user5.Name = "Giò";
            user5.Surname = "Maria";
            user5.Sex = GenderType.Other;
            user5.Country = "Italy";
            user5.Birth = DateTime.Parse("16/11/1990", CultureInfo.CurrentCulture);
            user5.Age = User.CalculateAge(user5.Birth);
            user5.myCart = new Cart();

            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            users.Add(user4);
            users.Add(user5);

            ListOfUsers myUsers = new ListOfUsers();
            myUsers.Users = users.ToArray();
            string sourcePath = ConfigurationManager.AppSettings["XMLUsers"];
            XmlSerializer serializer = new XmlSerializer(typeof(ListOfUsers));
            TextWriter writer = new StreamWriter(sourcePath);
            serializer.Serialize(writer, myUsers);
            writer.Close();
        }
        public void LoadUsers()
        {
            string sourcePath = ConfigurationManager.AppSettings["XMLUsers"];
            Console.WriteLine(File.Exists(sourcePath) ? "File exists." : "File does not exist.");
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ListOfUsers));
                StreamReader reader = new StreamReader(sourcePath);
                ListOfUsers myUsers = new ListOfUsers();
                myUsers = (ListOfUsers)serializer.Deserialize(reader);
                reader.Close();
                if (!myUsers.Equals(null))
                {
                    for (int i = 0; i < myUsers.Users.Length; i++)
                    {
                        User u = myUsers.Users[i];
                        if (u != null)
                            _userList.Add(u);
                    }
                }
                else
                    throw new Exception("User list is empty.");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}