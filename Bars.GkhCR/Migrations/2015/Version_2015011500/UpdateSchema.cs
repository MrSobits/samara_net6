namespace Bars.GkhCr.Migrations.Version_2015011500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015011500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015011400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_COMPETITION_PROTOCOL", new FileColumn("FILE_ID", "CR_COMPETITION_PROT_FILE"));

            Database.RemoveColumn("CR_COMPETITION_PROTOCOL", "EXEC_TIME");
            Database.AddColumn("CR_COMPETITION_PROTOCOL", new Column("EXEC_TIME", DbType.DateTime));

        }

        public override void Down()
        {
            Database.RemoveColumn("CR_COMPETITION_PROTOCOL", "FILE_ID");
        }
    }
}