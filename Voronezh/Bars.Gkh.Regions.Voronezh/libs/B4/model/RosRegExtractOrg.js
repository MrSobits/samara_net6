Ext.define('B4.model.RosRegExtractOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractOrg'
    },
    fields: [
        { name: 'Id' },
        { name: 'Org_Code_SP' },
        { name: 'Org_Content' },
        { name: 'Org_Code_OPF' },
        { name: 'Org_Name' },
        { name: 'Org_Inn' },
        { name: 'Org_Code_OGRN' },
        { name: 'Org_RegDate' },
        { name: 'Org_AgencyRegistration' },
        { name: 'Org_Code_CPP' },
        { name: 'Org_AreaUnit' },
        { name: 'Org_Loc_ID_Address' },
        { name: 'Org_Loc_Content' },
        { name: 'Org_Loc_RegionCode' },
        { name: 'Org_Loc_RegionName' },
        { name: 'Org_Loc_Code_OKATO' },
        { name: 'Org_Loc_Code_KLADR' },
        { name: 'Org_Loc_CityName' },
        { name: 'Org_Loc_StreetName' },
        { name: 'Org_Loc_Level1Name' },
        { name: 'Org_FLoc_ID_Address' },
        { name: 'Org_FLoc_Content' },
        { name: 'Org_FLoc_RegionCode' },
        { name: 'Org_FLoc_RegionName' },
        { name: 'Org_FLoc_Code_OKATO' },
        { name: 'Org_FLoc_Code_KLADR' },
        { name: 'Org_FLoc_CityName' },
        { name: 'Org_FLoc_StreetName' },
        { name: 'Org_FLoc_Level1Name' }
    ]
});