Ext.define('B4.model.dict.CapitalGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'capitalgroup'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Code' }
    ]
});