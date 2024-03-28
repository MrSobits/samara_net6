Ext.define('B4.store.longtermprobject.AccountCharge', {
    extend: 'B4.base.Store',
    requires: ['B4.model.realityobj.housingcommunalservice.AccountCharge'],
    autoLoad: false,
    model: 'B4.model.realityobj.housingcommunalservice.AccountCharge',
    groupField: 'Date'
});