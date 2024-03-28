Ext.define('B4.model.RosRegExtractRight', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractRight'
    },
    fields: [
        { name: 'Id' },
        { name: 'reg_id' },
        { name: 'owner_id' },
        { name: 'RightNumber' }
    ]
});