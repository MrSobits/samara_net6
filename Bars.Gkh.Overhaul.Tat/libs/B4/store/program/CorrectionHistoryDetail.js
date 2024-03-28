Ext.define('B4.store.program.CorrectionHistoryDetail', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrCorrectionStage2',
        listAction: 'GetHistoryDetail'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PropertyName' },
        { name: 'OldValue' },
        { name: 'NewValue' },
        { name: 'Type' }
    ]
});