Ext.define('B4.view.regop.unacceptedcharges.PacketGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.store.regop.UnacceptedChargePacket',
        'B4.enums.PaymentOrChargePacketState'
    ],

    title: 'Журнал расчета начислений',

    alias: 'widget.unacceptedchargepacketgrid',

    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.UnacceptedChargePacket');

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'SINGLE' }),
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    text: 'Наименование',
                    dataIndex: 'Description',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата формирования',
                    dataIndex: 'CreateDate',
                    flex: 1,
                    format: 'd.m.Y H:i'
                },
                {
                    text: 'Состояние',
                    dataIndex: 'PacketState',
                    renderer: function(val) {
                        return B4.enums.PaymentOrChargePacketState.displayRenderer(val);
                    },
                    flex: 1
                },
                {
                    text : "Пользователь",
                    dataIndex: 'UserName',
                    flex: 1
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Действия',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Export',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузить начисление'
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);

    }
});