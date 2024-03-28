Ext.define('B4.model.Extract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires:
    [
        'B4.enums.ExtractType'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Extract'
    },
    fields: [
        { name: 'Id' },
        { name: 'CreateDate' },
        { name: 'Type' },
        { name: 'IsParsed' },
        { name: 'IsActive' }
    ]
});