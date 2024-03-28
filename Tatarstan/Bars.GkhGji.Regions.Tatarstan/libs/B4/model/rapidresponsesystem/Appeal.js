Ext.define('B4.model.rapidresponsesystem.Appeal', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RapidResponseSystemAppeal'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits' },
        { name: 'Contragent' },
        { name: 'ContragentId' },
        { name: 'ContragentName' }
    ]
});