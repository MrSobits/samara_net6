namespace Bars.GkhCr.Migrations._2017.Version_2017060900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017060900")]
    [MigrationDependsOn(typeof(Version_2017052500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("CR_DICT_PROGRAM", new RefColumn("NORMATIVE_DOC_ID", "NORMATIVE_DOC_ID", "GKH_DICT_NORMATIVE_DOC", "ID"));

            this.Database.AddRefColumn("CR_DICT_PROGRAM", new FileColumn("FILE_ID", "CR_DICT_PROGRAM_FILE"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "NORMATIVE_DOC_ID");
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "FILE_ID");
        }
    }
}