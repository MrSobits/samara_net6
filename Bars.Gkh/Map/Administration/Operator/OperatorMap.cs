namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    

    /// <summary>Маппинг для "Оператор проекта БарсЖКХ"</summary>
    public class OperatorMap : BaseImportableEntityMap<Operator>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public OperatorMap() : 
                base("Оператор проекта БарсЖКХ", "GKH_OPERATOR")
        {
        }
        
        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeWorkplace, "Тип рабочего места").Column("TYPE_WORKPLACE").NotNull();
            this.Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            this.Property(x => x.IsActive, "Пользователь активен").Column("IS_ACTIVE");
            this.Property(x => x.ContragentType, "Тип контрагента (для 1468)").Column("PKU_PR").NotNull();
            this.Property(x => x.RisToken, "Токен кросс-авторизации РИС").Column("RIS_TOKEN");
            this.Property(x => x.MobileApplicationAccessEnabled, "Доступ к мобильному приложению").Column("MOBILE_APP_ACCESS_ENABLED").NotNull().DefaultValue(false);
            this.Reference(x => x.Contragent, "Контрагент (для 1468)").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.GisGkhContragent, "Контрагент (для ГИС ЖКХ)").Column("GIS_GKH_CONTRAGENT_ID").Fetch();
            this.Reference(x => x.User, "User").Column("USER_ID").NotNull().Fetch();
            this.Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").Fetch();
            this.Property(x => x.ExportFormat, "Формат выгрузки отчетов").Column("EXPORT_FORMAT");
            this.Reference(x => x.UserPhoto, "Фото пользователя").Column("USER_PHOTO_FILE_ID").Fetch();
        }
    }
}
