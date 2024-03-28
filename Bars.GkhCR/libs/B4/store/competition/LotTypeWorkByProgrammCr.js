Ext.define('B4.store.competition.LotTypeWorkByProgrammCr', {
    extend: 'B4.base.Store',
    fields: ['Id', 'TypeWork', 'RoMunicipality', 'RoAddress'],
    autoLoad: false,
    storeId: 'lotTypeWorkByProgrammCrStore',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCr',
        listAction: 'ListByProgramCr'
    }
});