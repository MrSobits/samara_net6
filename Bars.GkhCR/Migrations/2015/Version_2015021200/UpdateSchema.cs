namespace Bars.GkhCr.Migrations.Version_2015021200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015020400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // если таблица уже создана значит миграция 2015012200 уже накачена
            // соответственно просто переименовываем индекс
            if (Database.TableExists("CR_BUILDER_VIOLATOR_VIOL"))
            {
                Database.RemoveIndex("IND_CR_BUILDER_VIOLATOR_VIOL_BV", "CR_BUILDER_VIOLATOR_VIOL");
                Database.AddIndex("IND_CR_BUILD_VIOLATOR_VIOL_BV", false, "CR_BUILDER_VIOLATOR_VIOL", "BUILDER_VIOLATOR_ID");
                return;
            }

            // если же проверка выше не удалась
            // значит накатилась уже пустая миграция 2015012200
            // и создавать нужно всё
            Database.AddEntityTable("CR_BUILDER_VIOLATOR",
                    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "CR_BUILDER_VIOLATOR_BC", "CR_OBJ_BUILD_CONTRACT", "ID"),
                    new Column("TYPE_CREATION", DbType.Int16, ColumnProperty.NotNull, 10),
                    new Column("START_DATE", DbType.DateTime),
                    new Column("COUNT_DAYS_DELAY", DbType.Int64));

            Database.AddEntityTable("CR_BUILDER_VIOLATOR_VIOL",
                    new Column("NOTE", DbType.String, 1000),
                    new RefColumn("VIOL_ID", ColumnProperty.NotNull, "CR_BUILDER_VIOLATOR_VIOL_V", "CLW_VIOL_CLAIM_WORK", "ID"),
                    new RefColumn("BUILDER_VIOLATOR_ID", ColumnProperty.NotNull, "CR_BUILD_VIOLATOR_VIOL_BV", "CR_BUILDER_VIOLATOR", "ID"));

            Database.AddTable("CLW_BUILD_CLAIM_WORK",
                    new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                    new Column("CONTRACT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                    new Column("TYPE_CREATION", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_CLW_BUILD_CLAIM_C", false, "CLW_BUILD_CLAIM_WORK", "ID");
            Database.AddForeignKey("FK_CLW_BUILD_CLAIM_C", "CLW_BUILD_CLAIM_WORK", "ID", "CLW_CLAIM_WORK", "ID");
            Database.AddIndex("IND_CLW_BUILD_CLAIM_BC", false, "CLW_BUILD_CLAIM_WORK", "CONTRACT_ID");
            Database.AddForeignKey("FK_CLW_BUILD_CLAIM_BC", "CLW_BUILD_CLAIM_WORK", "CONTRACT_ID", "CR_OBJ_BUILD_CONTRACT", "ID");

            Database.AddEntityTable("CLW_BUILD_CLAIM_WORK_VIOL",
                    new Column("NOTE", DbType.String, 1000),
                    new RefColumn("VIOL_ID", ColumnProperty.NotNull, "CLW_BUILD_CLAIM_VIOL_V", "CLW_VIOL_CLAIM_WORK", "ID"),
                    new RefColumn("CLAIM_WORK_ID", ColumnProperty.NotNull, "CLW_BUILD_CLAIM_VIOL_C", "CLW_BUILD_CLAIM_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_BUILDER_VIOLATOR_VIOL");
            Database.RemoveTable("CR_BUILDER_VIOLATOR");

            Database.RemoveTable("CLW_BUILD_CLAIM_WORK_VIOL");

            Database.RemoveColumn("CLW_BUILD_CLAIM_WORK", "CONTRACT_ID");
            Database.RemoveTable("CLW_BUILD_CLAIM_WORK");
        }
    }
}