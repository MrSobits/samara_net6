Ext.define('B4.model.appealcits.CheckTimeChange', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CheckTimeChange'
    },

    fields: [
        {
            name: 'CreateDate'
        },
        {
            name: 'OldValue'
        },
        {
            name: 'NewValue'
        },
        {
            name: 'UserName'
        }
    ]
});