Ext.define('B4.model.dict.MKDLicTypeRequest', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicTypeRequest'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Code' }
    ]
});