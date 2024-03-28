Ext.define('B4.view.administration.notify.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.notifygrid',

    requires: [
        'B4.store.administration.notify.Message',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.grid.FilterBar'
    ],

    title: 'Уведомление пользователей',

    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.notify.Message');

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Title',
                    flex: 1,
                    text: 'Заголовок',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ObjectEditDate',
                    flex: 1,
                    text: 'Дата',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsActual',
                    flex: 1,
                    text: 'Актуально',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    },
                    filter: {
                        xtype: 'b4dgridfilteryesno',
                        operator: 'eq'
                    }

                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                {
                    ptype: 'filterbar',
                    renderHidden: false,
                    showShowHideButton: false,
                    showClearAllButton: false
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
                            columns: 2,
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