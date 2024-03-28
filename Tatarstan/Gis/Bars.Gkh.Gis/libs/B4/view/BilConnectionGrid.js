Ext.define('B4.view.BilConnectionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.bilconnectiongrid',
    requires: [
        'B4.store.BilConnection',
        'B4.enums.ConnectionType',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Update'
    ],
    initComponent: function() {
        var me = this;
        Ext.applyIf(me,
        {
            store: Ext.create('B4.store.BilConnection', { Name: 'bilConnection' }),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ConnectionType',
                    text: 'Тип подключения',
                    flex: 1,
                    dataIndex: 'ConnectionType'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Подключение',
                    flex: 3,
                    dataIndex: 'Connection'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });
        me.callParent(arguments);
    }
});