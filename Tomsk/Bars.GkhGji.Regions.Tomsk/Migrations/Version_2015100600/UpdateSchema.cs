namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015100600
{
	using System.Data;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
	using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015090700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
	    public override void Up()
	    {
			//сама таблица создается в более ранней миграции базового модуля, который мы всегда используем
			//Bars.GkhGji 2015081300

			Database.ExecuteNonQuery(@"insert into GJI_DICT_SURVEY_SUBJ (code, name, object_version, object_create_date, object_edit_date) values
									   ('10', 'соблюдение обязательных требований', 0, now(), now()),
									   ('20', 'соответствие сведений, содержащихся в уведомлении о начале осуществления отдельных видов предпринимательской деятельности, обязательным требованиям', 0, now(), now()),
									   ('30', 'выполнение предписаний органов государственного контроля (надзора)', 0, now(), now()),
									   ('40', 'проведение мероприятий по предотвращению причинения вреда жизни, здоровью граждан, вреда животным, растениям, окружающей среде', 0, now(), now()),
									   ('50', 'проведение мероприятий по предупреждению возникновения чрезвычайных ситуаций природного и техногенного характера', 0, now(), now()),
									   ('60', 'проведение мероприятий по обеспечению безопасности государства', 0, now(), now()),
									   ('70', 'проведение мероприятий по ликвидации последствий причинения такого вреда', 0, now(), now()),
									   ('80', 'соблюдение соискателем лицензии лицензионных требований', 0, now(), now());");

		    Database.RenameColumn("GJI_TOMSK_DISP_VERIFSUBJ", "TYPE_VERIF_SUBJ", "SURVEY_SUBJECT_ID");
			Database.ChangeColumn("GJI_TOMSK_DISP_VERIFSUBJ",
				new RefColumn("SURVEY_SUBJECT_ID", "GJI_TOMSK_DISP_VERIFSUBJ_SSD", "GJI_DICT_SURVEY_SUBJ", "ID"));
			
			//раньше это был Enum, сейчас таблица, чтобы сохранить старые данные проставляем ссылку на новую таблицу
		    Database.ExecuteNonQuery(@"
					update GJI_TOMSK_DISP_VERIFSUBJ d
					set survey_subject_id = dict.id
					from (select id, code from GJI_DICT_SURVEY_SUBJ) as dict
					where dict.code::int = d.survey_subject_id");
	    }

		public override void Down()
	    {
			Database.RenameColumn("GJI_TOMSK_DISP_VERIFSUBJ", "SURVEY_SUBJECT_ID", "TYPE_VERIF_SUBJ");
			Database.ChangeColumn("GJI_TOMSK_DISP_VERIFSUBJ", new Column("TYPE_VERIF_SUBJ", DbType.Int32));

			Database.ExecuteNonQuery(@"
					update GJI_TOMSK_DISP_VERIFSUBJ d
					set type_verif_subj = dict.code::int
					from (select id, code from GJI_DICT_SURVEY_SUBJ) as dict
					where dict.id = d.type_verif_subj");

			Database.ExecuteNonQuery(@"delete from GJI_DICT_SURVEY_SUBJ 
									   where code in ('10', '20', '30', '40', '50', '60', '70', '80');");
	    }
    }
}
