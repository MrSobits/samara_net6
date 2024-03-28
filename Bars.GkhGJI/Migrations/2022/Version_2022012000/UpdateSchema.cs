namespace Bars.GkhGji.Migrations._2022.Version_2022012000
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022012000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022011700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddEntityTable(
              "GJI_ACTCHECK_CONTROLLIST_ANSWER",
              new Column("QUESTION", DbType.String, 2500, ColumnProperty.NotNull),
              new Column("DESCRIPTION", DbType.String, 2500),
              new Column("NPD", DbType.String, 2500),
              new RefColumn("ACTCHECK_ID", "GJI_ACTCHECK_CONTROLLIST_ACT_ID", "GJI_ACTCHECK", "ID"),
              new Column("ANSWER", DbType.Int32, 4, ColumnProperty.NotNull, 30));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTCHECK_CONTROLLIST_ANSWER");
        }
    }
}