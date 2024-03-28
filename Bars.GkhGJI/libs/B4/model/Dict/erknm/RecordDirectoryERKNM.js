Ext.define('B4.model.dict.erknm.RecordDirectoryERKNM', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RecordDirectoryERKNM'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name'},
        { name: 'Code'},
        { name: 'IdentSMEV'},
        { name: 'EntityId'},
        { name: 'CodeERKNM' },
        { name: 'IdentERKNM' },
        { name: 'DirectoryERKNM' },

    ]
});