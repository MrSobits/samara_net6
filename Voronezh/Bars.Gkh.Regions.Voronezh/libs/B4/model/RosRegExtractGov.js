Ext.define('B4.model.RosRegExtractGov', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractGov'
    },
    fields: [
        { name: 'Id' },
        { name: 'Gov_Code_SP' },
        { name: 'Gov_Content' },
        { name: 'Gov_Name' },
        { name: 'Gov_OKATO_Code' },
        { name: 'Gov_Country' },
        { name: 'Gov_Address' }
    ]
});