namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021122100")]
    [MigrationDependsOn(typeof(Version_2021121400.UpdateSchema))]
    public class UpdateSchema: Migration
    { 
        /// <inheritdoc />
        public override void Up()
        {
           this.Database.AddColumn("GJI_INSPECTION_ACTIONISOLATED", new Column("INSPECTION_OBJECT", DbType.Int32));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION_ACTIONISOLATED","INSPECTION_OBJECT" );
        }
    }
}