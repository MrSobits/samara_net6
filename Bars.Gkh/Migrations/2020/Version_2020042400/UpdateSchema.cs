namespace Bars.Gkh.Migrations._2020.Version_2020042400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020042400")]
    [MigrationDependsOn(typeof(Version_2020041600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_IDENTITY_DOC_TYPE",
                new Column("REGEX", DbType.String));
            this.Database.AddColumn("GKH_DICT_IDENTITY_DOC_TYPE",
                new Column("REGEX_ERROR_MESSAGE", DbType.String));

            var query = @"update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[\d]{10}$/', 'Введите 10 цифр без пробелов') where code = '01';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[I,V,X,L,C]+-[А-Я]{2}[\d]{6}$/', 'Введите значение вида R-ББ999999') where code = '02';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[А-Я]{2}[\d]{6}[\d]?$/', 'Введите значение вида ББ9999990') where code = '03';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[А-Я]{2}[\d]{7}$/', 'Введите значение вида ББ9999999') where code = '04';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[А-Я]{2}[\d]{7}$/', 'Введите значение вида ББ9999999') where code = '05';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^.?[\d]{0,7}[\d]{1}$/', 'Введите значение вида S00000009') where code = '06';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^.?[\d]{0,7}[\d]{1}$/', 'Введите значение вида S00000009') where code = '07';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^.{0,10}$/', 'Введите значение вида SSSSSSSSSS') where code = '08';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[\d]{9}$/', 'Введите 9 цифр без пробелов') where code = '09';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[\d]{6}$/', 'Введите 6 цифр без пробелов') where code = '10';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^.?[\d]{0,7}[\d]{1}$/', 'Введите значение вида S00000009') where code = '11';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[\d]{14}$/', 'Введите 14 цифр без пробелов') where code = '12';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^[I,V,X,L,C]+-[А-Я]{2}[\d]{6}$/', 'Введите значение вида R-ББ999999') where code = '13';
                            update GKH_DICT_IDENTITY_DOC_TYPE set (REGEX,REGEX_ERROR_MESSAGE) = ('/^([\d]{3}-){2}[\d]{3} [\d]{2}$/', 'Введите значение вида 999-999-999 99') where code = '14';";
            this.Database.ExecuteNonQuery(query);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_IDENTITY_DOC_TYPE", "REGEX");
            this.Database.RemoveColumn("GKH_DICT_IDENTITY_DOC_TYPE", "REGEX_ERROR_MESSAGE");
        }
    }
}