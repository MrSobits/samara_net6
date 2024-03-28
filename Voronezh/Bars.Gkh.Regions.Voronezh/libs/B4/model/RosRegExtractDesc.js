Ext.define('B4.model.RosRegExtractDesc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractDesc'
    },
    fields: [
        { name: 'Id' },
        { name: 'Desc_ID_Object' },
        { name: 'Desc_CadastralNumber' },
        { name: 'Desc_ObjectType' },
        { name: 'Desc_ObjectTypeText' },
        { name: 'Desc_ObjectTypeName' },
        { name: 'Desc_AssignationCode' },
        { name: 'Desc_AssignationCodeText' },
        { name: 'Desc_Area' },
         { name: 'PersAcc' },        
        { name: 'Desc_AreaText' },
        { name: 'Desc_AreaUnit' },
        { name: 'Desc_Floor' },
        { name: 'Desc_ID_Address' },
        { name: 'Desc_AddressContent' },
        { name: 'Desc_RegionCode' },
        { name: 'Desc_RegionName' },
        { name: 'Desc_OKATO' },
        { name: 'Desc_KLADR' },
        { name: 'Desc_CityName' },
        { name: 'Desc_Urban_District' },
        { name: 'Desc_Locality' },
        { name: 'Desc_StreetName' },
        { name: 'Desc_Level1Name' },
        { name: 'Desc_Level2Name' },
         { name: 'Desc_ApartmentName' },
        { name: 'Reg_RegDate' },
        { name: 'Reg_RegNumber' },
         { name: 'Room_id' },
         { name: 'YesNoNotSet' }
        
    ]
});