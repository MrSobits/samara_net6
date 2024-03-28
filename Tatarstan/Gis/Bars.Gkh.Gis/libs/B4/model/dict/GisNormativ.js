Ext.define('B4.model.dict.GisNormativ', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisNormativDict'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Service' },
        { name: 'Value' },
        { name: 'Measure' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'Description' },
        { name: 'DocumentName' },
        { name: 'DocumentFile' }
    ]
});