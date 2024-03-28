Ext.define('B4.store.fssp.courtordergku.UploadDownloadInfo', {
    extend: 'B4.base.Store',
    requires: [
        'B4.model.fssp.courtordergku.UploadDownloadInfo'
    ],
    autoLoad: false,
    model: 'B4.model.fssp.courtordergku.UploadDownloadInfo',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});