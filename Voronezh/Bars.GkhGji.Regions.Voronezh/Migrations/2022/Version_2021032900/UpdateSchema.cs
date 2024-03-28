namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021032900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021032900")]
    [MigrationDependsOn(typeof(Version_2021031600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

           
            Database.AddColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", new Column("TYPE_REQUEST", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddRefColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", new RefColumn("ROOM_ID", ColumnProperty.None, "GJI_CH_SMEV_EM_HOUSE_ROOM_ID", "GKH_ROOM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", "ROOM_ID");
            Database.RemoveColumn("GJI_CH_SMEV_EMERGENCY_HOUSE", "TYPE_REQUEST");
        }


    }
}
