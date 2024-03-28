Ext.define('B4.model.RealtyObjTypeWorkCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealtyObjTypeWorkCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'WorkName' },
        { name: 'Period' },
        { name: 'PeriodName' }
    ]
});