Ext.define('B4.model.realityobj.Intercom', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Intercom'
    },
    fields: [
        { name: 'Id' },
        { name: 'RealityObject'},
        { name: 'IntercomCount'},
        { name: 'IntercomType' },
        { name: 'Recording', defaultValue: 30 },
        { name: 'ArchiveDepth' },
        { name: 'ArchiveAccess', defaultValue: 30 },
        { name: 'Tariff' },
        { name: 'InstallationDate' },
        { name: 'UnitMeasure' },
        { name: 'HasNotTariff' },
        { name: 'AnalogIntercomCount' },
        { name: 'IpIntercomCount' },
        { name: 'EntranceCount' },
        { name: 'IntercomInstallationDate' }
    ]
});