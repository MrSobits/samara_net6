namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class RealityObjectStructuralElementDomainService : FileStorageDomainService<RealityObjectStructuralElement>
    {
        private IDomainService<StructuralElementGroupAttribute> _atrService;
        private IDomainService<RealityObjectStructuralElementAttributeValue> _valuesService;

        public IDomainService<StructuralElementGroupAttribute> AtrService
        {
            get
            {
                return _atrService ?? (_atrService = Container.Resolve<IDomainService<StructuralElementGroupAttribute>>());
            }
        }

        public IDomainService<RealityObjectStructuralElementAttributeValue> ValuesService
        {
            get
            {
                return _valuesService ?? (_valuesService = Container.Resolve<IDomainService<RealityObjectStructuralElementAttributeValue>>());
            }
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<RealityObjectStructuralElement>();
            InTransaction(delegate
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject(FileProperties.Select((PropertyInfo x) => x.Name).ToArray());
                    if (value.RealityObject == null)
                    {
                        value.RealityObject = record.Entity.RealityObject;
                    }
                    Dictionary<string, FileInfo> dictionary = baseParams.Files.ToDictionary((KeyValuePair<string, FileData> fileData) => fileData.Key, (KeyValuePair<string, FileData> fileData) => SetFileInfoValue(value, fileData));
                    PropertyInfo[] fileProperties = FileProperties;
                    foreach (PropertyInfo propertyInfo in fileProperties)
                    {
                        if (record.Properties[propertyInfo.Name] == null && !dictionary.ContainsKey(propertyInfo.Name))
                        {
                            dictionary.Add(propertyInfo.Name, SetFileInfoValue(value, new KeyValuePair<string, FileData>(propertyInfo.Name, null)));
                        }
                    }

                    UpdateInternal(value);
                    foreach (FileInfo item in dictionary.Values.Where((FileInfo file) => file != null))
                    {
                        FileInfoService.Delete(item.Id);
                    }

                    values.Add(value);

                    SaveValues(record.NonObjectProperties["Values"], value);
                }
            });
            return new BaseDataResult(values);
        }

        public override IDataResult Save(BaseParams baseParams)
        {
            var values = new List<RealityObjectStructuralElement>();
            InTransaction(delegate
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    foreach (KeyValuePair<string, FileData> file in baseParams.Files)
                    {
                        SetFileInfoValue(value, file, getCurrentValue: false);
                    }

                    SaveInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["Values"], value);
                }
            });
            return new SaveDataResult(values);
        }

        private void SaveValues(object vals, RealityObjectStructuralElement value)
        {
            var atrValues = vals as List<object>;
            if (atrValues != null)
            {
                foreach (var atrValue in atrValues)
                {
                    var val = atrValue as DynamicDictionary;

                    if (val == null)
                        continue;

                    var id = val.GetAs<long>("Id");

                    if (id == 0)
                    {
                        ValuesService.Save(new RealityObjectStructuralElementAttributeValue
                        {
                            Object = value,
                            Attribute = AtrService.Load(val.GetAs<long>("Attribute")),
                            Value = val.Get("Value", string.Empty)
                        });
                    }
                    else
                    {
                        var obj = ValuesService.Get(id);

                        obj.Value = val.Get("Value", string.Empty);

                        ValuesService.Update(obj);
                    }
                }
            }
        }
    }
}