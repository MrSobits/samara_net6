namespace Bars.Gkh1468.Migrations.Version_2013102900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013092600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_PSTRUCT_PSTRUCT",
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_PSTRUCT_META_ATTR",
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddColumn("GKH_PSTRUCT_PART",
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PSTRUCT_PSTRUCT", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_PSTRUCT_META_ATTR", "EXTERNAL_ID");
            Database.RemoveColumn("GKH_PSTRUCT_PART", "EXTERNAL_ID");
        }
    }
}