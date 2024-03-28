namespace Bars.Gkh.Migrations._2023.Version_2023050131
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050131")]

    [MigrationDependsOn(typeof(Version_2023050130.UpdateSchema))]

    /// Является Version_2020042100 из ядра
    public class UpdateSchema : Migration
    {
        public const string DocumentTypeTableName = "GKH_DICT_IDENTITY_DOC_TYPE";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.DocumentTypeTableName,
                new Column("CODE", DbType.String, 10, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String));

            var query = $@"INSERT INTO {UpdateSchema.DocumentTypeTableName} (object_create_date,object_edit_date,object_version,code,name) VALUES
                                (now(),now(),1,'01','Паспорт гражданина Российской Федерации'),
                                (now(),now(),1,'02','Свидетельство органов ЗАГС, органа исполнительной власти или органа местного самоуправления о рождении гражданина'),
                                (now(),now(),1,'03','Паспорт моряка (удостоверение личности моряка)'),
                                (now(),now(),1,'04','Удостоверение личности военнослужащего'),
                                (now(),now(),1,'05','Военный билет военнослужащего'),
                                (now(),now(),1,'06','Временное удостоверение личности гражданина Российской Федерации'),
                                (now(),now(),1,'07','Справка об освобождении из мест лишения свободы'),
                                (now(),now(),1,'08','Паспорт иностранного гражданина либо иной документ, установленный федеральным законом или признаваемый в соответствии с международным договором Российской Федерации в качестве документа, удостоверяющего личность иностранного гражданина'),
                                (now(),now(),1,'09','Вид на жительство'),
                                (now(),now(),1,'10','Разрешение на временное проживание (для лиц без гражданства)'),
                                (now(),now(),1,'11','Удостоверение беженца'),
                                (now(),now(),1,'12','Миграционная карта'),
                                (now(),now(),1,'13','Паспорт гражданина СССР'),
                                (now(),now(),1,'14','СНИЛС'),
                                (now(),now(),1,'15','Удостоверение личности гражданина Российской Федерации'),
                                (now(),now(),1,'21','ИНН'),
                                (now(),now(),1,'22','Водительское удостоверение'),
                                (now(),now(),1,'24','Свидетельство о регистрации транспортного средства в органах Министерства внутренних дел Российской Федерации'),
                                (now(),now(),1,'25','Охотничий билет'),
                                (now(),now(),1,'26','Разрешение на хранение и ношение охотничьего оружия');";
            this.Database.ExecuteNonQuery(query);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.DocumentTypeTableName);
        }
    }
}