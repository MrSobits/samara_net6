namespace Bars.B4.Modules.FIAS.Migrations.Version_2016111100
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016111100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2016072100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("B4_FIAS_HOUSE", "START_DATE", DbType.DateTime);
            this.Database.AddColumn("B4_FIAS_HOUSE", "END_DATE", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("B4_FIAS_HOUSE", "START_DATE");
            this.Database.RemoveColumn("B4_FIAS_HOUSE", "END_DATE");
        }
    }
}