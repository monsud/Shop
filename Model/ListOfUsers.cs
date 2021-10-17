using System.Xml.Serialization;

namespace Shop.Model
{
    public class ListOfUsers
    {
        [XmlArrayItem(ElementName = "User", IsNullable = true, Type = typeof(User))]
        [XmlArray]
        private User[] users;

        public User[] Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}