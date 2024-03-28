Ext.define('B4.model.dict.HeatSeasonPeriodGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatSeasonPeriodGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});