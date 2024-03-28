Ext.define('B4.view.regop.personal_account.action.CancelPenalty.AccountsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.cancelpenaltygrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        'B4.base.Store'
    ],

    cls: 'x-large-head',

    accountOperationCode: null,

    initComponent: function () {
        var me = this,
            cancelPaymentInfoStore = Ext.create('B4.base.Store', {
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
                    { name: 'Id' },
                    { name: 'Municipality' },
                    { name: 'Address' },
                    { name: 'PersonalAccountNum' },
                    { name: 'Penalty' },
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
                    dataIndex: 'Penalty',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Отменить начисления в размере',
                    dataIndex: 'CancellationSum',
                    flex: 1,
                    filter: {
                        xtype: 'gkhdecimalfield',
                        operand: CondExpr.operands.eq
                    },
                    editor: {
                        xtype: 'gkhdecimalfield',
                        minValue: 0.01,
                        decimalPrecision: 5
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
                            var penalty = e.record.get("Penalty");
                            if (e.value > penalty) {
                                Ext.Msg.alert({
                                    title: 'Ошибка ввода данных',
                                    msg: 'Невозможно отменить сумму больше начисленной!',
                                    buttons: Ext.Msg.OK,
                                    icon: Ext.window.MessageBox.ERROR
                                });
                                e.record.reject();
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