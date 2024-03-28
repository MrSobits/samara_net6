Ext.define('B4.model.dict.erknm.DirectoryERKNM', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DirectoryERKNM'
    },
    fields: [
        { name: 'Id'},
        { name: 'Name'},
        { name: 'Code'},
        { name: 'CodeERKNM'},
        { name: 'EntityName'}
    ]
});