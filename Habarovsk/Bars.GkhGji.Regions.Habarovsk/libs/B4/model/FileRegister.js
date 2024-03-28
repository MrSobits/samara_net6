Ext.define('B4.model.FileRegister', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'FileRegister'
    },
    fields: [
        { name: 'Id' },
        { name: 'RealityObject'},
        { name: 'File' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'Address' },
        { name: 'MuName'}
    ]
});