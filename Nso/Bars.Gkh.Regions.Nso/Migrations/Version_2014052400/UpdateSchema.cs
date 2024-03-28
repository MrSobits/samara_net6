namespace Bars.Gkh.Regions.Nso.Migrations.Version_2014052400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Nso.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----НСО Органа местного самоуправления
            Database.AddTable(
                "GKH_NSO_LOCALGOV",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("FIO", DbType.String, 300),
                new Column("REG_NUM_NOTICE", DbType.String, 100),
                new Column("REG_DATE_NOTICE", DbType.DateTime),
                new Column("NUM_NPA", DbType.String, 100),
                new Column("DATE_NPA", DbType.DateTime),
                new Column("NAME_NPA", DbType.String, 300),
                new Column("INTERACTION_MU_NUM", DbType.String, 100),
                new Column("INTERACTION_MU_DATE", DbType.DateTime),
                new Column("INTERACTION_GJI_NUM", DbType.String, 100),
                new Column("INTERACTION_GJI_DATE", DbType.DateTime));


            Database.AddForeignKey("FK_GKH_NSO_LOCALGOV_L", "GKH_NSO_LOCALGOV", "ID", "GKH_LOCAL_GOVERNMENT", "ID");

            Database.ExecuteNonQuery(@"insert into GKH_NSO_LOCALGOV (id)
                                     select id from GKH_LOCAL_GOVERNMENT");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_NSO_LOCALGOV", "FK_GKH_NSO_LOCALGOV_L");
            Database.RemoveTable("GKH_NSO_LOCALGOV");
        }
    }
}