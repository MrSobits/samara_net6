namespace Sobits.GisGkh.Migrations._2019.Version_2019061300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;

    [Migration("2019061300")]
    [MigrationDependsOn(typeof(Version_2019061000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                BooleanFieldMap.TableName,
                new RefColumn(nameof(BooleanField.NsiItem).ToLower(), $"{ BooleanFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(BooleanField.Name).ToLower(), DbType.String),
                new Column(nameof(BooleanField.Value).ToLower(), DbType.Boolean));

            Database.AddEntityTable(
                DateFieldMap.TableName,
                new RefColumn(nameof(DateField.NsiItem).ToLower(), $"{ DateFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(DateField.Name).ToLower(), DbType.String),
                new Column(nameof(DateField.Value).ToLower(), DbType.DateTime));

            Database.AddEntityTable(
                EnumFieldMap.TableName,
                new RefColumn(nameof(EnumField.NsiItem).ToLower(), $"{ EnumFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(EnumField.Name).ToLower(), DbType.String),
                new Column(nameof(EnumField.Position).ToLower(), DbType.String, 2000));

            Database.AddEntityTable(
                FiasAddressRefFieldMap.TableName,
                new RefColumn(nameof(FiasAddressRefField.NsiItem).ToLower(), $"{ FiasAddressRefFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(FiasAddressRefField.Name).ToLower(), DbType.String),
                new Column(nameof(FiasAddressRefField.FiasGUID).ToLower(), DbType.String));

            Database.AddEntityTable(
                FloatFieldMap.TableName,
                new RefColumn(nameof(FloatField.NsiItem).ToLower(), $"{ FloatFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(FloatField.Name).ToLower(), DbType.String),
                new Column(nameof(FloatField.Value).ToLower(), DbType.Double));

            Database.AddEntityTable(
                IntegerFieldMap.TableName,
                new RefColumn(nameof(IntegerField.NsiItem).ToLower(), $"{ IntegerFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(IntegerField.Name).ToLower(), DbType.String),
                new Column(nameof(IntegerField.Value).ToLower(), DbType.String));

            Database.AddEntityTable(
                NsiFieldMap.TableName,
                new RefColumn(nameof(NsiField.NsiItem).ToLower(), $"{ NsiFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(NsiField.Name).ToLower(), DbType.String),
                new Column(nameof(NsiField.NsiRegNumber).ToLower(), DbType.String),
                new RefColumn(nameof(NsiField.NsiList).ToLower(), $"{ NsiFieldMap.TableName }_{ NsiListMap.TableName }_ID", $"{ NsiListMap.TableName }", nameof(NsiList.Id).ToLower()));

            Database.AddEntityTable(
                NsiRefFieldMap.TableName,
                new RefColumn(nameof(NsiRefField.NsiItem).ToLower(), $"{ NsiRefFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(NsiRefField.Name).ToLower(), DbType.String),
                new Column(nameof(NsiRefField.RefGUID).ToLower(), DbType.String),
                new RefColumn(nameof(NsiRefField.NsiRefItem).ToLower(), $"{ NsiRefFieldMap.TableName }_{ NsiItemMap.TableName }_REF_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()));

            Database.AddEntityTable(
                OkeiRefFieldMap.TableName,
                new RefColumn(nameof(OkeiRefField.NsiItem).ToLower(), $"{ OkeiRefFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(OkeiRefField.Name).ToLower(), DbType.String),
                new Column(nameof(OkeiRefField.Code).ToLower(), DbType.String));

            Database.AddEntityTable(
                StringFieldMap.TableName,
                new RefColumn(nameof(StringField.NsiItem).ToLower(), $"{ StringFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                new Column(nameof(StringField.Name).ToLower(), DbType.String),
                new Column(nameof(StringField.Value).ToLower(), DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveTable(StringFieldMap.TableName);
            Database.RemoveTable(OkeiRefFieldMap.TableName);
            Database.RemoveTable(NsiRefFieldMap.TableName);
            Database.RemoveTable(NsiFieldMap.TableName);
            Database.RemoveTable(IntegerFieldMap.TableName);
            Database.RemoveTable(FloatFieldMap.TableName);
            Database.RemoveTable(FiasAddressRefFieldMap.TableName);
            Database.RemoveTable(EnumFieldMap.TableName);
            Database.RemoveTable(DateFieldMap.TableName);
            Database.RemoveTable(BooleanFieldMap.TableName);
        }
    }
}