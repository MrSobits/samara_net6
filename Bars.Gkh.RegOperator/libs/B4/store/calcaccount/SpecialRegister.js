Ext.define('B4.store.calcaccount.SpecialRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.calcaccount.Special'],
    autoLoad: false,
    model: 'B4.model.calcaccount.Special',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialCalcAccount',
        listAction: 'ListRegister'
    }
});