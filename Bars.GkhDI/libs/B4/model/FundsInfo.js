Ext.define('B4.model.FundsInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FundsInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'DocumentName' },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'Size', defaultValue: null }
    ]
});