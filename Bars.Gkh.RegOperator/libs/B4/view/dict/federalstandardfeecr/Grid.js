Ext.define('B4.view.dict.federalstandardfeecr.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.PaymentDocInfo'
    ],

    title: 'Федеральный стандарт взноса на КР',
    alias: 'widget.federalstandardfeecrgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.FederalStandardFeeCr');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
               {
                    xtype: 'b4editcolumn',
                    scope: me
               },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Действует с',
                    flex: 1,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    text: 'Значение',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Действует по',
                    flex: 1,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});