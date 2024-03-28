Ext.define('B4.model.RosRegExtract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtract'
    },
    fields: [
        { name: 'Id' },
        { name: 'desc_id' },
        { name: 'right_id' }
    ]
});