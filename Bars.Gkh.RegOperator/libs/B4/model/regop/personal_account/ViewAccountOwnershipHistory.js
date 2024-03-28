Ext.define('B4.model.regop.personal_account.ViewAccountOwnershipHistory', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountId' },
        { name: 'OwnerId' },
        { name: 'RoomId' },
        { name: 'State' },
        { name: 'Municipality' },
        { name: 'RoomAddress' },
        { name: 'PersonalAccountNum' },
        { name: 'AccountOwner' },
        { name: 'OwnerType' },
        { name: 'AccountFormationVariant' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        timeout: 3 * 60 * 1000,
        listAction: 'ListAccountsByDate'
    }
});