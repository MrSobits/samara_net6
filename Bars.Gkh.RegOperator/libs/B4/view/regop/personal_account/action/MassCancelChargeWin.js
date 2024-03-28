Ext.define('B4.view.regop.personal_account.action.MassCancelChargeWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.masscancelchargewin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod',
        'B4.view.regop.personal_account.action.MassCancelChargeGrid',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.form.SelectField'
    ],

    modal: true,
    closable: false,
    width: 800,
    minWidth: 700,
    height: 550,
    minHeight: 300,
    title: 'Отмена начислений и корректировок за периоды',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    accountOperationCode: null,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.ChargePeriod',
                                    selectionMode: 'MULTI',
                                    textProperty: 'Name',
                                    labelWidth: 60,
                                    width: 350,
                                    editable: false,
                                    emptyText: 'Все периоды',

                                    onSelectAll: function() {
                                        var me = this;

                                        me.setValue();
                                        me.updateDisplayedText('Все периоды');
                                        me.selectWindow.hide();
                                    },

                                    trigger2Cls: '',
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            text: 'Дата открытия',
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'StartDate',
                                            flex: 1,
                                            filter: { xtype: 'datefield' }
                                        },
                                        {
                                            text: 'Дата закрытия',
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'EndDate',
                                            flex: 1,
                                            filter: { xtype: 'datefield' }
                                        },
                                        {
                                            text: 'Состояние',
                                            dataIndex: 'IsClosed',
                                            flex: 1,
                                            renderer: function(value) {
                                                return value ? 'Закрыт' : 'Открыт';
                                            }
                                        }
                                    ],
                                    name: 'ChargePeriod',
                                    fieldLabel: 'Период',
                                    flex: 2
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина',
                                    allowBlank: true,
                                    flex: 3
                                },
                                {
                                    xtype: 'b4filefield',
                                    labelWidth: 160,
                                    name: 'Document',
                                    allowBlank: false,
                                    fieldLabel: 'Документ-основание',
                                    flex: 4
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'vbox',
                    name: 'CheckboxContainer',
                    defaults: {
                        xtype: 'container',
                        layout: 'hbox',
                        padding: '0 0 10 0',
                        defaults: {
                            xtype: 'checkbox',
                            fieldStyle: 'vertical-align: middle;',
                            margin: '-2 0 0 10',
                            checked: true,
                            listeners: { change: { fn: me.handleCheckboxChange } }
                        }
                    },
                    items: [
                        {
                            items: [
                                {
                                    boxLabel: 'Отменить начисления по базовому тарифу',
                                    action: 'CancelBaseTariffCharge',
                                    name: 'CheckCancelBaseTariffCharge',
                                    width: 275
                                },
                                {
                                    boxLabel: 'Отменить ручные корректировки по базовому тарифу',
                                    action: 'CancelBaseTariffChange',
                                    name: 'CheckCancelBaseTariffChange'
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    boxLabel: 'Отменить начисления по тарифу решения',
                                    action: 'CancelDecisionTariffCharge',
                                    name: 'CheckCancelDecisionTariffCharge',
                                    width: 275
                                },
                                {
                                    boxLabel: 'Отменить ручные корректировки по тарифу решения',
                                    action: 'CancelDecisionTariffChange',
                                    name: 'CheckCancelDecisionTariffChange'
                                }
                            ]
                        },
                        {
                            items: [
                                {
                                    boxLabel: 'Отменить пени',
                                    action: 'CancelPenaltyCharge',
                                    name: 'CheckCancelPenaltyCharge',
                                    width: 275
                                },
                                {
                                    boxLabel: 'Отменить ручные корректировки по пени',
                                    action: 'CancelPenaltyChange',
                                    name: 'CheckCancelPenaltyChange'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'masscancelchargegrid',
                    flex: 1,
                    accountOperationCode: me.accountOperationCode
                }
            ]
        });

        me.callParent(arguments);
    },

    handleCheckboxChange: function(chbox) {
        var window = chbox.up('window'),
            grid = window.down('masscancelchargegrid');

        grid.getStore().load();
    }
});