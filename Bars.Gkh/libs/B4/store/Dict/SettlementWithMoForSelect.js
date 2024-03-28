Ext.define('B4.store.dict.SettlementWithMoForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.SettlementWithMo'],
    autoLoad: false,
    storeId: 'settlementWithMoForSelect',
    model: 'B4.model.dict.SettlementWithMo',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Municipality',
        listAction: 'ListSettlementWithParentMoName'
    }
});
