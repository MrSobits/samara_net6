Ext.define('B4.store.regop.personal_account.Distribution', {
    extend: 'B4.base.Store',
    fields: [
        { name: 'Id' },
        { name: 'PersonalAccountNum' },
        { name: 'AccountOwner' },
        { name: 'RoomAddress' },
        { name: 'State' },
        { name: 'OpenDate' },
        { name: 'RealArea'},
        { name: 'RoPayAccountNum' },
        { name: 'AccountFormationVariant' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'ListAccountsForDistribution',
        timeout: 120000
    },
    autoLoad: false
});