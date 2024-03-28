namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Маппинг <see cref="AdditionalParameters"/>
    /// </summary>
    public class AdditionalParametersMap : BaseImportableEntityMap<AdditionalParameters>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AdditionalParametersMap()
            : base("Bars.GkhCr.Entities.AdditionalParameters", "CR_OBJ_ADDITIONAL_PARAMS")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ObjectCr, "Объект КР").Column("OBJECT_ID").NotNull();
            this.Property(x => x.RequestKtsDate, "Дата поступления запроса в КТС").Column("REQUEST_KTS_DATE");
            this.Property(x => x.TechConditionKtsDate, "Дата выдачи технических условий").Column("TECH_COND_KTS_DATE");
            this.Property(x => x.TechConditionKtsRecipient, "Технические условия выданы (кому)").Column("TECH_COND_KTS_RECIPIENT");
            this.Property(x => x.RequestVodokanalDate, "Дата поступления запроса в МУП Водоканал").Column("REQUEST_VODOKANAL_DATE");
            this.Property(x => x.TechConditionVodokanalDate, "Дата выдачи технических условий").Column("TECH_COND_VODOKANAL_DATE");
            this.Property(x => x.TechConditionVodokanalRecipient, "Технические условия выданы (кому)").Column("TECH_COND_VODOKANAL_RECIPIENT");
            this.Property(x => x.EntryForApprovalDate, "Дата поступления проекта на согласование").Column("ENTRY_FOR_APPROVAL_DATE");
            this.Property(x => x.ApprovalKtsDate, "Дата согласования проекта в КТС").Column("APPROVAL_KTS_DATE");
            this.Property(x => x.ApprovalVodokanalDate, "Дата согласования проекта в МУП Водоканал").Column("APPROVAL_VODOKANAL_DATE");
            this.Property(x => x.InstallationPercentage, "Процент монтажа проекта").Column("INSTALL_PERCENT");
            this.Property(x => x.ClientAccepted, "Статус приемки объекта Заказчиком").Column("CLIENT_ACCEPT");
            this.Property(x => x.ClientAcceptedChangeDate, "Дата изменения статуса приемки объекта заказчиком").Column("CLIENT_ACCEPT_CHANGE_DATE");
            this.Property(x => x.InspectorAccepted, "Статус приемки объекта инспектором Ростехнадзора").Column("INSPECTOR_ACCEPT");
            this.Property(x => x.InspectorAcceptedChangeDate, "Дата изменения статуса приемки объекта инспектором").Column("INSPECTOR_ACCEPT_CHANGE_DATE");
        }
    }
}