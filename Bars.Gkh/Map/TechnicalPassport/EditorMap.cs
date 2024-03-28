namespace Bars.Gkh.Map.TechnicalPassport
{
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Enums.TechnicalPassport;
    using Bars.Gkh.Entities.TechnicalPassport;

    using NHibernate.Mapping.ByCode.Conformist;

    public class EditorMap : GkhBaseEntityMap<Editor>
    {
        public EditorMap()
            : base("TP_EDITOR")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Название").Column("NAME");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.EditorType, "Тип редактора").Column("EDITOR_TYPE");
            this.Property(x => x.ReferenceTableName, "Название таблицы (если тип редактора ссылочный)").Column("REFERENCE_TABLE_NAME");
            this.Property(x => x.DisplayValue, "Отображаемое значение (если тип редактора ссылочный)").Column("DISPLAY_VALUE");
            this.Property(x => x.AvaliableConstraints, "Список возможных ограничений").Column("AVALIABLE_CONSTRAINTS");
        }
    }

    public class EditorNHibernateMapping : ClassMapping<Editor>
    {
        public EditorNHibernateMapping()
        {
            this.Property(x => x.AvaliableConstraints, m =>
            {
                m.Type<ImprovedBinaryJsonType<List<ContstraintType>>>();
            });
        }
    }
}