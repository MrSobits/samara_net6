Ext.define('B4.model.fssp.courtordergku.FsspAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FsspAddress'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'PgmuAddress' }
    ]
});