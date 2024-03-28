Ext.define('B4.model.PropertyOwnerDecisionWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PropertyOwnerDecisionWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BasePropertyOwnerDecision' },
        { name: 'Work' },
        { name: 'WorkName' }
    ]
});