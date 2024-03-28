Ext.define('B4.store.generaldata.ManOrgReceptionCitizens', {
    extend: 'B4.base.Store',
    requires: ['B4.model.manorg.WorkMode'],
    autoLoad: false,
    storeId: 'generalDataManOrgReceptionCitizensStore',
    model: 'B4.model.manorg.WorkMode'
});