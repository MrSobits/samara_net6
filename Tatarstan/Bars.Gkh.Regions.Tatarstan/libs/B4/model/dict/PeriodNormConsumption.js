Ext.define('B4.model.dict.PeriodNormConsumption', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PeriodNormConsumption'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'StartDate' },
        { name: 'EndDate' }
    ]
});