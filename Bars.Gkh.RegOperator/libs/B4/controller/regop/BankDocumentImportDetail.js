Ext.define('B4.controller.regop.BankDocumentImportDetail', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonImportAspect',
        'B4.enums.PersonalAccountDeterminationState',
        'B4.enums.PaymentConfirmationState',
        'B4.model.regop.BankDocumentImport',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GeneralStateHistory'
    ],

    stores: [
        'regop.ImportedPayment',
        'regop.BankDocumentImport'
    ],

    views: [
        'regop.bankdocumentimport.Grid',
        'regop.bankdocumentimport.EditPanel',
        'regop.bankdocumentimport.PersonalAccountGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'regop.bankdocumentimport.EditPanel',
    mainViewSelector: 'bankdocimporteditpanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            modelName: 'regop.BankDocumentImport',
            editPanelSelector: 'bankdocimporteditpanel',
            name: 'bankdocimporteditpanelaspect',
            listeners: {
                'aftersetpaneldata': function(asp, rec) {
                    if (rec.get('CheckState') === 20) {
                        Ext.Msg.alert('Внимание',
                            'Подтвержденная сумма — ' +
                            rec.get('AcceptedSum') +
                            ' руб. — не соответствует сумме, учтенной по лицевым счетам — ' +
                            rec.get('DistributedSum') +
                            ' руб. (см. лог операций)');
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.Settings.BankDocumentImport.Comparing.CompareAndAccept',
                    applyTo: 'button[action=CompareAndAccept]',
                    selector: 'bankdocumentimportpagrid',
                    applyBy: function(component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankDocumentImport.Field.InternalCancel',
                    applyTo: 'button[action=InternalCancel]',
                    selector: 'bankdocimporteditpanel',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'generalstatehistory',
            name: 'bankDocImportConfirmationStateAspect',
            gridSelector: 'bankdocimporteditpanel gridpanel',
            stateCode: 'regop_imported_payment'
        }
    ],
    init: function () {
        var me = this;
        me.control({
            'bankdocimporteditpanel button[action=InternalAccept]': { 'click': { fn: me.onClickInternalAccept } },
            'bankdocimporteditpanel button[action=InternalCancel]': { 'click': { fn: me.onClickInternalCancel } },
            'bankdocimporteditpanel button[action=Compare]': { 'click': { fn: me.onClickCompare } },
            'bankdocumentimportpagrid button[action=Compare]': { 'click': { fn: me.onClickAcceptComparing } },
            'bankdocumentimportpagrid button[action=CompareAndAccept]': { 'click': { fn: me.onClickAcceptComparing } },
            'bankdocumentimportpagrid checkbox[name=ShowInActivePa]': { 'change': { fn: me.onChangeShowInActivePa } },
            'bankdocimporteditpanel checkbox[name=RsNotEqual]': { change: { fn: me.filterEditWindowCheck, scope: me } },
            'bankdocimporteditpanel gridpanel': {'cellclick': me.onCellClickRefund }
        });
        me.callParent(arguments);
    },

    onClickInternalAccept: function (btn) {
        var me = this,
            grid = btn.up('gridpanel'),
            window = btn.up('bankdocimporteditpanel'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Подтвердить оплаты?',
            states = {};

        var bankDocumentImportId = me.getContextValue(window, 'bankDocumentImportId');
        if (!selected || selected.length < 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для подтверждения!', 'error');
            return;
        }

        var importedPaymentIds = [];
        Ext.each(selected, function (item) {
            var padState = item.get('PersonalAccountDeterminationState');
            if (padState === 10) {
                states.isPersonalNonDistr = true;
            } else if (padState === 30) {
                states.isPersonalDistr = true;
            } else {
                states.isWrongPersonal = true;
            }

            var pcState = item.get('PaymentConfirmationState');
            if (pcState === 10) {
                states.isPaymentNonDistr = true;
            } else if (pcState === 20) {
                states.isPaymentDistr = true;
            } else if (pcState === 40) {
                states.isPaymentDel = true;
            } else if (pcState === 50) {
                states.isPaymentWaitConfirm = true;
            } else {
                states.isWrongPayment = true;
            }

            if (!((padState === 10 && pcState === 10) || (padState === 30 && pcState === 20))) {
                states.both = true;
            }

            if (padState === 30 && pcState === 10) {
                importedPaymentIds.push(item.getId());
            }
        });

        if (states.isWrongPayment || states.isWrongPersonal) {
            Ext.Msg.alert('Информация', 'В списке оплат есть запись с некорректным статусом определения лс или подтверждения оплаты. Подтверждение оплат невозможно');
            return;
        } else if (states.isPaymentDel) {
            Ext.Msg.alert('Информация', 'Оплата(ы) на статусе "Удалена". Подтверждение оплат(ы) невозможно');
            return;
        } else if (states.isPersonalDistr && !states.isPersonalNonDistr && states.isPaymentDistr && !states.isPaymentNonDistr) {
            Ext.Msg.alert('Информация', 'Оплаты уже подтверждены');
            return;
        } else if (!states.isPersonalDistr && states.isPersonalNonDistr && !states.isPaymentDistr && states.isPaymentNonDistr) {
            Ext.Msg.alert('Информация', 'ЛС не определен(ы). Подтверждение невозможно');
            return;
        } else if (states.isPaymentWaitConfirm && !states.isPaymentNonDistr && !states.isPaymentDistr && !states.isPaymentDel) {
            Ext.Msg.alert('Информация', 'Выбранные оплаты уже находятся в процессе подтверждения. Процесс можно отследить в разделе "Список задач"');
            return;
        } else if (!states.both) {
            Ext.Msg.alert({
                title: 'Подтверждение оплат',
                msg: 'Оплаты с неопределенными ЛС не могут быть подтверждены. Одна или несколько оплат уже подтверждены, повторное подтверждение для них выполнено не будет.',
                buttons: Ext.Msg.OK,
                fn: function() {
                    Ext.Msg.alert('Информация', 'Не выбраны платежи, у которых статус определения ЛС = "Определен" и статус подтверждения оплат = "Не подтвержден". '
                        + 'Оплаты не были подтверждены.');
                }
            });
            return;
        } else if (states.isPersonalDistr && (states.isPersonalNonDistr || !states.isPersonalNonDistr) && states.isPaymentDistr && states.isPaymentNonDistr) {
            msg = 'Оплаты с неопределенными ЛС не могут быть подтверждены. '
                + 'Одна или несколько оплат уже подтверждены, повторное подтверждение для них выполнено не будет. ' + msg;
        }  else if (states.isPersonalDistr && states.isPersonalNonDistr && !states.isPaymentDistr && states.isPaymentNonDistr) {
            msg = 'Оплаты с неопределенными ЛС не могут быть подтверждены. ' + msg;
        }
        
        if (states.isPaymentWaitConfirm) {
            Ext.Msg.alert('Информация', 'Среди выбранных оплат имеются оплаты, которые находятся в процессе подтверждения (раздел "Задачи")');
        }
        Ext.Msg.alert({
            title: 'Подтверждение оплат',
            msg: msg,
            buttons: Ext.Msg.OKCANCEL,
            fn: function (btnId) {
                var params;
                if (btnId === 'ok') {
                    me.mask('Подтверждение оплат...', grid);
                    params = {
                        bankDocumentImportId: bankDocumentImportId,
                        importedPaymentIds: Ext.JSON.encode(importedPaymentIds)
                    };

                    me.sendRequest(params, 'AcceptInternalPayments'
                    ).next(function (resp) {
                        var dec = Ext.JSON.decode(resp.responseText);
                        Ext.Msg.alert('Информация', dec.message || 'Задача успешно поставлена в очередь на обработку. ' +
                                'Информация о статусе подтверждения платежей содержится в пункте меню "Задачи"');
                        me.unmask();
                        store.load();
                        var queryResult = Ext.ComponentQuery.query('bankdocumentimportgrid');
                        if (queryResult.length > 0) {
                            queryResult[0].store.load();
                        }
                    }).error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Внимание', e.message || e);
                    });
                }
            }
        });
    },

    onClickInternalCancel: function(btn) {
        var me = this,
            grid = btn.up('gridpanel'),
            window = btn.up('bankdocimporteditpanel'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            waitingConfirmationCount = 0,
            waitingCancellation = 0,
            states = {};

        var bankDocumentImportId = me.getContextValue(window, 'bankDocumentImportId');
        if (!selected || selected.length < 1) {
            Ext.Msg.alert({
                title: 'Отмена оплат',
                msg: 'Отменить подтверждение для выбранного реестра?',
                buttons: Ext.Msg.OKCANCEL,
                fn: function(btnId) {
                    var params;
                    if (btnId === 'ok') {
                        me.mask('Отмена подтверждения оплат...', grid);
                        params = {
                            packetIds: bankDocumentImportId
                        };

                        me.sendRequest(params,
                            'CancelPayments'
                        ).next(function(resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Информация',
                                dec.message ||
                                'Задача успешно поставлена в очередь на обработку. ' +
                                'Информация о статусе отмены подтверждения оплат содержится в пункте меню "Задачи"');
                            me.unmask();
                            store.load();
                            var queryResult = Ext.ComponentQuery.query('bankdocumentimportgrid');
                            if (queryResult.length > 0) {
                                queryResult[0].store.load();
                            }
                        }).error(function(e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                        });
                    }
                }
            });
        } else {
            var importedPaymentIds = [];
            Ext.each(selected,
                function(item) {

                    var pcState = item.get('PaymentConfirmationState');
                    if (pcState == B4.enums.PaymentConfirmationState.NotDistributed) {
                        states.isPaymentNonDistr = true;
                    } else if (pcState == B4.enums.PaymentConfirmationState.Distributed) {
                        states.isPaymentDistr = true;
                    } else if (pcState == B4.enums.PaymentConfirmationState.Deleted) {
                        states.isPaymentDel = true;
                    } else if (pcState == B4.enums.PaymentConfirmationState.ConfirmationImpossible) {
                        states.isConfirmationImpossible = true;
                    }

                    if (pcState == B4.enums.PaymentConfirmationState.WaitingConfirmation) {
                        waitingConfirmationCount += 1;
                    }

                    if (pcState == B4.enums.PaymentConfirmationState.WaitingCancellation) {
                        waitingCancellation += 1;
                    }

                    importedPaymentIds.push(item.getId());
                });

            if (states.isPaymentNonDistr || states.isPaymentDel || states.isConfirmationImpossible) {
                Ext.Msg.alert('Информация',
                    'Отмена подтверждения  невозможна,  так как  имеется запись реестра  оплат со статусом "Не подтверждена" или(и) "Удалена", или (и) "Невозможно подтвердить"');
                return;
            }

            if (selected.length == waitingConfirmationCount) {
                Ext.Msg.alert('Информация',
                    'Выбранные записи находятся в процессе подтверждения. Отмена подтверждения невозможна');
                return;
            }

            if (selected.length == waitingCancellation) {
                Ext.Msg.alert('Информация',
                    'Выбранные записи находятся в процессе отмены подтверждения. Отмена подтверждения невозможна');
                return;
            }

            Ext.Msg.alert({
                title: 'Отмена оплат',
                msg: 'Отменить подтверждение для ' +
                    selected.length +
                    ' выбранных оплат? Для записей реестра в статусе "Ожидание подтверждения", "Ожидание отмены подтверждения" отмена проводится не будет',
                buttons: Ext.Msg.OKCANCEL,
                fn: function(btnId) {
                    var params;
                    if (btnId === 'ok') {
                        me.mask('Отмена оплат...', grid);
                        params = {
                            bankDocumentImportId: bankDocumentImportId,
                            importedPaymentIds: Ext.JSON.encode(importedPaymentIds)
                        };

                        me.sendRequest(params,
                            'CancelInternalPayments'
                        ).next(function(resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Информация',
                                dec.message ||
                                'Задача успешно поставлена в очередь на обработку. ' +
                                'Информация о статусе подтверждения платежей содержится в пункте меню "Задачи"');
                            me.unmask();
                            store.load();
                            var queryResult = Ext.ComponentQuery.query('bankdocumentimportgrid');
                            if (queryResult.length > 0) {
                                queryResult[0].store.load();
                            }
                        }).error(function(e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                        });
                    }
                }
            });
        }
    },

    onClickCompare: function (btn) {
        var me = this,
            grid = btn.up('bankdocimporteditpanel').down('gridpanel'),
            records = grid.getSelectionModel().getSelection();

        if (records.length === 1) {
            var record = records[0];
            if (record.get('PaymentConfirmationState') === B4.enums.ImportedPaymentPaymentConfirmState.Distributed && record.get('PersonalAccountDeterminationState') === B4.enums.ImportedPaymentPersAccDeterminateState.Defined) {
                Ext.Msg.alert('Внимание', 'Оплата по ЛС подтверждена. Сопоставление ЛС невозможно');
                return;
            }

            if (record.get('PaymentConfirmationState') === B4.enums.ImportedPaymentPaymentConfirmState.Distributed) {
                Ext.Msg.alert('Внимание', 'Оплата подтверждена. Сопоставление ЛС невозможно');
                return;
            }

            var paGrid = Ext.ComponentQuery.query('bankdocumentimportpagrid')[0];
            if (!paGrid) {
                paGrid = me.getView('regop.bankdocumentimport.PersonalAccountGrid').create(
                {
                    constrain: true,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    closeAction: 'destroy'
                });

                paGrid.show();
                paGrid.down('gridpanel').getStore().load();
            }
        } else {
            Ext.Msg.alert('Внимание', 'Операцию сопоставления ЛС необходимо проводить по одной записи');
            return;
        }
    },

    onClickAcceptComparing: function (btn) {
        var me = this,
            mainView = me.getMainView(),
            grid = btn.up('bankdocumentimportpagrid').down('gridpanel'),
            record = grid.getSelectionModel().getSelection()[0],
            paymentgrid = mainView.down('gridpanel'),
            paymentRecord = paymentgrid.getSelectionModel().getSelection()[0],
            params;

        if (record) {
            me.mask('Сопоставление ЛС...', grid);
            B4.Ajax.request({
                url: B4.Url.action('ComparePersonalAccount', 'ImportedPayment'),
                params: {
                    paymentId: paymentRecord.getId(),
                    paId: record.getId()
                }
            }).next(function () {
                me.unmask();
                grid.up().close();

                if (btn.action === 'Compare') {
                    Ext.Msg.show({
                        title: 'Сопоставление ЛС',
                        msg: 'Сопоставление ЛС прошло успешно',
                        width: 300,
                        buttons: Ext.Msg.OK,
                        icon: Ext.window.MessageBox.INFO
                    });

                    paymentgrid.getStore().load();
                } else {
                    me.mask('Подтверждение оплаты...', mainView);
                    params = {
                        bankDocumentImportId: me.getContextValue(mainView, 'bankDocumentImportId'),
                        importedPaymentIds: Ext.JSON.encode([paymentRecord.getId()])
                    };

                    me.sendRequest(params, 'AcceptInternalPayments'
                    ).next(function (resp) {
                        var dec = Ext.JSON.decode(resp.responseText);
                        Ext.Msg.alert('Информация', dec.message || 'Задача успешно поставлена в очередь на обработку. ' +
                                'Информация о статусе подтверждения платежей содержится в пункте меню "Задачи"');
                        me.unmask();
                        
                        paymentgrid.getStore().load();
                    }).error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Внимание', e.message || e);
                    });
                }

            }).error(function (err) {
                me.unmask();
                Ext.Msg.alert('Ошибка', err.message || err);
            });
        }
    },

    sendRequest: function (params, method) {
        return B4.Ajax.request({
            url: B4.Url.action(method, 'BankDocumentImport'),
            params: params,
            method: 'POST',
            timeout: 999999999
        });
    },

    show: function (id) {
        var me = this,
        view = me.getMainView() || Ext.widget('bankdocimporteditpanel');
        me.bindContext(view);
        me.application.deployView(view);
        me.getAspect('bankdocimporteditpanelaspect').setData(id);
        me.setContextValue(view, 'bankDocumentImportId', id);

        var store = view.down('grid').getStore();
        store.clearFilter(true);

        if (view.urlToken.indexOf('onlynotdefined') !== -1) {
            var headerFilterPlugin = view.down('grid').getPlugin('headerFilter');
            if (headerFilterPlugin) {
                var determinationStateField = headerFilterPlugin.fields.PersonalAccountDeterminationState,
                    confirmationStateField = headerFilterPlugin.fields.PaymentConfirmationState;

                if (determinationStateField) {
                    determinationStateField.setValue(B4.enums.PersonalAccountDeterminationState.NotDefined);
                }
                if (confirmationStateField) {
                    confirmationStateField.setValue(B4.enums.PaymentConfirmationState.NotDistributed);
                }

                view.urlToken = view.urlToken.replace('onlynotdefined', '');
            }
        }

        store.filter('bankDocumentImportId', id);
    },




    filterEditWindowCheck: function (cb) {
        var me = this,
            grid = cb.up('bankdocimporteditpanel gridpanel'),
            store = grid.getStore(),
            showRsNotEqual = grid.down('[name=RsNotEqual]').checked,
            window = cb.up('bankdocimporteditpanel');

        store.clearFilter(true);
        store.filter([
            { property: 'showRsNotEqual', value: showRsNotEqual },
            { property: 'bankDocumentImportId', value: me.getContextValue(window, 'bankDocumentImportId') }
        ]);
    },

    onChangeShowInActivePa: function(cb) {
        var me = this,
            grid = cb.up('bankdocumentimportpagrid gridpanel'),
            store = grid.getStore();

        store.clearFilter(true);
        store.filter([
            { property: 'showAll', value: cb.checked }
        ]);
    },

    onCellClickRefund: function (p1, td, cellIndex, rec, p4, p5, eOpts) {
        var text;
        var tip = Ext.create('Ext.tip.ToolTip', {
            dismissDelay: 0          
        });

        if (cellIndex === 15) {

            var confirmationState = rec.get('PaymentConfirmationState');
            if (confirmationState !== 70) {
                return;
            }

            var paymentType = rec.get('PaymentType');

            if (paymentType === 70) {
                text = 'Невозможно подтвердить, так как баланс базового кошелька + кошелька по тарифу решения  ЛС меньше, чем сумма возврата средств';
            } else if (paymentType === 80) {
                text = 'Невозможно подтвердить, так как баланс кошелька  ЛС  по пени меньше, чем сумма возврата средств';
            }         
        }

        if (text) {
            tip.html = text;
            tip.showAt(eOpts.xy);
        }
    }
});