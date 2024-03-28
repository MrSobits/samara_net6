Ext.define('B4.model.appealcits.AppCitPrFondObjectCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppCitPrFondObjectCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCitsPrescriptionFond' },
        { name: 'ObjectCr' },
        { name: 'Address' },
        { name: 'Program' }
    ]
});