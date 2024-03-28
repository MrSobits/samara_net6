Ext.define('B4.ux.config.RoleSelectField', {
    extend: 'B4.ux.config.TriggerSingleSelectEditor',
    alias: 'widget.roleselectfield',

    //#region override

    storePath: 'B4.store.Role',
    textProperty: 'Name',
    columnsGridSelect: [
        {
            xtype: 'gridcolumn',
            header: 'Наименование',
            dataIndex: 'Name',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
    ]

    //#endregion
});