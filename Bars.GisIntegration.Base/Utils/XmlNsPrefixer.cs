namespace Bars.GisIntegration.Base.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;

    public class XmlNsPrefixer
    {
        private const string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";

        private readonly Dictionary<string, string> fixedPrefixes = new Dictionary<string, string>();

        private Dictionary<string, string> namespaceToNewPrefixMapping;

        public void AddNamespace(string namespaceUri, string newPrefix)
        {
            this.fixedPrefixes[namespaceUri] = newPrefix;
        }

        public void Process(XmlDocument doc)
        {
            this.namespaceToNewPrefixMapping = new Dictionary<string, string>(this.fixedPrefixes);

            var element = doc.DocumentElement;
            var newElement = this.ChangePrefixes(doc, element);

            doc.LoadXml(newElement.OuterXml);
        }

        private XmlElement ChangePrefixes(XmlDocument doc, XmlElement element)
        {
            string newPrefix;
            if (this.TryGetPrefix(element.NamespaceURI, out newPrefix))
            {
                var newElement = string.IsNullOrEmpty(element.Prefix)
                                     ? doc.CreateElement(element.LocalName, element.NamespaceURI)
                                     : doc.CreateElement(newPrefix, element.LocalName, element.NamespaceURI);
                var children = new List<XmlNode>(element.ChildNodes.Cast<XmlNode>());
                var attributes = new List<XmlAttribute>(element.Attributes.Cast<XmlAttribute>());
                foreach (var child in children)
                {
                    newElement.AppendChild(child);
                }

                foreach (var attr in attributes)
                {
                    newElement.Attributes.Append(attr);
                }

                element = newElement;
            }

            var newAttributes = new List<XmlAttribute>();
            var modified = false;
            for (var i = 0; i < element.Attributes.Count; i++)
            {
                var attr = element.Attributes[i];

                if (attr.NamespaceURI == XmlNsPrefixer.XmlnsNamespace && this.TryGetPrefix(attr.Value, out newPrefix))
                {
                    var newAttr = doc.CreateAttribute("xmlns", newPrefix, XmlNsPrefixer.XmlnsNamespace);
                    newAttr.Value = attr.Value;
                    newAttributes.Add(newAttr);
                    modified = true;
                }
                else if (this.TryGetPrefix(attr.NamespaceURI, out newPrefix))
                {
                    var newAttr = doc.CreateAttribute(newPrefix, attr.LocalName, attr.NamespaceURI);
                    newAttr.Value = attr.Value;
                    newAttributes.Add(newAttr);
                    modified = true;
                }
                else
                {
                    newAttributes.Add(attr);
                }
            }

            if (modified)
            {
                element.Attributes.RemoveAll();
                foreach (var attr in newAttributes)
                {
                    element.Attributes.Append(attr);
                }
            }

            var toReplace = new List<KeyValuePair<XmlNode, XmlNode>>();
            foreach (XmlNode child in element.ChildNodes)
            {
                var childElement = child as XmlElement;
                if (childElement != null)
                {
                    var newChildElement = this.ChangePrefixes(doc, childElement);
                    if (newChildElement != childElement)
                    {
                        toReplace.Add(new KeyValuePair<XmlNode, XmlNode>(childElement, newChildElement));
                    }
                }
            }

            if (toReplace.Count > 0)
            {
                foreach (var t in toReplace)
                {
                    element.InsertAfter(t.Value, t.Key);
                    element.RemoveChild(t.Key);
                }
            }

            return element;
        }

        private string ToHex(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (var t in bytes)
            {
                result.Append(t.ToString("x2"));
            }

            return result.ToString();
        }

        private bool TryGetPrefix(string ns, out string prefix)
        {
            prefix = null;

            if (string.IsNullOrEmpty(ns))
            {
                return false;
            }

            if (ns == XmlNsPrefixer.XmlnsNamespace)
            {
                prefix = "xmlns";
                return true;
            }

            if (!this.fixedPrefixes.TryGetValue(ns, out prefix) && !this.namespaceToNewPrefixMapping.TryGetValue(ns, out prefix))
            {
                using (var md5 = MD5.Create())
                {
                    var hash = this.ToHex(md5.ComputeHash(Encoding.UTF8.GetBytes(ns)));
                    var offset = 0;
                    while (offset < hash.Length && this.namespaceToNewPrefixMapping.Values.Contains(prefix = $"_{hash.Substring(offset, 6)}"))
                    {
                        offset++;
                    }

                    this.namespaceToNewPrefixMapping.Add(ns, prefix);
                }
            }

            return true;
        }
    }
}