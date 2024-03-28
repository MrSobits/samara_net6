Ext.define('B4.model.smev.GASUData', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GASUData'
    },
    fields: [
        { name: 'Id'},
        { name: 'UnitMeasure' },
        { name: 'GASU' },
        { name: 'Value' },
        { name: 'Indexname' },
        { name: 'IndexUid' }
    ]
});