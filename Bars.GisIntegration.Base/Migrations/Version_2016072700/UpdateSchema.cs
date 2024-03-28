namespace Bars.GisIntegration.Base.Migrations.Version_2016072700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016072700")]
    [MigrationDependsOn(typeof(Version_2016071100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GI_INTEGR_DICT", new Column("DICT_GROUP", DbType.Int16));
            this.Database.AddColumn("GI_INTEGR_DICT", new Column("STATE", DbType.Int16, (short)DictionaryState.NotCompared));
            this.Database.AddColumn("GI_INTEGR_DICT", new Column("REC_COMPARE_DATE", DbType.DateTime));

            this.Database.ChangeColumnNotNullable("GI_INTEGR_DICT", "REGISTRY_NUMBER", false);

            this.Database.RemoveColumn("GI_INTEGR_DICT", "DATE_INTEG");

            this.Database.RemoveColumn("GI_INTEGR_REF_DICT", "CLASS_NAME");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.AddColumn("GI_INTEGR_REF_DICT", new Column("CLASS_NAME", DbType.String, 1000));

            this.Database.AddColumn("GI_INTEGR_DICT", new Column("DATE_INTEG", DbType.DateTime));

            this.Database.RemoveColumn("GI_INTEGR_DICT", "REC_COMPARE_DATE");
            this.Database.RemoveColumn("GI_INTEGR_DICT", "STATE");
            this.Database.RemoveColumn("GI_INTEGR_DICT", "DICT_GROUP");
        }
    }
}
