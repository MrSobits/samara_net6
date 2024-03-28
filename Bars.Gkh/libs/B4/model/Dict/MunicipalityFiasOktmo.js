Ext.define('B4.model.dict.MunicipalityFiasOktmo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MunicipalityFiasOktmo'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'OffName' },
        { name: 'FiasGuid' },
        { name: 'Oktmo' }
    ]
});