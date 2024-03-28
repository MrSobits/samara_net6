Ext.define('B4.model.QualificationRegister', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Qualification'
    },
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'ProgrammName' },
        { name: 'MunicipalityName' },
        { name: 'BuilderName' },
        { name: 'Sum' },
        { name: 'Rating' }
    ]
});