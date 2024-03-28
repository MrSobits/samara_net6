namespace Bars.Gkh.Map.TechnicalPassport
{
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Enums.TechnicalPassport;
    using Bars.Gkh.Entities.TechnicalPassport;

    using NHibernate.Mapping.ByCode.Conformist;

    public class FormAttributeMap : GkhBaseEntityMap<FormAttribute>
    {
        public FormAttributeMap()
            : base("TP_FORM_ATTRIBUTE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Название").Column("NAME");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.DisplayText, "Отображаемый текст").Column("DISPLAY_TEXT");
            this.Property(x => x.ColumnName, "Название столбца").Column("COLUMN_NAME");
            this.Property(x => x.Required, "Обязательность").Column("REQUIRED");
            this.Reference(x => x.Form, "Форма").Column("FORM_ID");
            this.Reference(x => x.Editor, "Редактор").Column("EDITOR_ID");
            this.Property(x => x.Contstraints, "Ограничения").Column("CONTSTRAINTS");
        }

        public class FormAttributeNHibernateMapping : ClassMapping<FormAttribute>
        {
            public FormAttributeNHibernateMapping()
            {
                this.Property(x => x.Contstraints, m =>
                {
                    m.Type<ImprovedBinaryJsonType<Dictionary<ContstraintType, int>>>();
                });
            }
        }
    }
}