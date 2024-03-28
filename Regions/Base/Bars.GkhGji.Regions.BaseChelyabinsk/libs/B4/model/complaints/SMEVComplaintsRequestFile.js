Ext.define('B4.model.complaints.SMEVComplaintsRequestFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsRequestFile'
    },
    fields: [
        { name: 'Id'},
        { name: 'SMEVComplaintsRequest' },
        { name: 'SMEVFileType' },
        { name: 'Name' },
        { name: 'FileInfo' }
    ]
});