Ext.define('B4.view.regop.RecalcPenaltyTraceDetailWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.recalcpenaltytracedetwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.base.Store'
    ],

    modal: true,
    width: 950,
    minHeight: 246,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    { name: 'Period' },
                    { name: 'CountDays' },
                    { name: 'RecalcPenalty' },
                    { name: 'Payment' },
                    { name: 'Reason' },
                    { name: 'PaymentDate' },
                    { name: 'Percentage' },
                    { name: 'Debt' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'PersonalAccountPeriodSummary',
                    listAction: 'ListRecalcPenaltyTraceDetail'
                },
                autoLoad: false
            });

        me.relayEvents(store, ['load'], 'store.');

        Ext.applyIf(me, {
            items: [{
                xtype: 'gridpanel',
                cls: 'x-large-head',
                height: 'auto',
                border: false,
                header: false,
                store: store,
                columnLines: true,
                enableColumnHide: false,
                listeners: {
                    beforeitemmouseenter: function () {
                        return false;
                    }
                },
                columns: [
                    {
                        text: 'Период перерасчета пени',
                        dataIndex: 'Period',
                        minWidth: 140
                    },
                    {
                        text: 'Причина разделения периода',
                        dataIndex: 'Reason',
                        flex: 1,
                        renderer: function (value, metaData) {
                            metaData.style = "white-space: normal;";
                            return value;
                        }
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Задолженность',
                        dataIndex: 'Debt',
                        format: '0.00',
                        width: 110
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Ставка рефинансирования',
                        dataIndex: 'Percentage',
                        format: '0.00',
                        width: 110
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'Кол-во дней просрочки',
                        dataIndex: 'CountDays',
                        format: '0',
                        width: 110
                    },
                    {
                        xtype: 'numbercolumn',
                        text: 'К начислению',
                        dataIndex: 'RecalcPenalty',
                        format: '0.00',
                        width: 110
                    }
                ]
            },
            {
                xtype: 'container',
                padding: 2,
                style: 'font: 10px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 14px;',
                html: '<div style="padding: 5px;">' +
                        '<div>Формула расчета задолженности:</div>' +
                        '<div style="margin-top: 10px;">' +
                            '<div>[ Задолженность ] = [ Входящее сальдо - Сальдо по пени - Оплаты - Перерасчет начислений + Изменение сальдо + Начисление ],</div>' +
                            '<div>где [ Входящее сальдо ] =  [ *Входящее сальдо за - 2 предыдущих закрытых периода ] или [ **Входящее сальдо за предыдущий закрытый период ] </div>' +
                            '<div style="margin-left: 22px;">' +
                                '<div>[ Сальдо по пени ] = [ Начисление пени +/- Перерасчет пени ],</div>' +
                                '<div>[ Оплаты ] = [ Оплаты, поступившие с даты начала закрытого периода до даты установленного срока оплаты ],</div>' +
                                '<div>[ Перерасчет начислений ] = [ Отрицательный перерасчет начислений  в предыдущих и текущем периодах ],</div>' +
                                '<div>[ Изменение сальдо ] = [ Отмена оплаты + Возврат средств + Слияние абонентов, совершенные в предыдущих и текущем периодах ],</div>' +
                                '<div>[ Начисление ] = [ Начисление за -2 предыдущий закрытый период (только для классического алгоритма) ]</div>' +
                            '</div>' +
                        '</div>' +
                        '<div style="margin-top: 10px;">' +
                            '<div>* - в случае расчета периодов после 1.07.2016</div>' +
                            '<div>** - в случае расчета периодов до 1.07.2016</div>' +
                        '</div>' +
                     '</div>'
            }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(btn) {
                                            btn.up('window').close();
                                        }
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