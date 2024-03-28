Ext.define('B4.store.realityobj.Contract', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.contract.Base'],
    autoLoad: false,
    storeId: 'realityObjContractStore',
    model: 'B4.model.manorg.contract.Base',
    sorters: [
        {
            property: 'StartDate',
            direction: 'ASC'
        }
    ]
});