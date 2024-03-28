namespace Bars.Gkh.ImportExport
{
    using System;
    using System.Collections.Generic;

    public class EntityExportContainer
    {
        private readonly Dictionary<Type, EntityExportMeta> _container = new Dictionary<Type, EntityExportMeta>();

        public Dictionary<Type, EntityExportMeta> Container { get { return _container; } } 

        public void Add(Type type, string description, bool isSystem = false)
        {
            _container.Add(type, new EntityExportMeta {Description = description, IsSystem = isSystem});
        }

        public void Add(Dictionary<Type, string> collection)
        {
            foreach (var kv in collection)
            {
                _container.Add(kv.Key, new EntityExportMeta {Description = kv.Value, IsSystem = false});
            }
        }

        
    }

    public class EntityExportMeta
    {
        public string Description { get; set; }

        public bool IsSystem { get; set; }
    }
}