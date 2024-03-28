Ext.define('B4.controller.regop.loan.Manage', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.regop.loan.ManageGrid',
        'B4.view.regop.loan.LoanGrid',
        'B4.store.regop.loan.Manage',
        'B4.store.regop.Loan',
        'B4.store.regop.loan.AvailableLoanProvidersStore',
        'B4.view.regop.loan.ManageWindow',
        'B4.view.regop.loan.ManageConfirmWindow',
        'B4.QuickMsg'
    ],

    stores: [
        'regop.loan.Manage',
        'regop.Loan'
    ],

    views: [
        'regop.loan.TakeLoanWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'loanmanagegrid' },
        { ref: 'moCombo', selector: 'loanmanagegrid b4combobox[name="municipality"]' },
        { ref: 'programCombo', selector: 'loanmanagegrid b4combobox[name="programcr"]' },
        { ref: 'collectWindow', selector: 'loanmanagewindow' },
        { ref: 'saldoField', selector: 'loanmanagegrid [name=AvailableSum]' },
        { ref: 'saldoAvailableField', selector: 'loanmanagegrid [name=AvailableLoanSum]' }
    ],

    index: function() {
        var me = this,
            view = me.getMainView();
        
        if (!view) {
            view = Ext.widget('loanmanagegrid');
            view.getStore().on('beforeload', me.onStoreBeforeLoad, me);
        }

        me.bindContext(view);
        me.application.deployView(view);
    },

    init: function () {
        var me = this;

        me.control({
            'loanmanagegrid': {
                'loanmanagegrid.load': {
                    fn: function (store) {
                        var reader = store.getProxy().getReader(),
                            jsonData = reader.jsonData;

                        if (jsonData && jsonData.success) {
                            me.fillRegopInfo(jsonData.additionalData);
                        }
                    },
                    scope: me
                },

                'rowaction': {
                    fn: function(grid, action, rec) {
                        var task = rec.get('Task');

                        if (task) {
                            me.application.getRouter().forward('tasks', true, { ParentId: task.ParentTaskId });
                        }
                    },
                    scope: me
                }
            },
            'loanmanagegrid [name=municipality]': {
                'change': {
                    fn: function () {
                        me.getMainView().getStore().load();
                    },
                    scope: me
                }
            },
            'loanmanagegrid b4updatebutton': {
                'click': {
                    fn: function () {
                        var me = this,
                            moVal = me.getMoCombo().getValue(),
                            programVal = me.getProgramCombo().getValue();

                        if (Ext.isEmpty(moVal) || Ext.isEmpty(programVal)) {
                            B4.QuickMsg.msg('Предупреждение', 'Муниципальное образование и краткосрочный план должны быть выбраны', 'warning');
                            return;
                        }
                        me.getMainView().getStore().load();
                    },
                    scope: me
                }
            },
            'loanmanagegrid [name=programcr]': {
                'change': {
                    fn: function () {
                        me.getMainView().getStore().load();
                    },
                    scope: me
                }
            },
            'loanmanagegrid [action=takeloan]': {
                'click': { fn: me.onClickTakeLoan, scope: me }
            },
            'loanmanagewindow [name=TypeLoanProcess]': {
                'change': { fn: me.onChangeTypeLoanProcess, scope: me }
            },
            'loanmanagewindow [action=accept]': {
                'click': { fn: me.onClickAccept, scope: me }
            }
        });

        B4.QuickMsg.msg('Предупреждение', 'Муниципальное образование и краткосрочный план должны быть выбраны', 'warning');
        me.callParent(arguments);
    },

    fillRegopInfo: function(regopInfo) {
        var me = this,
            mainView = me.getMainView(),
            nfNeedSum = mainView.down('[name=NeedSum]'),
            nfAvailableLoanSum = mainView.down('[name=AvailableLoanSum]'),
            nfAvailableSum = mainView.down('[name=AvailableSum]');

        if (regopInfo) {
            nfNeedSum.setValue(regopInfo.NeedSum);
            nfAvailableLoanSum.setValue(regopInfo.AvailableLoanSum);
            nfAvailableSum.setValue(regopInfo.AvailableSum);
            me.blockedSum = regopInfo.BlockedSum; // сумма заблокированных для займа средств на текущем уровне формирования займов
        }
    },

    onClickTakeLoan: function() {
        var me = this,
            grid = me.getMainView(),
            selected = grid.getSelectionModel().selected;
        
        if (!selected || selected.items.length == 0) {
            B4.QuickMsg.msg('Предупреждение', 'Не выбраны дома', 'warning');
            return;
        }

        if (!me.checkCalcAccount(selected.items)) {
            return;
        }

        if (!me.checkTaskState(selected.items)) {
            return;
        }

        me.showTakeLoanWindow(selected.items);
    },

    checkTaskState: function(items) {
        var success = true;

        Ext.each(items, function (item) {
            if (item.get('Task')) {
                B4.QuickMsg.msg('Предупреждение', 'Процесс взятия займа по дому уже запущен. Повторное взятие займа запрещено.', 'warning');
                return success = false;
            }

            return true;
        });

        return success;
    },

    checkCalcAccount: function(items) {
        var calcAccount = null,
            success = true;

        Ext.each(items, function (item) {
            if (Ext.isEmpty(calcAccount)) {
                calcAccount = item.get('CalcAccountNumber');
                return true;
            }

            if (calcAccount !== item.get('CalcAccountNumber')) {
                B4.QuickMsg.msg('Предупреждение', 'Выбранные дома относятся к разным расчетным счетам. Для массового формирования займа необходимо выбрать записи с одним номером счета', 'warning');
                return success = false;
            }

            return true;
        });

        return success;
    },

    showTakeLoanWindow: function(records) {
        var me = this,
            view = me.getMainView(),
            programVal = me.getProgramCombo().getValue(),
            win = me.getCollectWindow() || Ext.widget('loanmanagewindow'),
            sourcesStore,
            recordIds = [],
            needSum = 0,
            robjectsSaldoSum = 0;

        Ext.each(records, function(record) {
            recordIds.push(record.get('Id'));
            needSum += record.get('NeedSum');
            robjectsSaldoSum += record.get('OwnerSum') - record.get('OwnerLoanSum');
        });

        view.add(win);

        me.setContextValue(view, 'programId', programVal);
        me.setContextValue(view, 'recordIds', recordIds);

        sourcesStore = win.down('gridpanel').getStore();

        sourcesStore.on('beforeload', function (s, op) {
            op.params['roIds'] = Ext.encode(recordIds);
        });

        sourcesStore.load();
        
        win.setInitialDeficit(needSum, robjectsSaldoSum);
        win.show();

        return true;
    },

    onClickAccept: function(btn) {
        var me = this,
            win = btn.up('loanmanagewindow'),
            type = win.down('[name=TypeLoanProcess]').getValue();

        switch(type) {
            case 0:
                me.takeLoanAuto(win);
                break;
            case 10:
                me.takeLoanManual(win);
                break;
        }
    },

    takeLoanManual: function (win) {
        var me = this,
            initialDeficit = win.down('[name=InitialDeficit]').getValue(),
            currentDeficit = win.down('[name=CurrentDeficit]').getValue(),
            typeLoanProcess = win.down('[name=TypeLoanProcess]').getValue(),
            sourcesStore = win.down('grid').getStore(),
            data = [],
            saldoAvailableAmount = me.getSaldoAvailableField().getValue() - me.blockedSum,
            takeAmount = 0;
        
        if (initialDeficit === currentDeficit) {
            B4.QuickMsg.msg('Предупреждение', 'Не выбраны источники займа', 'warning');
            return;
        }

        Ext.each(sourcesStore.data.items, function(r) {
            data.push({
                TypeSource: r.get('TypeSource'),
                TakenMoney: r.get('TakenMoney')
            });

            takeAmount += Number.isFinite(r.get('TakenMoney')) ? r.get('TakenMoney') : 0;
        });

        if (data.length === 0) {
            B4.QuickMsg.msg('Предупреждение', 'Не выбраны источники займа', 'warning');
            return;
        }

        if (takeAmount > saldoAvailableAmount) {
            Ext.Msg.alert('Ошибка', 'Недостаточно средств для займа. Максимально допустимая сумма для займа с учетом неснижаемого размера фонда: ' + Ext.util.Format.currency(saldoAvailableAmount));
            return;
        }

        me.mask('Взятие займа...', B4.getBody().getActiveTab());

        B4.Ajax.request({
            url: B4.Url.action('TakeLoan', 'RealityObjectLoan'),
            params: {
                takenMoney: Ext.encode(data),
                programId: me.getContextValue(win, 'programId'),
                roIds: Ext.encode(me.getContextValue(win, 'recordIds')),
                typeLoanProcess: typeLoanProcess
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Успешное взятие займа', 'success');
            win.close();
            me.getMainView().getStore().load();
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    takeLoanAuto: function(win) {
        var me = this,
            typeLoanProcess = win.down('[name=TypeLoanProcess]').getValue(),
            initialDeficit = win.down('[name=InitialDeficit]').getValue(),
            saldoAvailableAmount = me.getSaldoAvailableField().getValue() - me.blockedSum;

        if (initialDeficit > saldoAvailableAmount) {
            Ext.Msg.alert('Ошибка', 'Недостаточно средств для займа. Максимально допустимая сумма для займа с учетом неснижаемого размера фонда: ' + Ext.util.Format.currency(saldoAvailableAmount));
            return;
        }

        me.mask('Взятие займа...', B4.getBody().getActiveTab());

        B4.Ajax.request({
            url: B4.Url.action('TakeLoan', 'RealityObjectLoan'),
            params: {
                programId: me.getContextValue(win, 'programId'),
                roIds: Ext.encode(me.getContextValue(win, 'recordIds')),
                typeLoanProcess: typeLoanProcess
            },
            timeout: 9999999
        }).next(function (result) {
            var obj;
            me.unmask();
            obj = Ext.decode(result.responseText);
            B4.QuickMsg.msg('Успешно', obj.message || "Задача успешно поставлена в очередь на обработку.<br>Информацию по задаче можно увидеть в разделе 'Задачи'", 'success');
            win.close();
            me.getMainView().getStore().load();
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err);
        });
    },

    onStoreBeforeLoad: function (store) {
        var me = this,
            moVal = me.getMoCombo().getValue(),
            programVal = me.getProgramCombo().getValue();

        if (Ext.isEmpty(moVal) || Ext.isEmpty(programVal)) {
            return false;
        }

        Ext.apply(store.getProxy().extraParams, {
            moId: moVal,
            programId: programVal
        });

        return true;
    },

    onChangeTypeLoanProcess: function (fld) {
        var grid = fld.up('loanmanagewindow').down('gridpanel[name=LoanSources]');

        grid.setDisabled(fld.getValue() == 0);
    }
});