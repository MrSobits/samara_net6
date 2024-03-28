namespace Bars.GkhGji.Migrations.Version_2013082200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013082200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013082100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Напоминание
            Database.AddEntityTable(
                "GJI_REMINDER",
                new Column("INSPECTION_ID", DbType.Int64, 22),
                new Column("DOCUMENT_ID", DbType.Int64, 22),
                new Column("APPEAL_CITS_ID", DbType.Int64, 22),
                new Column("CONTRAGENT_ID", DbType.Int64, 22),
                new Column("TYPE_REMINDER", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("CATEGORY_REMINDER", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("CHECK_DATE", DbType.DateTime),
                new Column("NUM", DbType.String, 500),
                new Column("ACTUALITY", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddIndex("IND_GJI_REMINDER_INS", false, "GJI_REMINDER", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_REMINDER_DOC", false, "GJI_REMINDER", "DOCUMENT_ID");
            Database.AddIndex("IND_GJI_REMINDER_APC", false, "GJI_REMINDER", "APPEAL_CITS_ID");
            Database.AddIndex("IND_GJI_REMINDER_CTR", false, "GJI_REMINDER", "CONTRAGENT_ID");

            Database.AddForeignKey("FK_GJI_REMINDER_INS", "GJI_REMINDER", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_REMINDER_DOC", "GJI_REMINDER", "DOCUMENT_ID", "GJI_DOCUMENT", "ID");
            Database.AddForeignKey("FK_GJI_REMINDER_APC", "GJI_REMINDER", "APPEAL_CITS_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_REMINDER_CTR", "GJI_REMINDER", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_REMINDER", "FK_GJI_REMINDER_INS");
            Database.RemoveConstraint("GJI_REMINDER", "FK_GJI_REMINDER_DOC");
            Database.RemoveConstraint("GJI_REMINDER", "FK_GJI_REMINDER_APC");
            Database.RemoveConstraint("GJI_REMINDER", "FK_GJI_REMINDER_CTR");

            Database.RemoveTable("GJI_REMINDER");
        }
    }
}