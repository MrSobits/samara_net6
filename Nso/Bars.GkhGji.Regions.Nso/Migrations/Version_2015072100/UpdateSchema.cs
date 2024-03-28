namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015072100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015071500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			//причина уведомления
			Database.AddEntityTable("GJI_NOTIFICATION_CAUSE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

			//способ управления МКД
			Database.AddEntityTable("GJI_MKD_MANAGEMENT_METHOD",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

			Database.ExecuteNonQuery(@"insert into gji_notification_cause (code, name, object_version, object_create_date, object_edit_date) values
										(10, 'Истечение срока действия договора', 0, now(), now()),
										(20, 'Досрочное расторжение договора', 0, now(), now()),
										(30, 'Смена способа управления', 0, now(), now()),
										(40, 'Способ управления выбирается впервые', 0, now(), now());");

			Database.ExecuteNonQuery(@"insert into gji_mkd_management_method (code, name, object_version, object_create_date, object_edit_date) values
										(10, 'Непосредственное управление', 0, now(), now()),
										(20, 'Управление товариществом собственников жилья либо жилищным кооперативом или иным специализированным потребительским кооперативом', 0, now(), now()),
										(30, 'Управление управляющей организацией', 0, now(), now());");
        }

        public override void Down()
        {
			Database.RemoveTable("GJI_NOTIFICATION_CAUSE");
			Database.RemoveTable("GJI_MKD_MANAGEMENT_METHOD");
        }
    }
}