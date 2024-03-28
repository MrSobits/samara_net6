namespace Bars.Gkh1468.Migrations.Version_2013121900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_PSTRUCT_PSTRUCT", new Column("VALID_FROM_MONTH", DbType.Int16));
            Database.AddColumn("GKH_PSTRUCT_PSTRUCT", new Column("VALID_FROM_YEAR", DbType.Int16));

            Database.ExecuteNonQuery(
            @"UPDATE gkh_pstruct_pstruct
                SET VALID_FROM_MONTH = EXTRACT(month FROM valid_start),
                    VALID_FROM_YEAR = EXTRACT(year FROM valid_start)");

            Database.RemoveColumn("GKH_PSTRUCT_PSTRUCT", "VALID_START");
            Database.RemoveColumn("GKH_PSTRUCT_PSTRUCT", "VALID_END");
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PSTRUCT_PSTRUCT", "VALID_FROM_MONTH");
            Database.RemoveColumn("GKH_PSTRUCT_PSTRUCT", "VALID_FROM_YEAR");

            Database.AddColumn("GKH_PSTRUCT_PSTRUCT", new Column("VALID_START", DbType.DateTime));
            Database.AddColumn("GKH_PSTRUCT_PSTRUCT", new Column("VALID_END", DbType.DateTime));
        }
    }
}
