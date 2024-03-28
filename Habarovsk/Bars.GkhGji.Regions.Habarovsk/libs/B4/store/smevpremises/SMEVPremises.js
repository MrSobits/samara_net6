Ext.define('B4.store.smevpremises.SMEVPremises', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smevpremises.SMEVPremises'],
    autoLoad: false,
    storeId: 'SMEVPremisesStore',
    model: 'B4.model.smevpremises.SMEVPremises'
});