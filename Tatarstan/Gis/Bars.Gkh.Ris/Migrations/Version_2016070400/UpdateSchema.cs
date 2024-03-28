namespace Bars.Gkh.Ris.Migrations.Version_2016070400
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016070400")]
    [MigrationDependsOn(typeof(Version_2016070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //if (this.Database.TableExists("GI_CONTAINER"))
            //{
            //    var fks = this.Database.GetForeignKeys("GI_CONTAINER");
            //    foreach (var fk in fks)
            //    {
            //        this.Database.RemoveColumn(fk.TableName, fk.ColumnName);
            //    }

            //    this.Database.RemoveTable("GI_CONTAINER");
            //}
        }
    }
}