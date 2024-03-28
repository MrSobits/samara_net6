using Bars.Gkh;

namespace Bars.GkhGji.Migrations.Version_2013032202
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013032202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013032200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            
            Database.AddColumn("GJI_DISPOSAL", new Column("KIND_CHECK_ID", DbType.Int64, 22));

            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "1" }, "TYPE_CHECK=10");
            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "2" }, "TYPE_CHECK=20");
            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "3" }, "TYPE_CHECK=30");
            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "4" }, "TYPE_CHECK=40");
            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "5" }, "TYPE_CHECK=50");
            Database.Update("GJI_DISPOSAL", new[] { "KIND_CHECK_ID" }, new[] { "8" }, "TYPE_CHECK=60");

            Database.AddForeignKey("FK_GJI_DISP_KCH", "GJI_DISPOSAL", "KIND_CHECK_ID", "GJI_DICT_KIND_CHECK", "ID");
            Database.AddIndex("IND_GJI_DISP_KCH", false, "GJI_DISPOSAL", "KIND_CHECK_ID");

            //добавляем таблицу правил проставления видов проверки
            Database.AddEntityTable("GJI_KIND_CHECK_RULE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.Int32, 4, ColumnProperty.NotNull),
                new Column("RULE_CODE", DbType.String, 100, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            
            Database.RemoveIndex("IND_GJI_DISP_KCH", "GJI_DISPOSAL");
            Database.RemoveConstraint("GJI_DISPOSAL", "FK_GJI_DISP_KCH");

            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "10" }, "KIND_CHECK_ID=1");
            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "20" }, "KIND_CHECK_ID=2");
            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "30" }, "KIND_CHECK_ID=3");
            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "40" }, "KIND_CHECK_ID=4");
            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "50" }, "KIND_CHECK_ID=5");
            Database.Update("GJI_DISPOSAL", new[] { "TYPE_CHECK" }, new[] { "60" }, "KIND_CHECK_ID=8");

            Database.RemoveColumn("GJI_DISPOSAL", "KIND_CHECK_ID");

            Database.RemoveTable("GJI_KIND_CHECK_RULE");
        }
    }
}
