﻿namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2020092300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020092300")]
    [MigrationDependsOn(typeof(Version_2020092200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {

            Database.AddTable("GJI_INSPECTION_LIC_REISS",
                   new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                   new Column("FORM_CHECK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                   new Column("LIC_REISSUANCE_ID", DbType.Int64, 22, ColumnProperty.NotNull));

            Database.AddForeignKey("FK_GJI_INSPECT_LIC_REISS", "GJI_INSPECTION_LIC_REISS", "ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_LIC_REISS_LR_ID", "GJI_INSPECTION_LIC_REISS", "LIC_REISSUANCE_ID", "GJI_CH_LICENSE_REISSUANCE", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_INSPECTION_LIC_REISS");

        }
    }
}


