Ext.define('B4.store.contragent.AdditionRole', {
    extend: 'B4.base.Store',
    requires: ['B4.model.contragent.ContragentAdditionRole'],
    autoLoad: false,
    model: 'B4.model.contragent.ContragentAdditionRole',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContragentAdditionRole',
        listAction: 'ListAdditionRole'
    }
});