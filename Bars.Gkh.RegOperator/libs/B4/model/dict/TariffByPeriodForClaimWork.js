Ext.define('B4.model.dict.TariffByPeriodForClaimWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TariffByPeriodForClaimWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'ChargePeriod' },
        { name: 'Municipality' },
        { name: 'Value' }
    ]
});