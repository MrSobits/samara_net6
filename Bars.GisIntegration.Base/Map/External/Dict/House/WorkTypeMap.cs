namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.HouseService
    /// </summary>
    public class WorkTypeMap : BaseEntityMap<WorkType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public WorkTypeMap() :
            base("NSI_WORK_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("WORK_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.WorkTypeName, "WORK_TYPE");
            this.Map(x => x.Level, "LEVEL");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.DictCodeShort, "DICT_CODE_SHORT");
            this.References(x => x.Okei, "OKEI_ID");
            this.References(x => x.Parent, "PARENT_ID");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
