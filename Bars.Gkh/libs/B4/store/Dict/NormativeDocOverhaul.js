Ext.define('B4.store.dict.NormativeDocOverhaul', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.NormativeDoc'],
    autoLoad: false,
    model: 'B4.model.dict.NormativeDoc',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormativeDoc',
        listAction: 'ListOverhaul'
    }
});