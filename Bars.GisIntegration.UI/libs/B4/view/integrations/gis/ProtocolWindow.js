Ext.define('B4.view.integrations.gis.ProtocolWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.protocolwindow',
    requires: [
        'B4.enums.MessageType',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    height: 600,
    title: 'Протокол',

    //признак того, что в окне должен отображаться протокол отправки данных
    //если false, отображается протокол подготовки данных
    sendDataProtocol: false,
    
    triggerId: undefined,

    initComponent: function() {
        var me = this,
           store;

        if (me.sendDataProtocol === true) {
            store = Ext.create('B4.store.SendDataProtocol');
        } else {
            store = Ext.create('B4.store.PrepareDataProtocol');
        }

        store.on('beforeload', me.beforeStoreLoad, me);

        var items = [
            {
                xtype: 'b4grid',
                store: store,
                name: 'ProtocolGrid',
                flex: 1,
                columns: me.getProtocolColumns(me.sendDataProtocol),
                plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
            }
        ];

        Ext.applyIf(me, {
            items: items
        });

        me.on('afterrender',
            function () {
                store.load();
            },
            me);

        me.callParent(arguments);
    },

    getProtocolColumns: function (sendDataProtocol) {

        var result = [
            {
                xtype: 'datecolumn',
                text: 'Время',
                format: 'd.m.Y H:i:s',
                dataIndex: 'DateTime',
                width: 130,
                filter: {
                    xtype: 'datefield',
                    operand: CondExpr.operands.eq
                }
            },
            {
                xtype: 'b4enumcolumn',
                enumName: 'B4.enums.MessageType',
                dataIndex: 'Type',
                text: 'Тип сообщения',
                width: 100,
                filter: true
            }           
        ];

        if (sendDataProtocol === true) {
            result.push({
                text: 'Наименование пакета',
                width: 150,
                sortable: true,
                dataIndex: 'PackageName',
                filter: {
                    xtype: 'textfield'
                }
            });
        }

        result.push({
            text: 'Текст сообщения',
            flex: 1,
            sortable: true,
            dataIndex: 'Text',
            filter: {
                xtype: 'textfield'
            }
        });

        return result;
    },

    beforeStoreLoad: function(store, operation, eOpts) {
        var me = this,
            params = operation.params;

        Ext.apply(params, {
            triggerId: me.triggerId,
            sendDataProtocol: me.sendDataProtocol
        });
    }
});
