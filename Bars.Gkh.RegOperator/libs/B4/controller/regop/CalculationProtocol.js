Ext.define('B4.controller.regop.CalculationProtocol', {
    extend: 'B4.base.Controller',

    requires: ['B4.Ajax',
               'B4.Url',
               'B4.enums.TariffSourceType'],

    stores: ['regop.ChargeParameterTrace',
             'regop.PenaltyParameterTrace',
             'regop.RecalcParameterTrace'],

    views: ['regop.CalculationProtocol',
            'regop.RecalcPenaltyTraceDetailWindow'],

    models:[
        'regop.ChargePeriod',
        'regop.personal_account.BasePersonalAccount'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'calculationprotocol'
        }
    ],

    init: function () {
        var me = this,
            actions;

        actions = {
            'calculationprotocol': {
                'recalcStore.load': {
                    fn: function(st) {
                        var me = this,
                            view = me.getCmpInContext('calculationprotocol');

                        if (st.getCount() > 0) {
                            var recalcReason = st.getAt(0).get('RecalcReason'),
                                reasonDate = st.getAt(0).get('RecalcReasonDate'),
                                reasonValue = st.getAt(0).get('RecalcReasonValue'),
                                value= '';

                            if (reasonValue) {
                                value = 'на новое значение ' + reasonValue;
                            }

                            if (recalcReason) {
                                view.down('#ReasonRecalc')
                                    .update({ RecalcReason: recalcReason, RecalcReasonDate: reasonDate, RecalcReasonValue: value });
                            } else {
                                view.down('#ReasonRecalc').setVisible(false);
                            }
                        } else {
                            view.down('#ReasonRecalc').setVisible(false);
                        }
                    },
                    scope: me
                },

                'penaltystore.load': {
                    fn: function(st) {
                        var me = this,
                            view = me.getMainView() || Ext.widget('calculationprotocol'),
                            calcDecision;
                        if (st.getCount() > 0 && st.getAt(0).get('CalcType')) {
                            if (st.getAt(0).get('CalcDecision') == true) {
                                calcDecision =
                                    ' (учитывая начисления по базовому тарифу + начисления по тарифу решения)';
                            } else {
                                calcDecision = ' (учитывая только начисления по базовому тарифу)';
                            }

                            var calcType = st.getAt(0).get('CalcType') + calcDecision;
                            view.down('#CalcType').update({ CalcType: calcType });
                        } else {
                            view.down('#CalcType').setVisible(false);
                        }
                    },
                    scope: me

                },

                'recalcPenaltystore.load': {
                    fn: function(st) {
                        var me = this,
                            view = me.getCmpInContext('calculationprotocol'),
                            calcDecision,
                            type;

                        if (st.getCount() > 0) {
                            if (st.getAt(0).get('CalcDecision') == true) {
                                calcDecision =
                                    ' (учитывая начисления по базовому тарифу + начисления по тарифу решения)';
                            } else {
                                calcDecision = ' (учитывая только начисления по базовому тарифу)';
                            }

                            type = st.getAt(0).get('CalcType');
                            if (type) {
                                var calcType = type + calcDecision;
                                view.down('#RecalcType').update({ RecalcType: calcType });
                            } else {
                                view.down('#RecalcType').setVisible(false);
                            }

                            var reason = st.getAt(0).get('RecalcReason');
                            var reasonDate = st.getAt(0).get('RecalcDate');

                            if (reason) {
                                view.down('#ReasonPenaltyRecalc')
                                    .update({ ReasonPenaltyRecalc: reason + ' ' + reasonDate });
                            } else {
                                view.down('#ReasonPenaltyRecalc').setVisible(false);
                            }
                        } else {
                            view.down('#RecalcType').setVisible(false);
                            view.down('#ReasonPenaltyRecalc').setVisible(false);
                        }
                    },
                    scope: me
                }

            },

            'calculationprotocol [name=PersonalAccount]': {
                change: {
                    fn: me.onChangeMainParams,
                    scope: me
                },
                beforeclear: {
                    fn: me.onClearPersAcc,
                    scope: me
                }
            },
            'calculationprotocol [name=ChargePeriod]': {
                change: {
                    fn: me.onChangeMainParams,
                    scope: me
                }
            },
            'calculationprotocol [action=update]': {
                click: {
                    fn: me.onChangeMainParams,
                    scope: me
                }
            },
            'calculationprotocol gridpanel[type=recalcPenalty] b4editcolumn': {
                'click': me.onEditRow
            },
            'calculationprotocol gridpanel[type=penalty]': {
                'cellclick': me.onCellClickPenalty
            },
            'calculationprotocol gridpanel[type=recalcPenalty]': {
                'cellclick': me.onCellClickRecalcPenalty
            },
            'calculationprotocol gridpanel[type=charge]': {
                'cellclick': me.onCellClickCharge
            },
            'calculationprotocol gridpanel[type=recalc]': {
                'cellclick': me.onCellClickRecalc
            }
        }

        me.control(actions);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('calculationprotocol');
        me.bindContext(view);
        me.application.deployView(view);
    },
    
    onChangeMainParams: function(cmp) {
        var form = cmp.up('calculationprotocol'),
            sfAccount = form.down('[name=PersonalAccount]'),
            sfPeriod = form.down('[name=ChargePeriod]'),
            dateOpenFld = form.down('[name=OpenDate]'),
            account = sfAccount.getValue(),
            accountId = Ext.isObject(account) ? account.Id : account,
            period = sfPeriod.getValue(),
            periodId = Ext.isObject(period) ? period.Id : period,
            chargeStore = form.down('[type=charge]').getStore(),
            penaltyStore = form.down('[type=penalty]').getStore(),
            recalcStore = form.down('[type=recalc]').getStore(),
            recalcPenaltyStore = form.down('[type=recalcPenalty]').getStore();
        
        chargeStore.clearFilter(true);
        penaltyStore.clearFilter(true);
        recalcStore.clearFilter(true);
        recalcPenaltyStore.clearFilter(true);

        if (Ext.isObject(account)) {
            dateOpenFld.setValue(account.OpenDate);
        }

        if (accountId && periodId) {
            chargeStore.filter([{ property: 'accountId', value: accountId }, { property: 'periodId', value: periodId }]);
            penaltyStore.filter([{ property: 'accountId', value: accountId }, { property: 'periodId', value: periodId }]);
            recalcStore.filter([{ property: 'accountId', value: accountId }, { property: 'periodId', value: periodId }]);
            recalcPenaltyStore.filter([{ property: 'accountId', value: accountId }, { property: 'periodId', value: periodId }]);
        } else {
            chargeStore.removeAll();
            penaltyStore.removeAll();
            recalcStore.removeAll();
            recalcPenaltyStore.removeAll();
        }
    },
    
    onClearPersAcc: function (fld) {
        fld.up('form').up('[name=OpenDate]').setValue(null);
    },

    setPeriod: function(form, period) {
        var dfDateStart = form.down('[name=DateStart]'),
            dfDateEnd = form.down('[name=DateEnd]'),
            tfsPeriodName = form.query('[name=PeriodName]'),
            periodDateStart = null,
            periodDateEnd = null,
            periodName = null;
        
        if (!Ext.isEmpty(period)) {
            periodDateStart = period.StartDate;
            periodDateEnd = period.EndDate;
            periodName = period.Name;
        }

        dfDateStart.setValue(periodDateStart);
        dfDateEnd.setValue(periodDateEnd);

        Ext.each(tfsPeriodName, function(field) {
            field.setValue(periodName);
        });
    },
    
    setAccountNumber: function(form, accountName) {
        var tfsAccountNumber = form.query('[name=AccountNumber]');

        Ext.each(tfsAccountNumber, function(field) {
            field.setValue(accountName);
        });
    },
    
    clearForm: function (summaryForm) {
        var fields = summaryForm.query('numberfield');

        Ext.each(fields, function(f) {
            f.setValue(null);
        });
    },
    
    show: function (periodId, accountId) {
        var me = this, sfAccount, sfPeriod, accountModel, periodModel,
            view = me.getMainView() || Ext.widget('calculationprotocol');
        
        me.bindContext(view);
        me.application.deployView(view);

        sfAccount = view.down('[name=PersonalAccount]');
        sfPeriod = view.down('[name=ChargePeriod]');

        periodModel = me.getModel('regop.ChargePeriod');

        periodModel.load(periodId, {
            success: function(rec) {
                sfPeriod.setValue(rec.data);
            },
            scope: me
        });

        accountModel = me.getModel('regop.personal_account.BasePersonalAccount');

        accountModel.load(accountId, {
            success: function (rec) {
                sfAccount.setValue(rec.data);
            },
            scope: me
        });
    },

    onEditRow: function(p1, p2, p3, p4, p5, rec) {
        var me = this,
           win = me.getCmpInContext('recalcpenaltytracedetwin'),
           store,
           activeTab;

        if (!win) {

            activeTab = B4.getBody().getActiveTab();

            win = Ext.create('B4.view.regop.RecalcPenaltyTraceDetailWindow', {
                constrain: true,
                closeAction: 'destroy',
                title: 'Формула перерасчета пени: Задолженность*Ставка рефенансирования/Коэффициент ставки рефенансирования*Количество дней просрочки',
                ctxKey: me.getCurrentContextKey(),
                modal: true
            });

            activeTab.add(win);
        }

        store = win.down('grid').getStore();

        store.on('load', function () {
            if (store.getCount() == 0) {
                Ext.Msg.alert('Ошибка', 'Нет данных для отображения');
            } else {
                win.show();
            }
        }, me, { single: true });

        store.clearFilter(true);

        store.filter([
            { property: 'guid', value: rec.get('CalculationGuid') },
            { property: 'dateStart', value: rec.get('DateStart') },
            { property: 'dateEnd', value: rec.get('DateEnd') }
        ]);
    },

    onCellClickPenalty: function (p1, td, cellIndex, rec, p4, p5, eOpts) {
        var text;
        var payment = rec.get('Payment');
        var tip = Ext.create('Ext.tip.ToolTip', {
            dismissDelay: 0,
            hide: function () {
                var me = this;
                me.clearTimer('dismiss');
                me.lastActive = new Date();
                if (me.anchorEl) {
                    me.anchorEl.hide();
                }
                me.getEl().switchOff({
                    easing: 'easeOut',
                    duration: 600,
                    remove: false,
                    useDisplay: false
                });
                me.callParent(arguments);
                delete me.triggerElement;
            }
        });

        switch (cellIndex) {
            case 0:
                {
                    text = rec.get('Reason');
                }
                break;
            case 1:
                {
                    text = '[Задолженность] = [Входящее сальдо] - [Сальдо по пени] - [Оплаты] - [Перерасчет начислений] + [Изменение сальдо] + [Начисление],<br>' +
                        'где [Входящее сальдо] = [*Входящее сальдо за - 2 предыдущих закрытых периода] или [**Входящее сальдо за предыдущий закрытый период]  <br>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Сальдо по пени] = [Начисление пени +/- Перерасчет пени], <br>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Оплаты] = [Оплаты, поступившие с даты начала закрытого периода до даты установленного срока оплаты], <br>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Перерасчет начислений] = [Отрицательный перерасчет начислений в предыдущих и текущем периодах], <br>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Изменение сальдо] = [Отмена оплаты + Возврат средств + Слияние абонентов, совершенные  в предыдущих и текущем периодах], <br>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Начисление] = [Начисление за -2 предыдущий закрытый период (только для классического алгоритма)] <br><br>' +
                        '* - в случае расчета периодов после 1.07.2016 <br>' +
                        '** - в случае расчета периодов до 1.07.2016';

                    tip.maxWidth = 710;
                    tip.width = 710;
                }
                break;
            case 3:
                {
                    var dateStart = new Date(rec.get('DateStart'));
                    var startDate = Ext.Date.format(dateStart, 'd.m.Y');
                    var endDate = Ext.Date.format(new Date(rec.get('DateEnd')), 'd.m.Y');
                    var penaltyDate = Ext.Date.format(new Date(rec.get('PenaltyDate')), 'd.m.Y');

                    if (payment !== 0) {
                            text = '[Количество дней просрочки] = [[' + endDate + '] - [' + startDate + '] + 1],<br>' +
                                'где [' + endDate + '] - [Дата поступления оплаты],<br>' +
                                '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[' + penaltyDate + '] - [Установленный срок оплаты с учетом допустимой просрочки] - ' +
                                'Данные из [Единые настройки приложения] - [Параметры начисления пени] - [Установленный срок оплаты] + [Допустимая просрочка]';
                        } else {
                            text = '[Количество дней просрочки] = [[' + endDate + '] - [' + startDate + '] + 1],<br>' +
                               'где [' + penaltyDate + '] - [Установленный срок оплаты с учетом допустимой просрочки] - ' +
                               'Данные из [Единые настройки приложения] - [Параметры начисления пени] - [Установленный срок оплаты] + [Допустимая просрочка]';
                        }
                    
                    tip.maxWidth = 350;
                    tip.width = 350;
                }
                break;
        }

        if (text) {
            tip.html = text;
            tip.showAt(eOpts.xy);
        }
    },

    onCellClickRecalcPenalty: function (p1, td, cellIndex, rec, p4, p5, eOpts) {
        var calcType = rec.get('CalcType');
        if (cellIndex == 4 && calcType != 'None') {
            var penalty = rec.get('Penalty').toFixed(2),
                recalcHistory = rec.get('RecalcHistory'),
                currentPenalty = rec.get('CurrentPenalty'),
                periodName = rec.get('PeriodName'),
                text = '[' + penalty + '] = [' + currentPenalty + ']';

            Ext.Array.each(recalcHistory, function(r) {
                text += ' + [' + r.RecalcSum.toFixed(2) + ']';
            });

            text += ',<br> где ' + '[' + currentPenalty + '] = [' + 'начисление пени за ' + periodName + '] ';

            if (recalcHistory) {
                Ext.Array.each(recalcHistory, function(r) {
                    text += ',<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[' + r.RecalcSum.toFixed(2) + '] = [Перерасчет пени в ' + r.CalcPeriod + ']';
                });
            }

            var tip = Ext.create('Ext.tip.ToolTip', {
                dismissDelay: 0,
                html: text,
                hide: function() {
                    var me = this;
                    me.clearTimer('dismiss');
                    me.lastActive = new Date();
                    if (me.anchorEl) {
                        me.anchorEl.hide();
                    }
                    me.getEl().switchOff({
                        easing: 'easeOut',
                        duration: 600,
                        remove: false,
                        useDisplay: false
                    });
                    me.callParent(arguments);
                    delete me.triggerElement;
                }
            });

            tip.showAt(eOpts.xy);
        }
    },

    onCellClickCharge: function (p1, td, cellIndex, rec, p4, p5, eOpts) {
        var text,
            dataIndex = p1.getGridColumns()[cellIndex].dataIndex;

        switch (dataIndex) {
            case 'Tariff':
                {
                    var tariffSource = rec.get('TariffSource');
                    if (tariffSource) {
                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaysizeByType) {
                            text = 'Данные из [Региональный фонд] - [Размеры взносов на КР]' +
                                '<br>[Муниципальный район] = [' +  tariffSource.Municipality + ']' +
                                '<br>[Исключение по типу дома] = [' +  tariffSource.RoType + ']';
                        }
               
                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaysizeByMu) {
                            text = 'Данные из [Региональный фонд] - [Размеры взносов на КР]' +
                                '<br>[Муниципальный район] = [' + tariffSource.Municipality + ']';
                        }
               
                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.EntranceSize) {
                            text = 'Данные из [Жилой дом] - [Сведения о подъездах] - [Исключение по подъезду]';
                        }
               
                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaySizeByProtocol) {
                            text = 'Данные из [Жилой дом] - [Протоколы решений] - [Принятое решение]' +
                                '<br>[Номер протокола] = [' + tariffSource.ProtocolNum + ']' +
                                '<br>[Дата протокола] = [' + Ext.Date.format(new Date(tariffSource.ProtocolDate), 'd.m.Y') + ']';
                        }
                    }
                }
                break;

            case 'Share':
                {
                    var date = rec.get('DateActualShare');
                    if (date) {
                        var dateActualShare = Ext.Date.format(new Date(rec.get('DateActualShare')), 'd.m.Y');

                        text = 'Данные из [Реестр абонентов] - [Карточка абонента] - ' +
                            '<br>[Сведения о помещениях]' +
                            '<br>[Дата действия параметра] = [' + dateActualShare + ']';
                    }
                }
                break;

            case 'RoomArea':
                {
                    var dateActualArea = Ext.Date.format(new Date(rec.get('DateActualArea')), 'd.m.Y');

                    text = 'Данные из [Жилой дом] - [Сведения о помещениях] - ' +
                        '<br>[Дата действия параметра] = [' + dateActualArea + ']';
                }
                break;
        }

        if (text) {
            var tip = Ext.create('Ext.tip.ToolTip',
            {
                dismissDelay: 0,
                maxWidth: 320,
                width: 320,
                html: text,
                hide: function() {
                    var me = this;
                    me.clearTimer('dismiss');
                    me.lastActive = new Date();
                    if (me.anchorEl) {
                        me.anchorEl.hide();
                    }
                    me.getEl().switchOff({
                        easing: 'easeOut',
                        duration: 600,
                        remove: false,
                        useDisplay: false
                    });
                    me.callParent(arguments);
                    delete me.triggerElement;
                }
            });

            tip.showAt(eOpts.xy);
        }
    },

    onCellClickRecalc: function (p1, td, cellIndex, rec, p4, p5, eOpts) {
        var text,
            width = 320,
            dataIndex = p1.getGridColumns()[cellIndex].dataIndex;

        switch (dataIndex) {
            case 'Tariff':
                {
                    var tariffSource = rec.get('TariffSource');
                    if (tariffSource) {
                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaysizeByType) {
                            text = 'Данные из [Региональный фонд] - [Размеры взносов на КР]' +
                                '<br>[Муниципальный район] = [' + tariffSource.Municipality + ']' +
                                '<br>[Исключение по типу дома] = [' + tariffSource.RoType + ']';
                        }

                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaysizeByMu) {
                            text = 'Данные из [Региональный фонд] - [Размеры взносов на КР]' +
                                '<br>[Муниципальный район] = [' + tariffSource.Municipality + ']';
                        }

                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.EntranceSize) {
                            text = 'Данные из [Жилой дом] - [Сведения о подъездах] - [Исключение по подъезду]';
                        }

                        if (tariffSource.TariffSourceType == B4.enums.TariffSourceType.PaySizeByProtocol) {
                            text = 'Данные из [Жилой дом] - [Протоколы решений] - [Принятое решение]' +
                                '<br>[Номер протокола] = [' + tariffSource.ProtocolNum + ']' +
                                '<br>[Дата протокола] = [' + Ext.Date.format(new Date(tariffSource.ProtocolDate), 'd.m.Y') + ']';
                        }
                    }
                }
                break;

            case 'Share':
                {
                    var date = rec.get('DateActualShare');
                    if (date) {
                        var dateActualShare = Ext.Date.format(new Date(rec.get('DateActualShare')), 'd.m.Y');

                        text = 'Данные из [Реестр абонентов] - [Карточка абонента] - ' +
                            '<br>[Сведения о помещениях]' +
                            '<br>[Дата действия параметра] = [' + dateActualShare + ']';
                    }
                }
                break;

            case 'RoomArea':
                {
                    var dateActualArea = Ext.Date.format(new Date(rec.get('DateActualArea')), 'd.m.Y');

                    text = 'Данные из [Жилой дом] - [Сведения о помещениях] - ' +
                        '<br>[Дата действия параметра] = [' + dateActualArea + ']';
                }
                break;

            case 'Fact_Charge':
                {
                    var factCharge = rec.get('Fact_Charge').toFixed(2),
                        recalcHistory = rec.get('RecalcHistory'),
                        currentCharge = rec.get('CurrentCharge'),
                        period = rec.get('Period'),
                        periodName = rec.get('PeriodName'),
                        countDays = rec.get('CountDays'),
                        countDaysMonth = rec.get('CountDaysMonth');

                    width = 420;

                    if (countDays !== countDaysMonth) {
                        text = '[' + factCharge + '] = ([' + currentCharge + ']';

                        Ext.Array.each(recalcHistory,
                            function(r) {
                                text += ' + [' + r.RecalcSum.toFixed(2) + ']' + ')*' + countDays + '/' + countDaysMonth;
                            });
                    } else {
                        text = '[' + factCharge + '] = [' + currentCharge + ']';

                        Ext.Array.each(recalcHistory,
                            function (r) {
                                text += ' + [' + r.RecalcSum.toFixed(2) + ']';
                            });       
                    }

                    text += ',<br> где ' + '[' + currentCharge + '] = [' + 'начисление за ' + periodName + ']';

                    if (recalcHistory) {
                        Ext.Array.each(recalcHistory,
                            function(r) {
                                text += ',<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[' +
                                    r.RecalcSum.toFixed(2) + '] = ' + '[Перерасчет в ' + r.CalcPeriod + ']';
                            });
                    }

                    if (countDays !== countDaysMonth) {
                        text += ',<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + '[' + countDays + '] - [Количество дней начисления за период ' + period + ']';
                        text += ',<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + '[' + countDaysMonth + '] - [Количество дней начисления за ' + periodName + ']';
                    }
                }
                break;
        }
        
        if (text) {
            var tip = Ext.create('Ext.tip.ToolTip',
            {
                dismissDelay: 0,
                maxWidth: width,
                width: width,
                html: text,
                hide: function () {
                    var me = this;
                    me.clearTimer('dismiss');
                    me.lastActive = new Date();
                    if (me.anchorEl) {
                        me.anchorEl.hide();
                    }
                    me.getEl().switchOff({
                        easing: 'easeOut',
                        duration: 600,
                        remove: false,
                        useDisplay: false
                    });
                    me.callParent(arguments);
                    delete me.triggerElement;
                }
            });

            tip.showAt(eOpts.xy);
        }
    }
});