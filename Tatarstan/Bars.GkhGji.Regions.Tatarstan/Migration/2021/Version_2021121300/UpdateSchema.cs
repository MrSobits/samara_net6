namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121300
{
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using DatabaseExtensions = Bars.Gkh.Utils.DatabaseExtensions;

    [Migration("2021121300")]
    [MigrationDependsOn(typeof(Version_2021120801.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        private List<Column> listColumn = new List<Column>
        {
            new Column("TYPE_FORM", DbType.Int32)
        };
        
        /// <inheritdoc />
        public override void Up()
        {
            DatabaseExtensions.AddJoinedSubclassTable(this.Database, "GJI_INSPECTION_ACTIONISOLATED", "GJI_INSPECTION", "GJI_INSPECTION_GJI_INSPECTION_ACTIONISOLATED_ID", listColumn.ToArray());
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_INSPECTION_ACTIONISOLATED");
        }
    }
}