Ext.define('B4.model.OwnerAccountRoomComparison', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'OwnerAccountRoomComparison'
    },
    fields: [
        { name: 'Id' },
        { name: 'BasePersonalAccount' },
        { name: 'Account_num' },
        { name: 'Room' },
        { name: 'DataUpdateDate' },
        { name: 'DataUpdateDateFrom' },
        { name: 'AddressContent' },
        { name: 'FIO' },
        { name: 'ROAddress' },
        { name: 'RoomArea' },
        { name: 'IsAccountsMerged' },
        { name: 'IsDataUpdated' },

    ]
});