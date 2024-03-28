Ext.define('B4.store.RoomAddress', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'Room',
        listAction: 'ListRoomAddress'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Address'}
    ]
});