namespace Bars.B4.Modules.Analytics.Web.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;

    /// <summary>
    /// 
    /// </summary>
    public class DataSourceTreeBuilder
    {
        public Node BuildTree(DataSource dataSource, bool firstLevel = false, bool checkable = true)
        {
            var tree = checkable
                ? new CheckableNode
                {
                    text = dataSource.Name,
                    children = new List<Node>(),
                    leaf = false,
                    id = dataSource.Id.ToString(),
                    subroot = true
                }
                : new Node
                {
                    text = dataSource.Name,
                    children = new List<Node>(),
                    leaf = false,
                    id = dataSource.Id.ToString(),
                    subroot = true
                };

            if (!firstLevel)
            {
                var meta = dataSource.GetMetaData();

                if (meta != null)
                {
                    var root = new Node { leaf = false, text = meta.Name, type = meta.Name, id = meta.FullName };

                    foreach (var propInfo in meta.GetProperties())
                    {
                        AppendChildNode(root, propInfo.PropertyType, propInfo.GetDisplayName());
                    }

                    tree.children.Add(root);
                }
            }

            return tree;
        }

        public Node BuildTree(IEnumerable<DataSource> dataSources, bool firstLevel = false, bool checkable = true)
        {
            var root = new Node { text = "Источники данных", children = new List<Node>(), leaf = false, id = "root" };
            foreach (var dataSource in dataSources)
            {
                root.children.Add(BuildTree(dataSource, firstLevel, checkable));
            }
            return root;
        }

        private Node AppendChildNode(Node parent, Type childType, string childText)
        {
            if (parent.children == null)
            {
                parent.children = new List<Node>();
            }
            parent.leaf = false;

            Node child;
            if (childType.IsPrimitive || childType.IsAssignableFrom(typeof(decimal)) ||
                childType.IsAssignableFrom(typeof(string)))
            {
                child = new Node { text = childText, leaf = true, type = GetTypeDisplay(childType) };
            }
            else if (childType.IsArray)
            {
                var typeDisplay = GetTypeDisplay(childType);
                child = new Node
                {
                    type = typeDisplay,
                    children = new List<Node>(),
                    leaf = false,
                    text = childText
                };
                var itemType = childType.GetElementType();
                AppendChildNode(child, itemType, GetTypeDisplay(itemType));
            }
            else if (typeof(IEnumerable).IsAssignableFrom(childType))
            {
                var genArgs = childType.GetGenericArguments();
                var itemType = genArgs.Length > 0 ? genArgs[0] : typeof(object);
                var typeDisplay = GetTypeDisplay(childType);
                child = new Node
                {
                    type = typeDisplay,
                    children = new List<Node>(),
                    leaf = false,
                    text = childText
                };
                AppendChildNode(child, itemType, GetTypeDisplay(itemType));
            }
            else if (childType.IsClass)
            {
                child = new Node
                {
                    children = new List<Node>(),
                    leaf = false,
                    text = childText,
                    type = GetTypeDisplay(childType)
                };
                foreach (var propInfo in childType.GetProperties())
                {
                    AppendChildNode(child, propInfo.GetType(), propInfo.GetDisplayName());
                }
            }
            else
            {
                child = new Node { text = childText, leaf = true, type = GetTypeDisplay(childType) };
            }

            parent.children.Add(child);
            return child;
        }

        public class Node
        {
            public bool leaf;
            public string text;
            public string id;
            public string type;
            public bool loaded = true;
            public List<Node> children;
            public bool subroot;
        }

        public class CheckableNode : Node
        {
            public bool @checked;
        }

        private string GetTypeDisplay(Type type)
        {
            if (type.IsEnum)
            {
                return "Перечисление";
            }
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return "Целое число";
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return "Действительное число";
                case TypeCode.Boolean:
                    return "Логическое";
                case TypeCode.DateTime:
                    return "Дата";
                case TypeCode.String:
                    return "Строка";
                default:
                    return type.Name;
            }
        }
    }
}
