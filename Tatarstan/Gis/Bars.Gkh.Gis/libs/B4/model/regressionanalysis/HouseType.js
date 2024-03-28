Ext.define('B4.model.regressionanalysis.HouseType', {
    extend: 'Ext.data.Model',
    idProperty: 'id',    
    fields: [
        'id',
        'text',
        'EntityId'
    ],
    proxy: {
        url: 'RegressionAnalysis/GetHouseTypes',
        type: 'ajax'
    }
});