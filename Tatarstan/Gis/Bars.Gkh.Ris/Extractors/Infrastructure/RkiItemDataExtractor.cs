namespace Bars.Gkh.Ris.Extractors.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;
    using Bars.GisIntegration.Base.Entities.Infrastructure;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Экстрактор ОКИ
    /// </summary>
    public class RkiItemDataExtractor : BaseDataExtractor<RisRkiItem, OkiObject>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<OkiObject> GetExternalEntities(DynamicDictionary parameters)
        {
            var okiRepository = this.Container.ResolveDomain<OkiObject>();

            long[] selectedRecordsIds = { };

            var selectedRecords = parameters.GetAs("selectedRecords", string.Empty);
            if (selectedRecords.ToUpper() == "ALL")
            {
                selectedRecordsIds = new long[0]; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedRecordsIds = selectedRecords.ToLongArray();
            }

            var contragent = parameters.GetAs("Contragent", this.Contragent);

            try
            {
                var res = okiRepository
                    .GetAll()
                    .Where(x => x.DataSupplier != null && x.DataSupplier.Ogrn == contragent.Ogrn)
                    .WhereIf(selectedRecordsIds.Any(), x => selectedRecordsIds.Contains(x.Id))
                    .ToList();
                return res;
            }
            finally
            {
                this.Container.Release(okiRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(OkiObject externalEntity, RisRkiItem risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Contragent = this.Contragent;
            risEntity.Operation = string.IsNullOrEmpty(risEntity.Guid)
                ? RisEntityOperation.Create
                : RisEntityOperation.Update;

            risEntity.Name = externalEntity.ObjectName;
            risEntity.BaseCode = externalEntity.RunBase?.DictCode;
            risEntity.BaseGuid = externalEntity.RunBase?.GisGuid;
            risEntity.EndManagmentDate = externalEntity.ManagTo;
            risEntity.IndefiniteManagement = externalEntity.IsUnlimManag;
            risEntity.RisRkiContragent = this.Contragent;
            risEntity.Municipalities = externalEntity.IsMoBalance;
            risEntity.TypeCode = externalEntity.OkiType?.DictCode;
            risEntity.TypeGuid = externalEntity.OkiType?.GisGuid;
            risEntity.WaterIntakeCode = externalEntity.WaterIntakeType?.DictCode;
            risEntity.WaterIntakeGuid = externalEntity.WaterIntakeType?.GisGuid;
            risEntity.ESubstationCode = externalEntity.ElectroSubstantionType?.DictCode;
            risEntity.ESubstationGuid = externalEntity.ElectroSubstantionType?.GisGuid;
            risEntity.PowerPlantCode = externalEntity.ElectroStationType?.DictCode;
            risEntity.PowerPlantGuid = externalEntity.ElectroStationType?.GisGuid;
            risEntity.FuelCode = externalEntity.FuelType?.DictCode;
            risEntity.FuelGuid = externalEntity.FuelType?.GisGuid;
            risEntity.GasNetworkCode = externalEntity.GasNetType?.DictCode;
            risEntity.GasNetworkGuid = externalEntity.GasNetType?.GisGuid;
            risEntity.OktmoCode = externalEntity.MoTerritory?.Oktmo;
            risEntity.OktmoName = externalEntity.MoTerritory?.MoName;
            risEntity.IndependentSource = externalEntity.IsAutonom;
            risEntity.Deterioration = externalEntity.Wearout;
            risEntity.CountAccidents = externalEntity.CrashCount?.ToInt();
            risEntity.AddInfo = externalEntity.Comment;
            risEntity.FiasAddress = externalEntity.ObjectAddress;
            if (externalEntity.StartUpFrom != null && externalEntity.StartUpFrom <= short.MaxValue)
            {
                risEntity.CommissioningYear = (short)externalEntity.StartUpFrom;
            }
        }
    }
}