Ext.define('B4.model.otherservice.TariffForConsumersOtherService', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TariffForConsumersOtherService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OtherService', defaultValue: null },
        { name: 'DateStart', defaultValue: null, type: 'date' },
        { name: 'DateEnd', defaultValue: null, type: 'date' },
        { name: 'TariffIsSetFor', defaultValue: 10 },
        { name: 'TypeOrganSetTariffDi', defaultValue: 10 },
        { name: 'Cost', defaultValue: null }
    ]
});