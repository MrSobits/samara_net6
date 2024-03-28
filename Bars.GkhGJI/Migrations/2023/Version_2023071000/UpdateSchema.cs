namespace Bars.GkhGji.Migrations._2023.Version_2023071000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023071000")]
    [MigrationDependsOn(typeof(_2023.Version_2023061500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DECISION_CON_SUBJ", new Column("PHYSICAL_PERSON_INN", DbType.String, 15));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DECISION_CON_SUBJ", "PHYSICAL_PERSON_INN");
        }
    }
}