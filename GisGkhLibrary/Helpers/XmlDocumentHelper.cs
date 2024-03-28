using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GisGkhLibrary.Helpers
{
    internal static class XmlDocumentHelper
    {
        /// <summary>
        /// Создание объекта типа XmlDocument из строки
        /// </summary>
        internal static XmlDocument Create(string xmlData)
        {
            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            try
            {
                xmlDocument.LoadXml(xmlData);
            }
            catch (XmlException xmlEx)
            {
                throw new InvalidOperationException(string.Format("Некорректный xml документ: {0}", xmlEx.Message), xmlEx);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Создание объекта типа XmlDocument из файла
        /// </summary>
        internal static XmlDocument Load(string pathName)
        {
            var xmlDocument = new XmlDocument { PreserveWhitespace = true };
            try
            {
                xmlDocument.Load(pathName);
            }
            catch (XmlException xmlEx)
            {
                throw new InvalidOperationException(string.Format("Некорректный xml документ: {0}", xmlEx.Message), xmlEx);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Создает менеджер пространств имен по объекту XmlDocument
        /// </summary>
        internal static XmlNamespaceManager CreateNamespaceManager(this XmlDocument xml)
        {
            var manager = new XmlNamespaceManager(xml.NameTable);
            foreach (var name in GetNamespaceDictionary(xml))
            {
                manager.AddNamespace(name.Key, name.Value);
            }

            return manager;
        }

        internal static IDictionary<string, string> GetNamespaceDictionary(this XmlDocument xml)
        {
            var nameSpaceList = xml.SelectNodes(@"//namespace::*[not(. = ../../namespace::*)]").OfType<XmlNode>();
            return nameSpaceList.Distinct(new LocalNameComparer()).ToDictionary(xmlNode => xmlNode.LocalName, xmlNode => xmlNode.Value);
        }

        internal class LocalNameComparer : IEqualityComparer<XmlNode>
        {
            public bool Equals(XmlNode x, XmlNode y)
            {
                return x.LocalName == y.LocalName;
            }

            public int GetHashCode(XmlNode obj)
            {
                return obj.LocalName.GetHashCode();
            }
        }
    }
}