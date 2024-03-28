namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    using NHibernate.Util;

    public class InspectionBaseTypeDomainService : BaseDomainService<InspectionBaseType>
    {
        public override IDataResult Save(Bars.B4.BaseParams baseParams)
        {
            List<InspectionBaseType> values = new List<InspectionBaseType>();
            this.InTransaction(() =>
            {
                foreach (SaveRecord<InspectionBaseType> record in this.GetSaveParam(baseParams).Records)
                {
                    InspectionBaseType obj = record.AsObject(new string[0]);
                    this.SaveInternal(obj);

                    SaveInspectionTypeBaseKindCheck(obj, record.NonObjectProperties.GetAs<List<KindCheckGji>>("KindCheck"));
                    
                    values.Add(obj);
                }
            });
            
            return new SaveDataResult((object) values);
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            List<InspectionBaseType> values = new List<InspectionBaseType>();
            this.InTransaction(() =>
            {
                foreach (SaveRecord<InspectionBaseType> record in this.GetSaveParam(baseParams).Records)
                {
                    InspectionBaseType obj = record.AsObject(new string[0]);
                    this.UpdateInternal(obj);

                    SaveInspectionTypeBaseKindCheck(obj, record.NonObjectProperties.GetAs<List<KindCheckGji>>("KindCheck"));
                    
                    values.Add(obj);
                }
            });
            
            return new BaseDataResult((object) values);
        }

        private void SaveInspectionTypeBaseKindCheck(InspectionBaseType inspectionType, IEnumerable<KindCheckGji> kindChecks)
        {
            if (kindChecks == null)
            {
                return;
            }
            
            var inspectionBaseTypeKindCheckDomain = this.Container.ResolveDomain<InspectionBaseTypeKindCheck>();

            using (Container.Using(inspectionBaseTypeKindCheckDomain))
            {
                var existedObjects = inspectionBaseTypeKindCheckDomain.GetAll()
                    .Where(x => x.InspectionBaseType.Id == inspectionType.Id)
                    .AsEnumerable();
                var objectsToSave = kindChecks
                    .Where(x => existedObjects.All(y => y.KindCheck.Id != x.Id))
                    .Select(x => new InspectionBaseTypeKindCheck()
                {
                    InspectionBaseType = inspectionType,
                    KindCheck = x
                });
                var objectsToDelete = existedObjects
                    .Where(x => kindChecks.All(y => y.Id != x.KindCheck.Id))
                    .Select(x => x.Id);

                objectsToDelete.ForEach(x => inspectionBaseTypeKindCheckDomain.Delete(x));
                objectsToSave.ForEach(x => inspectionBaseTypeKindCheckDomain.SaveOrUpdate(x));
            }
        }
    }
}