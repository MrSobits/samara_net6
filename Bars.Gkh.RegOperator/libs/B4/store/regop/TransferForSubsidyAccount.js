Ext.define('B4.store.regop.TransferForSubsidyAccount', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.Transfer',
    requires: ['B4.model.regop.Transfer'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'Transfer',
        listAction: 'ListTransferForSubsidyAccount'
    }
});