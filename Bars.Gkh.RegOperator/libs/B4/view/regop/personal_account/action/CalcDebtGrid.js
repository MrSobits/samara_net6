Ext.define('B4.view.regop.personal_account.action.CalcDebtGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.calcdebtgrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.store.regop.personal_account.CalcDebtDetail'
    ],

    cls: 'x-large-head',
    columnLines: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.CalcDebtDetail');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountOwnerName',
                    sortable: false,
                    width: 250,
                    text: 'Абонент'
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'OwnerType',
                    sortable: false,
                    text: 'Тип абонента',
                    enumName: 'B4.enums.regop.PersonalAccountOwnerType'
                },
                {
                    text: 'Доля собстенности',
                    dataIndex: 'AreaShare',
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Начислено по БТ',
                    dataIndex: 'ChargeBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение долга по БТ',
                    dataIndex: 'DistributionDebtBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Начислено по ТР',
                    dataIndex: 'ChargeDecTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение долга по ТР',
                    dataIndex: 'DistributionDebtDecTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Начислено по пени',
                    dataIndex: 'ChargePenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение долга по пени',
                    dataIndex: 'DistributionDebtPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Оплата по БТ',
                    dataIndex: 'PaymentBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение оплаты по БТ',
                    dataIndex: 'DistributionPayBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Оплата по ТР',
                    dataIndex: 'PaymentDecTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение оплаты по ТР',
                    dataIndex: 'DistributionPayDecTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Оплата по пени',
                    dataIndex: 'PaymentPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    renderer: function (val) {
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Распределение оплаты по пени',
                    dataIndex: 'DistributionPayPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.util.Format.currency(val || 0);
                    }
                },
                {
                    text: 'Исходящее сальдо по БТ',
                    dataIndex: 'SaldoOutBaseTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.isNumeric(val) ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    text: 'Исходящее сальдо по ТР',
                    dataIndex: 'SaldoOutDecisionTariff',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.isNumeric(val) ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    text: 'Исходящее сальдо по пени',
                    dataIndex: 'SaldoOutPenalty',
                    flex: 1,
                    sortable: false,
                    decimalSeparator: ',',
                    editor: {
                        xtype: 'numberfield'
                    },
                    renderer: function (val, meta) {
                        meta.style = 'background-color:#fefee0;';
                        return Ext.isNumeric(val) ? Ext.util.Format.currency(val) : '';
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать долг',
                                    textAlign: 'left',
                                    action: 'calcDebt'
                                }
                            ]
                        }
                    ]
                }
            ],

            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});