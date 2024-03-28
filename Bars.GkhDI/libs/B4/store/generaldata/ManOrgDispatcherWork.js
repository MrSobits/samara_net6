Ext.define('B4.store.generaldata.ManOrgDispatcherWork', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.WorkMode'],
    autoLoad: false,
    storeId: 'generalDataManOrgDispatcherWorkStore',
    model: 'B4.model.manorg.WorkMode'
});