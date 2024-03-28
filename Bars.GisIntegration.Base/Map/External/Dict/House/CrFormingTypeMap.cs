namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.CrFormingType
    /// </summary>
    public class CrFormingTypeMap : BaseEntityMap<CrFormingType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CrFormingTypeMap() :
            base("NSI_CR_FORMING_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("CR_FORMING_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "CR_FORMING_TYPE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
