namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.InnerWallType
    /// </summary>
    public class InnerWallTypeMap : BaseEntityMap<InnerWallType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public InnerWallTypeMap() :
            base("NSI_INNER_WALL_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("INNER_WALL_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "INNER_WALL_TYPE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
