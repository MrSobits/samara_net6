Ext.define('B4.view.GeneralStateHistory.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.generalstatehistorygrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            store: this.store,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ChangeDate',
                    flex: 1,
                    text: 'Дата изменения',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UserName',
                    flex: 1,
                    text: 'Пользователь',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StartState',
                    flex: 1,
                    text: 'Старый статус',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FinalState',
                    flex: 1,
                    text: 'Новый статус',
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
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