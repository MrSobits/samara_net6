Ext.define('B4.store.gisGkh.RoomGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.gisGkh.RoomGridModel'],
    model: 'B4.model.gisGkh.RoomGridModel',
    storeId: 'gisGkhRoomGridStore',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhExecute',
        listAction: 'ListRooms'
    }
});