Ext.define('B4.store.specialobjectcr.DefectList', {
    extend: 'B4.base.Store',
    requires: ['B4.model.specialobjectcr.DefectList'],
    groupField: 'WorkName',
    autoLoad: false,
    model: 'B4.model.specialobjectcr.DefectList'
});