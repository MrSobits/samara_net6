namespace Bars.GkhGji.Migrations._2014.Version_2014101600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2014.Version_2014101500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Добавление столбцов в таблицу GJI_PRESCRIPTION_CANCEL
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("TYPE_PRESCRIPTION", DbType.Int32, 30));

            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("DECIS_MAKE_AUTH_ID", DbType.Int64, 22));
            Database.AddForeignKey("FK_GJI_PRESCR_DECIS", "GJI_PRESCRIPTION_CANCEL", "DECIS_MAKE_AUTH_ID", "GJI_DICT_DECISMAKEAUTH", "ID");

            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("DATE_DECISION_COURT", DbType.DateTime));
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("PETITION_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("DATE_PETITION", DbType.DateTime));
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("DESCRIPTION_SET", DbType.String, 500));
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("TYPE_PROLONG", DbType.Int32, 30));
            Database.AddColumn("GJI_PRESCRIPTION_CANCEL", new Column("DATE_PROLONG", DbType.DateTime));

            // Добавление новой таблицы GJI_PRES_CANCELVIOL_REF
            Database.AddEntityTable(
                "GJI_PRES_CANCELVIOL_REF",
                new Column("PRES_CANCEL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("INSPECT_VIOL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("NEW_DATE_REMOV", DbType.DateTime));
            Database.AddForeignKey("FK_GJI_PRESCR_CANCEL", "GJI_PRES_CANCELVIOL_REF", "PRES_CANCEL_ID", "GJI_PRESCRIPTION_CANCEL", "ID");
            Database.AddForeignKey("FK_GJI_PRESCR_VIOL", "GJI_PRES_CANCELVIOL_REF", "INSPECT_VIOL_ID", "GJI_PRESCRIPTION_VIOLAT", "ID");

            // Добавление столбцов в таблицу GJI_INSPECTION_VIOLATION
            Database.AddColumn("GJI_INSPECTION_VIOLATION", new Column("DATE_CANCEL", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "TYPE_PRESCRIPTION");

            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "DECIS_MAKE_AUTH_ID");
            Database.RemoveConstraint("GJI_PRESCRIPTION_CANCEL", "FK_GJI_PRESCR_DECIS");

            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "DATE_DECISION_COURT");
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "PETITION_NUMBER");
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "DATE_PETITION");
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "DESCRIPTION_SET");
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "TYPE_PROLONG");
            Database.RemoveColumn("GJI_PRESCRIPTION_CANCEL", "DATE_PROLONG");

            Database.RemoveTable("GJI_PRES_CANCELVIOL_REF");

            Database.RemoveColumn("GJI_INSPECTION_VIOLATION", "DATE_CANCEL");
        }
    }
}
