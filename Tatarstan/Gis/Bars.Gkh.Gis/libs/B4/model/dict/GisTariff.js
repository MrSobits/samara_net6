Ext.define('B4.model.dict.GisTariff', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisTariffDict'
    },
    fields: [
        { name: 'Id' },
        { name: 'EiasUploadDate' },
        { name: 'EiasEditDate' },
        { name: 'Municipality' },
        { name: 'Service' },
        { name: 'Contragent' },
        { name: 'ContragentInn' },
        { name: 'ActivityKind' },
        { name: 'ContragentName' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'TariffKind' },
        { name: 'ZoneCount', useNull: true, defaultValue: null },
        { name: 'TariffValue', useNull: true, defaultValue: null },
        { name: 'TariffValue1', useNull: true, defaultValue: null },
        { name: 'TariffValue2', useNull: true, defaultValue: null },
        { name: 'TariffValue3', useNull: true, defaultValue: null },
        { name: 'IsNdsInclude', useNull: true, defaultValue: false },
        { name: 'IsSocialNorm', useNull: true, defaultValue: false },
        { name: 'IsMeterExists', useNull: true, defaultValue: false },
        { name: 'IsElectricStoveExists', useNull: true, defaultValue: false },
        { name: 'UnitMeasure' },
        { name: 'Floor', useNull: true, defaultValue: null },
        { name: 'ConsumerType', useNull: true, defaultValue: null },
        { name: 'SettelmentType', useNull: true, defaultValue: null },
        { name: 'ConsumerByElectricEnergyType', useNull: true, defaultValue: null },
        { name: 'RegulatedPeriodAttribute' },
        { name: 'BasePeriodAttribute' }
    ]
});