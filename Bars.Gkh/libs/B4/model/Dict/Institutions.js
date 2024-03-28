Ext.define('B4.model.dict.Institutions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Institutions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Abbreviation' },
        { name: 'Address' },
        { name: 'AddressName' }
    ]
});