Ext.define('B4.store.objectcr.ObjectCRForMassBuilderForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Municipality', 'Address'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetListObjectCRByMassBuilderId'
    }
});
