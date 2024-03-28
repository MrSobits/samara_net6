namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис экспорта в формате 4.0
    /// </summary>
    public class Format40DataExportService : IFormat40DataExportService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод отправки ТехПаспорта в ГИС ЖКХ
        /// </summary>
        public IDataResult SendTechPassport(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var manOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var formatDataExportTask = this.Container.Resolve<IDomainService<FormatDataExportTask>>();

            try
            {
                var manOrgContractList = manOrgContractRealityObject.GetAll()
                    .Where(x => x.RealityObject.Id == realityObjectId)
                    .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate <= DateTime.Now)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= DateTime.Now)
                    .Select(x => x.ManOrgContract)
                    .ToList();

                var contragentId = manOrgContractList
                    .WhereIf(manOrgContractList.Count != 1, 
                        x => x.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgJskTsj)
                    .Select(x => x.ManagingOrganization.Contragent.Id)
                    .FirstOrDefault();

                if (contragentId == 0)
                {
                    return new BaseDataResult(false, "Головная организация не была определена");
                }

                //В baseParams добавляем необходимые для метода Save параметры
                PrepareBaseParams(baseParams, "HouseEntityGroup", contragentId, realityObjectId);

                return formatDataExportTask.Save(baseParams);
            }
            finally
            {
                this.Container.Release(manOrgContractRealityObject);
                this.Container.Release(formatDataExportTask);
            }
        }


        /// <summary>
        /// Метод отправки договора/устава в ГИС ЖКХ
        /// </summary>
        public IDataResult SendDuUstav(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");
            var entityGroup = baseParams.Params.GetAs<string>("entityGroup");
            var manOrgBaseContract = this.Container.Resolve<IDomainService<ManOrgBaseContract>>();
            var formatDataExportTask = this.Container.Resolve<IDomainService<FormatDataExportTask>>();

            try
            {
                var contragentId = manOrgBaseContract.GetAll()
                    .Where(x => x.Id == contractId)
                    .Select(x => x.ManagingOrganization.Contragent.Id)
                    .FirstOrDefault();

                if (contragentId == 0)
                {
                    return new BaseDataResult(false, "Головная организация не была определена");
                }

                if (entityGroup == "DuEntityGroup" || entityGroup == "UstavEntityGroup")
                {
                    //В baseParams добавляем необходимые для метода Save параметры
                    PrepareBaseParams(baseParams, entityGroup, contragentId, contractId);

                    return formatDataExportTask.Save(baseParams); 
                }

                return new BaseDataResult(false, "Некорректное название секции");
            }
            finally
            {
                this.Container.Release(manOrgBaseContract);
                this.Container.Release(formatDataExportTask);
            }
        }

        /// <summary>
        /// Добавляет указанные параметры в baseParams.
        /// </summary>
        /// <param name="entityGroup">Название секции</param>
        /// <param name="mainContragentId">Идентификатор контрагента</param>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <remarks>Метод подготавливает и преобразует baseParams в необходимый для метода Save вид.</remarks>
        private void PrepareBaseParams(BaseParams baseParams, string entityGroup, long mainContragentId, long entityId)
        {
            DynamicDictionary recordsParams = new DynamicDictionary(),
                BaseParams = new DynamicDictionary(),
                Params = new DynamicDictionary(),
                InspectionFilter = new DynamicDictionary(),
                PersAccFilter = new DynamicDictionary(),
                ProgramVersionFilter = new DynamicDictionary(),
                ObjectCrFilter = new DynamicDictionary(),
                DuUstavFilter = new DynamicDictionary(),
                RealityObjectFilter = new DynamicDictionary();

            var entityGroupList = new List<string>();
            entityGroupList.Add(entityGroup);

            var entityParamList = new List<long>();
            entityParamList.Add(entityId);

            var recordsParamsList = new List<DynamicDictionary>();
            recordsParamsList.Add(recordsParams);

            Params.Add("MainContragent", mainContragentId);
            Params.Add("InspectionFilter", InspectionFilter);
            Params.Add("PersAccFilter", PersAccFilter);
            Params.Add("ProgramVersionFilter", ProgramVersionFilter);
            Params.Add("ObjectCrFilter", ObjectCrFilter);
            Params.Add("DuUstavFilter", DuUstavFilter);
            Params.Add("RealityObjectFilter", RealityObjectFilter);

            string entityGroupCodeList = string.Empty;
            switch (entityGroup)
            {
                case "HouseEntityGroup":
                    entityGroupCodeList = "RealityObjectList";
                    break;
                case "DuEntityGroup":
                case "UstavEntityGroup":
                    entityGroupCodeList = "DuUstavList";
                    break;
            }
            Params.Add(entityGroupCodeList, entityParamList);

            BaseParams.Add("Params", Params);

            recordsParams.Add("StartNow", true);
            recordsParams.Add("EntityGroupCodeList", entityGroupList);
            recordsParams.Add("BaseParams", BaseParams);
            recordsParams.Add("MainContragent", mainContragentId);

            baseParams.Params.Add("records", recordsParamsList);
        }
    }
}
