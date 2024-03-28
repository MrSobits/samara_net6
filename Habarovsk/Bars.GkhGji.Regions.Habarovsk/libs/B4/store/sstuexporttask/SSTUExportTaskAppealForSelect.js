Ext.define('B4.store.sstuexporttask.SSTUExportTaskAppealForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'AppealNum', 'AppealDate', 'AppState'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'SSTUExportTaskAppeal',
        listAction: 'GetListExportableAppeals'
    }
});
