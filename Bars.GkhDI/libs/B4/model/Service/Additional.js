Ext.define('B4.model.service.Additional', {
    extend: 'B4.model.service.Base',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdditionalService'
    },
    fields: [
        { name: 'Periodicity', defaultValue: null },
        { name: 'Document' },
        { name: 'DocumentNumber' },
        { name: 'DocumentFrom', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'Total', defaultValue: null }
    ]
});