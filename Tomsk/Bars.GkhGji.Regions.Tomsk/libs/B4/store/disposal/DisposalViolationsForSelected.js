Ext.define('B4.store.disposal.DisposalViolationsForSelected', {
    extend: 'B4.base.Store',
    storeId: 'disposalViolationsForSelected',
    autoLoad: false,
    fields: ['Id', 'Name', 'MunicipalityName', 'Address', 'DatePlanRemoval', 'Description']
});