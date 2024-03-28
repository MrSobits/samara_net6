using System.Xml;
using System.Xml.Linq;

namespace SMEV3Library.Helpers
{
    public static class XMLToLinqHelper
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = false;
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XmlDocument ToXmlDocument(XElement element)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = false;
            using (var xmlReader = element.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}
