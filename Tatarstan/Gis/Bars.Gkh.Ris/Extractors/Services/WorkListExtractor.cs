namespace Bars.Gkh.Ris.Extractors.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    /// <summary>
    /// Экстрактор данных перечней работ и услуг
    /// </summary>
    public class WorkListExtractor : BaseDataExtractor<WorkList, RepairObject>
    {
        /// <summary>
        /// Сервис для сохранения вложений файлов на рест-сервис
        /// </summary>
        public IAttachmentService AttachmentService { get; set; }

        private Dictionary<string, RisHouse> risHouseByFiasGuid = null;
        private Dictionary<string, RisContract> actualContractsByHouseFias = null;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var risHouseDomain = this.Container.ResolveDomain<RisHouse>();
            var contractObjectDomain = this.Container.ResolveDomain<ContractObject>();

            try
            {
                this.risHouseByFiasGuid = risHouseDomain.GetAll()
                    .Where(x => x.FiasHouseGuid != null && x.FiasHouseGuid != "")
                    .GroupBy(x => x.FiasHouseGuid)
                    .ToDictionary(x => x.Key, x => x.First());

                this.actualContractsByHouseFias = contractObjectDomain.GetAll()
                    .Where(x => x.FiasHouseGuid != null && x.FiasHouseGuid != "")
                    .Where(x => x.Contract != null) // у нас загружены только актуальные договоры
                    .ToArray()
                    .Select(x => new
                    {
                        x.FiasHouseGuid,
                        x.Contract
                    })
                    .GroupBy(x => x.FiasHouseGuid)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Contract).First());
            }
            finally
            {
                this.Container.Release(risHouseDomain);
                this.Container.Release(contractObjectDomain);
            }

        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<RepairObject> GetExternalEntities(DynamicDictionary parameters)
        {
            var repairObjects = default(List<RepairObject>);
            var selectedRepairObjectsParam = parameters.GetAs<string>("selectedRepairObjects");

            if (string.IsNullOrEmpty(selectedRepairObjectsParam) || selectedRepairObjectsParam.ToUpper() == "ALL")
            {
                repairObjects = this.GetRepairObjectsQuery().ToList();
            }
            else
            {
                var repairObjectDomain = this.Container.ResolveDomain<RepairObject>();
                var selectedRepairObjectIds = selectedRepairObjectsParam.ToLongArray();

                try
                {
                    repairObjects = repairObjectDomain.GetAll()
                        .Where(x => selectedRepairObjectIds.Contains(x.Id))
                        .ToList();
                }
                finally
                {
                    this.Container.Release(repairObjectDomain);
                }
            }

            return repairObjects;
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="repairObject">Сущность внешней системы</param>
        /// <param name="workList">Ris сущность</param>
        protected override void UpdateRisEntity(RepairObject repairObject, WorkList workList)
        {
            workList.ExternalSystemEntityId = repairObject.Id;

            workList.ExternalSystemName = "gkh";

            workList.Contract = this.actualContractsByHouseFias.Get(repairObject.RealityObject.HouseGuid);

            workList.House = this.risHouseByFiasGuid.Get(repairObject.RealityObject.HouseGuid);

            workList.MonthFrom = repairObject.RepairProgram.Period.DateStart.Month;

            workList.YearFrom = (short)repairObject.RepairProgram.Period.DateStart.Year;

            workList.MonthTo = repairObject.RepairProgram.Period.DateEnd.HasValue
                ? repairObject.RepairProgram.Period.DateEnd.Value.Month
                : 0;

            workList.YearTo = (short)(repairObject.RepairProgram.Period.DateEnd.HasValue
                ? repairObject.RepairProgram.Period.DateEnd.Value.Year
                : 0);

            if (repairObject.ReasonDocument != null)
            {
                try
                {
                    workList.Attachment = this.AttachmentService.CreateAttachment(
                        repairObject.ReasonDocument,
                        repairObject.Comment);
                }
                catch (FileNotFoundException)
                {
                    workList.Attachment = null;
                    this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл перечня работ и услуг " + repairObject.Comment + " не найден"));
                }
            }
        }

        /// <summary>
        /// Метод получения запроса для выбора объектов текущего ремонта
        /// </summary>
        /// <returns>Запрос</returns>
        private IQueryable<RepairObject> GetRepairObjectsQuery()
        {
            var repairObjectDomain = this.Container.ResolveDomain<RepairObject>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();

            try
            {
                var currentContragent = this.Contragent;

                // Найти объекты недвижимости, у которых заведены действующие договоры управления =>
                // => В договоре должно быть НЕ заполнено поле "Основание расторжения" И его "Дата окончания управления" > Текущая дата или = null
                var realityObjectIds = manOrgContractRealityObjectDomain.GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == currentContragent.GkhId)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Today)
                    .Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == "")
                    .Select(x => x.RealityObject.Id);

                // Найти объекты текущего ремонта
                var repairObjects = repairObjectDomain.GetAll()
                    .Where(
                        x => x.RepairProgram != null
                            && x.RepairProgram.TypeProgramRepairState == TypeProgramRepairState.Active
                            || x.RepairProgram.TypeProgramRepairState == TypeProgramRepairState.New)
                    .Where(x => x.RepairProgram.Period != null)
                    .Where(x => x.RealityObject != null && realityObjectIds.Any(y => x.RealityObject.Id == y));

                return repairObjects;
            }
            finally
            {
                this.Container.Release(repairObjectDomain);
                this.Container.Release(manOrgContractRealityObjectDomain);
            }
        }
    }
}
