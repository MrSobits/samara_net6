Ext.define('B4.model.service.TariffForConsumers', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TariffForConsumers'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'DateStart', defaultValue: null, type: 'date' },
        { name: 'DateEnd', defaultValue: null, type: 'date' },
        { name: 'TariffIsSetFor', defaultValue: 10 },
        { name: 'TypeOrganSetTariffDi', defaultValue: 10 },
        { name: 'Cost', defaultValue: null }
    ]
});