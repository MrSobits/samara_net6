Ext.define('B4.model.finactivity.Docs', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityDocs'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'BookkepingBalance', defaultValue: null },
        { name: 'BookkepingBalanceAnnex', defaultValue: null }
    ]
});