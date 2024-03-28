namespace Bars.GkhGji.Migrations._2021.Version_2021122800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021122800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021122401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_PROTOCOL", new Column("FAMILIARIZE_REFUSAL", DbType.Boolean, ColumnProperty.None, false));
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("ANSWER_TYPE", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.TypeAppealAnswer.NotSet));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "FAMILIARIZE_REFUSAL");
        }
    }
}