Ext.define('B4.model.dict.CentralHeatingStation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CentralHeatingStation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Abbreviation' },
        { name: 'Address' },
        { name: 'AddressName' }
    ]
});