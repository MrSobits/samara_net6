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
    /// Экстрактор частей сети
    /// </summary>
    public class RkiNetPiecesExtractor : BaseDataExtractor<RisNetPieces, NetPart>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<NetPart> GetExternalEntities(DynamicDictionary parameters)
        {
            var okiCommunalServiceRepository = this.Container.ResolveDomain<NetPart>();

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

            try
            {
                var res = okiCommunalServiceRepository
                    .GetAll()
                    .Where(x => x.DataSupplier != null && x.DataSupplier.Ogrn == this.Contragent.Ogrn)
                    .Where(x => x.OkiObject != null)
                    .WhereIf(selectedRecordsIds.Any(), x => selectedRecordsIds.Contains(x.OkiObject.Id))
                    .ToList();
                return res;
            }
            finally
            {
                this.Container.Release(okiCommunalServiceRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(NetPart externalEntity, RisNetPieces risEntity)
        {
            var rkiRepository = this.Container.ResolveDomain<RisRkiItem>();

            try
            {
                risEntity.ExternalSystemEntityId = externalEntity.Id;
                risEntity.ExternalSystemName = "ris";
                risEntity.Contragent = this.Contragent;
                risEntity.Operation = string.IsNullOrEmpty(risEntity.Guid)
                    ? RisEntityOperation.Create
                    : RisEntityOperation.Update;
                risEntity.RkiItem = rkiRepository
                    .GetAll()
                    .Where(x => x.ExternalSystemName == "ris")
                    .FirstOrDefault(x => x.ExternalSystemEntityId == externalEntity.OkiObject.Id);
                risEntity.Name = externalEntity.NetPartName;
                risEntity.Diameter = externalEntity.Diameter;
                risEntity.Length = externalEntity.Length;
                risEntity.NeedReplaced = externalEntity.ChangeKmCnt;
                risEntity.Wearout = externalEntity.Wearout;
                risEntity.PressureCode = externalEntity.GasPressure?.DictCode;
                risEntity.PressureGuid = externalEntity.GasPressure?.GisGuid;
                risEntity.PressureName = externalEntity.GasPressure?.Value;
                risEntity.VoltageCode = externalEntity.Voltage?.DictCode;
                risEntity.VoltageGuid = externalEntity.Voltage?.GisGuid;
                risEntity.VoltageName = externalEntity.Voltage?.Value;
            }
            finally
            {
                this.Container.Release(rkiRepository);
            }
        }
    }
}