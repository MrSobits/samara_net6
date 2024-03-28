Ext.define('B4.model.GisDataBank', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisDataBank'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'Municipality' },
        { name: 'Name' },
        { name: 'Key' }
    ]
});