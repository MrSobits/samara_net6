namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017111500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017111500")]
    [MigrationDependsOn(typeof(Version_2017110600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Категория подателей заявления
            Database.AddEntityTable(
                "GJI_CH_DICT_EGRN_APPLICANT_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            //-----Тип документа ЕГРН
            Database.AddEntityTable(
               "GJI_CH_DICT_EGRN_DOC_TYPE",
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
               new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

        }
        public override void Down()
        {
            Database.RemoveTable("GJI_CH_DICT_EGRN_DOC_TYPE");
            Database.RemoveTable("GJI_CH_DICT_EGRN_APPLICANT_TYPE");
        }
    }
}