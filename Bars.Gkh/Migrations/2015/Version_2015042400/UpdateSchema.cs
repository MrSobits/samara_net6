namespace Bars.Gkh.Migrations._2015.Version_2015042400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015042300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CLW_DICT_PETITION_TYPE",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("SHORT_NAME", DbType.String, 500),
                new Column("FULL_NAME", DbType.String, 3000));
            Database.AddIndex("IND_CLW_PETITION", true, "CLW_DICT_PETITION_TYPE", "CODE");

            Database.AddEntityTable("CLW_DICT_STATE_DUTY",
                new Column("COURT_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("FORMULA", DbType.String, 250),
                new Column("formula_params", DbType.Binary, ColumnProperty.Null));

            Database.AddEntityTable("CLW_STATE_DUTY_PETITION",
                new RefColumn("STATE_DUTY_ID", ColumnProperty.NotNull, "CLW_ST_DUTY_PET_DUTY", "CLW_DICT_STATE_DUTY", "ID"),
                new RefColumn("PETITION_TYPE_ID", ColumnProperty.NotNull, "CLW_ST_DUTY_PET_PET", "CLW_DICT_PETITION_TYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_STATE_DUTY_PETITION");
            Database.RemoveTable("CLW_DICT_STATE_DUTY");
            Database.RemoveTable("CLW_DICT_PETITION_TYPE");
        }
    }
}