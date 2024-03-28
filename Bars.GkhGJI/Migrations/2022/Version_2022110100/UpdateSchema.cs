namespace Bars.GkhGji.Migrations._2022.Version_2022110100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022110100")]
    [MigrationDependsOn(typeof(Version_2022103100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {

            this.Database.AddRefColumn("GJI_APPCIT_DECISION", new RefColumn("SIGNER_ID", ColumnProperty.None, "GJI_APPCIT_DECISION_SIGNER", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddColumn("GJI_APPCIT_DECISION", new Column("REPRESENTATIVE", DbType.String, 1500));
            Database.AddColumn("GJI_APPCIT_DECISION", new Column("TYPE_PRESENCE", DbType.Int32, 4, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_RESOLUTION_DECISION", new Column("REPRESENTATIVE", DbType.String, 1500));
            Database.AddColumn("GJI_RESOLUTION_DECISION", new Column("TYPE_PRESENCE", DbType.Int32, 4, ColumnProperty.NotNull, 0));

            this.Database.AddEntityTable(
           "GJI_APPCIT_DECISION_LT",
           new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_APPCIT_DECISION_LT_AD", "GJI_APPCIT_DECISION", "ID"),
           new Column("DECIDED", DbType.Binary),
           new Column("ESTABLISHED", DbType.Binary));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_DECISION_LT");
            this.Database.RemoveColumn("GJI_RESOLUTION_DECISION", "REPRESENTATIVE");
            this.Database.RemoveColumn("GJI_RESOLUTION_DECISION", "TYPE_PRESENCE");
            this.Database.RemoveColumn("GJI_APPCIT_DECISION", "REPRESENTATIVE");
            this.Database.RemoveColumn("GJI_APPCIT_DECISION", "TYPE_PRESENCE");
            this.Database.RemoveColumn("GJI_APPCIT_DECISION", "SIGNER_ID");

        }
    }
}