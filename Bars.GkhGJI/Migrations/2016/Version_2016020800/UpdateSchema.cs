namespace Bars.GkhGji.Migrations.Version_2016020800
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

	/// <summary>
	/// Миграция 2016020800
	/// </summary>
	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2016020800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2016020300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		/// <summary>
		/// Накатить
		/// </summary>
		public override void Up()
		{
			//Направление деятельности
			if (!this.Database.TableExists("GJI_ACTIVITY_DIRECTION"))
			{
				this.Database.AddEntityTable("GJI_ACTIVITY_DIRECTION",
					new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
					new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));
			}

			//Справочник "Коды документов"
			if (!this.Database.TableExists("GJI_DOCUMENT_CODE"))
			{
				this.Database.AddEntityTable("GJI_DOCUMENT_CODE",
					new Column("TYPE", DbType.Int32),
					new Column("CODE", DbType.Int32),
					new Column("EXTERNAL_ID", DbType.String, 36));
				this.Database.AddIndex("IND_GJI_DOCUMENT_CODE_TYPE", false, "GJI_DOCUMENT_CODE", "TYPE");
				this.Database.AddIndex("IND_GJI_DOCUMENT_CODE_CODE", false, "GJI_DOCUMENT_CODE", "CODE");
			}

			//вид документа-основания
			if (!this.Database.TableExists("GJI_KIND_BASE_DOC"))
			{
				this.Database.AddEntityTable("GJI_KIND_BASE_DOC",
					new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
					new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));
			}

			//причина уведомления
			if (!this.Database.TableExists("GJI_NOTIFICATION_CAUSE"))
			{
				this.Database.AddEntityTable("GJI_NOTIFICATION_CAUSE",
					new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
					new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

				this.Database.ExecuteNonQuery(@"insert into gji_notification_cause (code, name, object_version, object_create_date, object_edit_date) values
										(10, 'Истечение срока действия договора', 0, now(), now()),
										(20, 'Досрочное расторжение договора', 0, now(), now()),
										(30, 'Смена способа управления', 0, now(), now()),
										(40, 'Способ управления выбирается впервые', 0, now(), now());");
			}

			//способ управления МКД
			if (!this.Database.TableExists("GJI_MKD_MANAGEMENT_METHOD"))
			{
				this.Database.AddEntityTable("GJI_MKD_MANAGEMENT_METHOD",
					new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
					new Column("CODE", DbType.String, 100, ColumnProperty.NotNull));

				this.Database.ExecuteNonQuery(@"insert into gji_mkd_management_method (code, name, object_version, object_create_date, object_edit_date) values
										(10, 'Непосредственное управление', 0, now(), now()),
										(20, 'Управление товариществом собственников жилья либо жилищным кооперативом или иным специализированным потребительским кооперативом', 0, now(), now()),
										(30, 'Управление управляющей организацией', 0, now(), now());");
			}

			if (!this.Database.TableExists("GJI_DICT_SURVEY_SUBJ_REQ"))
			{
				this.Database.AddEntityTable("GJI_DICT_SURVEY_SUBJ_REQ",
					new Column("CODE", DbType.String, 300),
					new Column("NAME", DbType.String, 500));
			}

			if (!this.Database.TableExists("GJI_DICT_RES_VIOL_CLAIM"))
			{
				this.Database.AddEntityTable("GJI_DICT_RES_VIOL_CLAIM",
					new Column("CODE", DbType.String, 300),
					new Column("NAME", DbType.String, 500));
			}

			if (!this.Database.TableExists("GJI_DICT_TYPE_FACT_VIOL"))
			{
				this.Database.AddEntityTable("GJI_DICT_TYPE_FACT_VIOL",
					new Column("CODE", DbType.String, 300),
					new Column("NAME", DbType.String, 500));
			}
		}

		/// <summary>
		/// Откатить
		/// </summary>
		public override void Down()
		{
			this.Database.RemoveTable("GJI_ACTIVITY_DIRECTION");
			this.Database.RemoveTable("GJI_DOCUMENT_CODE");
			this.Database.RemoveTable("GJI_KIND_BASE_DOC");
			this.Database.RemoveTable("GJI_NOTIFICATION_CAUSE");
			this.Database.RemoveTable("GJI_MKD_MANAGEMENT_METHOD");
			this.Database.RemoveTable("GJI_DICT_SURVEY_SUBJ_REQ");
			this.Database.RemoveTable("GJI_DICT_RES_VIOL_CLAIM");
			this.Database.RemoveTable("GJI_DICT_TYPE_FACT_VIOL");
		}
	}
}