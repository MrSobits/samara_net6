namespace Bars.B4.Modules.Analytics.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Bars.B4.Modules.Analytics.Extensions;

    public class XmlSerializer
    {
        private readonly IDictionary<Type, List<object>> _serializeCollects;

        public XmlSerializer()
        {
            _serializeCollects = new Dictionary<Type, List<object>>();
        }

        public IEnumerable<XElement> Serialize(Type type, object obj)
        {
            var result = new List<XElement>();
            FillDict(type, obj);

            foreach (var key in _serializeCollects.Keys)
            {
                var root = new XElement(string.Format("{0}_Data", key.Name));
                foreach (var item in _serializeCollects[key])
                {
                    root.Add(PlainSerialize(key, item));
                }
                result.Add(root);
            }

            return result;
        }

        private void FillDict(Type type, object obj)
        {
            if (obj.GetType().IsCollectionType())
            {
                foreach (var item in (IEnumerable) obj)
                {
                    AddSerializeObj(type, item);
                }
            }
            else
            {
                AddSerializeObj(type, obj);
            }
        }

        private void AddSerializeObj(Type type, object obj)
        {
            if (!_serializeCollects.ContainsKey(type))
            {
                _serializeCollects[type] = new List<object>();
            }
            _serializeCollects[type].Add(obj);

            foreach (var propInfo in type.GetProperties())
            {
                if (!propInfo.PropertyType.IsSimpleType())
                {

                    FillDict(
                        propInfo.PropertyType.IsCollectionType()
                            ? propInfo.PropertyType.GetElementType()
                            : propInfo.PropertyType, propInfo.GetValue(obj, new object[0]));
                }
            }
        }

        private XElement PlainSerialize(Type type, object obj)
        {
            var root = new XElement(type.Name);
            foreach (var propInfo in type.GetProperties())
            {
                if (propInfo.PropertyType.IsSimpleType())
                {
                    root.Add(new XElement(propInfo.Name, propInfo.GetValue(obj, new object[0])));
                }
            }
            return root;
        }

    }
}
