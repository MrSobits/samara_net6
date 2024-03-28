namespace Bars.Gkh.Ris.Migrations.Version_2016052000
{
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using B4.Modules.Ecm7.Framework;

    [Migration("2016052000")]
    [MigrationDependsOn(typeof(Version_2015121800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.RemoveTable("RIS_WORKLIST_ATTACHMENT");

            //this.Database.AddRefColumn("RIS_WORKLIST", new RefColumn("ATTACHMENT_ID", "RIS_WORKLIST_ATTACHMENT", "RIS_ATTACHMENT", "ID"));
        }

        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_WORKLIST", "ATTACHMENT_ID");

            //this.Database.AddEntityTable("RIS_WORKLIST_ATTACHMENT",
            //    new RefColumn("WORKLIST_ID", "RIS_ATTACH_WORKLIST", "RIS_WORKLIST", "ID"),
            //    new RefColumn("ATTACHMENT_ID", "RIS_WORKITEM_ATTACH", "RIS_ATTACHMENT", "ID"));
        }
    }
}