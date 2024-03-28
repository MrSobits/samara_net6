Ext.define('B4.controller.SubsidyIncome', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    stores: [
        'regop.SubsidyIncome',
        'regop.subsidyincome.Detail'
    ],

    views: [
        'subsidyincome.Grid',
        'subsidyincome.DetailWindow',
        'subsidyincome.DetailGrid',
        'subsidyincome.AddRealObjWindow'
    ],

    models: [
        'SubsidyIncome'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'subsidyincomegrid'
        },
        {
            ref: 'detailWindow',
            selector: 'subsidyincomedetailwin'
        },
        {
            ref: 'addRealObjWindow',
            selector: 'subsidyincomeaddrealobjwin'
        }
    ],

    distributionState: {
        DISTRIBUTED: 10,
        NOT_DISTRIBUTED: 20,
        CANCELED: 30,
        DELETED: 40,
        PARTIALLY_DISTRIBUTED: 50
    },

    aspects: [
            {
                xtype: 'gkhbuttonimportaspect',
                name: 'subsidyincomeimportaspect',
                buttonSelector: 'subsidyincomegrid gkhbuttonimport',
                ownerWindowSelector: 'subsidyincomegrid',
                codeImport: 'SubsidyIncomeImport'
            }
    ],

    init: function () {
        var me = this;

        me.control({
            'subsidyincomegrid [name=ShowConfirmed]': { change: { fn: me.updateSubsidyIncomeGrid, scope: me } },
            'subsidyincomegrid [name=ShowDeleted]': { change: { fn: me.updateSubsidyIncomeGrid, scope: me } },
            'subsidyincomegrid b4updatebutton': { click: { fn: me.updateSubsidyIncomeGrid, scope: me } },
            'subsidyincomegrid': { rowaction: { fn: me.onGridRowAction, scope: me } },
            'subsidyincomegrid button[action=confirm]': { 'click': { fn: me.onClickConfirm, scope: me } },
            'subsidyincomegrid button[action=undoconfirm]': { 'click': { fn: me.onClickUndoConfirm, scope: me } },
            'subsidyincomegrid button[action=delete]': { 'click': { fn: me.onClickCancelPayment, scope: me } },
            'subsidyincomedetailwin button[action=confirm]': { 'click': { fn: me.onDetailsClickConfirm, scope: me } },
            'subsidyincomedetailwin button[action=addrealobj]': { 'click': { fn: me.onClickAddRealObj, scope: me } },
            'subsidyincomedetailwin button[action=undoconfirm]': { 'click': { fn: me.onDetailsClickUndoConfirm, scope: me } },
            'subsidyincomeaddrealobjwin b4savebutton': { 'click': { fn: me.addRealObj, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('subsidyincomegrid'),
            store = view.getStore();

        me.bindContext(view);
        me.application.deployView(view);

        store.on('beforeload', function (st, options) {
            options.params.showConfirmed = view.down('[name=ShowConfirmed]').getValue();
            options.params.showDeleted = view.down('[name=ShowDeleted]').getValue();
        }, me);

        me.getAspect('subsidyincomeimportaspect').loadImportStore();
    },
    
    updateSubsidyIncomeGrid: function (comp) {
        comp.up('subsidyincomegrid').getStore().load();
    },

    onGridRowAction: function (grid, action, rec) {
        var me = this, win;

        if (action.toLowerCase() !== 'edit') {
            return;
        }

        win = me.getDetailWindow();

        if (!win) {
            win = Ext.widget('subsidyincomedetailwin', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                renderTo: B4.getBody().getActiveTab().getEl(),
                entityId: rec.getId()
            });
        }

        win.show();
    },

    onClickConfirm: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            recs = grid.getSelectionModel().getSelection(),
            recIds = [];

        if (recs.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        Ext.each(recs, function (rec) {
            if (rec.get('DistributeState') === me.distributionState.NOT_DISTRIBUTED || rec.get('DistributeState') === me.distributionState.PARTIALLY_DISTRIBUTED) {
                recIds.push(rec.get('Id'));
            }
        });

        if (recIds.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Запись должна быть в статусе "Не распределен" или "Частично распределен"!', 'error');
            return;
        }

        B4.Ajax.request({
            url: B4.Url.action('/SubsidyIncome/CheckDate'),
            params: {
                recIds: Ext.JSON.encode(recIds)
            }
        }).next(function () {
            me.unmask();
            me.continueConfirm(recIds);
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
        });
    },

    continueConfirm: function (recIds, detailIds, detailStore) {
        var me = this;

        me.mask('Потверждение распределения...');
        B4.Ajax.request({
            url: B4.Url.action('Apply', 'SubsidyIncome'),
            method: 'POST',
            timeout: 999999,
            params: {
                recIds: Ext.JSON.encode(recIds),
                detailIds: Ext.JSON.encode(detailIds)
            }
        }).next(function () {
            Ext.Msg.alert('Результат', 'Распределение потверждено');
            me.getMainView().getStore().load();

            if (detailStore) {
                detailStore.load();
            }

            me.unmask();
        }).error(function (response) {
            var message = "Ошибка потверждения распределения";
            if (response && response.message) {
                message = response.message;
            }

            Ext.Msg.alert('Ошибка', message);

            me.unmask();
        });
    },

    onClickUndoConfirm: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            recIds = [],
            recs = grid.getSelectionModel().getSelection();

        if (recs.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        Ext.each(recs, function (rec) {
            if (rec.get('DistributeState') === me.distributionState.DISTRIBUTED || rec.get('DistributeState') === me.distributionState.PARTIALLY_DISTRIBUTED) {
                recIds.push(rec.get('Id'));
            }
        });

        if (recIds.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Запись должна быть в статусе "Распределен" или "Частично распределен"!', 'error');
            return;
        }

        Ext.Msg.confirm('Отмена потверждения распределения', 'Отменить потверждение?', function (btnId) {
            if (btnId === "yes") {
                me.mask('Отмена потверждения распределения...');
                B4.Ajax.request({
                    url: B4.Url.action('Undo', 'SubsidyIncome'),
                    method: 'POST',
                    timeout: 999999,
                    params: {
                        recIds: Ext.JSON.encode(recIds)
                    }
                }).next(function () {
                    Ext.Msg.alert('Результат', 'Потверждение отменено');
                    me.getMainView().getStore().load();
                    me.unmask();
                }).error(function (response) {
                    var message = "Ошибка отмены потверждения";
                    if (response && response.message) {
                        message = response.message;
                    }

                    Ext.Msg.alert('Ошибка', message);

                    me.unmask();
                });
            }
        });
    },

    onClickCancelPayment: function () {
        var me = this,
            records = me.getMainView().getSelectionModel().getSelection(),
            anyRecordInValidState = false,
            recordsInDifferentStates = false,
            recordsAlreadyDeleted = true,
            iterState = undefined;

        if (!records.length) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        Ext.Array.each(records, function (r) {
            if (r.get('DistributeState') !== me.distributionState.CANCELED
                || r.get('DistributeState') !== me.distributionState.DELETED) {
                recordsAlreadyDeleted = false;
            }

            if (r.get('DistributeState') === me.distributionState.NOT_DISTRIBUTED) {
                anyRecordInValidState = true;
            }

            if (iterState && iterState !== r.get('DistributeState')) {
                recordsInDifferentStates = true;
            }

            iterState = r.get('DistributeState');
        });

        if (recordsAlreadyDeleted) {
            B4.QuickMsg.msg('Ошибка', 'Выбраны записи со статусом "Удалена"', 'info');
            return;
        }

        if (anyRecordInValidState === false) {
            B4.QuickMsg.msg('Ошибка', 'Операция возможна только для неподтвержденных реестров субсидий!', 'error');
            return;
        }

        var cancelFn = function (ids) {
            me.mask('Отмена зачисления...');
            B4.Ajax.request({
                url: B4.Url.action('UndoCheckin', 'Distribution'),
                method: 'POST',
                params: {
                    distributionIds: Ext.JSON.encode(ids),
                    distributionSource: 30
                }
            }).next(function () {
                Ext.Msg.alert('Результат', 'Потверждение удалено');
                me.getMainView().getStore().load();
                me.unmask();
            }).error(function (response) {
                var message = "Ошибка удаления потверждения";
                if (response && response.message) {
                    message = response.message;
                }

                Ext.Msg.alert('Ошибка', message);

                me.unmask();
            });
        };

        var confirmFn = function () {
            Ext.Msg.show({
                title: 'Отмена потверждения',
                msg: 'На статусе "Удален" операции по подтверждению будут невозможны". Применить?',
                buttons: Ext.Msg.OKCANCEL,
                icon: Ext.window.MessageBox.INFO,
                fn: function (btnId) {
                    if (btnId === "ok") {
                        var ids = Ext.Array.map(records, function (r) {
                            return r.getId();
                        });

                        cancelFn(ids);
                    }
                }
            });
        };

        if (recordsInDifferentStates) {
            Ext.MessageBox.show({
                title: 'Выбраны записи с разными статусами',
                msg: 'Выбраны записи с разными статусами. Операция будет произведена только для записей со статусом "Не распределен". Отменить потверждение?',
                buttons: Ext.Msg.OKCANCEL,
                icon: Ext.window.MessageBox.INFO,
                fn: function (btn) {
                    if (btn === 'ok') {
                        confirmFn();
                    }
                }
            });
        } else {
            confirmFn();
        }
    },

    onDetailsClickConfirm: function (btn) {
        var me = this,
            window = btn.up('window'),
            grid = window.down('grid'),
            recs = grid.getSelectionModel().getSelection(),
            detailIds = [];

        if (recs.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        Ext.each(recs, function (rec) {
            if (rec.get('RealityObject') === null) {
                B4.QuickMsg.msg('Ошибка', 'Подтвердить оплаты можно только с определенным домом!', 'error!', 'error');
                return;
            }

            if (rec.get('IsConfirmed') === false) {
                detailIds.push(rec.get('Id'));
            } else {
                B4.QuickMsg.msg('Ошибка', 'Подтвердить оплаты можно только со статусом «Не подтвержден»!', 'error!', 'error');
                return;
            }
        });

        B4.Ajax.request({
            url: B4.Url.action('/SubsidyIncome/CheckDate'),
            params: {
                detailIds: detailIds
            }
        }).next(function () {
            me.unmask();
            me.continueConfirm(null, detailIds, grid.getStore());
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
        });
    },

    onClickAddRealObj: function (btn) {
        var me = this,
            window = btn.up('window'),
            grid = window.down('grid'),
            recs = grid.getSelectionModel().getSelection(),
            rec;

        if (recs.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        if (recs.length > 1) {
            B4.QuickMsg.msg('Ошибка', 'Невозможно сопоставить сразу несколько записей!', 'error!', 'error');
            return;
        }

        rec = recs[0];

        if (rec.get('IsConfirmed') === true) {
            B4.QuickMsg.msg('Ошибка', 'Невозможно поменять дом у потвержденной записи!', 'error!', 'error');
            return;
        }


        if (rec.get('IsDefined') === true) {
            Ext.Msg.confirm('Внимание', 'Выбранный дом уже определен в системе. Редактировать данные?', function(btnId) {
                if (btnId === "yes") {
                    me.showAddRealObjWin(rec);
                }
            });
        } else {
            me.showAddRealObjWin(rec);
        }
    },

    showAddRealObjWin: function(rec) {
        var me = this,
            win;

            win = me.getAddRealObjWindow();

            if (!win) {
                win = Ext.widget('subsidyincomeaddrealobjwin', {
                    ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    rec: rec
                });
            }

            win.show();

    },

    addRealObj: function(btn) {
        var me = this,
            win = btn.up('window'),
            roField = win.down('[name=RealityObject]'),
            roId = roField.getValue(),
            rec = win.rec,
            detailWin = me.getDetailWindow(),
            grid = detailWin.down('grid');

        if (roId) {
            rec.set('RealityObject', roId);
            rec.save({
                callback: function() {
                    win.close();
                    grid.getStore().load();
                }
            });
        } else {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

    }
});