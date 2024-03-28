Ext.define('B4.view.heatinputperiod.BoilerGrid', {
    extend: 'B4.ux.grid.Panel',
    border: false,
    alias: 'widget.boilerGrid',
    store: 'heatinputperiod.Boiler',
    columnLines: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Title',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Count',
                    flex: 1,
                    text: 'Всего'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Started',
                    flex: 1,
                    text: 'Запущено'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    flex: 1,
                    text: '%'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NotStarted',
                    flex: 1,
                    text: 'Не запущены'
                }
            ],
            dockedItems: [
                {
                    xtype: 'buttongroup',
                    columns: 1,
                    items: [
                        {
                            xtype: 'button',
                            text: 'Изменить',
                            itemId: 'boilerChangeBtn'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});