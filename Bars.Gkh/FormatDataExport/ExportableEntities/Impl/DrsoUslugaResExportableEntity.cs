namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Коммунальные услуги и ресурсы к договору ресурсоснабжения
    /// </summary>
    public class DrsoUslugaResExportableEntity : BaseExportableEntity<PublicServiceOrgContractService>
    {
        /// <inheritdoc />
        public override string Code => "DRSOUSLUGARES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Rso;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var addressDict = this.ProxySelectorFactory.GetSelector<DrsoAddressProxy>()
                .ExtProxyListCache
                .Where(x => x.DrsoId.HasValue)
                .ToDictionary(x => x.DrsoId.Value, x => x.Id.ToString());

            return this.GetFiltred(x => x.ResOrgContract.PublicServiceOrg.Contragent)
                .Select(x => new
                {
                    x.Id,
                    x.Service.ExportId,
                    DrsoId = x.ResOrgContract.Id,
                    ServiceId = x.Service.Id,
                    CommunalResource = x.CommunalResource.Name,
                    x.StartDate,
                    x.EndDate,
                    x.HeatingSystemType,
                    x.SchemeConnectionType
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        addressDict.Get(x.DrsoId), // 2. Адрес объектов жилищного фонда к договору ресурсоснабжения
                        x.ExportId.ToStr(), // 3. Вид коммунальной услуги
                        this.GetCommunalResource(x.CommunalResource).ToStr(), // 4. Тарифицируемый ресурс
                        this.GetDate(x.StartDate), // 5. Дата начала поставки ресурса
                        this.GetDate(x.EndDate), // 6. Дата окончания поставки ресурса
                        this.GetHeatingSystemType(x.HeatingSystemType).ToStr(), // 7. Тип системы теплоснабжения (Открытая/Закрытая)
                        this.GetSchemeConnectionType(x.SchemeConnectionType).ToStr(), // 8. Тип системы теплоснабжения (Централизованная/ нецентрализованная)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                case 7:
                    // для коммунальных ресурсов «Тепловая энергия» и «Горячая вода»
                    return (row.Cells[3] == "3" || row.Cells[3] == "4") && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Адрес объектов жилищного фонда к договору ресурсоснабжения",
                "Вид коммунальной услуги",
                "Тарифицируемый ресурс",
                "Дата начала поставки ресурса",
                "Дата окончания поставки ресурса",
                "Тип системы теплоснабжения (Открытая/Закрытая)",
                "Тип системы теплоснабжения (Централизованная/ нецентрализованная)"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DRSOADDRESS",
                "DICTUSLUGA"
            };
        }

        private int? GetCommunalResource(string resource)
        {
            switch (resource.ToLower())
            {
                case "питьевая вода":
                    return 1;
                case "техническая вода":
                    return 2;
                case "горячая вода":
                    return 3;
                case "тепловая энергия":
                    return 4;
                case "теплоноситель":
                    return 5;
                case "поддерживаемая мощность":
                    return 6;
                case "сточные воды":
                    return 7;
                case "электрическая энергия":
                    return 8;
                case "природный газ (метан)":
                    return 9;
                case "сжиженный газ (пропан-бутан)":
                    return 10;
                case "топливо твердое":
                    return 11;
                case "топливо печное бытовое":
                    return 12;
                case "керосин":
                    return 13;
                default:
                    return null;
            }
        }

        private int? GetHeatingSystemType(HeatingSystemType? heatingSystemType)
        {
            switch (heatingSystemType)
            {
                case HeatingSystemType.Opened:
                    return 1;
                case HeatingSystemType.Closed:
                    return 2;
                default:
                    return null;
            }
        }

        private int? GetSchemeConnectionType(SchemeConnectionType? schemeConnectionType)
        {
            switch (schemeConnectionType)
            {
                case SchemeConnectionType.Independent:
                    return 1;
                case SchemeConnectionType.Dependent:
                    return 2;
                default:
                    return null;
            }
        }
    }
}