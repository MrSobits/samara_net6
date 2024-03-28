Ext.define('B4.view.smevndfl.AnswerGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.smevndflanswergrid',
    height: 300,
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.SMEVFileType',
    ],

    title: 'Данные ответа',
    store: 'smev.SMEVNDFLAnswer',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'INNUL',
                    text: 'ИНН',
                    flex: 1
                },   
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KPP',
                    text: 'КПП',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrgName',
                    text: 'Организация',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Rate',
                    text: 'Ставка',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueCode',
                    text: 'Код дохода',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Month',
                    text: 'Месяц',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueSum',
                    text: 'Сумма дохода',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecoupmentCode',
                    text: 'Код вычет',
                    flex: 1,
                    hidden: true
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RecoupmentSum',
                    text: 'Сумма вычет',
                    flex: 1,
                    hidden: true
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DutyBase',
                    text: 'Нал база',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DutySum',
                    text: 'Нал.сум.исч',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnretentionSum',
                    text: 'Не удерж.',
                    flex: 1
                },  
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RevenueTotalSum',
                    text: 'Сумма общая',
                    flex: 1
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
                            columns: 2,
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