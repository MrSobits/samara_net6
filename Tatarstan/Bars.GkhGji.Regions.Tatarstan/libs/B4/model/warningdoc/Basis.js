Ext.define('B4.model.warningdoc.Basis', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningDocBasis'
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'Name' },
    ]
});