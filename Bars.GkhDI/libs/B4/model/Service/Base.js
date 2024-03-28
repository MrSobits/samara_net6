Ext.define('B4.model.service.Base', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Service'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TemplateService', defaultValue: null },
        { name: 'Name', defaultValue: null },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'Provider', defaultValue: null },
        { name: 'ProviderName' },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'Profit', defaultValue: null },
        { name: 'TypeGroupServiceDi', defaultValue: 10 },
        { name: 'KindServiceDi', defaultValue: 10 },
        { name: 'Tariff' },
        { name: 'TariffIsSetFor', defaultValue: 10 },
        { name: 'DateStart' },
        { name: 'percent' },
        { name: 'ScheduledPreventiveMaintanance', defaultValue: 30 }
    ]
});