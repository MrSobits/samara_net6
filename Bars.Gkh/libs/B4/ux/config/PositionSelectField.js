Ext.define('B4.ux.config.PositionSelectField', {
    extend: 'B4.ux.config.TriggerSingleSelectEditor',
    alias: 'widget.positionselectfield',

    //#region override

    storePath: 'B4.store.dict.Position',
    textProperty: 'Name',
    columnsGridSelect: [
        {
            xtype: 'gridcolumn',
            header: 'Код',
            dataIndex: 'Code',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            xtype: 'gridcolumn',
            header: 'Должность',
            dataIndex: 'Name',
            flex: 3,
            filter: { xtype: 'textfield' }
        },
    ]

    //#endregion
});