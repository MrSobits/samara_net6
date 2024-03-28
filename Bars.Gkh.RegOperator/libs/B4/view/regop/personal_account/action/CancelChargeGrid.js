Ext.define('B4.view.regop.personal_account.action.CancelChargeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.cancelchargegrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store'
    ],

    cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            cancelPaymentInfoStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasePersonalAccount',
                    listAction: 'GetAccountChargeInfoInPeriod'
                },
                fields: [
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'Address' },
                    { name: 'PersonalAccountNum' },
                    { name: 'ChargeSum' },
                    { name: 'CancellationSum' }
                ]
            });

        Ext.applyIf(me, {
            store: cancelPaymentInfoStore,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер ЛС',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Сумма начислений за период',
                    dataIndex: 'ChargeSum',
                    flex: 1,
                    decimalSeparator: ',',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Отменить начисления в размере',
                    dataIndex: 'CancellationSum',
                    flex: 1,
                    decimalSeparator: ',',
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.01
                    },
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                }
            ],

            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function (editor, e) {
                            if ((e.record.data["CancellationSum"] && e.record.data["ChargeSum"])
                                && e.record.data["CancellationSum"] > e.record.data["ChargeSum"]) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Невозможно отменить сумму больше начисленной!',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });

                                e.record.set(e.field, e.record.data["ChargeSum"]);
                            }
                        }
                    }
                })
            ],

            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});