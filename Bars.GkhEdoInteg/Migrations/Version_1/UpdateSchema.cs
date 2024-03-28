// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateSchema.cs" company="">
//   
// </copyright>
// <summary>
//   The update schema.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhEdoInteg.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Источник поступления
            Database.AddEntityTable(
                "INTGEDO_REVENSOURCE",
                new RefColumn("REVENUESOURCE_ID", ColumnProperty.NotNull, "INTGEDO_REV_SRC_R", "GJI_DICT_REVENUESOURCE", "ID"),
                new Column("CODE_EDO", DbType.Int64, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));


            //Форма поступления
            Database.AddEntityTable(
                "INTGEDO_REVENUEFORM",
                new RefColumn("REVENUEFORM_ID", ColumnProperty.NotNull, "INTGEDO_REV_FORM_R", "GJI_DICT_REVENUEFORM", "ID"),
                new Column("CODE_EDO", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));


            //Вид обращения
            Database.AddEntityTable(
                "INTGEDO_KINDSTATEM",
                new RefColumn("KINDSTATEM_ID", ColumnProperty.NotNull, "INTGEDO_KSTAT_KS", "GJI_DICT_KINDSTATEMENT", "ID"),
                new Column("CODE_EDO", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36)
               );

            //Инспектор
            Database.AddEntityTable(
                "INTGEDO_INSPECTOR",
                new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "INTGEDO_INS_INSP", "GKH_DICT_INSPECTOR", "ID"),
                new Column("CODE_EDO", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36)
               );

            //Инспектор
            Database.AddEntityTable(
                "INTGEDO_APPCITS",
                new RefColumn("APPEAL_CITS_ID", ColumnProperty.NotNull, "INTGEDO_APPCITS_AC", "GJI_APPEAL_CITIZENS", "ID"),
                new Column("CODE_EDO", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("IS_EDO", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DATE_ACTUAL", DbType.DateTime),
                new Column("EXTERNAL_ID", DbType.String, 36)
               );
        }

        public override void Down()
        {
            Database.RemoveTable("INTGEDO_REVENSOURCE");
            Database.RemoveTable("INTGEDO_REVENUEFORM");
            Database.RemoveTable("INTGEDO_KINDSTATEM");
            Database.RemoveTable("INTGEDO_INSPECTOR");

            Database.RemoveTable("INTGEDO_APPCITS");
        }
    }
}