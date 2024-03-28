Ext.define('B4.store.objectcr.BuildContractForMassBuild', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.BuildContractForMassBuild'],
    autoLoad: false,
    storeId: 'buildContractForMassBuildStore',
    model: 'B4.model.objectcr.BuildContractForMassBuild',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetBuildContractsForMassBuild'
    }
});
