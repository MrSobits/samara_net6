Ext.define('B4.model.dict.Position', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'position'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'NameGenitive' },
        { name: 'NameDative' },
        { name: 'NameAccusative' },
        { name: 'NameAblative' },
        { name: 'NamePrepositional' }
    ]
});