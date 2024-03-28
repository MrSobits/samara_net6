namespace Bars.GkhCr.DomainService.ObjectCr
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using NHibernate.Util;

    /// <summary>
    /// Домен-сервис <see cref="DesignAssignment"/>
    /// </summary>
    public class DesignAssignmentDomainService : FileStorageDomainService<DesignAssignment>
    {
        /// <summary>
        /// Домен-сервис <see cref="DesignAssignmentTypeWorkCr"/>
        /// </summary>
        public IDomainService<DesignAssignmentTypeWorkCr> DesignAssignmentTypeWorkCrDomain { get; set; }

        /// <summary>Создать объект</summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат создания</returns>
        public override IDataResult Save(BaseParams baseParams)
        {
            var values = new List<DesignAssignment>();
            this.InTransaction(() =>
            {
                var records = baseParams.Params["records"] as List<object>;
                foreach (var record in this.GetSaveParam(baseParams).Records)
                {
                    var obj = record.AsObject("TypeWorksCr");

                    foreach (var file in baseParams.Files)
                    {
                        this.SetFileInfoValue(obj, file, false);
                    }
                    
                    this.SaveInternal(obj);

                    var typeWorkIds = (records[values.Count] as DynamicDictionary).GetAs<long[]>("TypeWorksCr"); ;
                    this.SaveTypeWorks(obj, typeWorkIds);

                    values.Add(obj);
                }
            });
            return new SaveDataResult(values);
        }

        /// <summary>Сохранить изменения объекта</summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат обновления</returns>
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<DesignAssignment>();
            this.InTransaction(() =>
            {
                var records = baseParams.Params["records"] as List<object>;
                foreach (var record in this.GetSaveParam(baseParams).Records)
                {
                    var value = record.AsObject(this.FileProperties.Select(x => x.Name).Union(new[] { "TypeWorksCr" }).ToArray());
                    var dictionary = baseParams.Files.ToDictionary(fileData => fileData.Key, fileData => this.SetFileInfoValue(value, fileData));

                    foreach (var fileProperty in this.FileProperties)
                    {
                        if (record.Properties[fileProperty.Name] == null && !dictionary.ContainsKey(fileProperty.Name))
                        {
                            dictionary.Add(fileProperty.Name, this.SetFileInfoValue(value, new KeyValuePair<string, FileData>(fileProperty.Name, null)));
                        }
                    }
                    
                    this.UpdateInternal(value);

                    foreach (var persistentObject in dictionary.Values.Where(file => file != null))
                    {
                        this.FileInfoService.Delete(persistentObject.Id);
                    }

                    var typeWorkIds = (records[values.Count] as DynamicDictionary).GetAs<long[]>("TypeWorksCr");
                    this.SaveTypeWorks(value, typeWorkIds);

                    values.Add(value);
                }
            });
            return new BaseDataResult(values);
        }

        private void SaveTypeWorks(DesignAssignment value, long[] ids)
        {
            var exists = this.DesignAssignmentTypeWorkCrDomain.GetAll().Where(x => x.DesignAssignment.Id == value.Id).ToList();
            var typeworksDomain = Container.Resolve<IDomainService<TypeWorkCr>>();
            var delete = exists.Where(x => !ids.Contains(x.TypeWorkCr.Id));            
            EnumerableExtensions.ForEach(delete, x => this.DesignAssignmentTypeWorkCrDomain.Delete(x.Id));

            foreach (var id in ids.Where(x => !exists.Select(y => y.TypeWorkCr.Id).Contains(x)))
            {
                var typeWork = new DesignAssignmentTypeWorkCr
                {
                    DesignAssignment = value,
                    TypeWorkCr = typeworksDomain.Get(id)
                    // TypeWorkCr = new TypeWorkCr { Id = id }
                };

                this.DesignAssignmentTypeWorkCrDomain.Save(typeWork);
                value.Add(typeWork);
            }
        }
    }
}