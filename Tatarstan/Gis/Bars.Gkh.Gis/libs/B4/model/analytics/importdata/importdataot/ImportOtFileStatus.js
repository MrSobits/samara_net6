Ext.define('B4.model.analytics.importdata.importdataot.ImportOtFileStatus', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ImportDataOt'
    },
    fields: [
        { name: 'Id' },
        { name: 'UserId' },
        { name: 'FileName' },
        { name: 'TypeStatus' },
        { name: 'TypeAction' },
        { name: 'FileId' },
        { name: 'Progress' },
        { name: 'UserName' },
        { name: 'ImportId' }
    ]
});
