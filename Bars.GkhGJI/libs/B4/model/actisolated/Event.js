Ext.define('B4.model.actisolated.Event', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedRealObjEvent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolatedRealObj', defaultValue: null },
        { name: 'Name' },
        { name: 'Term' },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null }
    ]
});