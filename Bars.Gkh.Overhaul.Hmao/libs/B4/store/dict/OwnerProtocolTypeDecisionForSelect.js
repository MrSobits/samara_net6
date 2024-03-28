Ext.define('B4.store.dict.OwnerProtocolTypeDecisionForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.OwnerProtocolTypeDecision'],
    autoLoad: false,
    storeId: 'ownerProtocolTypeDecisionForSelect',
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'OwnerProtocolTypeDecisions',
        listAction: 'SelectDecision'
    }
});