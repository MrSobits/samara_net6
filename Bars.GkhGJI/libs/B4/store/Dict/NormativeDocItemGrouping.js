Ext.define('B4.store.dict.NormativeDocItemGrouping', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.NormativeDocItem'],
    autoLoad: false,
    groupField: 'NormativeDocName',
    model: 'B4.model.dict.NormativeDocItem'
});