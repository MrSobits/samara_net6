Ext.define('B4.view.regop.unacceptedcharges.ChargesGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.store.regop.UnacceptedCharge',
        'B4.grid.feature.Summary',
        'B4.enums.PaymentOrChargePacketState',
        'B4.enums.CrFundFormationDecisionType',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum'
    ],

    //title: 'Неподтвержденные начисления',

    alias: 'widget.unacceptedchargegrid',
    //cls: 'x-large-head',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PersonalAccountCharge');

        Ext.apply(me, {
            store: store,
            columns: [
               {
                   text: 'Счет',
                   dataIndex: 'AccountNum',
                   flex: 1,
                   filter: { xtype: 'textfield' }
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountState',
                    menuText: 'Статус ЛС',
                    text: 'Статус ЛС',
                    width: 150,
                    renderer: function (val) {
                        return val && val.Name ? val.Name : '';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    scope: this
                },
               {

                   xtype: 'b4enumcolumn',
                   dataIndex: 'AccountFormationVariant',
                   enumName: 'B4.enums.CrFundFormationDecisionType',
                   text: 'Способ формирования фонда',
                   flex: 1,
                   filter: true
               },
               {
                   text: 'Номер расчетного счета',
                   dataIndex: 'ContragentAccountNumber',
                   flex: 1,
                   sortable: false
               },
               {
                   text: 'Начислено всего',
                   dataIndex: 'Charge',
                   xtype: 'numbercolumn',
                   flex: 1,
                   filter: {
                       xtype: 'numberfield',
                       hideTrigger: true,
                       operand: CondExpr.operands.eq
                   },
                   summaryType: 'sum',
                   summaryRenderer: function (val) {
                       return Ext.util.Format.currency(val);
                   }
                },
                {
                    text: 'Начислено по базовому тарифу',
                    dataIndex: 'ChargeBaseTariff',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Начислено по тарифу решения',
                    dataIndex: 'OverPlus',
                    xtype: 'numbercolumn',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Перерасчет по базовому тарифу',
                    dataIndex: 'RecalcByBaseTariff',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Перерасчет по тарифу решения',
                    dataIndex: 'RecalcByDecisionTariff',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Пени',
                    dataIndex: 'Penalty',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'numbercolumn',
                    text: 'Перерасчет пени',
                    dataIndex: 'RecalcPenalty',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    text: 'Примечание',
                    dataIndex: 'Description',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'actioncolumn',
                    icon: B4.Url.content('content/img/icons/pencil_go.png'),
                    width: 20,
                    sortable: false,
                    tooltip: 'Перейти в карточку ЛС',
                    handler: function (grid, rowIndex, colIndex, el, e, rec) {
                        Ext.History.add(Ext.String.format('personal_acc_details/{0}', rec.get('AccountId')));
                    },
                    scope: me
                }
            ],

            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            features: [{ ftype: 'b4_summary' }],

            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);

    }
});