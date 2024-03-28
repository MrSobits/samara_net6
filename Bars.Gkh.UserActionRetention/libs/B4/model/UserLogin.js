Ext.define('B4.model.UserLogin', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: ['Id', 'Name'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'UserLogin',
        listAction: 'List'
    }
});
