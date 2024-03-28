namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018033000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018033000")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            AddReg();
            AddDesc();
            AddGov();
            AddOrg();
            AddPers();
            AddOwner();
            AddRight();
            AddExtract();
            CreateViews();
        }
        public override void Down()
        {
            DropViews();
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACT", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTRIGHT", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTORG", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTPERS", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTGOV", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTOWNER", Schema = "IMPORT" });
        }
        #region Tables
        private void AddDesc()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTDESC", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("Desc_ID_Object", DbType.String, 1000),
                new Column("Desc_CadastralNumber", DbType.String, 1000),
                new Column("Desc_ObjectType", DbType.String, 1000),
                new Column("Desc_ObjectTypeText", DbType.String, 1000),
                new Column("Desc_ObjectTypeName", DbType.String, 1000),
                new Column("Desc_AssignationCode", DbType.String, 1000),
                new Column("Desc_AssignationCodeText", DbType.String, 1000),
                new Column("Desc_Area", DbType.String, 1000),
                new Column("Desc_AreaText", DbType.String, 1000),
                new Column("Desc_AreaUnit", DbType.String, 1000),
                new Column("Desc_Floor", DbType.String, 1000),
                new Column("Desc_ID_Address", DbType.String, 1000),
                new Column("Desc_AddressContent", DbType.String, 1000),
                new Column("Desc_RegionCode", DbType.String, 1000),
                new Column("Desc_RegionName", DbType.String, 1000),
                new Column("Desc_OKATO", DbType.String, 1000),
                new Column("Desc_KLADR", DbType.String, 1000),
                new Column("Desc_CityName", DbType.String, 1000),
                new Column("Desc_Urban_District", DbType.String, 1000),
                new Column("Desc_Locality", DbType.String, 1000),
                new Column("Desc_StreetName", DbType.String, 1000),
                new Column("Desc_Level1Name", DbType.String, 1000),
                new Column("Desc_Level2Name", DbType.String, 1000),
                new Column("Desc_ApartmentName", DbType.String, 1000)
                );
        }
        private void AddOrg() {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTORG", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("Org_Code_SP", DbType.String, 1000),
                new Column("Org_Content", DbType.String, 1000),
                new Column("Org_Code_OPF", DbType.String, 1000),
                new Column("Org_Name", DbType.String, 1000),
                new Column("Org_Inn", DbType.String, 1000),
                new Column("Org_Code_OGRN", DbType.String, 1000),
                new Column("Org_RegDate", DbType.String, 1000),
                new Column("Org_AgencyRegistration", DbType.String, 1000),
                new Column("Org_Code_CPP", DbType.String, 1000),
                new Column("Org_AreaUnit", DbType.String, 1000),
                //Organization->Location
                new Column("Org_Loc_ID_Address", DbType.String, 1000),
                new Column("Org_Loc_Content", DbType.String, 1000),
                new Column("Org_Loc_RegionCode", DbType.String, 1000),
                new Column("Org_Loc_RegionName", DbType.String, 1000),
                new Column("Org_Loc_Code_OKATO", DbType.String, 1000),
                new Column("Org_Loc_Code_KLADR", DbType.String, 1000),
                new Column("Org_Loc_CityName", DbType.String, 1000),
                new Column("Org_Loc_StreetName", DbType.String, 1000),
                new Column("Org_Loc_Level1Name", DbType.String, 1000),
                //Organization->FactLocation
                new Column("Org_FLoc_ID_Address", DbType.String, 1000),
                new Column("Org_FLoc_Content", DbType.String, 1000),
                new Column("Org_FLoc_RegionCode", DbType.String, 1000),
                new Column("Org_FLoc_RegionName", DbType.String, 1000),
                new Column("Org_FLoc_Code_OKATO", DbType.String, 1000),
                new Column("Org_FLoc_Code_KLADR", DbType.String, 1000),
                new Column("Org_FLoc_CityName", DbType.String, 1000),
                new Column("Org_FLoc_StreetName", DbType.String, 1000),
                new Column("Org_FLoc_Level1Name", DbType.String, 1000));
        }
        private void AddPers() {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTPERS", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("Pers_Code_SP", DbType.String, 1000),
                new Column("Pers_Content", DbType.String, 1000),
                new Column("Pers_FIO_Surname", DbType.String, 1000),
                new Column("Pers_FIO_First", DbType.String, 1000),
                new Column("Pers_FIO_Patronymic", DbType.String, 1000),
                new Column("Pers_DateBirth", DbType.String, 1000),
                new Column("Pers_Place_Birth", DbType.String, 1000),
                new Column("Pers_Citizen", DbType.String, 1000),
                new Column("Pers_Sex", DbType.String, 1000),
                new Column("Pers_DocContent", DbType.String, 1000),
                new Column("Pers_DocType_Document", DbType.String, 1000),
                new Column("Pers_DocName", DbType.String, 1000),
                new Column("Pers_DocSeries", DbType.String, 1000),
                new Column("Pers_DocNumber", DbType.String, 1000),
                new Column("Pers_DocDate", DbType.String, 1000),
                //Person->Location
                new Column("Pers_Loc_ID_Address", DbType.String, 1000),
                new Column("Pers_Loc_Content", DbType.String, 1000),
                new Column("Pers_Loc_CountryCode", DbType.String, 1000),
                new Column("Pers_Loc_CountryName", DbType.String, 1000),
                new Column("Pers_Loc_RegionCode", DbType.String, 1000),
                new Column("Pers_Loc_RegionName", DbType.String, 1000),
                new Column("Pers_Loc_Code_OKATO", DbType.String, 1000),
                new Column("Pers_Loc_Code_KLADR", DbType.String, 1000),
                new Column("Pers_Loc_DistrictName", DbType.String, 1000),
                new Column("Pers_Loc_Urban_DistrictName", DbType.String, 1000),
                new Column("Pers_Loc_LocalityName", DbType.String, 1000),
                new Column("Pers_Loc_StreetName", DbType.String, 1000),
                new Column("Pers_Loc_Level1Name", DbType.String, 1000),
                new Column("Pers_Loc_Level2Name", DbType.String, 1000),
                new Column("Pers_Loc_Level3Name", DbType.String, 1000),
                new Column("Pers_Loc_ApartmentName", DbType.String, 1000),
                new Column("Pers_Loc_Other", DbType.String, 1000),
                //Person->FactLocation
                new Column("Pers_Floc_ID_Address", DbType.String, 1000),
                new Column("Pers_Floc_Content", DbType.String, 1000),
                new Column("Pers_Floc_CountryCode", DbType.String, 1000),
                new Column("Pers_Floc_CountryName", DbType.String, 1000),
                new Column("Pers_Floc_RegionCode", DbType.String, 1000),
                new Column("Pers_Floc_RegionName", DbType.String, 1000),
                new Column("Pers_Floc_Code_OKATO", DbType.String, 1000),
                new Column("Pers_Floc_Code_KLADR", DbType.String, 1000),
                new Column("Pers_Floc_DistrictName", DbType.String, 1000),
                new Column("Pers_Floc_Urban_DistrictName", DbType.String, 1000),
                new Column("Pers_Floc_FlocalityName", DbType.String, 1000),
                new Column("Pers_Floc_StreetName", DbType.String, 1000),
                new Column("Pers_Floc_Level1Name", DbType.String, 1000),
                new Column("Pers_Floc_Level2Name", DbType.String, 1000),
                new Column("Pers_Floc_Level3Name", DbType.String, 1000),
                new Column("Pers_Floc_ApartmentName", DbType.String, 1000),
                new Column("Pers_Floc_Other", DbType.String, 1000));
        }
        private void AddGov() {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTGOV", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("Gov_Code_SP", DbType.String, 1000),
                new Column("Gov_Content", DbType.String, 1000),
                new Column("Gov_Name", DbType.String, 1000),
                new Column("Gov_OKATO_Code", DbType.String, 1000),
                new Column("Gov_Country", DbType.String, 1000),
                new Column("Gov_Address", DbType.String, 1000)
        );
        }
        private void AddReg()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTREG", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("Reg_ID_Record", DbType.String, 1000),
                new Column("Reg_RegNumber", DbType.String, 1000),
                new Column("Reg_Type", DbType.String, 1000),
                new Column("Reg_Name", DbType.String, 1000),
                new Column("Reg_RegDate", DbType.String, 1000),
                new Column("Reg_ShareText", DbType.String, 1000)
                );
        }

        private void AddOwner()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTOWNER", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new RefColumn("GOV_ID", "ROSREGEXTRACT_GOV_ID", "ROSREGEXTRACTGOV", "ID"),
                new RefColumn("PERS_ID", "ROSREGEXTRACT_PERS_ID", "ROSREGEXTRACTPERS", "ID"),
                new RefColumn("ORG_ID", "ROSREGEXTRACT_ORG_ID", "ROSREGEXTRACTORG", "ID")
                );
        }
        private void AddRight()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTRIGHT", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new RefColumn("REG_ID", "ROSREGEXTRACTRIGHT_REG_ID", "ROSREGEXTRACTREG", "ID"),
                new RefColumn("OWNER_ID", "ROSREGEXTRACTRIGHT_OWNER_ID", "ROSREGEXTRACTOWNER", "ID"),
                new Column("RightNumber", DbType.String, 1000)
                );
        }
        private void AddExtract()
        {
            var tableName = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACT", Schema = "IMPORT" };
            this.Database.AddTable(tableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new RefColumn("DESC_ID", "ROSREGEXTRACT_DESC_ID", "ROSREGEXTRACTDESC", "ID"),
                new RefColumn("RIGHT_ID", "ROSREGEXTRACT_RIGHT_ID", "ROSREGEXTRACTRIGHT", "ID")
                );
        }
        #endregion
        #region Views
        private void CreateViews()
        {
            this.Database.ExecuteNonQuery(@"
                CREATE OR REPLACE VIEW
                    import.viewrosregextractpers
                AS SELECT 
                    rrepers.Id,
                    pers_code_sp,
                    pers_content,
                    pers_fio_surname,
                    pers_fio_first,
                    pers_fio_patronymic,
                    pers_datebirth,
                    pers_place_birth,
                    pers_citizen,
                    pers_sex,
                    pers_doccontent,
                    pers_doctype_document,
                    pers_docname,
                    pers_docseries,
                    pers_docnumber,
                    pers_docdate,
                    pers_loc_id_address,
                    pers_loc_content,
                    pers_loc_countrycode,
                    pers_loc_countryname,
                    pers_loc_regioncode,
                    pers_loc_regionname,
                    pers_loc_code_okato,
                    pers_loc_code_kladr,
                    pers_loc_districtname,
                    pers_loc_urban_districtname,
                    pers_loc_localityname,
                    pers_loc_streetname,
                    pers_loc_level1name,
                    pers_loc_level2name,
                    pers_loc_level3name,
                    pers_loc_apartmentname,
                    pers_loc_other,
                    pers_floc_id_address,
                    pers_floc_content,
                    pers_floc_countrycode,
                    pers_floc_countryname,
                    pers_floc_regioncode,
                    pers_floc_regionname,
                    pers_floc_code_okato,
                    pers_floc_code_kladr,
                    pers_floc_districtname,
                    pers_floc_urban_districtname,
                    pers_floc_flocalityname,
                    pers_floc_streetname,
                    pers_floc_level1name,
                    pers_floc_level2name,
                    pers_floc_level3name,
                    pers_floc_apartmentname,
                    pers_floc_other,
                    rightnumber,
                    reg_id_record,
                    reg_regnumber,
                    reg_type,
                    reg_name,
                    reg_regdate,
                    reg_sharetext,
                    desc_id_object,
                    desc_cadastralnumber,
                    desc_objecttype,
                    desc_objecttypetext,
                    desc_objecttypename,
                    desc_assignationcode,
                    desc_assignationcodetext,
                    desc_area,
                    desc_areatext,
                    desc_areaunit,
                    desc_floor,
                    desc_id_address,
                    desc_addresscontent,
                    desc_regioncode,
                    desc_regionname,
                    desc_okato,
                    desc_kladr,
                    desc_cityname,
                    desc_urban_district,
                    desc_locality,
                    desc_streetname,
                    desc_level1name,
                    desc_level2name,
                    desc_apartmentname
                FROM 
                    import.rosregextractpers rrepers
                    join import.rosregextractowner rreowner on rrepers.id=rreowner.pers_id
                    join import.rosregextractright rreright on rreowner.id=rreright.owner_id
                    join import.rosregextractreg rrereg on rreright.reg_id=rrereg.id
                    join import.rosregextract rre on rre.right_id=rreright.id
                    join import.rosregextractdesc rredesc on rredesc.id=rre.desc_id
                ");
            this.Database.ExecuteNonQuery(@"
                CREATE OR REPLACE VIEW
                    import.viewrosregextractorg
                AS SELECT
                    rreorg.Id,
                    org_code_sp,
                    org_content,
                    org_code_opf,
                    org_name,
                    org_inn,
                    org_code_ogrn,
                    org_regdate,
                    org_agencyregistration,
                    org_code_cpp,
                    org_areaunit,
                    org_loc_id_address,
                    org_loc_content,
                    org_loc_regioncode,
                    org_loc_regionname,
                    org_loc_code_okato,
                    org_loc_code_kladr,
                    org_loc_cityname,
                    org_loc_streetname,
                    org_loc_level1name,
                    org_floc_id_address,
                    org_floc_content,
                    org_floc_regioncode,
                    org_floc_regionname,
                    org_floc_code_okato,
                    org_floc_code_kladr,
                    org_floc_cityname,
                    org_floc_streetname,
                    org_floc_level1name,
                    rightnumber,
                    reg_id_record,
                    reg_regnumber,
                    reg_type,
                    reg_name,
                    reg_regdate,
                    reg_sharetext,
                    desc_id_object,
                    desc_cadastralnumber,
                    desc_objecttype,
                    desc_objecttypetext,
                    desc_objecttypename,
                    desc_assignationcode,
                    desc_assignationcodetext,
                    desc_area,
                    desc_areatext,
                    desc_areaunit,
                    desc_floor,
                    desc_id_address,
                    desc_addresscontent,
                    desc_regioncode,
                    desc_regionname,
                    desc_okato,
                    desc_kladr,
                    desc_cityname,
                    desc_urban_district,
                    desc_locality,
                    desc_streetname,
                    desc_level1name,
                    desc_level2name,
                    desc_apartmentname
                FROM
                    import.rosregextractorg rreorg
                    join import.rosregextractowner rreowner on rreorg.id=rreowner.org_id
                    join import.rosregextractright rreright on rreowner.id=rreright.owner_id
                    join import.rosregextractreg rrereg on rreright.reg_id=rrereg.id
                    join import.rosregextract rre on rre.right_id=rreright.id
                    join import.rosregextractdesc rredesc on rredesc.id=rre.desc_id
                ");
            this.Database.ExecuteNonQuery(@"
                CREATE OR REPLACE VIEW
                    import.viewrosregextractgov
                AS SELECT 
                    rregov.Id,
                    gov_code_sp,
                    gov_content,
                    gov_name,
                    gov_okato_code,
                    gov_country,
                    gov_address,
                    rightnumber,
                    reg_id_record,
                    reg_regnumber,
                    reg_type,
                    reg_name,
                    reg_regdate,
                    reg_sharetext,
                    desc_id_object,
                    desc_cadastralnumber,
                    desc_objecttype,
                    desc_objecttypetext,
                    desc_objecttypename,
                    desc_assignationcode,
                    desc_assignationcodetext,
                    desc_area,
                    desc_areatext,
                    desc_areaunit,
                    desc_floor,
                    desc_id_address,
                    desc_addresscontent,
                    desc_regioncode,
                    desc_regionname,
                    desc_okato,
                    desc_kladr,
                    desc_cityname,
                    desc_urban_district,
                    desc_locality,
                    desc_streetname,
                    desc_level1name,
                    desc_level2name,
                    desc_apartmentname
                FROM
                    import.rosregextractgov rregov
                    join import.rosregextractowner rreowner on rregov.id=rreowner.pers_id
                    join import.rosregextractright rreright on rreowner.id=rreright.owner_id
                    join import.rosregextractreg rrereg on rreright.reg_id=rrereg.id
                    join import.rosregextract rre on rre.right_id=rreright.id
                    join import.rosregextractdesc rredesc on rredesc.id=rre.desc_id
                ");
        }
        private void DropViews()
        {
            this.Database.ExecuteNonQuery(@"
                DROP VIEW import.viewrosregextractgov;
                DROP VIEW import.viewrosregextractpers;
                DROP VIEW import.viewrosregextractorg;
            ");
        }
        #endregion
    }
}