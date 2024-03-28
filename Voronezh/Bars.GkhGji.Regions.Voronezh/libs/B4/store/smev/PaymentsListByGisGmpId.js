Ext.define('B4.store.smev.PaymentsListByGisGmpId', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smev.PayReg'],
    autoLoad: false,
    model: 'B4.model.smev.PayReg',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PAYREGExecute',
        listAction: 'GetPaymentsListByGisGmpId'
    }
});