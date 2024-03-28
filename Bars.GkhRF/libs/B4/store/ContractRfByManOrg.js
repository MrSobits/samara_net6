Ext.define('B4.store.ContractRfByManOrg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.ContractRf'],
    autoLoad: false,
    storeId: 'contractRfStore',
    model: 'B4.model.ContractRf',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractRf',
        listAction: 'ListByManOrg'
    }
});