Ext.define('B4.store.manorg.contract.Transfer', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.contract.Transfer'],
    autoLoad: false,
    storeId: 'manOrgContractRelationStore',
    model: 'B4.model.manorg.contract.Transfer',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgContractRelation',
        listAction: 'List'
    }
});