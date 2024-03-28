Ext.define('B4.store.regoperator.Account', {
    extend: 'B4.base.Store',
    requires: ['B4.model.regoperator.Account'],
    autoLoad: false,
    model: 'B4.model.regoperator.Account',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpAccount',
        listAction: 'List'
    }
});