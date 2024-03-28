Ext.define('B4.store.account.ContragentForSpecial', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    model: 'B4.model.Contragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Contragent',
        listAction: 'ListForSpecialAccount'
    }
});