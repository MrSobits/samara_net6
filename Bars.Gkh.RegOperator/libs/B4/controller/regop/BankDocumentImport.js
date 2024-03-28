Ext.define('B4.controller.regop.BankDocumentImport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.PersonalAccountDeterminationState',
        'B4.enums.PaymentConfirmationState',
        'B4.enums.ImportedPaymentPaymentConfirmState',
        'B4.enums.ImportedPaymentPersAccDeterminateState',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.data.Connection'
    ],

    stores: [
        'regop.BankDocumentImport'
    ],

    views: [
        'regop.bankdocumentimport.Grid',
        'regop.bankdocumentimport.ImportWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'regop.bankdocumentimport.Grid',
    mainViewSelector: 'bankdocumentimportgrid',

    aspects: [
        {
            xtype: 'gkhbuttonimportaspect',
            name: 'bankDocumentImportAspect',
            buttonSelector: 'bankdocumentimportgrid #btnImport',
            codeImport: 'BankDocument',
            windowImportView: 'regop.bankdocumentimport.ImportWindow',
            windowImportSelector: '#imporWindow',
            maxArchiveSize: 1048576 * 30, //bytes, 1mb * 30,
            archiveExtensions: ['rar', '7z', 'zip'],
            getUserParams: function () {
                var me = this;
                var combo = me.windowImport.down('[name="providerCode"]');
                if (combo) {
                    me.params = me.params || {};
                    me.params.providerCode = combo.getValue();
                    me.params.serializerCode = combo.getRecord(combo.getValue()).Serializer;
                    me.params.overwrite = me.windowImport.down('[name="selectOverwrite"]').getValue();
                    me.params.fsGorodCode = me.windowImport.down('[name="fsGorodCode"]').getValue();
                    me.params.distrPenalty = me.windowImport.down('[name=DistributePenalty]').getValue();
                }
            },
            refreshData: function () {
                this.controller.getStore('regop.BankDocumentImport').load();
            },
            getWindowImportView: function (importId) {
                if (importId === 'Bars.Gkh.RegOperator.Imports.SocialSupport.SocialSupportImport') {
                    return 'import.SocialSupportImportWindow';
                }
                return false;
            },
            isArchive: function(extension) {
                var me = this;

                if (!extension || !me.archiveExtensions) {
                    return false;
                }
                
                return me.archiveExtensions.indexOf(extension) !== -1;
            },
            onSaveClick: function () {
                var me = this,
                    formImport = me.windowImport.getForm();
                
                if (formImport.isValid()) {
                    me.getUserParams();
                    me.params.importId = me.importId;

                    var files = me.windowImport.files;
                    if (!files || files.length === 0) {
                        Ext.Msg.alert('Ошибка загрузки', 'Не выбраны файлы для импорта');
                        return;
                    }

                    if (me.maxArchiveSize) {
                        for (var i = 0; i < files.length; i++) {
                            var file = files[i];
                            if (me.isArchive(file.extension) && file.data.size > me.maxArchiveSize) {
                                Ext.Msg.alert('Ошибка загрузки', 'Файл ' + file.name + ' превышает допустимый размер 30мб. Выберите другой файл');
                                return;
                            }
                        }
                    }

                    var formData = new FormData();
                    for (var i = 0; i < files.length; i++) {
                        formData.append(files[i].name, files[i].data);
                    }

                    me.mask('Загрузка данных', me.windowImport);

                    var connection = Ext.create('B4.data.Connection');
                    connection.request({
                        url: B4.Url.action('/BankDocumentImport/Import'),
                        method: 'POST',
                        timeout: me.timeout,
                        params: me.params,
                        rawData: formData,
                        success: function (response) {
                            var decoded = Ext.decode(response.responseText);
                            
                            me.refreshData(me.params.importId);
                            me.unmask();
                            var message;
                            if (!Ext.isEmpty(decoded.message)) {
                                if (!Ext.isEmpty(decoded.title)) {
                                    message = decoded.title + ' <br/>';
                                } else {
                                    message = '';
                                }

                                message += decoded.message;
                            } else if (!Ext.isEmpty(decoded.title)) {
                                message = decoded.title;
                            } else {
                                message = '';
                            }

                            message = message + ' <br/>' + 'Закрыть окно загрузки?';

                            if (decoded.status == 40) {
                                Ext.Msg.alert(decoded.title, decoded.message);
                            } else {
                                Ext.Msg.confirm('Успешная загрузка', message, function (confirmationResult) {
                                    if (confirmationResult == 'yes') {
                                        me.closeWindow();
                                    } else {
                                        var grid = me.windowImport.down('gridpanel');

                                        me.windowImport.files = [];
                                        grid.getStore().removeAll();
                                    }
                                }, me);
                            }

                        },
                        failure: function (response) {
                            var decoded = Ext.decode(response.responseText);

                            me.refreshData(me.params.importId);
                            me.unmask();
                            var message = '';

                            if (!Ext.isEmpty(decoded.title)) {
                                message = decoded.title;
                            }

                            if (!Ext.isEmpty(decoded.message)) {

                                if (!Ext.isEmpty(message)) {
                                    message = message + ' <br/>';
                                }

                                message = message + decoded.message;
                            }

                            Ext.Msg.alert('Ошибка загрузки', message);
                        }
                    });
                } else {
                    //получаем все поля формы
                    var fields = formImport.getFields();

                    var invalidFields = '';

                    //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<br>' + field.fieldLabel;
                        }
                    });

                    //выводим сообщение
                    Ext.Msg.alert('Ошибка загрузки', 'Не заполнены обязательные поля: ' + invalidFields);
                }
            }
        },
        {
            xtype: 'grideditformaspect',
            name: 'BasePersonalAccountEditFormAspect',
            gridSelector: 'bankdocumentimportgrid',
            storeName: 'regop.BankDocumentImport',
            modelName: 'regop.BankDocumentImport',
            editRecord: function (record) {
                Ext.History.add('bank_doc_import_details/' + record.get('Id') + '/');
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.Accounts.BankDocumentImport.Accept',
                    applyTo: 'button[action=Accept]',
                    selector: 'bankdocumentimportgrid',
                    applyBy:
                        function (component, allowed) {
                            this.setVisible(component, allowed);
                        }
                },
                {
                    name: 'GkhRegOp.Accounts.BankDocumentImport.Field.PersonalAccountNumber',
                    applyTo: 'textfield[name=PersonalAccountNumber]',
                    selector: 'bankdocumentimportgrid',
                    applyBy:
                        function (component, allowed) {
                            this.setVisible(component, allowed);
                        }
                },
                {
                    name: 'GkhRegOp.Accounts.BankDocumentImport.Field.PaymentDate',
                    applyTo: 'datefield[name=PaymentDate]',
                    selector: 'bankdocumentimportgrid',
                    applyBy:
                        function (component, allowed) {
                            this.setVisible(component, allowed);

                            // если скрыты оба поля - скрываем панель
                            if (!component.up().down('PersonalAccountNumber') && !allowed) {
                                this.setVisible(component.up(), false);
                            } else {
                                this.setVisible(component.up(), true);
                            }
                        }
                }
            ]
        }
    ],
    init: function () {
        var me = this;
        me.control({
            'bankdocumentimportgrid button[action=Accept]': { 'click': { fn: me.onClickAccept } },
            'bankdocumentimportgrid button[action=Cancel]': { 'click': { fn: me.onClickCancel } },
            'bankdocumentimportgrid button[action=Delete]': { 'click': { fn: me.onClickDelete } },
            'bankdocumentimportgrid button[action=Check]': { 'click': { fn: me.onClickCheck } },
            'bankdocumentimportgrid checkbox[name="CheckShowConfirmed"]': { change: { fn: me.filterCheck, scope: me } },
            'bankdocumentimportgrid checkbox[name="CheckShowDeleted"]': { change: { fn: me.filterCheck, scope: me } },
            'bankdocumentimportgrid checkbox[name="CheckShowRegisters"]': { change: { fn: me.filterCheck, scope: me } },
            '[name=bankdocumentimportfilesgrid] [action=AddFiles]': { click: { fn: me.onAddFilesClick, scope: me } },
            '[name=bankdocumentimportfilesgrid] [name=FileImport]': { change: { fn: me.onFileImportChange, scope: me } },
            '[name=bankdocumentimportfilesgrid] b4deletecolumn': { click: { fn: me.onFileDeleteClick, scope: me } },
            'bankdocumentimportgrid button[action=Search]': { click: { fn: me.filterCheck, scope: me } },
            'bankdocumentimportgrid textfield[name=PersonalAccountNumber]': { enterclickevent: { fn: me.filterCheck, scope: me } },
            'bankdocumentimportgrid datefield[name=PaymentDate]': { enterclickevent: { fn: me.filterCheck, scope: me } },
            'bankdocumentimportgrid': { beforeheaderfiltersapply: { fn: me.beforeheaderfiltersapply } },
        });
        me.callParent(arguments);
    },

    prepareStates: function (item, states) {
        var padState = item.get('PersonalAccountDeterminationState');
        if (padState === 10) {
            states.isPersonalNonDistr = true;
        } else if (padState === 20) {
            states.isPersonalPartDistr = true;
        } else if (padState === 30) {
            states.isPersonalDistr = true;
        } else {
            states.isPersonalWrong = true;
        }

        var pcState = item.get('PaymentConfirmationState');
        if (pcState === 10) {
            states.isPaymentNonDistr = true;
        } else if (pcState === 20) {
            states.isPaymentPartDistr = true;
        } else if (pcState === 30) {
            states.isPaymentDistr = true;
        } else if (pcState === 40) {
            states.isPaymentDel = true;
        } else if (pcState === 50) {
            states.isPaymentWaitConfirm = true;
        } else if (pcState === 60) {
            states.isPaymentWaitCancell = true;
        } else if (pcState === 70) {
            states.isPaymentConImpossible = true;
        } else {
            states.isPaymentWrong = true;
        }
    },

    deletePayments: function (sender, grid, docIds, store, msg) {
        var me = sender;
        Ext.Msg.alert({
            title: 'Удаление реестра оплат',
            msg:  msg+ ' На статусе "Удален" операции по подтверждению оплат будут невозможны. Применить?' ,
            buttons: Ext.Msg.OKCANCEL,
            fn: function (btnId) {
                var params;
                if (btnId === "ok") {
                    me.mask('Удаление реестра оплат...', grid);
                    params = {
                        packetIds: Ext.JSON.encode(docIds)
                    };
                    me.sendRequest(params, 'DeletePayments'
                    ).next(function (resp) {
                        var dec = Ext.JSON.decode(resp.responseText);
                        Ext.Msg.alert('Информация', dec.message);
                        me.unmask();
                        store.load();
                    }).error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Внимание', e.message || e);
                    });
                }
            }
        });
    },

    onClickCheck: function(btn) {
        var me = this,
            grid = btn.up('bankdocumentimportgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            uncheckable,
            docIds = [];

        Ext.each(selected,
            function(item) {
                var states = {};
                me.prepareStates(item, states);
                if (!states.isPaymentDistr && !states.isPaymentPartDistr) {
                    uncheckable = true;
                } else {
                    docIds.push(item.getId());
                }
            });

        if (uncheckable) {
            if (docIds.length == 0) {
                Ext.Msg.alert('Информация',
                    'Выбранные реестры не подтверждены. Проверить возможно только подтвержденные или частично подтвержденные записи');
                return;
            } else {
                Ext.Msg.alert('Информация',
                    'Некоторые из выбранных реестров не подтверждены и не будут проверены. Проверить возможно только подтвержденные или частично подтвержденные реестры');
            }
        }

        me.mask('Проверка...', grid);

        me.sendRequest({
                    packetIds: Ext.encode(docIds)
                },
                'CheckPayments'
            )
            .next(function(resp) {
                var dec = Ext.decode(resp.responseText);
                Ext.Msg.alert('Информация',
                    dec.message ||
                    'Задача успешно поставлена в очередь на обработку. ' +
                    'Информация о статусе подтверждения оплат содержится в пункте меню "Задачи"');
                me.unmask();
                store.load();
            })
            .error(function(e) {
                me.unmask();
                Ext.Msg.alert('Внимание', e.message || e);
            });
    },

    onClickDelete: function (btn) {
        var me = this,
            grid = btn.up('bankdocumentimportgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = "",
            states = {};

        var docIds = [];
        Ext.each(selected, function (item) {
            me.prepareStates(item, states);
            docIds.push(item.getId());
        });
        if ((states.isPaymentWaitConfirm || states.isPaymentDistr || states.isPaymentNonDistr || states.isPaymentPartDistr || states.isPaymentDel) && states.isPaymentWaitCancell) {
            Ext.Msg.alert('Информация', ' Среди выбранных реестров оплат имеются реестры, которые находятся в процессе отмены подтверждения (раздел "Задачи"). Удаление для таких реестров невозможно');
            return;
        }

        if (states.isPaymentWaitConfirm && states.isPaymentWaitCancell) {
            Ext.Msg.alert('Информация', ' Выбранные реестры находятся в процессе подтверждения и отмены подтверждения. Удаление невозможно');
            return;
        }
        if (states.isPaymentWaitCancell) {
            Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе отмены подтверждения. Удаление невозможно');
            return;
        }
        if (states.isPaymentWaitConfirm && !states.isPaymentDistr && !states.isPaymentNonDistr && !states.isPaymentPartDistr && !states.isPaymentDel) {
            Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе подтверждения. Удаление невозможно.');
            return;
        } else if (states.isPaymentDistr || states.isPaymentPartDistr) {
            Ext.Msg.alert({
                title: 'Информация',
                msg: 'Операция возможна только для неподтвержденных реестров оплат. Отменить подтверждение оплат?',
                buttons: Ext.Msg.OKCANCEL,
                fn: function(btnId) {
                    if (btnId === "ok") {
                        me.cancelPayments(me, grid, docIds, store, me.etePayments);
                    }
                }
            });
            return;
        } else if ((states.isPersonalDistr || states.isPersonalNonDistr || states.isPersonalPartDistr) && states.isPaymentNonDistr && !states.isPaymentDistr && !states.isPaymentPartDistr) {
            if (states.isPaymentWaitConfirm) {
                msg = 'Среди выбранных реестров оплат имеются реестры, которые находятся в процессе подтверждения (раздел "Задачи"). Удаление для таких реестров невозможно ' + msg;
            }
            me.deletePayments(me, grid, docIds, store, msg);
        } else if (states.isPaymentConImpossible) {
            me.deletePayments(me, grid, docIds, store, msg);    
        } else {
            Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе отмены подтверждения. Удаление невозможно.');
        }
    },

    onClickCancel: function(btn) {
        var me = this,
            grid = btn.up('bankdocumentimportgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Отменить подтверждение для выбранных реестров оплат?',
            states = {},
            docIds = [];

        Ext.each(selected, function(item) {
            me.prepareStates(item, states);
            docIds.push(item.getId());
        });


        if (states.isPaymentWrong || states.isPersonalWrong) {
            Ext.Msg.alert('Информация', 'В списке реестра оплат есть запись с некорректным статусом определения лс или подтверждения оплаты. Отмена подтверждения реестра оплат невозможна');
            return;
        }
        if (states.isPaymentWaitCancell && !states.isPaymentNonDistr && !states.isPaymentPartDistr && !states.isPaymentDistr && !states.isPaymentDel && !states.isPaymentWaitConfirm ) {
            Ext.Msg.alert('Информация', 'Выбранные реестры уже находятся в процессе отмены подтверждения. Процесс можно отследить в разделе "Список задач"');
            return;
        }
        if (states.isPaymentWaitConfirm && !states.isPaymentPartDistr && !states.isPaymentNonDistr && !states.isPaymentDistr && !states.isPaymentDel && !states.isPaymentWaitConfirm) {
            Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе подтверждения. Отмена подтверждения невозможна');
            return;
        } else if (states.isPaymentDel) {
            Ext.Msg.alert('Информация', 'В списке реестра оплат есть запись со статусом "Удален". Отмена подтверждения реестра оплат невозможна');
            return;
        } else if ((states.isPersonalDistr || states.isPersonalPartDistr || states.isPersonalNonDistr)
                && !states.isPaymentDistr && !states.isPaymentPartDistr && states.isPaymentNonDistr) {
            /*все не подтверждены и (либо определены, либо частично определены, либо не определены)*/
            Ext.Msg.alert('Информация', 'Выбранные записи не подтверждены');
            return;
        } else if ((states.isPaymentDistr || states.isPaymentPartDistr) && states.isPaymentNonDistr) {
            msg = 'Один или несколько реестров не подтверждены, повторная отмена подтверждения для них выполнена не будет. ' + msg;
        }
        if (states.isPaymentWaitConfirm) {
            Ext.Msg.alert('Информация', 'Среди выбранных реестров оплат имеются реестры, которые находятся в процессе подтверждения (раздел "Задачи").'
                + ' Отмена подтверждения для таких реестров невозможна');
        }
        Ext.Msg.alert({
            title: 'Отмена оплат',
            msg: msg,
            buttons: Ext.Msg.OKCANCEL,
            fn: function (btnId) {
                if (btnId === "ok") {
                    me.cancelPayments(me, grid, docIds, store);
                }
            }
        });
    },

    cancelPayments: function (sender, grid, docIds, store, callback) {
        var tempPacketIds = [],
            me = sender,
            selected = grid.getSelectionModel().getSelection();
        Ext.each(selected, function (item) {
            // выбираем все записы из выбранных пользователем, где Статус определения ЛС = Defined и Статус подтверждения оплат = NotDistributed или PartiallyDistributed
            if ((item.get('PersonalAccountDeterminationState') === 20 || item.get('PersonalAccountDeterminationState') === 30)
                && (item.get('PaymentConfirmationState') === 30 || item.get('PaymentConfirmationState') === 20)) {
                tempPacketIds.push(item.getId());
            }
        });
        me.mask('Отмена оплат...', grid);
        me.params = {
            packetIds: Ext.JSON.encode(tempPacketIds)
        };

        me.sendRequest(params, 'CancelPayments'
        ).next(function(resp) {
            var dec = Ext.JSON.decode(resp.responseText);
            Ext.Msg.alert('Информация', dec.message || "Задача успешно поставлена в очередь на обработку. " +
                'Информация о статусе подтверждения оплат содержится в пункте меню "Задачи"');
            me.unmask();
            store.load();
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Внимание', e.message || e);
        });
    },

    onClickAccept: function (btn) {
        var me = this,
            grid = btn.up('bankdocumentimportgrid'),
            store = grid.getStore(),
            selected = grid.getSelectionModel().getSelection(),
            msg = 'Подтвердить оплаты?',
            isFirst = true,
            previousDeterminationState,
            previousConfirmationState,
            allIdenticalStates = true,
            allConfirmationStateNotDistributed = true,
            allConfirmationStateWaitingConfirmation = false,
            allDeterminationStateDefined = true,
            allConfirmationStateWaitingCancellation = false,
            allConfirmationStateNotDistributedOrDistributed = true;
        if (!selected || selected.length < 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать оплаты для подтверждения!', 'error');
        } else {
            Ext.each(selected, function (item) {
                // Статус определения ЛС
                var personalAccountDeterminationState = item.get('PersonalAccountDeterminationState');
                // Статус подтверждения оплат
                var paymentConfirmationState = item.get('PaymentConfirmationState');

                // проверка, что все состояния одинаковы для выбранных элементов
                // для первого раза - ничего не делаем. Меняем состояние
                if (isFirst) {
                    isFirst = false;
                } else {
                    // если это не первый проход
                    // если у нас все состояния одинаковы(до этого значение не менялось), тогда ->
                    if (allIdenticalStates) {
                        // сравнивание текущие значения состояний и предыдущие, и запоминаем это в булевую переменную
                        allIdenticalStates = previousDeterminationState === personalAccountDeterminationState && previousConfirmationState === paymentConfirmationState;
                    }
                }
                // запоминаем текущие состояния в предыдущие
                previousDeterminationState = personalAccountDeterminationState;
                previousConfirmationState = paymentConfirmationState;

                // проверяем, являются ли все Статусы подтверждения оплат = WaitingConfirmation
                if (paymentConfirmationState === B4.enums.PaymentConfirmationState.WaitingConfirmation) {
                    allConfirmationStateWaitingConfirmation = true;
                }
                if (paymentConfirmationState === B4.enums.PaymentConfirmationState.WaitingCancellation) {
                    allConfirmationStateWaitingCancellation = true;
                }
                // проверяем, являются ли все Статусы подтверждения оплат = NotDistributed
                if (paymentConfirmationState !== B4.enums.PaymentConfirmationState.NotDistributed) {
                    allConfirmationStateNotDistributed = false;
                }
                // проверяем, являются ли все Статусы определения ЛС = Defined
                if (personalAccountDeterminationState !== B4.enums.PersonalAccountDeterminationState.Defined) {
                    allDeterminationStateDefined = false;
                }
                // проверяем, являются ли все Статусы подтверждения оплат = NotDistributed или Статусы определения ЛС = Defined
                if (paymentConfirmationState !== B4.enums.PaymentConfirmationState.NotDistributed || paymentConfirmationState !== B4.enums.PersonalAccountDeterminationState.Defined) {
                    allConfirmationStateNotDistributedOrDistributed = false;
                }
            });

            // если статусы реестров равны 
            if (allIdenticalStates) {
                // если Статус подтверждения оплат = WaitingConfirmation
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.WaitingConfirmation) {
                    Ext.Msg.alert('Информация', 'Выбранные реестры уже находятся в процессе подтверждения. Процесс можно отследить в разделе "Список задач"');
                    return;
                }
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.WaitingCancellation) {
                    Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе отмены подтверждения. Подтверждение невозможно');
                    return;
                }

                // если Статус подтверждения оплат = Distributed и Статус определения ЛС = Defined
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.Distributed && previousDeterminationState === B4.enums.PersonalAccountDeterminationState.Defined) {
                    Ext.Msg.alert('Информация', 'Реестр оплат(ы) уже подтвержден(ы)');
                    return;
                }
                // если Статус подтверждения оплат = Deleted
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.Deleted) {
                    Ext.Msg.alert('Информация', 'Реестры оплат(ы) в статусе "Удален". Подтверждение оплат(ы) невозможно');
                    return;
                }
                // если Статус подтверждения оплат = NotDistributed и Статус определения ЛС = NotDefined
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.NotDistributed && previousDeterminationState === B4.enums.PersonalAccountDeterminationState.NotDefined) {
                    Ext.Msg.alert('Информация', 'ЛС реестра не определен(ы). Подтверждение оплат(ы) невозможно');
                    return;
                }
                // если Статус подтверждения оплат = ConnfirmationImpossible
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.ConfirmationImpossible) {
                    Ext.Msg.alert('Информация', 'Реестры оплат(ы) в статусе "Невозможно подтвердить". Подтверждение оплат(ы) невозможно');
                    return;
                }
            } else {
                // если статусы для выбранных реестров разные
                if (previousConfirmationState === B4.enums.PaymentConfirmationState.WaitingConfirmation && previousConfirmationState === B4.enums.PaymentConfirmationState.WaitingCancellation) {
                    Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе подтверждения и отмены подтверждения. Подтверждение невозможно');
                    return;
                }
                if (allConfirmationStateWaitingConfirmation && !allConfirmationStateWaitingCancellation) {
                    Ext.Msg.alert('Информация', 'Среди выбранных реестров оплат имеются реестры, которые находятся в процессе подтверждения (раздел "Задачи")');
                }
                if (allConfirmationStateWaitingCancellation && !allConfirmationStateWaitingConfirmation) {
                    Ext.Msg.alert('Информация', 'Среди выбранных реестров оплат имеются реестры, которые находятся в процессе отмены подтверждения (раздел "Задачи").');
                }
                if (allConfirmationStateWaitingCancellation && allConfirmationStateWaitingConfirmation) {
                    Ext.Msg.alert('Информация', 'Выбранные реестры находятся в процессе подтверждения и отмены подтверждения. Подтверждение невозможно.');
                    return;
                }

                //если все Статусы подтверждения оплат = NotDistributed
                if (allConfirmationStateNotDistributed) {
                    msg = "Оплаты с неопределенными ЛС не могут быть подтверждены.  Оплаты с определенными ЛС будут подтверждены. Подтвердить оплаты?";
                }
                    //если все Статус определения ЛС = Defined и Статус подтверждения оплат = NotDistributed Or Distributed
                else if (allDeterminationStateDefined && allConfirmationStateNotDistributedOrDistributed) {
                    msg = "Одна или несколько оплат уже подтверждены, повторное подтверждение для них выполнено не будет. Подтвердить выбранные оплаты?";
                } else {
                    msg = "Оплаты с неопределенными ЛС не могут быть подтверждены. Оплаты с определенными ЛС будут подтверждены. Одна или несколько оплат уже подтверждены, повторное подтверждение для них выполнено не будет. Подтвердить оплаты?";
                }
            }

            Ext.Msg.prompt({
                title: 'Подтверждение оплат',
                msg: msg,
                multiline: 1,
                buttons: Ext.Msg.OKCANCEL,
                fn: function (btnId, text) {
                    var params;
                    if (btnId === "ok") {
                        var tempPacketIds = [];
                        Ext.each(selected, function (item) {
                            // выбираем все записы из выбранных пользователем, где Статус определения ЛС = Defined и Статус подтверждения оплат = NotDistributed или PartiallyDistributed
                            if ((item.get('PersonalAccountDeterminationState') === 20 || item.get('PersonalAccountDeterminationState') === 30)
                                && (item.get('PaymentConfirmationState') === 10 || item.get('PaymentConfirmationState') === 20)) {
                                tempPacketIds.push(item.getId());
                            }
                        });
                        me.mask('Подтверждение оплат...', grid);
                        params = {
                            packetIds: Ext.JSON.encode(tempPacketIds),
                            cprocName: text
                        };

                        me.sendRequest(params, 'AcceptPayments'
                        ).next(function (resp) {
                            var dec = Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Информация', dec.message || "Задача успешно поставлена в очередь на обработку. " +
                                'Информация о статусе подтверждения оплат содержится в пункте меню "Задачи"');
                            me.unmask();
                            store.load();
                        }).error(function (e) {
                            me.unmask();
                            Ext.Msg.alert('Внимание', e.message || e);
                        });
                    }
                }
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

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('bankdocumentimportgrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getAspect('bankDocumentImportAspect').loadImportStore();
        me.getStore('regop.BankDocumentImport').load();
    },


    filterCheck: function (cb) {
        var grid = cb.up('bankdocumentimportgrid'),
            store = grid.getStore(),
            showConfirmed = grid.down('[name=CheckShowConfirmed]').checked,
            showDeleted = grid.down('[name=CheckShowDeleted]').checked,
            showRegisters = grid.down('[name=CheckShowRegisters]').checked,
            personalAccountNumber = grid.down('[name=PersonalAccountNumber]').value,
            paymentDate = grid.down('[name=PaymentDate]').value;

        store.clearFilter(true);
        store.filter([
            { property: 'showConfirmed', value: showConfirmed },
            { property: 'showDeleted', value: showDeleted },
            { property: 'showRegisters', value: showRegisters },
            { property: 'personalAccountNumber', value: personalAccountNumber },
            { property: 'paymentDate', value: paymentDate }
        ]);
    },

    onAddFilesClick: function (el) {
        var fileImport = el.up('gridpanel').down('[name=FileImport]');
        fileImport.fileInputEl.dom.click();
    },

    onFileImportChange: function(el) {
        var files = el.fileInputEl.dom.files;
        var grid = el.up('[name=bankdocumentimportfilesgrid]'),
            store = grid.getStore();

        var form = grid.up();
        form.files = form.files || [];

        var fileNames = [];
        for (var i = 0; i < files.length; i++) {
            if (!store.findRecord('FileName', files[i].name)) {
                fileNames.push({ FileName: files[i].name });
                form.files.push({
                    name: files[i].name,
                    extension: files[i].name.split('.')[1],
                    data: files[i]
                });
            }
        }

        store.add(fileNames);
        el.fileInputEl.dom.value = null;
    },

    onFileDeleteClick: function(gridView, cell, rowIndex, cellIndex, e, record) {
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
            if (result === 'yes') {
                var grid = gridView.up(),
                    form = grid.up();

                if (form.files) {
                    form.files = Ext.Array.filter(form.files, function(file) {
                        return file.name !== record.get('FileName');
                    });
                }
                grid.getStore().remove(record);
            }
        });
    },

    beforeheaderfiltersapply: function(grid, filters, store) {
        this.filterCheck(grid.down());
        return false;
    }
});