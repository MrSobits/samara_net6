namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на реест квалификационного отбора"</summary>
    public class ViewQualificationMap : PersistentObjectMap<ViewQualification>
    {
        public ViewQualificationMap() : 
                base("Вьюха на реест квалификационного отбора", "VIEW_CR_OBJECT_QUALIFICATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ProgrammName, "Программа КР").Column("PROGRAM");
            this.Property(x => x.MunicipalityName, "Муниципальное образование").Column("MUNICIPALITY");
            this.Property(x => x.BuilderName, "Подрядчик").Column("BUILDER");
            this.Property(x => x.Sum, "Сумма").Column("TYPE_WORK_SUM");
            this.Property(x => x.Rating, "Рейтинг").Column("RATING");
            this.Property(x => x.Address, "Рейтинг").Column("ADDRESS");
            this.Property(x => x.QualMemberCount, "Кол-во участников квал отбора").Column("QUAL_MEMBER_COUNT");
        }
    }
}
