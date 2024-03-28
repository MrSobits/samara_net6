Ext.define('B4.store.objectcr.TypeWorkCr', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.TypeWorkCr'],
    autoLoad: false,
    groupField: 'FinanceSourceName',
    storeId: 'typeWorkCrStore',
    model: 'B4.model.objectcr.TypeWorkCr'
});