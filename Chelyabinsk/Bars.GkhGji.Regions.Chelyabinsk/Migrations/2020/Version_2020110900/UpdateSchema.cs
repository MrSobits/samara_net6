namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020110900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020110900")]
    [MigrationDependsOn(typeof(Version_2020102800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            //-----Тип объекта запроса ЕГРН
            Database.AddEntityTable(
                "GJI_CH_DICT_EGRN_OBJECT_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.None),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            Database.ExecuteNonQuery($@"insert into public.GJI_CH_DICT_EGRN_OBJECT_TYPE (CODE, NAME, object_version, object_create_date, object_edit_date) VALUES
('002001001000', 'Земельный участок', 0, '{date}'::date, '{date}'::date),
('002001002000', 'Здание', 0, '{date}'::date, '{date}'::date),
('002001003000', 'Помещение', 0, '{date}'::date, '{date}'::date),
('002001004000', 'Сооружение', 0, '{date}'::date, '{date}'::date),
('002001005000', 'Объект незавершенного строительства', 0, '{date}'::date, '{date}'::date),
('002001006000', 'Предприятие как имущественный комплекс', 0, '{date}'::date, '{date}'::date),
('002001008000', 'Единый недвижимый комплекс', 0, '{date}'::date, '{date}'::date),
('002001009000', 'Машино-место', 0, '{date}'::date, '{date}'::date),
('002001010000', 'Иной объект недвижимости', 0, '{date}'::date, '{date}'::date)");
            var reader = Database.ExecuteQuery("select id from public.GJI_CH_DICT_EGRN_OBJECT_TYPE where code = '002001003000'");
            reader.Read();
            long id = reader.GetInt64(0);
            reader.Close();
            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("EGRN_OBJECT_TYPE_ID", DbType.Int64, ColumnProperty.NotNull, id));
            Database.ChangeDefaultValue("GJI_CH_SMEV_EGRN", "EGRN_OBJECT_TYPE_ID", null);
            Database.AddForeignKey("GJI_CH_SMEV_EGRN_OBJTYPE_ID", "GJI_CH_SMEV_EGRN", "EGRN_OBJECT_TYPE_ID", "GJI_CH_DICT_EGRN_OBJECT_TYPE", "ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "EGRN_OBJECT_TYPE_ID");
            Database.RemoveTable("GJI_CH_DICT_EGRN_OBJECT_TYPE");
        }
    }
}