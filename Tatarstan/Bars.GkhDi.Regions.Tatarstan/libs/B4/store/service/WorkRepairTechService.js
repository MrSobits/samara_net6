Ext.define('B4.store.service.WorkRepairTechService', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.WorkRepairTechServ'],
    autoLoad: false,
    storeId: 'workRepairTechServiceStore',
    model: 'B4.model.service.WorkRepairTechServ'
});