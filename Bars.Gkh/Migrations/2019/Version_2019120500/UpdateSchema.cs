namespace Bars.Gkh.Migrations._2019.Version_2019120500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019120500")]
    
    [MigrationDependsOn(typeof(Version_2019112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_MANORG_REQ_SMEV",
                new Column("RPGU_REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("SMEV_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("SMEV_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("SMEV_ID", DbType.Int64, ColumnProperty.None),
                new RefColumn("LIC_REQUEST_ID", "FK_SMEV_LIC_REQUEST", "GKH_MANORG_LIC_REQUEST", "ID"),
                new RefColumn("INSPECTOR_ID", "FK_SMEV_LIC_REQUEST_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));



        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_MANORG_REQ_SMEV");

        }
    }
}