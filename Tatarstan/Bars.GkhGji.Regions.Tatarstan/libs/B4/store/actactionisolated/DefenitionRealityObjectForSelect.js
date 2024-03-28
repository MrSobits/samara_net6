Ext.define('B4.store.actactionisolated.DefenitionRealityObjectForSelect', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'Address', 'Municipality'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActActionIsolated',
        listAction: 'GetRealityObjectsForDefinition'
    }
});