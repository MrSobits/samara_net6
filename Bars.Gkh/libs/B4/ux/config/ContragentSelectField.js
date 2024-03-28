Ext.define('B4.ux.config.ContragentSelectField', {
    extend: 'B4.ux.config.TriggerSingleSelectEditor',
    alias: 'widget.contragentselectfield',

    requires: [
        'B4.store.Contragent'
    ],

    //#region override

    storePath: 'B4.store.Contragent',
    textProperty: 'Name',
    columnsGridSelect: [
        {
            xtype: 'gridcolumn',
            header: 'Наименование',
            dataIndex: 'Name',
            flex: 3,
            filter: { xtype: 'textfield' }
        },
        {
            xtype: 'gridcolumn',
            header: 'ИНН',
            dataIndex: 'Inn',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            xtype: 'gridcolumn',
            header: 'КПП',
            dataIndex: 'Kpp',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
    ]

    //#endregion
});