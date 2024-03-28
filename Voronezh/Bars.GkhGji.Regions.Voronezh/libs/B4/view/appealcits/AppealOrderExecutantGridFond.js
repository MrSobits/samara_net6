Ext.define('B4.view.appealcits.AppealOrderExecutantGridFond', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.appealorderexecutantgridfond',
    store: 'appealcits.AppealOrderExecutant',
    title: 'Исполнители ФКР',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [              
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    text: 'ФИО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    text: 'Должность'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    flex: 1,
                    text: 'Телефон'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Email',
                    flex: 1,
                    text: 'Электронная почта'
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});