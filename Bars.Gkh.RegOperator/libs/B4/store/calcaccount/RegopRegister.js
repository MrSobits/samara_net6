Ext.define('B4.store.calcaccount.RegopRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.calcaccount.Regop'],
    autoLoad: false,
    model: 'B4.model.calcaccount.Regop',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegopCalcAccount',
        listAction: 'ListRegister'
    }
});