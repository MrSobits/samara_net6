Ext.define('B4.store.fssp.courtordergku.Litigation', {
    extend: 'B4.base.Store',
    requires: [
        'B4.model.fssp.courtordergku.Litigation'
    ],
    autoLoad: false,
    model: 'B4.model.fssp.courtordergku.Litigation',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});