Ext.define('B4.model.dict.MeasuresReduceCosts', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MeasuresReduceCosts'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MeasureName' }
    ]
});