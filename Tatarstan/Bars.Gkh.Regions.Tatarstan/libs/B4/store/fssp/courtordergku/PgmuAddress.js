Ext.define('B4.store.fssp.courtordergku.PgmuAddress', {
    extend: 'B4.base.Store',
    requires: [
        'B4.model.fssp.courtordergku.PgmuAddress'
    ],
    autoLoad: false,
    model: 'B4.model.fssp.courtordergku.PgmuAddress',
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ]
});