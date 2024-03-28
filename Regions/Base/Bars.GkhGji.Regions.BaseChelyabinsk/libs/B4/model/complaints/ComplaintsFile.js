Ext.define('B4.model.complaints.ComplaintsFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsLT',
        listAction: 'ListComplaintFiles'
    },
    fields: [        
        { name: 'Name' },
        { name: 'SMEVFileType' },
        { name: 'FileInfo' }
    ]
});