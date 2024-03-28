Ext.define('B4.model.dict.service.BilNormativStorage', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BilNormativStorage'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Name' },
        { name: 'Value' },
        { name: 'Measure' },
        { name: 'Description' }
    ]
});