namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060500")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Поскольку раньше в Смоленске был модуль Нижнег оновгорода, а после произошел перенос функционала ПЕреношу миграции Новгорода в этот модуль
            UpFromOtherModule();
            
            // По новому функционалу 
            Database.AddTable("GJI_PRESCR_CANCEL_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("CANCEL_RESULT", DbType.String, 2000),
                new Column("DESCRIPTION_SET", DbType.String, 2000),
                new Column("PETITION_DATE", DbType.Date),
                new Column("PETITION_NUM", DbType.String, 100),
                new Column("TYPE_CANCEL", DbType.Int16, 10));

            Database.AddForeignKey("FK_GJI_PRESCR_CANCEL_SMOL_ID", "GJI_PRESCR_CANCEL_SMOL", "ID", "GJI_PRESCRIPTION_CANCEL", "ID");
        }

        public override void Down()
        {
            // По новому функционал
            Database.RemoveConstraint("GJI_PRESCR_CANCEL_SMOL", "FK_GJI_PRESCR_CANCEL_SMOL_ID");

            Database.RemoveTable("GJI_PRESCR_CANCEL_SMOL");

            // Поскольку раньше модуль ГЖИ смоленск был на модуле новгорода , а после решили перенести функционал в Смоленск, то переношу миграции в этот модуль
            DownFromOtherModule();
        }

        private void UpFromOtherModule()
        {
             Database.AddEntityTable("GJI_CHECK_TIME_CH",
                new Column("NEW_VALUE", DbType.DateTime, ColumnProperty.Null),
                new Column("OLD_VALUE", DbType.DateTime, ColumnProperty.Null),

                new RefColumn("USER_ID", ColumnProperty.Null, "GJI_CHTIME_USER", "B4_USER", "ID"),
                new RefColumn("APPEAL_ID", "GJI_CHTIME_APPEAL", "GJI_APPEAL_CITIZENS", "ID"));

            Database.AddEntityTable("GJI_NNOV_INSP_VIOL_WORD",
                new Column("WORDING", DbType.String, 2000),
                new RefColumn("INSPECTION_VIOL_ID", ColumnProperty.NotNull, "GJI_NNOV_INSP_VIOL_WORD", "GJI_INSPECTION_VIOLATION", "ID"));

            Database.AddEntityTable(
                "GJI_NNOV_DOCGJI_PERSINFO",
                new Column("ADDRESS", DbType.String),
                new Column("JOB", DbType.String),
                new Column("POSITION", DbType.String),
                new Column("BIRTHDAY_AND_PLACE", DbType.String),
                new Column("IDENTITY_DOCUMENT", DbType.String),
                new Column("SALARY", DbType.String),
                new Column("MARITAL_STATUS", DbType.String),
                new RefColumn(
                    "DOCUMENT_ID", ColumnProperty.NotNull, "GJI_NNOV_DOCGJI_PERSINFO_DOC", "GJI_DOCUMENT", "ID"));


            //-----Мероприятия по контролю распоряжения
            Database.AddEntityTable(
                "GJI_NNOV_DISP_CON_MEASURE",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTROL_MEASURES_NAME", DbType.String, 2000));

            Database.AddForeignKey("FK_CONTROL_MEASURES_NAME", "GJI_NNOV_DISP_CON_MEASURE", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
        }

        private void DownFromOtherModule()
        {
            Database.RemoveConstraint("GJI_NNOV_DISP_CON_MEASURE", "FK_CONTROL_MEASURES_NAME");
            Database.RemoveTable("GJI_NNOV_DISP_CON_MEASURE");

            Database.RemoveTable("GJI_NNOV_DOCGJI_PERSINFO");

            Database.RemoveTable("GJI_NNOV_INSP_VIOL_WORD");

            Database.RemoveTable("GJI_CHECK_TIME_CH");
        }
    }
}