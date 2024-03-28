Ext.define('B4.model.dict.HeatingSeasonResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatingSeasonResolution'
    },
    fields: [
        { name: 'Municipality' },
        { name: 'MunicipalityName' },
        { name: 'AcceptDate' },
        { name: 'Doc' },
        { name: 'HeatSeasonPeriodGji' },
        { name: 'Phantom' }
    ]

});