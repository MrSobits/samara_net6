Ext.define('B4.view.regop.CalculationProtocol', {
    extend: 'Ext.form.Panel',
    alias: 'widget.calculationprotocol',

    requires: [
        'Ext.grid.Panel',
        'Ext.panel.Panel',
        'B4.form.SelectField',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.AreaShare',
        'B4.store.regop.personal_account.BasePersonalAccount',
        'B4.store.regop.ChargePeriod',
        'B4.store.regop.ChargeParameterTrace',
        'B4.store.regop.PenaltyParameterTrace',
        'B4.store.regop.RecalcParameterTrace',
        'B4.store.regop.RecalcPenaltyTrace'
    ],
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll:true,
    title: 'Протокол расчета',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this,
            chargeStore = Ext.create('B4.store.regop.ChargeParameterTrace'),
            penaltyStore = Ext.create('B4.store.regop.PenaltyParameterTrace'),
            recalcStore = Ext.create('B4.store.regop.RecalcParameterTrace'),
            recalcPenaltyStore = Ext.create('B4.store.regop.RecalcPenaltyTrace');

        me.relayEvents(penaltyStore, ['load'], 'penaltystore.');
        me.relayEvents(recalcPenaltyStore, ['load'], 'recalcPenaltystore.');
        me.relayEvents(recalcStore, ['load'], 'recalcStore.');

        Ext.apply(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    bodyPadding: '10 5',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        xtype: 'b4selectfield',
                        labelAlign: 'right',
                        editable: false,
                        windowContainerSelector: '#'+me.getId(),
                        windowCfg: {
                            modal: true
                        }
                    },
                    items: [
                        {
                            name: 'PersonalAccount',
                            store: 'B4.store.regop.personal_account.BasePersonalAccount',
                            labelWidth: 200,
                            width: 400,
                            fieldLabel: 'Протокол расчета по ЛС',
                            isGetOnlyIdProperty: false,
                            columns: [
                                { maxWidth: 140, dataIndex: 'PersonalAccountNum', text: 'Номер счета', flex: 1, filter: { xtype: 'textfield'} },
                                { dataIndex: 'RoomAddress', text: 'Адрес', flex: 1, filter: { xtype: 'textfield' } },
                                { dataIndex: 'AccountOwner', text: 'Владелец счета', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            textProperty: 'PersonalAccountNum'
                        }, {
                            name: 'ChargePeriod',
                            store: 'B4.store.regop.ChargePeriod',
                            fieldLabel: 'за',
                            labelWidth: 23,
                            columns: [
                                { dataIndex: 'Name', text: 'Наименование', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'StartDate',
                                    text: 'Дата начала',
                                    maxWidth: 100,
                                    flex: 1,
                                    filter: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        operand: CondExpr.operands.eq
                                    }
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    dataIndex: 'EndDate',
                                    text: 'Дата окончания',
                                    maxWidth: 100,
                                    flex: 1,
                                    filter: {
                                        xtype: 'datefield',
                                        format: 'd.m.Y',
                                        operand: CondExpr.operands.eq
                                    }
                                }
                            ],
                            textProperty: 'Name',
                            isGetOnlyIdProperty: false,
                            width: 275
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'OpenDate',
                            fieldLabel: 'Дата открытия ЛС',
                            labelWidth: 120,
                            readOnly: true
                        },
                        {
                            xtype: 'button',
                            text: 'Обновить',
                            action: 'update'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '100%',
                    title: '<b>Расчет начислений</b>',
                    style: {
                        marginBottom: '10px'
                    },
                    items: [
                        {
                            xtype: 'gridpanel',
                            title: 'Формула расчета: (Площадь помещения*Тариф*Доля собственности*Количество дней с этими параметрами)/Количество дней в месяце',
                            type: 'charge',
                            store: chargeStore,
                            collapsible: true,
                            margin: '10 5 10 5',
                            features: [
                                {
                                    ftype: 'summary'
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Period',
                                    flex: 1,
                                    text: 'Период начисления взносов',
                                    sotrable: false,
                                    summaryRenderer: function() {
                                        return Ext.String.format('Итого:');
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Tariff',
                                    flex: 1,
                                    text: 'Тариф на КР',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return Ext.util.Format.currency(val);
                                    }
                                }, {
                                    xtype: 'areasharecolumn',
                                    dataIndex: 'Share',
                                    flex: 1,
                                    text: 'Доля собственности',
                                    sotrable: false,
                                    metaStyle: 'cursor:pointer;'
                                }, {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'RoomArea',
                                    flex: 1,
                                    text: 'Площадь помещения',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return val;
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'CountDays',
                                    flex: 1,
                                    text: 'Количество дней начисления',
                                    sotrable: false
                                }, {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Summary',
                                    flex: 1,
                                    text: 'Итого',
                                    sotrable: false,
                                    summaryType: 'sum',
                                    summaryRenderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val);
                                    },
                                    renderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val);
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '100%',
                    title: '<b>Расчет пени</b>',
                    style: {
                        marginBottom: '10px'
                    },
                    items: [
                        {
                            xtype: 'container',
                            itemId: 'CalcType',
                            margin: '0 0 0 10px',
                            height: 14,
                            tpl: '<div style="height:100px; font-size:11px; color: #0a4f84">Алгоритм расчета пени: {CalcType}</div>'
                        },
                        {
                            xtype: 'gridpanel',
                            title: 'Формула расчета: Задолженность*(Ставка рефинансирования/100)/300*Количество дней',
                            type: 'penalty',
                            store: penaltyStore,
                            collapsible: true,
                            margin: '10 5 10 5',
                            features: [
                                {
                                    ftype: 'summary'
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Period',
                                    flex: 1,
                                    text: 'Период начисления пени',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return Ext.String.format(val);
                                    },
                                    summaryRenderer: function() {
                                        return Ext.String.format('Итого:');
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'PenaltyDebt',
                                    flex: 1,
                                    text: 'Задолженность',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return Ext.util.Format.currency(val);
                                    }
                                }, {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Percentage',
                                    flex: 1,
                                    text: 'Ставка рефинансирования',
                                    sotrable: false
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'CountDays',
                                    flex: 1,
                                    text: 'Количество дней просрочки',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return val;
                                    }
                                }, {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Summary',
                                    flex: 1,
                                    text: 'Итого',
                                    sotrable: false,
                                    summaryType: 'sum',
                                    summaryRenderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val);
                                    },
                                    renderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val);
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '100%',
                    title: '<b>Перерасчет начислений</b>',
                    style: {
                        marginBottom: '10px'
                    },
                    items: [
                        {
                            xtype: 'container',
                            itemId: 'ReasonRecalc',
                            margin: '5 0 0 5px',
                            height: 14,
                            tpl: '<div style="height:100px; font-size:11px; color: #0a4f84">Причина перерасчета: {RecalcReason} {RecalcReasonDate} {RecalcReasonValue}</div>'
                        },
                        {
                            xtype: 'gridpanel',
                            title: 'Формула расчета: Начисление по новым параметрам - Фактическое начисление',
                            type: 'recalc',
                            store: recalcStore,
                            collapsible: true,
                            margin: '10 5 10 5',
                            features: [
                                {
                                    ftype: 'summary'
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Period',
                                    flex: 1,
                                    text: 'Период перерасчета начисления взносов',
                                    sotrable: false,
                                    summaryRenderer: function() {
                                        return Ext.String.format('Итого:');
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Tariff',
                                    flex: 1,
                                    text: 'Тариф',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return val;
                                    }
                                },
                                {
                                    xtype: 'areasharecolumn',
                                    dataIndex: 'Share',
                                    flex: 1,
                                    text: 'Доля собственности',
                                    sotrable: false,
                                    metaStyle: 'cursor:pointer;'
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'RoomArea',
                                    flex: 1,
                                    text: 'Площадь помещения',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return val;
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'CountDays',
                                    flex: 1,
                                    text: 'Количество дней начисления',
                                    sotrable: false
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Recalc_Charge',
                                    flex: 1,
                                    text: 'К начислению',
                                    sotrable: false,
                                    renderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val, null, 3).replace(/(\d+\D\d{2}[1-9]?)0+$/, '$1');
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Fact_Charge',
                                    flex: 1,
                                    text: 'Фактическое начисление',
                                    sotrable: false,
                                    renderer: function (val,meta) {
                                        meta.style = 'cursor:pointer;';
                                        val = val || 0;
                                        return Ext.util.Format.currency(val, null, 3).replace(/(\d+\D\d{2}[1-9]?)0+$/, '$1');
                                    }
                                },
                                {
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Summary',
                                    flex: 1,
                                    text: 'Итого',
                                    sotrable: false,
                                    filter: { xtype: 'textfield' },
                                    summaryType: 'sum',
                                    summaryRenderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val);
                                    },
                                    renderer: function(val) {
                                        val = val || 0;
                                        return Ext.util.Format.currency(val, null, 3).replace(/(\d+\D\d{2}[1-9]?)0+$/, '$1');
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    anchor: '100%',
                    title: '<b>Перерасчет пени</b>',
                    items: [
                        {
                            xtype: 'container',
                            itemId: 'ReasonPenaltyRecalc',
                            margin: '5 0 0 5px',
                            height: 14,
                            tpl: '<div style="height:100px; font-size:11px; color: #0a4f84">Причина перерасчета: {ReasonPenaltyRecalc}</div>'
                        },
                        {
                            xtype: 'container',
                            itemId: 'RecalcType',
                            margin: '5 0 0 5px',
                            height: 14,
                            tpl: '<div style="height:100px; font-size:11px; color: #0a4f84">Алгоритм перерасчета пени: {RecalcType}</div>'
                        },
                        {
                            xtype: 'gridpanel',
                            title: 'Формула расчета: Начислено пени по новым параметрам – Фактическое начисление пени',
                            type: 'recalcPenalty',
                            store: recalcPenaltyStore,
                            collapsible: true,
                            margin: '10 5 10 5',
                            features: [
                                {
                                    ftype: 'summary'
                                }
                            ],
                            columns: [
                                {
                                    xtype: 'b4editcolumn',
                                    scope: me
                                },
                                {
                                    dataIndex: 'PeriodName',
                                    flex: 1,
                                    text: 'Период перерасчета пени',
                                    sotrable: false,
                                    summaryRenderer: function() {
                                        return Ext.String.format('Итого:');
                                    }
                                },
                                {
                                    dataIndex: 'CountDays',
                                    flex: 1,
                                    text: 'Количество дней просрочки',
                                    sotrable: false
                                },
                                {
                                    dataIndex: 'RecalcPenalty',
                                    flex: 1,
                                    text: 'К начислению',
                                    sotrable: false,
                                    renderer: function(val) {
                                        return Ext.util.Format.currency(val || 0);
                                    }
                                },
                                {
                                    dataIndex: 'Penalty',
                                    flex: 1,
                                    text: 'Фактическое начисление',
                                    sotrable: false,
                                    renderer: function (val, meta) {
                                        meta.style = 'cursor:pointer;';
                                        return Ext.util.Format.currency(val || 0);
                                    }
                                },
                                {
                                    dataIndex: 'Summary',
                                    flex: 1,
                                    text: 'Итого',
                                    sotrable: false,
                                    filter: { xtype: 'textfield' },
                                    summaryType: 'sum',
                                    summaryRenderer: function(val) {
                                        return Ext.util.Format.currency(val || 0);
                                    },
                                    renderer: function(val) {
                                        return Ext.util.Format.currency(val || 0);
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});