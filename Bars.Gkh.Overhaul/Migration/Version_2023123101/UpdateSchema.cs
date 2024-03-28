namespace Bars.Gkh.Overhaul.Migration.Version_2023123101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023123101")]
    [MigrationDependsOn(typeof(Version_2023123100.UpdateSchema))]
    // Является Version_2019061700 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_STRUCT_EL_FEATURE_VIOL",
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "OVRHL_STRUCT_EL_VIOL_STRUCT_EL_ID", "OVRHL_STRUCT_EL", "ID"),
                new RefColumn("FEATURE_VIOL_ID", ColumnProperty.NotNull, "OVRHL_STRUCT_EL_VIOL_FEATURE_VIOL_ID", "GJI_DICT_FEATUREVIOL", "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_STRUCT_EL_FEATURE_VIOL");
        }
    }
}