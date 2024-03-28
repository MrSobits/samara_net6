Ext.define('B4.view.eaisintegration.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.eaisintegrationgrid',

    requires: [
        'B4.store.appealcits.AppealCitsTransferResult',
        'B4.ux.grid.Panel',
        'B4.enums.AppealCitsTransferType',
        'B4.enums.AppealCitsTransferStatus',
        'B4.view.Control.GkhFileColumn',

        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.File',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.grid.FilterBar',
        'B4.ux.grid.plugin.HeaderFilters',
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.appealcits.AppealCitsTransferResult');

        me.addEvents(
            'gotoappeal',
            'resendappeal'
        );

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Переход к обращению',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        me.fireEvent('gotoappeal', rec);
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.AppealCitsTransferType',
                    filter: true,
                    header: 'Действие',
                    dataIndex: 'Type',
                    width: 170,
                },
                {
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    dataIndex: 'AppealCitsNumber',
                    text: 'Номер документа обращения'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'StartDate',
                    text: 'Дата и время начала запуска'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    dataIndex: 'EndDate',
                    text: 'Дата и время окончания запуска'
                },
                {
                    header: 'Пользователь',
                    dataIndex: 'User',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.AppealCitsTransferStatus',
                    filter: true,
                    header: 'Статус',
                    dataIndex: 'Status',
                    width: 170,
                },
                {
                    xtype: 'filecolumn',
                    dataIndex: 'LogFile',
                    text: 'Лог'
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Перезапустить отправку данных',
                    icon: B4.Url.content('content/img/icons/arrow_refresh.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        me.fireEvent('resendappeal', rec);
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        var me = this;
                                        me.up('grid').getStore().load();
                                    }
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