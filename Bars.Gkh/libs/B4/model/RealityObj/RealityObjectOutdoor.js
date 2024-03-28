Ext.define('B4.model.realityobj.RealityObjectOutdoor', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectOutdoor'
    },
    fields: [
        { name: 'MunicipalityFiasOktmo' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Area', defaultValue: null },
        { name: 'AsphaltArea', defaultValue: null },
        { name: 'Description', defaultValue: null },
        { name: 'Locality', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'RealityObjects', defaultValue: null },
        { name: 'RepairPlanYear', defaultValue: null }
    ]
});