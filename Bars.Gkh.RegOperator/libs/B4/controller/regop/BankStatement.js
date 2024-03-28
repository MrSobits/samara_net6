Ext.define('B4.controller.regop.BankStatement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.deliveryagent.Navigation',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GkhButtonImportBankStatementAspect',
        'B4.aspects.GeneralStateHistory',
        'B4.aspects.ButtonDataExport',
        'Ext.grid.feature.Summary',
        'B4.enums.MoneyDirection',
        'B4.enums.DistributionState'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'BankAccountStatementGroup',
        'BankAccountStatement'
    ],

    stores: [
        'BankAccountStatementGroup',
        'BankAccountStatement',
        'regop.BankDocumentImport',
        'DistributionDetail'
    ],

    views: [
        'regop.bankstatement.Grid',
        'regop.bankstatement.AddWindow',
        'regop.bankstatement.DistributionSelectWindow',
        'SelectWindow.MultiSelectWindow',
        'regop.bankstatement.DetailGrid',
        'regop.bankstatement.DetailWindow',
        'regop.bankstatement.SetDistributableWindow',
        'regop.bankstatement.ChangePaymentDetailsWindow',
        'regop.bankstatement.distributable_auto.DistributionDetailWinBase',
        'regop.bankstatement.distributable_auto.SpecialAccountDistributionGrid'
    ],

    refs: [
        {
            ref: 'addWindow',
            selector: 'rbankstatementaddwin'
        },
        {
            ref: 'mainView',
            selector: 'rbankstatementgrid'
        },
        {
            ref: 'distributeWindow',
            selector: 'bankstatementdistrwin'
        },
        {
            ref: 'detailWindow',
            selector: 'rbankstatementdetailwin'
        },
        {
            ref: 'setDistributableWindow',
            selector: 'setdistributablewin'
        },
        {
            ref: 'distributionDetailWin',
            selector: 'distributiondetailwinbase'
        },
        {
            ref: 'changePaymentDetailsWindow',
            selector: 'changepaymentdetailswin'
        }
    ],

    distributionSource: {
        BANK_STMT: 10,
        SUSP_ACC: 20
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'BankStatement',
            permissions: [
                {
                    name: 'GkhRegOp.Accounts.BankOperations.SetDistributable',
                    applyTo: 'rbankstatementgrid button[name=operation] menuitem[action=setdistributable]',
                    selector: 'rbankstatementgrid'
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectContragent_View',
                    applyTo: 'button[action=SelectContragent]',
                    selector: 'rbankstatementaddwin',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                        component.allowed = allowed;
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNum_View',
                    applyTo: 'button[action=SelectPayerAccountNum]',
                    selector: 'rbankstatementaddwin',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectRecipientContragent_View',
                    applyTo: 'button[action=SelectRecipientContragent]',
                    selector: 'rbankstatementaddwin',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                        component.allowed = allowed;
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectRecipientAccountNumOutcome_View',
                    applyTo: '[name=MoneyDirection]',
                    selector: 'rbankstatementaddwin',
                    applyOn: {
                        event: 'setperm'
                    },
                    applyBy: function (component, allowed) {
                        var moneyDirection = component.getValue(),
                            button = component.up('window').down('button[action=SelectRecipientAccountNum]');

                        if (moneyDirection === B4.enums.MoneyDirection.Outcome) {
                            button.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectRecipientAccountNumIncome_View',
                    applyTo: '[name=MoneyDirection]',
                    selector: 'rbankstatementaddwin',
                    applyOn: {
                        event: 'setperm'
                    },
                    applyBy: function (component, allowed) {
                        var moneyDirection = component.getValue(),
                            button = component.up('window').down('button[action=SelectRecipientAccountNum]');

                        if (moneyDirection === B4.enums.MoneyDirection.Income) {
                            button.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNumOutcome_View',
                    applyTo: '[name=MoneyDirection]',
                    selector: 'rbankstatementaddwin',
                    applyOn: {
                        event: 'setperm'
                    },
                    applyBy: function (component, allowed) {
                        var moneyDirection = component.getValue(),
                            button = component.up('window').down('button[action=SelectPayerAccountNum]');

                        if (moneyDirection === B4.enums.MoneyDirection.Outcome) {
                            button.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.SelectPayerAccountNumIncome_View',
                    applyTo: '[name=MoneyDirection]',
                    selector: 'rbankstatementaddwin',
                    applyOn: {
                        event: 'setperm'
                    },
                    applyBy: function (component, allowed) {
                        var moneyDirection = component.getValue(),
                            button = component.up('window').down('button[action=SelectPayerAccountNum]');

                        if (moneyDirection === B4.enums.MoneyDirection.Income) {
                            button.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.Accounts.BankOperations.Field.InternalCancel',
                    applyTo: 'button[action=InternalCancel]',
                    selector: 'rbankstatementdetailwin',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },

                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerName_Edit', applyTo: 'textfield[name=PayerName]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerAccountNum_Edit', applyTo: 'textfield[name=PayerAccountNum]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerInn_Edit', applyTo: 'textfield[name=PayerInn]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerBik_Edit', applyTo: 'textfield[name=PayerBik]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerKpp_Edit', applyTo: 'textfield[name=PayerKpp]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerCorrAccount_Edit', applyTo: 'textfield[name=PayerCorrAccount]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.PayerBank_Edit', applyTo: 'textfield[name=PayerBank]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },

                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientName_Edit', applyTo: 'textfield[name=RecipientName]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientAccountNum_Edit', applyTo: 'textfield[name=RecipientAccountNum]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientInn_Edit', applyTo: 'textfield[name=RecipientInn]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientBik_Edit', applyTo: 'textfield[name=RecipientBik]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientKpp_Edit', applyTo: 'textfield[name=RecipientKpp]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientCorr_Edit', applyTo: 'textfield[name=RecipientCorr]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } },
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientBank_Edit', applyTo: 'textfield[name=RecipientBank]', selector: 'rbankstatementaddwin', applyBy: function (component, allowed) { component.setReadOnly(!allowed); } }
            ]
        },
        {
            xtype: 'requirementaspect',
            viewSelector: 'rbankstatementgrid',
            requirements: [
                { name: 'GkhRegOp.Accounts.BankOperations.Field.RecipientAccountNum_Rqrd', applyTo: 'textfield[name=RecipientAccountNum]', selector: 'rbankstatementaddwin' }
            ]
        },
        {
            xtype: 'generalstatehistory',
            name: 'bankStatementDistributeStateAspect',
            gridSelector: 'rbankstatementgrid',
            stateCode: 'regop_bank_acc_statement'
        },
        {
            /*
            * Индивидуальный аспект для импорта в связи с доп. параметрами
            */
            xtype: 'gkhbuttonimportbankstatementaspect',
            name: 'statementimportaspect',
            buttonSelector: 'rbankstatementgrid gkhbuttonimport',
            ownerWindowSelector: 'rbankstatementgrid',
            codeImport: 'BankAccountStatementImport'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'BankStatementButtonExportAspect',
            gridSelector: 'rbankstatementgrid',
            buttonSelector: 'rbankstatementgrid [action="ExportToExcel"]',
            controllerName: 'BankAccountStatement',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'linkdocumentaspect',
            gridSelector: 'rbankstatementgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#rbankstatementlinkmultiselectwin',
            storeSelect: 'regop.BankDocumentImport',
            storeSelected: 'regop.BankDocumentImport',
            titleSelectWindow: 'Выбор документов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            addButtonSelector: 'rbankstatementgrid  button[name=operation] menuitem[action=link]',
            rightGridConfig: {
                features: [{
                    ftype: 'summary',
                    id: 'summary'
                }],
                listeners: {
                    render: function (grid) {
                        grid.getStore().on('datachanged', function () {
                            grid.getView().refresh();
                        });
                    }
                }
            },
            toolbarItems: [
                { xtype: 'textfield', readOnly: true, name: 'DocumentSum', fieldLabel: 'Сумма', labelWidth: 33 }
            ],
            columnsGridSelect: [
                {
                    xtype: 'datecolumn',
                    text: 'Дата сводного реестра',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                { text: 'Номер сводного реестра', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                {
                    text: 'Сумма по реестру',
                    dataIndex: 'ImportedSum',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    }
                },
                { text: 'Наименование платежного агента', dataIndex: 'PaymentAgentName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                {
                    xtype: 'datecolumn',
                    text: 'Дата сводного реестра',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    flex: 1,
                    summaryRenderer: function () {
                        return Ext.String.format('Сумма:');
                    }
                },
                { text: 'Номер сводного реестра', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                {
                    text: 'Сумма по реестру',
                    dataIndex: 'ImportedSum',
                    flex: 1,
                    renderer: function (val) { return val ? Ext.util.Format.currency(val) : null; },
                    summaryType: 'sum'
                }
            ],
            onBeforeLoad: function (store, operation) {
                var rec = this.controller.getMainView().getSelectionModel().getLastSelected();

                operation.params['statementId'] = rec.getId();
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                'beforegridaction': function (asp, grid, action) {
                    var rec;

                    if (action.toLowerCase() !== 'add') {
                        return true;
                    }

                    var recCount = grid.getSelectionModel().getCount();

                    if (recCount == 0) {
                        B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
                        return false;
                    }

                    if (recCount > 1) {
                        B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать одну запись!', 'error');
                        return false;
                    }

                    return true;
                },
                'getdata': function (asp, records) {
                    var ids = [],
                        rec;

                    if (records.length === 0) {
                        B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать записи!', 'error');
                        return false;
                    }

                    Ext.each(records.items, function (r) {
                        ids.push(r.getId());
                    });

                    rec = asp.getGrid().getSelectionModel().getLastSelected();

                    B4.Ajax.request({
                        url: B4.Url.action('/BankAccountStatement/LinkDocument'),
                        params: {
                            statementId: rec.getId(),
                            docIds: Ext.encode(ids)
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        B4.QuickMsg.msg('Успешно', 'Привязка выполнена успешно', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Привязка невозможна!', 'error');
                    });

                    return false;
                },
                'panelrendered': function (asp, opts) {
                    var records = asp.controller.getMainView().getSelectionModel().getSelection(),
                        sums = Ext.Array.map(records, function (r) { return r.get('Sum'); }),
                        sum = Ext.Array.sum(sums);

                    opts.window.down('textfield[name="DocumentSum"]').setValue(Ext.util.Format.number(sum, '0.000.00'));
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'rbankstatementgrid b4addbutton': { 'click': { fn: me.onClickAdd, scope: me } },
            'rbankstatementaddwin b4savebutton': { 'click': { fn: me.onClickSave, scope: me } },
            'rbankstatementaddwin button[action=SelectContragent]': { 'click': { fn: me.onClickButton('PayerNameSelectField'), scope: me } },
            'rbankstatementaddwin button[action=SelectRecipientContragent]': { 'click': { fn: me.onClickButton('RecipientNameSelectField'), scope: me } },
            'rbankstatementaddwin button[action=SelectPayerAccountNum]':
                {
                    'click': {
                        fn: function (btn) {
                            var win = btn.up('window'),
                                moneyDirection = win.down('b4combobox[name=MoneyDirection]').getValue();
                            if (moneyDirection === B4.enums.MoneyDirection.Income) {
                                me.onClickButton('PayerAccountNumContragentSelectField')(btn);
                            } else {
                                me.onClickButton('PayerAccountNumSelectField')(btn);
                            }
                        },
                        scope: me
                    }
                },
            'rbankstatementaddwin button[action=SelectRecipientAccountNum]': {
                'click': {
                    fn: function (btn) {
                        var win = btn.up('window'),
                            moneyDirection = win.down('b4combobox[name=MoneyDirection]').getValue();
                        if (moneyDirection === B4.enums.MoneyDirection.Outcome) {
                            me.onClickButton('RecipientAccountContragentSelectField')(btn);
                        } else {
                            me.onClickButton('RecipientAccountSelectField')(btn);
                        }
                    }, scope: me
                }
            },
            'rbankstatementaddwin [name=MoneyDirection]': {
                'change': function (cmb, newValue) {
                    var me = this,
                        win = cmb.up('window'),
                        formRecipient = win.down('[type=Recipient]'),
                        formPayer = win.down('[type=Payer]'),
                        buttonRecipientContragent = win.down('button[action=SelectRecipientContragent]'),
                        buttonPayerContragent = win.down('button[action=SelectContragent]');

                    if (newValue === B4.enums.MoneyDirection.Income) {
                        me.loadRegopRequisits(formRecipient, 'Recipient', formPayer, 'Payer');
                    } else if (newValue === B4.enums.MoneyDirection.Outcome) {
                        me.loadRegopRequisits(formPayer, 'Payer', formRecipient, 'Recipient');
                    }

                    buttonRecipientContragent
                        .setVisible(buttonRecipientContragent.allowed && newValue === B4.enums.MoneyDirection.Outcome);

                    buttonPayerContragent
                        .setVisible(buttonPayerContragent.allowed && newValue === B4.enums.MoneyDirection.Income);

                    cmb.fireEvent('setperm', me);
                }
            },

            'rbankstatementgrid': { 'rowaction': { fn: me.onGridRowAction, scope: me } },
            'rbankstatementgrid button[action=distribute]': { 'click': { fn: me.onClickDistribute, scope: me } },
            'rbankstatementgrid button[action=cancel]': { 'click': { fn: me.onClickCancel, scope: me } },
            'rbankstatementgrid button[action=delete]': { 'click': { fn: me.onClickCancelPayment, scope: me } },

            'rbankstatementdistrselectwin button[action=selectdistribution]': { 'click': { fn: me.onSelectDistribution, scope: me } },

            'rbankstatementgrid checkbox[name=ShowDistributed]': { 'change': function () { me.getMainView().getStore().load(); } },
            'rbankstatementgrid checkbox[name=ShowDeleted]': { 'change': function () { me.getMainView().getStore().load(); } },
            'rbankstatementgrid b4selectfield[name=bsAccountNumber]': { 'change': function () { me.getMainView().getStore().load(); } },

            'rbankstatementdetailgrid': { 'render': function (grid) { grid.getStore().load(); } },

            'distributiondetailwinbase button[action=acceptDistribution]': { 'click': { fn: me.onClickAcceptAutoDistribute, scope: me } },

            'setdistributablewin b4savebutton': { 'click': { fn: me.onSetDistributable, scope: me } },
            'changepaymentdetailswindow b4savebutton': { 'click': { fn: me.onChangePaymentDetails, scope: me } },
            'rbankstatementgrid  button[name=operation] menuitem': { 'click': { fn: me.onClickButtonOperation, scope: me } },

            'rbankstatementdetailwin button[action=InternalCancel]': { 'click': { fn: me.onClickCancelInternal, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function (params) {
        var me = this,
            view = me.getMainView(),
            bsAccountNumber;

        if (!view) {
            view = Ext.widget('rbankstatementgrid');

            view.getStore().on('beforeload', function (s, oper) {
                oper.params['showDistributed'] = view.down('checkbox[name=ShowDistributed]').getValue();
                oper.params['showDeleted'] = view.down('checkbox[name=ShowDeleted]').getValue();
                oper.params['specAccountIds'] = view.down('b4selectfield[name=bsAccountNumber]').getValue();
                var datef = view.down('#dfDateFrom');
                var notRenderedVal = datef.getValue();
                var renderedVal = Ext.Date.format(notRenderedVal, 'd-m-Y');
               // oper.params['dateReceiptFrom'] = view.down('[name=DateReceiptFrom]').getValue();
                oper.params['dateReceiptFrom'] = renderedVal;
                var notRenderedDateTo = view.down('[name=DateReceiptBy]').getValue();
                oper.params['dateReceiptBy'] = Ext.Date.format(notRenderedDateTo, 'd-m-Y');
               // oper.params['dateReceiptBy'] = view.down('[name=DateReceiptBy]').getValue();
            });
        }

        if (view) {
            bsAccountNumber = view.down('[name=bsAccountNumber]');
            if (bsAccountNumber) {
                bsAccountNumber.getStore().on('beforeload', function (s, oper) {
                    var lastParams = view.getStore().lastOptions.params;
                    oper.params.showDistributed = lastParams.showDistributed;
                    oper.params.showDeleted = lastParams.showDeleted;
                    oper.params.complexFilterBs = lastParams.complexFilter;
                    oper.params.dateReceiptFrom = lastParams.dateReceiptFrom;
                    oper.params.dateReceiptBy = lastParams.dateReceiptBy;
                });
            }

        }

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();

        me.getAspect('statementimportaspect').loadImportStore();

        if (params && params.Id) {
            var model = me.getModel('BankAccountStatement');
            model.load(params.Id, {
                success: function (rec) {
                    me.onGridRowAction(view, 'edit', rec);
                },
                scope: me
            });
        }
    },

    onClickAdd: function () {
        var me = this,
            win = me.getAddWindow(),
            model = me.getModel('BankAccountStatement'),
            form;

        if (!win) {
            win = Ext.widget('rbankstatementaddwin', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.getMainView().add(win);
            form = win.getForm();
            form.loadRecord(new model());
            form.updateRecord();
            form.isValid();
            win.show();
        }
    },

    onClickButton: function (name) {
        return function (btn) {
            var sfPayerNameSelectField = btn.up('fieldset').down('b4selectfield[name=' + name + ']');

            if (sfPayerNameSelectField) {
                sfPayerNameSelectField.onTrigger1Click();
            }
        };
    },

    onClickAcceptAutoDistribute: function (btn) {
        var me = this,
            win = btn.up('window'),
            params = win.getDistributionParams();
        debugger;
        me.mask('Подтверждение распределения', me.getMainView());

        B4.Ajax.request({
            url: B4.Url.action('/Distribution/Validate'),
            params: params
        }).next(function () {
            win.close();
            me.continueSoftValidating(params);
        }).error(function (e) {
            debugger;
            Ext.Msg.confirm('Внимание',
                'Выявлено несоответствие Р/С по выбранным лицевым счетам',
                function (result) {
                    if (result === 'yes') {
                        win.close();
                        me.continueSoftValidating(params);
                    }
                    else {
                        win.close();
                        me.unmask();
                        B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
                    }
                });
           
        });
    },

    continueSoftValidating: function (params) {
        var me = this;

        B4.Ajax.request({
            url: B4.Url.action('SoftValidate', 'Distribution'),
            method: 'POST',
            params: params,
            timeout: 2 * 60 * 60 * 1000 // 2 часа
        }).next(function () {
            me.continueAutoDistribution(params);
        }).error(function (res) {
            Ext.Msg.confirm('Внимание', (res.message || 'Ошибка при распределении!') + ' Применить распределение?', function (result) {
                if (result === 'yes') {
                    me.continueAutoDistribution(params);
                } else {
                    me.unmask();
                }
            });
        });
    },

    continueAutoDistribution: function (params) {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('/Distribution/Apply'),
            timeout: 10 * 60 * 1000, // 10 минут
            params: params
        }).next(function (response) {
            var resp = Ext.decode(response.responseText);
            me.unmask();
            me.getMainView().getStore().load();
            B4.QuickMsg.msg('Результат', resp.message || 'Распределение успешно произведено', 'success');
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
        });
    },

    onClickSetDistributable: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            rec = grid.getSelectionModel().getLastSelected(),
            recsSelected = grid.getSelectionModel().getSelection().length,
            win;

        if (recsSelected !== 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать ровно одну запись', 'error');
            return;
        }

        win = me.getSetDistributableWindow();
        if (!win) {
            win = Ext.widget('setdistributablewin', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.getMainView().add(win);
            var form = win.getForm();
            form.loadRecord(rec);
            win.show();
        }
    },

    onClickChangePaymentDetails: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            rec = grid.getSelectionModel().getLastSelected(),
            recsSelected = grid.getSelectionModel().getSelection().length,
            win;

        if (recsSelected !== 1) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать ровно одну запись', 'error');
            return;
        }

        win = me.getChangePaymentDetailsWindow();
        if (!win) {
            win = Ext.widget('changepaymentdetailswindow', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                renderTo: B4.getBody().getActiveTab().getEl()
            });

            me.getMainView().add(win);
            form = win.getForm();
            form.loadRecord(rec);
            win.show();
        }
    },

    onSetDistributable: function (btn) {
        var win = btn.up('window'),
            form = win.getForm(),
            rec = form.getRecord();

        form.updateRecord();
        win.mask();
        rec.save().next(function () {
            win.unmask();
            win.close();
        }).error(function (e) {
            win.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Не удалось сохранить запись', 'error');
        });
    },

    onChangePaymentDetails: function (btn) {
        var win = btn.up('window'),
            form = win.getForm(),
            rec = form.getRecord();

        form.updateRecord();
        win.mask();
        rec.save().next(function () {
            win.unmask();
            win.close();
        }).error(function (e) {
            win.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Не удалось сохранить запись', 'error');
        });
    },

    onClickDistribute: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            recs = grid.getSelectionModel(),
            recsSelected = grid.getSelectionModel().getSelection(),
            anyError = false,
            jurInn = 0,
            dates = [];

        if (!recs || recsSelected.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        if (recsSelected.length === 1) {
            jurInn = recsSelected[0].raw.PayerInn;
        }

        if (jurInn == null || jurInn == undefined || jurInn == '') {
            jurInn = 0;
        }

        Ext.each(recsSelected,
            function (rec) {
                if (recsSelected.length === 1 && rec.get('DistributeState') === B4.enums.DistributionState.Distributed) {
                    B4.QuickMsg.msg('Ошибка', 'Нельзя распределить запись дважды!', 'error');
                    anyError = true;
                    return false;
                };

                if (rec.get('DistributeState') === B4.enums.DistributionState.Deleted) {
                    B4.QuickMsg.msg('Ошибка', 'Нельзя распределить удаленную запись!', 'error');
                    anyError = true;
                    return false;
                }

                if (rec.get('DistributeState') === B4.enums.DistributionState.WaitingDistribution) {
                    B4.QuickMsg.msg('Ошибка', 'Распределение уже выполняется!', 'error');
                    anyError = true;
                    return false;
                };

                if (rec.get('DistributeState') === B4.enums.DistributionState.WaitingCancellation) {
                    B4.QuickMsg.msg('Ошибка', 'Запись в процессе отмены, распределение невозможно!', 'error');
                    anyError = true;
                    return false;
                };

                if (rec.get('DistributeState') !== B4.enums.DistributionState.NotDistributed && rec.get('DistributeState') !== B4.enums.DistributionState.PartiallyDistributed) {
                    B4.QuickMsg.msg('Ошибка', 'Запись должна быть в статусе "Не распределен" или "Частично распределен"!',
                        'error');
                    anyError = true;
                    return false;
                }

                if (rec.get('MoneyDirection') === B4.enums.MoneyDirection.Outcome && recsSelected.length > 1) {
                    B4.QuickMsg.msg('Ошибка', 'Массовое распределение по расходным операциям невозможно!',
                        'error');
                    anyError = true;
                    return false;
                }

                dates.push(rec.get('DateReceipt'));
                return true;
            });

        if (anyError) {
            return;
        }

        me.mask('Проверка дат распределения', me.getMainView());

        B4.Ajax.request({
            url: B4.Url.action('/Distribution/CheckDate'),
            params: {
                dates: Ext.JSON.encode(dates)
            }
        }).next(function () {
            me.unmask();
            me.continueCheckIn(recsSelected, jurInn);
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message || 'Распределение невозможно', 'error');
        });
    },

    onSelectDistribution: function (btn) {
        var me = this,
            win = btn.up('window'),
            cbx = win.down('[name=DistributionType]'),
            route = cbx.getValue(),
            detailWin = me.getDistributionDetailWin(),
            rec = cbx.getStore().findRecord('Name', cbx.getRawValue(), 0, false, false, true),
            recRaw = rec.raw;

        if (!route) {
            B4.QuickMsg.msg('Ошибка', 'Выберите тип распределения!', 'error');
            return;
        }

        //if (win.distributionIds.length > 1 && !recRaw.DistributableAutomatically) {
        //    B4.QuickMsg.msg('Ошибка', 'Распределение по данному типу возможно только для одного документа!', 'error');
        //    return;
        //}

        if (!recRaw.DistributableAutomatically) {
            me.application.redirectTo(Ext.String.format('{0}/{1}&{2}&{3}&{4}&{5}',
                route,
                win.distributionIds,
                recRaw.Code,
                ('' + win.sum).replace('.', 'dot'),
                me.distributionSource.BANK_STMT,
                win.inn));
        } else {
            if (detailWin) {
                detailWin.destroy();
            }

            detailWin = Ext.widget('distributiondetailwinbase',
            {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                distributionIds: win.distributionIds,
                distributionSource: me.distributionSource.BANK_STMT,
                code: recRaw.Code,
                renderTo: B4.getBody().getActiveTab().getEl(),
                gridAlias: route
            });

            detailWin.show();
            detailWin.down('grid').getStore().load();
        }

        win.close();
    },

    continueCheckIn: function (recs, jurInn) {
        var me = this,
            win = me.getDistributeWindow(),
            allSum = 0,
            ids = [];

        if (win && !win.getBox().width) {
            win = win.destroy();
        }

        Ext.each(recs, function (rec) {
            ids.push(rec.get('Id'));
        });

        Ext.each(recs, function (rec) {
            allSum += rec.get('RemainSum');
        });

        if (!win) {
            win = Ext.widget('rbankstatementdistrselectwin', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                distributionIds: ids,
                distributionSource: 10,
                permissionNs: 'GkhRegOp.Accounts.BankOperations.Distributions',
                sum: allSum,
                inn: jurInn,
                renderTo: B4.getBody().getActiveTab().getEl()
            });
        }

        win.show();
        win.getForm().isValid();
    },

    onClickSave: function (btn) {
        var me = this,
            win = btn.up('window'),
            form = win.getForm(),
            rec;
        form.updateRecord();
        rec = form.getRecord();

        if (form.isValid()) {
            me.mask('Сохранение', B4.getBody().getActiveTab());
            form.submit({
                url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                params: {
                    records: Ext.encode([rec.getData()])
                },
                success: function () {
                    me.unmask();
                    win.close();
                    B4.QuickMsg.msg('Успешно', 'Успешно сохранено!', 'success');
                },
                failure: function (frm, action) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                }
            });
        }
    },

    onClickCancel: function () {
        var me = this,
            recIds = [],
            cancelMsg = 'Отменить распределения?',
            recs = me.getMainView().getSelectionModel().getSelection();

        if (recs.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Необходимо выбрать запись!', 'error');
            return;
        }

        Ext.each(recs, function (rec) {
            if (rec.get('DistributeState') === B4.enums.DistributionState.Distributed || rec.get('DistributeState') === B4.enums.DistributionState.PartiallyDistributed) {
                recIds.push(rec.get('Id'));
            }
        });

        if (recIds.length === 0) {
            B4.QuickMsg.msg('Ошибка', 'Запись должна быть в статусе "Распределен" или "Частично распределен"!', 'error');
            return;
        }

        if (recIds.length != recs.length) {
            cancelMsg = 'Выбраны записи с разными статусами. Отмена распределения будет выполнена только для записей со статусами "Распределена", "Частично распределена". Отменить распределения?';
        }

        Ext.Msg.confirm('Отмена распределения', cancelMsg, function (btnId) {
            if (btnId === 'yes') {
                me.mask('Отмена распределения...');
                B4.Ajax.request({
                    url: B4.Url.action('Undo', 'Distribution'),
                    method: 'POST',
                    params: {
                        distributionIds: Ext.JSON.encode(recIds),
                        distributionSource: 10
                    },
                    timeout: 9999999
                }).next(function (response) {
                    var resp = Ext.decode(response.responseText);
                    Ext.Msg.alert('Результат', resp.message || 'Распределение отменено');
                    me.getMainView().getStore().load();
                    me.unmask();
                }).error(function (response) {
                    var message = 'Ошибка отмены распределения.';
                    if (response && response.message) {
                        message += response.message;

                        var rows = me.getMainView().getSelectionModel().getSelection();
                        Ext.each(rows, function (row) {
                            if (row.get('SuspenseAccount')) {
                                return false;
                            }
                        });
                    }

                    Ext.Msg.alert('Ошибка', message);

                    me.unmask();
                });
            }
        });
    },

    onClickCancelInternal: function () {
        var me = this,
            recIds = [],
            cancelMsg = 'Отменить распределения?',
            win = me.getDetailWindow(),
            grid = win.down('rbankstatementdetailgrid'),
            recs = grid.getSelectionModel().getSelection();

        if (recs.length === 0) {
            Ext.Msg.confirm('Отмена распределения', cancelMsg, function (btnId) {
                if (btnId === 'yes') {
                    me.mask('Отмена распределения...', win);
                    B4.Ajax.request({
                        url: B4.Url.action('Undo', 'Distribution'),
                        method: 'POST',
                        params: {
                            distributionIds: Ext.JSON.encode(win.entityId),
                            distributionSource: 10
                        },
                        timeout: 10 * 60 * 1000
                    }).next(function (response) {
                        var resp = Ext.decode(response.responseText);
                        Ext.Msg.alert('Результат', resp.message || 'Распределение отменено');
                        grid.getStore().load();
                        me.unmask();
                    }).error(function (response) {
                        var message = 'Ошибка отмены распределения.';
                        if (response && response.message) {
                            message += response.message;

                            var rows = me.getMainView().getSelectionModel().getSelection();
                            Ext.each(rows, function (row) {
                                if (row.get('SuspenseAccount')) {
                                    return false;
                                }
                            });
                        }

                        Ext.Msg.alert('Ошибка', message);

                        me.unmask();
                    });
                }
            });
            return;
        }

        Ext.each(recs,
            function (rec) {
                recIds.push(rec.get('Id'));
            });

        Ext.Msg.confirm('Отмена распределения', cancelMsg, function (btnId) {
            if (btnId === 'yes') {
                me.mask('Отмена распределения...', win);
                B4.Ajax.request({
                    url: B4.Url.action('UndoPartially', 'Distribution'),
                    method: 'POST',
                    params: {
                        distributionIds: Ext.JSON.encode(win.entityId),
                        detailIds: Ext.JSON.encode(recIds),
                        distributionSource: 10
                    },
                    timeout: 10 * 60 * 1000
                }).next(function (response) {
                    me.unmask();
                    var resp = Ext.decode(response.responseText);
                    Ext.Msg.alert('Результат', resp.message || 'Распределение отменено');
                    grid.getStore().load();
                }).error(function () {
                    Ext.Msg.alert('Ошибка', 'Ошибка отмены распределения.');

                    win.unmask();
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
            if (r.get('DistributeState') !== B4.enums.DistributionState.Deleted) {
                recordsAlreadyDeleted = false;
            }


            if (r.get('DistributeState') === B4.enums.DistributionState.NotDistributed
                || r.get('DistributeState') === B4.enums.DistributionState.Distributed
                    || (r.get('DistributeState') === B4.enums.DistributionState.PartiallyDistributed)) {
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
            B4.QuickMsg.msg('Ошибка', 'Операция возможна только для записей со статусами "Не распределена/Распределена/Частично распределена"!', 'error');
            return;
        }

        var cancelFn = function (ids) {
            me.mask('Отмена зачисления...');
            B4.Ajax.request({
                url: B4.Url.action('UndoOperationOrUndoCheckin', 'Distribution'),
                method: 'POST',
                timeout: 5 * 60 * 60 * 1000, // 5 часов
                params: {
                    distributionIds: Ext.JSON.encode(ids),
                    distributionSource: 10
                }
            }).next(function () {
                Ext.Msg.alert('Результат', 'Успешное удаление');
                me.getMainView().getStore().load();
                me.unmask();
            }).error(function (response) {
                var message = 'Ошибка удаления';
                if (response && response.message) {
                    message = response.message;
                }

                Ext.Msg.alert('Ошибка', message);

                me.unmask();
            });
        };

        var confirmFn = function () {
            Ext.Msg.show({
                title: 'Отмена зачисления',
                msg: 'При удалении выписка в статусе "Распределена" и "Частично распределена" будет отменена. Продолжить?',
                buttons: Ext.Msg.OKCANCEL,
                icon: Ext.window.MessageBox.INFO,
                fn: function (btnId) {
                    if (btnId === 'ok') {
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
                msg: 'Выбраны записи с разными статусами. Операция будет произведена только для записей со статусом "Не распределен". Отменить распределение?',
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

    onGridRowAction: function (grid, action, rec) {
        var me = this,
            distrCode = rec.get('DistributionCode'),
            distr, win, cmb, cmbStore;

        if (action.toLowerCase() !== 'edit') {
            return;
        }

        if (rec.get('DistributeState') !== B4.enums.DistributionState.Distributed && rec.get('DistributeState') !== B4.enums.DistributionState.PartiallyDistributed) {
            B4.QuickMsg.msg('Ошибка', 'Детализация существует только по распределенным записям!', 'error');
            return;
        }

        win = me.getDetailWindow();

        if (!win) {
            win = Ext.widget('rbankstatementdetailwin', {
                ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : '',
                renderTo: B4.getBody().getActiveTab().getEl(),
                entityId: rec.getId(),
                source: 10
            });
        }

        cmb = win.down('[name=DistributionType]');

        win.down('rbankstatementdetailgrid').down('gridcolumn[dataIndex=PaymentAccount]')
            .setVisible(rec.get('MoneyDirection') === B4.enums.MoneyDirection.Income);

        cmbStore = cmb.getStore();

        cmbStore.load();

        cmbStore.on('load', function (store, records) {
            if (distrCode) {
                Ext.each(distrCode.split(','), function (code) {

                    Ext.each(records, function (rec) {
                        if (rec.get('Code') === code) {
                            if (distr) {
                                distr = Ext.String.format('{0}, {1}', distr, rec.get('Name'));
                            } else {
                                distr = rec.get('Name');
                            }
                        }
                    });
                });

                cmb.setValue(distr);
            }
        });

        win.show();
    },

    loadRegopRequisits: function (form, prefix, formClear, prefixClear) {
        var me = this;

        me.mask('Загрузка...', B4.getBody().getActiveTab());

        B4.Ajax.request(
            B4.Url.action('/RegOperator/GetCurrentRegop')
        ).next(function (response) {
            var obj, tfName, tfInn, tfKpp;
            me.unmask();
            obj = Ext.decode(response.responseText);

            if (obj) {
                tfName = form.down('[name=' + prefix + 'Name]');
                tfInn = form.down('[name=' + prefix + 'Inn]');
                tfKpp = form.down('[name=' + prefix + 'Kpp]');

                if (Ext.isEmpty(tfName.getValue())) {
                    tfName.setValue(obj.Name);
                }

                if (Ext.isEmpty(tfInn.getValue())) {
                    tfInn.setValue(obj.Inn);
                }

                if (Ext.isEmpty(tfKpp.getValue())) {
                    tfKpp.setValue(obj.Kpp);
                }

                if (!Ext.isEmpty(formClear)) {
                    formClear.down('[name=' + prefixClear + ']').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Name]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'AccountNum]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Inn]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Kpp]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Bik]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Corr]').setValue(null);
                    formClear.down('[name=' + prefixClear + 'Bank]').setValue(null);
                }
            }
        }).error(function () {
            me.unmask();
        });
    },
    onClickButtonOperation: function (item) {
        var me = this,
            action = item.action.toLowerCase();

        switch (action) {
            case 'setdistributable':
                me.onClickSetDistributable(item);
                break;
            case 'changepaymentdetails':
                me.onClickChangePaymentDetails(item);
                break;
        }

    }
});