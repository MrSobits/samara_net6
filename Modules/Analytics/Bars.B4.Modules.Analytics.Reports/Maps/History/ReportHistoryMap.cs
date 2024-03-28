namespace Bars.B4.Modules.Analytics.Reports.Maps.History
{
    using System.Collections.Generic;
    
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.B4.Modules.Analytics.Reports.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ReportHistoryMap : BaseEntityMap<ReportHistory>
    {
        /// <inheritdoc />
        public ReportHistoryMap() : base(typeof(ReportHistory).FullName, "AL_REPORT_HISTORY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ReportType, "Тип отчета").Column("REPORT_TYPE").NotNull();
            this.Property(x => x.ReportId, "Id отчета").Column("REPORT_ID");

            this.Property(x => x.Date, "Дата печати").Column("DATE").NotNull();
            this.Property(x => x.Name, "Наименование отчета").Column("NAME").Length(250);
            this.Property(x => x.ParameterValues, "Словарь значений параметров").Column("PARAM_VALUES");

            this.Reference(x => x.File, "Файл").Column("FILE_ID").NotNull().Fetch();
            this.Reference(x => x.Category, "Категория отчета").Column("CATEGORY_ID").NotNull().Fetch();
            this.Reference(x => x.User, "Пользователь").Column("USER_ID").Fetch();
        }

        public class ReportHistoryNHibernateMapping : ClassMapping<ReportHistory>
        {
            public ReportHistoryNHibernateMapping()
            {
                this.Property(x => x.ParameterValues, m =>
                {
                    m.Type<ImprovedBinaryJsonType<Dictionary<string, ReportHistoryParam>>>();
                });
            }
        }
    }
}