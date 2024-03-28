Ext.define('B4.model.menu.ManagingOrgDataMenu', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MenuDi'
    },
    fields: [
        { name: 'text' },
        { name: 'controller' },
        { name: 'percent' }
    ]
});