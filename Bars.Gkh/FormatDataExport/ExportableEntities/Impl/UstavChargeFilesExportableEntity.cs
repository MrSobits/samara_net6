namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Протоколы собраний о принятии размера платы за жилое помещение по уставу
    /// </summary>
    public class UstavChargeFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVCHARGEFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var fileCollection = this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ExtProxyListCache
                .Where(x => x.PaymentProtocolFile != null);

            return this.AddFilesToExport(fileCollection, x => x.PaymentProtocolFile)
                .Select(x => new ExportableRow(x.PaymentProtocolFile,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        "1", // передавать всегда 1
                        this.GetStrId(x.PaymentProtocolFile),
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный идентификатор решения о размере платы за жилое помещение по уставу",
                "Тип протокола",
                "Протокол общего собрания членов ТСЖ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(UstavChargeExportableEntity));
        }
    }
}