Ext.define('B4.store.regop.personal_account.InMemoryRoomAccountStore', {
    extend: 'Ext.data.Store',
    fields: [
        {name: 'PersonalAccountNum'},
        {name: 'AreaShare'},
        {name: 'NewShare'},
        {name: 'RoomId'},
        {name: 'AccountId'}
    ],
    proxy: {
        type: 'memory'
    }
});