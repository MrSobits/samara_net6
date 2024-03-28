namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Информация о температурном графике
    /// </summary>
    public class DrsoTempExportableEntity : BaseExportableEntity<PublicServiceOrgTemperatureInfo>
    {
        /// <inheritdoc />
        public override string Code => "DRSOTEMP";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.Contract.PublicServiceOrg.Contragent)
                .Select(x => new
                {
                    x.Id,
                    DrsoId = x.Contract.Id,
                    x.OutdoorAirTemp,
                    x.CoolantTempSupplyPipeline,
                    x.CoolantTempReturnPipeline
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.DrsoId.ToStr(), // 2. Договор ресурсоснабжения
                        x.OutdoorAirTemp.ToStr(), // 3. Температура наружного воздуха
                        x.CoolantTempSupplyPipeline.ToStr(), // 4. Температура теплоносителя в подающем трубопроводе
                        x.CoolantTempReturnPipeline.ToStr() // 5. Температура теплоносителя в обратном трубопроводе
                    })
                )
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Договор ресурсоснабжения",
                "Температура наружного воздуха",
                "Температура теплоносителя в подающем трубопроводе",
                "Температура теплоносителя в обратном трубопроводе"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSO"
            };
        }
    }
}