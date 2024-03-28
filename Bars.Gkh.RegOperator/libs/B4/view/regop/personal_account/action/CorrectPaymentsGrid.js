Ext.define('B4.view.regop.personal_account.action.CorrectPaymentsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.correctpaymentsgrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store',
        'B4.enums.regop.WalletType',
        'B4.form.ComboBox',
        'Ext.grid.feature.Summary'
    ],

    cls: 'x-large-head',
    accountOperationCode: 'CorrectPaymentsOperation',

    initComponent: function () {
        var me = this,
            сorrectPaymentsInfoStore = Ext.create('B4.base.Store', {
                autoLoad: false,
                idProperty: 'Id',
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'BasePersonalAccount',
                    listAction: 'GetOperationDataForUI',
                    extraParams: {
                        operationCode: me.accountOperationCode
                    },
                    actionMethods: {
                        read: 'POST'
                    }
                },
                fields: [
                    { name: 'PaymentType' },
                    { name: 'Payment' },
                    { name: 'Debt' },
                    { name: 'TakeAmount' },
                    { name: 'EnrollAmount' }
                ]
            });

        Ext.applyIf(me, {
            store: сorrectPaymentsInfoStore,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaymentType',
                    width: 160,
                    sortable: false,
                    text: 'Тип оплат',
                    renderer: function (val) { return B4.enums.regop.WalletType.displayRenderer(val); },
                    summaryRenderer: function (val) {
                        return 'Итого:';
                    }

                },
                {
                    text: 'Сумма оплат, всего',
                    dataIndex: 'Payment',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Текущая сумма задолженности',
                    dataIndex: 'Debt',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Снять',
                    dataIndex: 'TakeAmount',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'gkhdecimalfield'
                    },
                    renderer: function (val) {
                        return val > 0 ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Зачислить',
                    dataIndex: 'EnrollAmount',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'gkhdecimalfield'
                    },
                    renderer: function (val) {
                        return val > 0 ? Ext.util.Format.currency(val) : '';
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                }
            ],

            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function (editor, e) {
                            if (e.record.data['TakeAmount'] && e.record.data['TakeAmount'] > e.record.data['Payment']) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Сумма снятия превышает сумму оплат!',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });

                                e.record.set(e.field, e.record.data['Payment']);
                            }

                            if (e.record.data['TakeAmount'] && e.record.data['TakeAmount'] < 0) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Сумма снятия должна быть больше 0!',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });

                                e.record.set(e.field, null);
                            }

                            if (e.record.data['EnrollAmount'] && e.record.data['EnrollAmount'] < 0) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Сумма зачисления должна быть больше 0!',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });

                                e.record.set(e.field, null);
                            }
                        }
                    }
                })
            ],
            features: [{
                ftype: 'summary'
            }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});