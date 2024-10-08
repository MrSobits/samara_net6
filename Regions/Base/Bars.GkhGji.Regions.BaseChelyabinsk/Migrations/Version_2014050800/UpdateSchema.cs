﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014050800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----НСО Распоряжение
            this.Database.AddTable(
                "GJI_NSO_DISPOSAL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TIME_VISIT_SART", DbType.DateTime, 25),
                new Column("TIME_VISIT_END", DbType.DateTime, 25));
            this.Database.AddForeignKey("FK_GJI_NSO_DISP_DOC", "GJI_NSO_DISPOSAL", "ID", "GJI_DISPOSAL", "ID");

            this.Database.ExecuteNonQuery(@"insert into GJI_NSO_DISPOSAL (id)
                                     select id from GJI_DISPOSAL");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GJI_NSO_DISPOSAL", "FK_GJI_NSO_DISP_DOC");
            this.Database.RemoveTable("GJI_NSO_DISPOSAL");
        }
    }
}