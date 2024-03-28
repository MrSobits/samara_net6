Ext.define('B4.store.specaccowner.RealityObjectOnSpecAcc', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Address' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'GjiScript',
        listAction: 'ListRealityObjectOnSpecAcc'
    }
});