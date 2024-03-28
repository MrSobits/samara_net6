Ext.define('B4.model.dict.TaxSystem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaxSystem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ShortName' }
    ]
});