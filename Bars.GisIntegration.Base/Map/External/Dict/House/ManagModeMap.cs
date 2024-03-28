namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.ManagMode
    /// </summary>
    public class ManagModeMap : BaseEntityMap<ManagMode>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManagModeMap() :
            base("NSI_MANAG_MODE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("MANAG_MODE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "MANAG_MODE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
