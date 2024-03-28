Ext.define('B4.model.regop.personal_account.ApartmentNumber', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Room'
    },

    fields: [
        { name: 'Id' },
        { name: 'RoomNum' },
        { name: 'Entrance' },
        { name: 'Area' },
        { name: 'Type' }
    ]
});