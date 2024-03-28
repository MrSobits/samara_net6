namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    public class AppealCitsTransferResultMap : BaseEntityMap<AppealCitsTransferResult>
    {
        /// <inheritdoc />
        public AppealCitsTransferResultMap() : 
                base("Результат обмена данными обращений граждан", "GJI_APPCIT_TRANSFER_RESULT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Type, "Тип операции").Column("TYPE").NotNull();
            this.Property(x => x.Status, "Статус операции").Column("STATUS").NotNull();
            this.Property(x => x.StartDate, "Дата и время начала").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата и время окончания").Column("END_DATE");
            this.Property(x => x.ExportParams, "Параметры запроса").Column("EXPORT_PARAMS");

            this.Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").NotNull();
            this.Reference(x => x.User, "Пользователь").Column("USER_ID");
            this.Reference(x => x.LogFile, "Файл лога").Column("LOG_FILE_ID");
        }

        public class AppealCitsTransferResultMapNHibernateMapping : ClassMapping<AppealCitsTransferResult>
        {
            public AppealCitsTransferResultMapNHibernateMapping()
            {
                this.Property(x => x.ExportParams, m =>
                {
                    m.Type<ImprovedBinaryJsonType<BaseParams>>();
                });
            }
        }
    }
}
