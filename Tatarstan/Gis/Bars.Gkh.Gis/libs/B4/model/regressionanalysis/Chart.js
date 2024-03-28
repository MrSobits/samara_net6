Ext.define('B4.model.regressionanalysis.Chart', {
    extend: 'B4.base.Model',
    idProperty: 'Id',    

    proxy: {
        url: 'RegressionAnalysis/GetChartData',
        type: 'ajax',
        reader: {
            type: 'json',
            root: 'data'
        }
    }
});