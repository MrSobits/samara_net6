Ext.define('B4.model.belaypolicy.Event', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayPolicyEvent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EventDate' },
        { name: 'Description' },
        { name: 'BelayPolicy', defaultValue: null },
        { name: 'FileInfo', defaultValue: null }
    ]
});