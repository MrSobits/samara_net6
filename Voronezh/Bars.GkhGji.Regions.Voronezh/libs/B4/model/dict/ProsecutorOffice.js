Ext.define('B4.model.dict.ProsecutorOffice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProsecutorOffice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'FederalCentersCode' },
        { name: 'LocalAreasCode' },
        { name: 'RegionsCode' },
        { name: 'Code' },
        { name: 'Municipality' }
    ]
});