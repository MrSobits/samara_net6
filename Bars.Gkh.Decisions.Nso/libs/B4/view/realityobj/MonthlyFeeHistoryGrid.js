Ext.define('B4.view.realityobj.MonthlyFeeHistoryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.monthlyfeehistorygrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.MonthlyFeeAmountDecHistory'
    ],

    title: 'История изменения Тарифа',
    store:'MonthlyFeeAmountDecHistory',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    text: 'Дата протокола',
                    flex: 1,
                    dataIndex: 'ProtocolDate',
                    xtype: 'datecolumn',
                    format: 'd.m.Y'
                },
                {
                    text: 'Значение',
                    flex: 1,
                    dataIndex: 'Value',
                    xtype: 'numbercolumn'
                },
                {
                    text: 'Дата действия с',
                    flex: 1,
                    dataIndex: 'From',
                    xtype: 'datecolumn',
                    format: 'd.m.Y'
                },
                {
                    text: 'Дата действия по',
                    flex: 1,
                    dataIndex: 'To',
                    xtype: 'datecolumn',
                    format: 'd.m.Y'
                },
                {
                    
                    text: 'Дата создания записи',
                    flex: 1,
                    dataIndex: 'ObjectCreateDate',
                    xtype: 'datecolumn',
                    format: 'd.m.Y'
                },
                {
                    text: 'Пользователь',
                    flex: 1,
                    dataIndex: 'UserName',
                    xtype: 'gridcolumn'
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