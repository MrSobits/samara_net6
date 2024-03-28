Ext.define('B4.store.multipleAnalysis.Indicator', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.multipleAnalysis.Indicator'],
    model: 'B4.model.multipleAnalysis.Indicator',
    root: {
        children: [],
        expanded: true,
        loaded: true
    }
});
