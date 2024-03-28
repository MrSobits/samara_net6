Ext.define('B4.store.payment.SanitaryEngineeringRepair', {
    extend: 'B4.base.Store',
    requires: ['B4.model.payment.Item'],
    autoLoad: false,
    storeId: 'sanitaryEngineeringRepairStore',
    typePayment: 'SanitaryEngineeringRepair',
    model: 'B4.model.payment.Item'
});