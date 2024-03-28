namespace Bars.Gkh.Migrations.Version_2013103000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013103000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013102902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_CIT_SUG_COMMENT", new Column("QUESTION", DbType.String, 2000));
            Database.ChangeColumn("GKH_CIT_SUG_COMMENT", new Column("ANSWER", DbType.String, 2000));
        }

        public override void Down()
        {
            // В down Нет смысла менять обратно колонки на прежнюю длину, поскольку раньше стояла длина по 255 символов
            // и уже если будут существующие данные то при уменьшении длины произойдет потеря данных
        }
    }
}
