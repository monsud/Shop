using System.Xml.Serialization;

namespace Shop.Model
{

    public class ListOfProducts
    {
        [XmlArrayItem(ElementName = "Product", IsNullable = true, Type = typeof(Product))]
        [XmlArray]
        private Product[] products;

        public Product[] Products
        {
            get { return products; }
            set { products = value; }
        }
    }
}
