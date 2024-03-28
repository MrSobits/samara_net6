Ext.define('B4.store.vdgoviolators.VDGOViolatorsMinOrgContr', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'VDGOViolatorsMinOrgContrStore',
    model: 'B4.model.Contragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VDGOViolators',
        listAction: 'GetListMinOrgContragent'
    }
});