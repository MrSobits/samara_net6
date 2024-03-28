Ext.define('B4.store.regressionanalysis.HouseType', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.regressionanalysis.HouseType'],
    model: 'B4.model.regressionanalysis.HouseType',
    root: {
        children: [],
        expanded: true,
        loaded: true
    },
    autoLoad: false
});
