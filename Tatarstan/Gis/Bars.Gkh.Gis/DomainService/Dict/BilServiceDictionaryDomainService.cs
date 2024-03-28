namespace Bars.Gkh.Gis.DomainService.Dict
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DomainService.BaseParams;

    using Bars.Gkh.Entities.Dicts;

    using Entities.Kp50;

    public class BilServiceDictionaryDomainService : BaseDomainService<BilServiceDictionary>
    {
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");

            var bilServiceDictList = GetAll().Where(x => ids.Contains(x.Id)).ToList();

            foreach (var bilServiceDict in bilServiceDictList)
            {
                bilServiceDict.Service = null;
                Save(bilServiceDict);
            }

            return new BaseDataResult(ids);
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<BilServiceDictionary>();
            
            var saveParam = GetSaveParam(baseParams);
            foreach (var record in saveParam.Records)
            {
                var servProp = record.Properties.ContainsKey("Service") ? record.Properties["Service"] as ServiceDictionary : null;

                if (servProp != null 
                    && record.Entity.Service != null
                    && servProp.Id != record.Entity.Service.Id)
                {
                    return new BaseDataResult(false, 
                        string.Format("Услуга биллинга \"{0}\" уже привязана к эталонной услуге \"{1}\"",
                            record.Entity.ServiceName, record.Entity.Service.Name));
                }
                
                var value = record.AsObject();
                UpdateInternal(value);
                values.Add(value);
            }

            return new BaseDataResult(values);
        }
    }
}