namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Управляющие организации, товарищества собственников жилья, кооперативы
    /// </summary>
    public class UoExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "UO";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UoProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.LeaderPhone,
                        x.AdministrativeStaffCount.ToStr(),
                        x.EngineersCount.ToStr(),
                        x.EmployeesCount.ToStr(),
                        this.GetDecimal(x.ShareSf),
                        this.GetDecimal(x.ShareMo),
                        x.IsTsj.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Контактный номер председателя",
                "Штатная численность административного персонала",
                "Штатная численность инженеров",
                "Штатная численность рабочих",
                "Доля участия субъекта Российской Федерации в уставном капитале организации",
                "Доля участия муниципального образования в уставном капитале организации",
                "Признак ТСЖ/кооператив"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentExportableEntity));
        }
    }
}