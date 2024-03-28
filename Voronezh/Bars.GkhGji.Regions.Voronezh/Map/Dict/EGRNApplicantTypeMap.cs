namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для справочника Категория подателей заявления </summary>
    public class EGRNApplicantTypeMap : BaseEntityMap<EGRNApplicantType>
    {
        
        public EGRNApplicantTypeMap() : 
                base("Категория подателей заявления", "GJI_CH_DICT_EGRN_APPLICANT_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Property(x => x.Description, "Комментарий").Column("DESCRIPTION");
        }
    }
}
