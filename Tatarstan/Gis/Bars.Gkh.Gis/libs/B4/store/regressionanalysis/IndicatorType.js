Ext.define('B4.store.regressionanalysis.IndicatorType', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.regressionanalysis.IndicatorType'],
    model: 'B4.model.regressionanalysis.IndicatorType',
    root: {
        text: 'Все индикаторы',
        children: [],
        expanded: true,
        loaded: true
    },
    autoLoad: false
});
