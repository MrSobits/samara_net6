namespace Bars.B4.Modules.FIAS.Migrations.Version_2017090600
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017090600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2016111100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("B4_FIAS_HOUSE", "STRUCTURE_TYPE", DbType.Byte, ColumnProperty.NotNull, 0);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("B4_FIAS_HOUSE", "STRUCTURE_TYPE");
        }
    }
}