Ext.define('B4.store.MpRoleForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Role'],
    autoLoad: false,
    model: 'B4.model.Role',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MpRole',
        listAction: 'GetRoles'
    },
    sorters: [{
        property: 'Name',
        direction: 'ASC'
    }],
});