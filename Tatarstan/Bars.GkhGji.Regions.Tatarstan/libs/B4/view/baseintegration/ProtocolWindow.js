Ext.define('B4.view.baseintegration.ProtocolWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.enums.MessageType',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.baseintegration.TriggerProtocolDataRecord'
    ],

    alias: 'widget.integrationprotocolwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 800,
    height: 500,
    title: 'Протокол',
    maximizable: true,
    closeAction: 'destroy',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.baseintegration.TriggerProtocolDataRecord');

        Ext.applyIf(me, {
            defaults: {
                anchor: '0 0'
            },
            items: [
                {
                    xtype: 'gridpanel',
                    columnLines: true,
                    store: store,
                    columns: [
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'DateTime',
                            width: 130,
                            text: 'Дата',
                            format: 'd.m.Y H:i:s',
                            filter: {
                                xtype: 'datefield'
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'Type',
                            flex: 1,
                            text: 'Тип сообщения',
                            enumName: 'B4.enums.MessageType',
                            filter: true
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Text',
                            flex: 1.5,
                            text: 'Текст сообщения',
                            filter: {
                                xtype: 'textfield'
                            }
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
                                    items: [
                                        {
                                            xtype: 'b4updatebutton',
                                            listeners: {
                                                click: function () {
                                                    store.load();
                                                }
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
                }
            ]
        });

        me.callParent(arguments);
    }
});