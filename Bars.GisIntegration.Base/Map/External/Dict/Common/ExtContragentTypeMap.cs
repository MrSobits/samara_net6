namespace Bars.GisIntegration.Base.Map.External.Dict.Common
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Gkh.Ris.Entities.External.Dict.Common.ExtContragentType
    /// </summary>
    public class ExtContragentTypeMap : BaseEntityMap<ExtContragentType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtContragentTypeMap() :
            base("NSI_CONTRAGENT_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("CONTRAGENT_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ContragentTypeName, "CONTRAGENT_TYPE");
            this.Map(x => x.ContragentTypeNameShort, "CONTRAGENT_TYPE_SHORT");
            this.Map(x => x.IsActual, "IS_ACTUAL");
        }

    }
}
