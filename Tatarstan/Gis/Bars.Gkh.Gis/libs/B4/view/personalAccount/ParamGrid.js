Ext.define('B4.view.personalAccount.ParamGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.personalAccount_param_grid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.PersonalAccountParam'
    ],

    title: 'Параметры лицевого счета',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PersonalAccountParam');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, text: 'Наименование', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'ValPrm', flex: 1, text: 'Значение в расчетном месяце', filter: { xtype: 'textfield' } }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 1,
                    items: [
                        { xtype: 'b4updatebutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});