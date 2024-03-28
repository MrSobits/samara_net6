Ext.define('B4.model.specialobjectcr.Qualification', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialQualification'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr', defaultValue: null },
        { name: 'Builder', defaultValue: null },
        { name: 'BuilderName' },
        { name: 'Sum', defaultValue: null },
        { name: 'Rating' },
        { name: 'Dict'}    
    ]
});