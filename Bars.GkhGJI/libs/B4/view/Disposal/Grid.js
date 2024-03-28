Ext.define('B4.view.disposal.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.DisposalTextValues'
    ],

    title: '',
    store: 'Disposal',
    itemId: 'disposalGrid',
    closable: true,
    
    initComponent: function() {
        var me = this;

        me.title = 'Реестр ' + B4.DisposalTextValues.getGenetiveManyCase();

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width:200,
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100
                }
                
            ],
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
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                }
                            ]
                        }
                    ]
                        },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});