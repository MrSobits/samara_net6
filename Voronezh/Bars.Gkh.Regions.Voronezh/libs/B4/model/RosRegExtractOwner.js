Ext.define('B4.model.RosRegExtractOwner', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractOwner'
    },
    fields: [
        { name: 'Id' },
        { name: 'gov_id' },
        { name: 'org_id' },
        { name: 'pers_id' }
    ]
});