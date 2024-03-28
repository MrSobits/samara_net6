Ext.define('B4.model.actcheck.ActionCarriedOutEvent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckActionCarriedOutEvent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckAction' },
        { name: 'EventType' }
    ]
});