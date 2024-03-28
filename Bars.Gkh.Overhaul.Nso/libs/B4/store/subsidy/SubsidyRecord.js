Ext.define('B4.store.subsidy.SubsidyRecord', {
    extend: 'B4.base.Store',
    requires: ['B4.model.subsidy.SubsidyRecord'],
    autoLoad: false,
    model: 'B4.model.subsidy.SubsidyRecord',
    groupField: 'IsShortTerm'
});