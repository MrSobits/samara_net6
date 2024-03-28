Ext.define('B4.model.contragent.ActivityStage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityStage'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'EntityId', defaultValue: null },
        { name: 'EntityType', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd' },
        { name: 'ActivityStageType', defaultValue: null },
        { name: 'Description' },
        { name: 'Document', defaultValue: null }
    ]
});