Ext.define('B4.view.synclog.DetailsWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.synclogdetailswindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'fit' },
    minWidth: 500,
    minHeight: 250,
    height: 500,
    width: 1200,
    closable: true,
    maximizable: true,
    modal: true,
    title: 'Информация о действии',
    closeAction: 'destroy',
    constrain: true,

    requires: [
        'B4.form.ComboBox',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.RowExpander',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4grid',
                    border: false,
                    store: 'SyncActionDetails',
                    columnLines: true,
                    itemId: 'actionDetailsGrid',
                    columns: [
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            text: 'Время запроса',
                            dataIndex: 'RequestTime',
                            width: 150,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y H:i:s',
                            text: 'Время ответа',
                            dataIndex: 'ResponseTime',
                            width: 150,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y'
                            }
                        },
                        {
                            text: 'Детали операции',
                            width: 120,
                            dataIndex: 'Details',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Код ошибки',
                            width: 100,
                            dataIndex: 'ErrorCode',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Имя ошибки',
                            width: 300,
                            dataIndex: 'ErrorName',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Описание ошибки',
                            flex: 1,
                            dataIndex: 'ErrorDescription',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'actioncolumn',
                            hideable: false,
                            width: 20,
                            tooltip: "Скачать пакет",
                            icon: B4.Url.content('content/img/icons/disk.png'),
                            handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                                var scope = this.up('grid');
                                scope.fireEvent('rowaction', scope, 'getfile', rec);
                            }
                        },
                        {
                            xtype: 'actioncolumn',
                            hideable: false,
                            width: 20,
                            tooltip: "Повторить запрос",
                            icon: B4.Url.content('content/img/icons/arrow_refresh_small.png'),
                            handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                                var scope = this.up('grid');
                                scope.fireEvent('rowaction', scope, 'replay', rec);
                            }
                        }
                    ],
                    plugins: [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ],
                    viewConfig: {
                        loadMask: true
                    },
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'SyncActionDetails',
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    setActionName: function(name) {
        this.setTitle('Информация о действии ' + name);
        return this;
    }
});