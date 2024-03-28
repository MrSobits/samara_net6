Ext.define('B4.store.regop.owner.RoomAreaShare', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Checked'},
        { name: 'RoomNum' },
        { name: 'AreaShare' },
        { name: 'ChamberNum' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'Room'
    }
});