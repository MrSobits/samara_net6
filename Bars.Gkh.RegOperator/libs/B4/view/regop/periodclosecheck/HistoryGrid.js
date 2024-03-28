Ext.define('B4.view.regop.periodclosecheck.HistoryGrid',
{
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.periodclosecheckhistorygrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.period.CloseCheckHistory');

        Ext.applyIf(me,
        {
            store: store,
            columns: [
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    text: 'Дата и время установки',
                    dataIndex: 'ChangeDate',
                    width: 200
                },
                {
                    text: 'Кто установил',
                    dataIndex: 'User',
                    flex: 1,
                    renderer: function(val) {
                        return val.Name || val.Login;
                    }
                },
                {
                    text: 'Обязательность',
                    dataIndex: 'IsCritical',
                    renderer: function(value) {
                        return value ? 'Да' : 'Нет';
                    },
                    flex: 1
                }
            ],

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);

    }
});