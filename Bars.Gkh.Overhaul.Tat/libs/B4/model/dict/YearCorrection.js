Ext.define('B4.model.dict.YearCorrection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'YearCorrection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Year', defaultValue: null }
    ]
});