Ext.define('B4.controller.regop.period.CloseChecking',
{
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Url'
    ],

    models: [
        'regop.period.CloseCheckResult'
    ],

    stores: [
        'regop.period.CloseCheckResult'
    ],

    views: [
        'regop.periodclosecheck.ResultsGrid',
        'regop.periodclosecheck.HistoryRollbackWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'periodclosecheckresultsgrid'
        }
    ],

    mainView: 'regop.periodclosecheck.ResultsGrid',
    mainViewSelector: 'periodclosecheckresultsgrid',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.AccountingMonth.PeriodChecking.Check',
                    applyTo: 'button[name=RunChecks]',
                    selector: 'periodclosecheckresultsgrid'
                },
                {
                    name: 'GkhRegOp.AccountingMonth.PeriodChecking.Close',
                    applyTo: 'button[name=RunChecksAndClose]',
                    selector: 'periodclosecheckresultsgrid'
                },
                {
                    name: 'GkhRegOp.AccountingMonth.PeriodChecking.RollbackClosedPeriod',
                    applyTo: 'button[name=RollbackClosedPeriod]',
                    selector: 'periodclosecheckresultsgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'periodclosecheckresultsgrid': {
                'cellclick': {
                    fn: me.onCellClick,
                    scope: me
                }
            },
            'periodclosecheckresultsgrid [name=ChargePeriod]': {
                'change': {
                    fn: me.onPeriodChange,
                    scope: me
                }
            },
            'periodclosecheckresultsgrid [name=RunChecks]': {
                'click': {
                    fn: function() { me.onRunChecks(false); },
                    scope: me
                }
            },
            'periodclosecheckresultsgrid [name=RunChecksAndClose]': {
                'click': {
                    fn: function () { me.onCloseCheckPeriod(); },
                    scope: me
                }
            },
            'periodclosecheckresultsgrid [name=RollbackClosedPeriod]': {
                'click': {
                    fn: me.onRollbackClosedPeriod,
                    scope: me
                }
            },
            'periodcloserollbackhistorywindow b4savebutton': {
                'click': {
                    fn: me.onApplyRollbackClosedPeriod,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('periodclosecheckresultsgrid'),
            cp = view.down('[name=ChargePeriod]');

        me.bindContext(view);
        me.application.deployView(view);

        if (!cp.getValue()) {
            me.setCurrentPeriod(cp);
        }
    },

    setCurrentPeriod: function(cp) {
        var me = this,
            grid = cp.up('grid');

        me.mask('Загрузка...', grid);
        B4.Ajax.request({
                url: B4.Url.action('GetOpenPeriod', 'ChargePeriod'),
                timeout: 9999999
            })
            .next(function(response) {
                if (response == null) {
                    return;
                }

                var res = Ext.decode(response.responseText);

                if (!res.data) {
                    Ext.Msg.alert('Не создан период!', 'Необходимо создать период!');
                    me.unmask();
                    return;
                }

                if (res.data) {
                    cp._openPeriod = res.data;
                    cp.setValue(res.data);
                } else {
                    cp.setValue(null);
                }

                me.unmask();
            })
            .error(function(response) {
                me.updateGrid();
                var message = 'Ошибка загрузки данных. Попробуйте обновить старницу';
                if (response) {
                    var res = Ext.decode(response.responseText);
                    message = res.data.message;
                }
                me.unmask();
                Ext.Msg.alert('Результат', message);
            });
    },

    onPeriodChange: function(cp, newVal) {
        var grid = cp.up('grid'),
            store = grid.getStore(),
            periodId = newVal && newVal.Id;

        if (!newVal && cp._openPeriod) {
            Ext.defer(function() {
                    cp.setValue(cp._openPeriod);
                },
                100);
            return false;
        }

        grid.down('[name=bgChecks]').setDisabled(!cp._openPeriod || cp._openPeriod.Id !== periodId);
        
        store.clearFilter(true);
        store.filter('periodId', periodId);
    },

    onCellClick: function (view, _, cellIndex, record) {
        var column = view.ownerCt.columns[cellIndex],
            group;
        if (column.dataIndex !== 'PersAccGroup') {
            return;
        }

        group = record.get(column.dataIndex);
        if (!group || !group.Id) {
            return;
        }

        this.application.redirectTo('regop_personal_account?pagroup=' + group.Id);
    },

    onCloseCheckPeriod: function () {
        var me = this;

        me.mask(me.getMainView());

        B4.Ajax.request({
            url: B4.Url.action('GetCountDocumentFormedInOpenPerod', 'ChargePeriod'),
            timeout: 360000
        })
        .next(function (resp) {
            var res = Ext.decode(resp.responseText);
            if (res.data > 0) {
                me.unmask();
                Ext.Msg.confirm('Внимание', Ext.String.format('Сформировано документов в открытом периоде по {0} лс. Уверены ли Вы, что необходимо закрыть период?',
                    res.data), function (result) {
                        if (result === 'yes') {
                            me.onRunChecks(true);
                        }
                    });
            } else {
                me.onRunChecks(true);
            }
        })
        .error(function (err) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', err.message || err, 'error');
        });
    },

    onRunChecks: function(closeAfterCheck) {
        var me = this,
            grid = me.getMainView();

        me.mask('Запуск...', grid);
        B4.Ajax.request({
                url: B4.Url.action(closeAfterCheck ? 'RunCheckAndClosePeriod' : 'RunCheck', 'PeriodCloseCheckResult'),
                timeout: 9999999
            })
            .next(function(response) {
                me.unmask();

                var res = Ext.decode(response.responseText);

                if (res.Success) {
                    grid.getStore().load();
                    B4.QuickMsg.msg('Проверка', 'Задачи поставлены в очередь на обработку', 'success');
                } else {
                    B4.QuickMsg.msg('Проверка', res.message || 'Произошла ошибка при постановке задач', 'error');
                }
            })
            .error(function(response) {
                me.unmask();
                B4.QuickMsg.msg('Ошибка', response.message, 'error');
            });
    },

    onRollbackClosedPeriod: function() {
        var win = Ext.create('B4.view.regop.periodclosecheck.HistoryRollbackWindow');
        win.show();
    },

    onApplyRollbackClosedPeriod: function (button) {
        var me = this,
            win = button.up('periodcloserollbackhistorywindow'),
            isDeleteSnapshot = win.down('checkbox[name=IsDeleteSnapshot]').getValue(),
            reason = win.down('textarea[name=Reason]').getValue();

        Ext.Msg.confirm('Внимание', 'Вы уверены, что хотите откатить закрытый период? ', function (result) {
            if (result === 'yes') {
                me.mask('Откат периода...', win);
                B4.Ajax.request({
                    url: B4.Url.action('RollbackClosedPeriod', 'ChargePeriod'),
                    params: {
                        isDeleteSnapshot: isDeleteSnapshot,
                        reason: reason
                    },
                        timeout: 3600000
                    })
                    .next(function(response) {
                        me.unmask();

                        var result = Ext.decode(response.responseText);
                        if (result.success) {
                            B4.QuickMsg.msg('Откат закрытого периода', 'Откат периода успешно завершен', 'success');
                        } else {
                            B4.QuickMsg.msg('Откат закрытого периода', result.message || 'Произошла ошибка при постановке задач', 'error');
                        }
                    })
                    .error(function (response) {
                        var message = response.message || '';
                        me.unmask();
                        B4.QuickMsg.msg('Ошибка', message, 'error');
                    });
            }
        });
    }
});