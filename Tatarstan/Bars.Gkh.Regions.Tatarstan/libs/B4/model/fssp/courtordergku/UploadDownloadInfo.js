Ext.define('B4.model.fssp.courtordergku.UploadDownloadInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'UploadDownloadInfo',
        actionName: 'List',
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DownloadFile' },
        { name: 'DateDownloadFile' },
        { name: 'User' },
        { name: 'Status' },
        { name: 'LogFile' },
        { name: 'UploadFile' }
    ]
});