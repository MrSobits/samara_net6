Ext.define('B4.view.realityobj.ProgramCrGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjprogramcrgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Программы КР',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.ObjectCr');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    width: 40,
                    xtype: 'actioncolumn',
                    icon: 'content/img/icons/arrow_out.png',
                    handler: function(view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'redirect', record);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramCrName',
                    text: 'Программа',
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PeriodName',
                    flex: 1,
                    text: 'Период'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    flex: 1,
                    renderer: function (v) {
                        return v && v.Name ? v.Name : '';
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4updatebutton'}
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});