Ext.define('B4.model.dict.ProsecutorOffice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProsecutorOfficeDict'
    },
    fields: [
        { name: 'Type'},
        { name: 'Code' },
        { name: 'ErknmCode' },
        { name: 'Name'},
        { name: 'FederalDistrictCode'},
        { name: 'FederalDistrictName'},
        { name: 'FederalCenterCode'},
        { name: 'FederalCenterName'},
        { name: 'DistrictCode'},
        { name: 'ParentId' },
        { name: 'OkatoTer' },
        { name: 'OkatoKod1' },
        { name: 'OkatoKod2' },
        { name: 'OkatoKod3' },
        { name: 'UseDefault' }
    ]
});