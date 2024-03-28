namespace Bars.GkhDi.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Документы об утверждении тарифов ЖКУ
    /// </summary>
    public class TarifExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "TARIF";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Ogv |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<TarifProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DocumentName,
                        x.DocumentNumber,
                        this.GetDate(x.DocumentDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.IsPublished.ToStr(),
                        x.RegionCode.ToStr(),
                        x.Type.ToStr(),
                        this.GetStrId(x.AttachmentFile)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 3, 4, 5, 6, 8, 9 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код документа",
                "Наименование документа",
                "Номер документа",
                "Дата принятия документа органом власти",
                "Дата начала действия тарифа",
                "Дата окончания действия тарифа",
                "Опубликован",
                "Код региона ФИАС",
                "Вид тарифа",
                "Скан-копия документа"
            };
        }
    }
}