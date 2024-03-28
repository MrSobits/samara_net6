Ext.define('B4.store.integrations.bills.Acknowledgment', {
    extend: 'B4.base.Store',
    requires: ['B4.model.integrations.bills.Acknowledgment'],
    autoLoad: false,
    storeId: 'acknowledgmentDictStore',
    model: 'B4.model.integrations.bills.Acknowledgment'
});