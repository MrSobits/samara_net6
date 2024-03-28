Ext.define('B4.model.dict.MunicipalitySourceFinancing', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MunicipalitySourceFinancing'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'AddKr' },
        { name: 'AddFk' },
        { name: 'AddEk' },
        { name: 'SourceFinancing' },
        { name: 'Kvr' },
        { name: 'Kvsr' },
        { name: 'Kif' },
        { name: 'Kfsr' },
        { name: 'Kcsr' },
        { name: 'Kes' }
    ]
});