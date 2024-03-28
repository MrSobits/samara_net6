Ext.define('B4.aspects.regop.personal_account.action.SplitAccount', {
    extend: 'B4.aspects.GridEditWindow',

	alias: 'widget.splitaccountaspect',

	targetAccountsForSplit: null,
	roomId: null,
	ownershipTypeNew: null,
	globalSavePayments: null,

    requires: [
        'B4.model.regop.personal_account.SplitAccountInfo',
        'B4.model.regop.personal_account.BasePersonalAccount',

        'B4.store.regop.owner.IndividualAccountOwner',

        'B4.view.regop.personal_account.action.PersonalAccountSplit.AddWindow',
        'B4.view.regop.personal_account.action.PersonalAccountSplit.MainWindow',
        'B4.view.regop.personal_account.action.PersonalAccountSplit.AccountsGrid',
        'B4.view.regop.personal_account.action.PersonalAccountSplit.DistributionWindow',
        'B4.view.regop.personal_account.action.PersonalAccountSplit.DistributionAccountGrid',

        'B4.enums.regop.PersonalAccountOwnerType',
		'B4.enums.regop.SplitAccountDistributionType',
		'B4.enums.RoomOwnershipType', /**/

        'Ext.grid.Panel', 
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    gridSelector: 'personalaccountsplitaccountsgrid',
    editFormSelector: 'personalaccountsplitaddwindow',
    modelName: 'regop.personal_account.SplitAccountInfo',
    editWindowView: 'regop.personal_account.action.PersonalAccountSplit.AddWindow',
    mainWindowSelector: 'personalaccountsplitwin',
    distributionWindowSelector: 'splitdistributionwindow',
    accountOperationCode: 'PersonalAccountSplitOperation',

    init: function (controller) {
        var me = this;

        me.callParent(arguments);
        controller.control({
            'personalaccountsplitaddwindow b4combobox[name=OwnerType]': { 'change' : { fn : me.onChangeOwnerType, scope: me } },
            'personalaccountsplitaddwindow b4selectfield[name=Contragent]': { 'change' : { fn : me.onSelectContragent, scope: me } },
            'personalaccountsplitaddwindow button[action=selectOwner]': { 'click' : { fn : me.onClickSelectOnwer, scope: me } },

            'personalaccountsplitwin b4savebutton': { 'click' : { fn : me.onContinueSplitting, scope: me } },

            'splitdistributionwindow button[action=GetDebtSums]': { 'click' : { fn : me.onCalcDebtClick, scope: me } },
            'splitdistributionwindow combobox[name=DistributionType]': { 'change' : { fn : me.onChangeDistributionType, scope: me } },
            'splitdistributionwindow checkbox[name=SavePayments]': { 'change' : { fn : me.onCalcDebtClick, scope: me } },
            'splitdistributionwindow b4savebutton': { 'click' : { fn : me.onApplyDistribution, scope: me } }
        });
    },

    listeners: {
        beforegridaction: function(asp, grid, action) {
            var me = this;

            if (!me.getMainWindow().down('[name=SplitDate]').isValid()) {
                Ext.Msg.alert('Ошибка', 'На заполено обязательное поле: Дата разделения');
                return false;
            }

            return true;
        },
        
        beforerowaction: function(asp, grid, action, rec) {
            return !rec.getId() && action === 'delete';
        },
        
        beforesetformdata: function(asp, rec, form) {
            rec.set('OpenDate', this.getMainWindow().down('[name=SplitDate]').getValue());
        },

        getdata: function(asp, rec) {
			var ownerType = rec.get('OwnerType'),
                contragent = this.getForm().down('[name=Contragent]').value;

            switch(ownerType) {
                case B4.enums.regop.PersonalAccountOwnerType.Individual:
                    rec.set(
                        'OwnerName', 
                        Ext.String.format('{0} {1} {2}', 
                            rec.get('Surname'),
                            rec.get('FirstName'),
                            rec.get('SecondName'))
                        .trim());
                    break;

                case B4.enums.regop.PersonalAccountOwnerType.Legal:
                    rec.set('OwnerName', contragent && contragent.ShortName);
                    break;
            }

            rec.set('AccountOwner', asp.getForm().ownerId);
        }
    },

    showMainWindow: function(accountId) {
        var me = this,
            win = me.getMainWindow(),
            form = win.getForm().getForm(),
            store = win.getStore(),
            persAccModel = me.controller.getModel('regop.personal_account.BasePersonalAccount'),
            model = me.getModel(),
            record;

        me.controller.mask('Загрузка...', me.controller.getMainView());
        persAccModel.load(accountId, {
            success: function(rec) {
				me.controller.unmask();
				RoomId = rec.get('Room').Id
                win.down('[name=SplitDate]').setMinValue(new Date(rec.get('OpenDate')));

                form.loadRecord(rec);
                form.isValid();
                record = model.create(rec.getData());
                record.set('NewAreaShare', record.get('AreaShare'));

                store.add(record);
                win.show();

                B4.Ajax.request({
                    url: B4.Url.action('GetTotalAreaShare', 'Room'),
                    params: {
                        id: rec.get('Room').Id
                    }
                }).next(function(resp) {
                    var obj = Ext.decode(resp.responseText);
                    form.totatArea = obj.data;
                });
            },
            failure: function(rec, operation) {
                me.controller.unmask();
                Ext.Msg.alert('Ошибка', 'Ошибка при загрузке лицевого счета');
            }
        });
    },

	onContinueSplitting: function () {
		var me = this,
            win = me.getMainWindow(),
            formPanel = win.getForm(),
            form = formPanel.getForm(),
            record = form.getRecord(),
            totalAreaShareExceptSourceAccount = form.totatArea - record.get('AreaShare'),
            store = win.getStore(),
			targetAccounts = store.data.items,
			targetAccountsForSplit = store.data.items,
            areaSum = store.getSum(targetAccounts, 'NewAreaShare') + totalAreaShareExceptSourceAccount,
            distrWin,
            splitDate = win.down('[name=SplitDate]').getValue(),
            successValidation = true,
			callback;
		if (targetAccountsForSplit.length  && targetAccountsForSplit.length >1)
		{
			roomId = targetAccountsForSplit[0].get('Room').Id;
			ownershipTypeNew = targetAccountsForSplit[1].get('OwnershipTypeNewLs');
		}

        if (!form.isValid()) {
            var errorMessage = me.getFormErrorMessage(formPanel);
            Ext.Msg.alert('Ошибка!', errorMessage);
            return;
        }

        if (!targetAccounts || !targetAccounts.length || targetAccounts.length < 2) {
            Ext.Msg.alert('Ошибка!', 'Необходимо добавить хотя бы один лицевой счет');
            return;
        }

        if (areaSum > 1) {
            Ext.Msg.alert('Ошибка!', Ext.String.format('Сумма долей собственности по текущему помещению превышает допустимое значение!<br>' +
                '(Текущая сумма долей собственности с учетом всех ЛС помещения равна: {0})', areaSum));
            return;
        }

        Ext.each(targetAccounts, function(account) {
            if (!account.getId() && ( !account.get('NewAreaShare') || account.get('NewAreaShare') <= 0)) {
                successValidation = false;
                return false;
            }
        });

        if (!successValidation) {
            Ext.Msg.alert('Ошибка!', 'Необходимо заполнить новые доли собственности');
            return;
        }

        callback = function() {
            
            if (win.down('[name=CloseCurrent]').checked && targetAccounts[0].get('NewAreaShare') !== 0) {
                Ext.Msg.alert('Ошибка!', 'Доля собственности закрываемого лицевого счета должна быть равна нулю!');
                return;
            }

            distrWin = Ext.widget(me.distributionWindowSelector, { prevWindow: win, store: store });
            distrWin.down('[name=SplitDate]').setValue(splitDate);
            distrWin.mainRecord = record;

            me.onCalcDebtClick();
            win.hide();
            distrWin.show();
        }
        
        if (win.down('[name=CloseCurrent]').checked) {
            Ext.Msg.confirm('Подтвердите действия',
                'Текущий лицевой счет будет закрыт. Продолжить?',
                function(result) {
                    if (result === 'yes') {
                        if (targetAccounts[0].get('NewAreaShare') !== 0) {
                            // ставим таймаут, т.к. при закрытии confirm'а
                            // ломается состояние окна alert
                            setTimeout(function(){
                                Ext.Msg.alert('Ошибка!', 'Доля собственности закрываемого лицевого счета должна быть равна нулю!');
                            }, 100);
                            return;
                        }
                        callback.call(me);
                    }
                });
        } else {
            callback.call(me);
        }
    },

    onChangeOwnerType: function(cb, newValue) {
        var me = this,
            metas = B4.enums.regop.PersonalAccountOwnerType.getItemsMeta(),
            formView = me.getForm(),
            ownerBtn = formView.down('button[action=selectOwner]');

        ownerBtn.setVisible(newValue === B4.enums.regop.PersonalAccountOwnerType.Individual);

        Ext.each(metas, function(meta) {
            var view = formView.down(Ext.String.format('container[name={0}]', meta.Name));

            view.setVisible(meta.Value === newValue);

            view.query('field').forEach(function(field) {
                field.setValue(null);
                field.allowBlank = meta.Value !== newValue || !field.required;
            });
        });

        formView.getForm().isValid();
    },

    onClickSelectOnwer: function() {
        var me = this,
            store =  Ext.create('B4.store.regop.owner.IndividualAccountOwner'),
            gridCfg = {
                xtype: 'gridpanel',
                store: store,
                selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', { mode: 'SINGLE' }),
                columns: [
                    {
                        xtype: 'gridcolumn',
                        dataIndex: 'Name',
                        header: 'Наименование',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    }
                ],
                plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                dockedItems: [
                    {
                        xtype: 'pagingtoolbar',
                        displayInfo: true,
                        store: store,
                        dock: 'bottom'
                    }
                ],
                listeners: {
                    'itemdblclick': function(grid, record) {
                        me.onSelectIndividualOwner(me, record);
                        grid.up('window').close();
                    }
                }
            },
            wndConfig = {
                height: 500,
                width: 600,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                layout: 'fit',
                title: 'Выбор абонента',
                closeAction: 'destroy',
                items: [ gridCfg ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'buttongroup',
                                columns: 1,
                                items: [
                                    {
                                        xtype: 'button',
                                        text: 'Выбрать',
                                        iconCls: 'icon-accept',
                                        handler: function(btn) {
                                            var rec = btn.up('window').down('gridpanel').getSelectionModel().getSelection();

                                            if (!rec || rec.length == 0) {
                                                Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                                return;
                                            }

                                            rec = rec[0];
                                            if (Ext.isEmpty(rec)) {
                                                Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                                return;
                                            }

                                            me.onSelectIndividualOwner(me, rec);
                                            btn.up('window').close();
                                        },
                                        scope: me
                                    }
                                ]
                            }, '->', {
                                xtype: 'buttongroup',
                                columns: 1,
                                items: [
                                    {
                                        xtype: 'button',
                                        text: 'Закрыть',
                                        iconCls: 'icon-decline',
                                        handler: function(btn) {
                                            btn.up('window').close();
                                        },
                                        scope: me
                                    }
                                ]
                            }
                        ]
                    }
                ]
            };

        Ext.create('Ext.window.Window', wndConfig).show();
        store.load();
    },

    onSelectIndividualOwner: function(asp, rec) {
        var me = this,
            form = me.getForm(),
            firstNameTf = form.down('[name=FirstName]'),
            surnameTf = form.down('[name=Surname]'),
            secondNameTf = form.down('[name=SecondName]');

        firstNameTf.setReadOnly(true);
        surnameTf.setReadOnly(true);
        secondNameTf.setReadOnly(true);

        firstNameTf.setValue(rec.get('FirstName'));
        surnameTf.setValue(rec.get('Surname'));
        secondNameTf.setValue(rec.get('SecondName'));

        form.ownerId = rec.getId();
    },

    onSelectContragent: function(sf, newValue) {
        var me = this,
            inn = newValue && newValue.Inn,
            kpp = newValue && newValue.Kpp,
            form = me.getForm();

        form.down('[name=Inn]').setValue(inn);
        form.down('[name=Kpp]').setValue(kpp);
    },

    getMainWindow: function() {
        var me = this,
            win;
        if (this.mainWindowSelector) {
            win = this.componentQuery(this.mainWindowSelector);

            if (!win) {
                win = Ext.widget(me.mainWindowSelector);
            }
        }

        return win;
    },

    getDistributionWindow: function() {
        var me = this,
            win;
        if (this.distributionWindowSelector) {
            win = this.componentQuery(this.distributionWindowSelector);

            if (!win) {
                win = Ext.widget(me.distributionWindowSelector);
            }
        }

        return win;
    },

	saveRecord: function (rec) {
        this.getGrid().getStore().add(rec);
        this.getForm().close();
    },
    
    deleteRecord: function(rec) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                me.getGrid().getStore().remove(rec);
            }
        }, me);
    },

    onCalcDebtClick: function() {
		var me = this,
			win = me.getDistributionWindow(),
			rec = win.mainRecord,
			store = win.getStore(),
			splitDate = win.down('[name=SplitDate]').getValue(),
			savePayments = win.down('checkbox[name=SavePayments]').checked,
			debtField = win.down('[name=DebtSum]'),
			gridRec = store.getById(rec.getId()),
			cbDistributionType = win.down('[name=DistributionType]');
		debugger;
		globalSavePayments = savePayments;
			

        me.controller.mask('Расчет долга...', win);
        me.getDebts(rec, splitDate, savePayments).next(function(debts) {
                var debtSum = debts.BaseTariffDebt + debts.DecisionTariffDebt + debts.PenaltyDebt;

                debtField.setValue(debtSum);
                win.debts = debts;

                for(var prop in debts) {
                    gridRec.set(prop, debts[prop]);
                }
            
                me.controller.unmask();

                me.onChangeDistributionType(cbDistributionType, cbDistributionType.getValue());
            })
        .error(function(msg) {
            me.controller.unmask();
            Ext.Msg.alert('Ошибка!', msg || 'Произошла ошибка при получении данных');
        });
    },

    getDebts: function(record, splitDate, savePayments) {
        var me = this,
            continuation = new Deferred(),
			callback = function (response) {
				globalSavePayments = savePayments;
                var resp = Ext.decode(response.responseText);
                continuation.call(resp.data);
            },
            errorCallback = function(response) {
                var resp = response && response.responseText && Ext.decode(response.responseText),
                    respMessage = response.message || (resp && resp.message);

                continuation.fail(respMessage);
            };

        B4.Ajax.request({
            url: B4.Url.action('GetOperationDataForUI', 'BasePersonalAccount'),
            timeout: 1000 * 60 * 10, // 10 минут
            params: {
                id: record.getId(),
                operationCode: me.accountOperationCode,
                operationType: 'GetDistributableDebts',
                splitDate: splitDate,
                savePayments: savePayments
            }
        })
        .next(callback)
        .error(errorCallback);

        return continuation;
    },

	onChangeDistributionType: function (cb, newValue) {

        var me = this,
            win = me.getDistributionWindow(),
            rec = win.mainRecord,
            debts = win.debts,
            store = win.getStore(),
            records = store.data.items,
            data = Ext.Array.map(records, function(rec) { return rec.getData(); }),
            distributionType = win.down('[name=DistributionType]').getValue();

        if (newValue !== B4.enums.regop.SplitAccountDistributionType.Manual) {
            me.controller.mask('Распределение...', win);    
            B4.Ajax.request({
                url: B4.Url.action('GetOperationDataForUI', 'BasePersonalAccount'),
                timeout: 1000 * 60 * 10, // 10 минут
                params: {
                    id: rec.getId(),
                    operationCode: me.accountOperationCode,
                    operationType: 'DistributeDebts',
                    debts: Ext.encode(debts),
					records: Ext.encode(data),
					savePayments: globalSavePayments,
                    distributionType: distributionType
                }
            })
            .next(function(response) {
                var records = Ext.decode(response.responseText).data;
                    
                store.removeAll();
                records.forEach(function(rec) {
                    store.add(me.getModel().create(rec));
                });
                me.controller.unmask();
                win.getGrid().getView().refresh(); // рефрешим грид, т.к. summaryRow тупит
            })
            .error(function(response) {
                var resp = response && response.responseText && Ext.decode(response.responseText),
                    respMessage = response.message || (resp && resp.message);

                me.controller.unmask();
                Ext.Msg.alert('Ошибка!', respMessage || 'Произошла ошибка во время распределения');
            });
        }
    },

    onApplyDistribution: function() {
        var me = this,
            win = me.getDistributionWindow(),
            rec = win.mainRecord,
            debts = win.debts,
            store = win.getStore(),
            records = store.data.items,
            data = Ext.Array.map(records, function(rec) { return rec.getData(); }),
            submitForm = me.getMainWindow().getForm().getForm(),
            closeCurrentAccount = me.getMainWindow().down('[name=CloseCurrent]').checked;

        if (Math.round(Ext.Array.sum(Ext.Array.map(data, function(el) { return el.NewBaseTariffDebt; }))*100)/100 !== debts.BaseTariffDebt) {
            Ext.Msg.alert('Ошибка!', 'Сумма распределения по базовому тарифу не совпадает с распределяемой задолженностью по базовому тарифу');
            return;
        }     

        if (Math.round(Ext.Array.sum(Ext.Array.map(data, function(el) { return el.NewDecisionTariffDebt; }))*100)/100 !== debts.DecisionTariffDebt) {
            Ext.Msg.alert('Ошибка!', 'Сумма распределения по тарифу решения не совпадает с распределяемой задолженностью по тарифу решения');
            return;
        }  

        if (Math.round(Ext.Array.sum(Ext.Array.map(data, function (el) { return el.NewPenaltyDebt; })) * 100) / 100 !== debts.PenaltyDebt) {
            Ext.Msg.alert('Ошибка!', 'Сумма распределения по пени не совпадает с распределяемой задолженностью по пени');
            return;
        }

		me.controller.mask('Разделение лицевого счета', win);
		mainwin = me.getMainWindow();
		mainstore = mainwin.getStore();
			targetAccountsForSplit = mainstore.data.items;
        submitForm.submit({
            url: B4.Url.action('ExecuteOperation', 'BasePersonalAccount'),
            timeout: 1000 * 60 * 5, // 5 минут
            params: {
                operationCode: me.accountOperationCode,
                debts: Ext.encode(debts),
                records: Ext.encode(data),
				closeCurrentAccount: closeCurrentAccount,
				RoomId: RoomId,
				savePayments: globalSavePayments,
				ownershipTypeNew: ownershipTypeNew
            },
            success: function(form, action) {
                win.prevWindow.destroy();
                win.destroy();

                me.controller.unmask();
                Ext.Msg.alert('Успешно', 'Разделение выполнено успешно');
            },
            failure: function(form, action) {
                var msg = action.response 
                        && action.response.responseText 
                        && Ext.decode(action.response.responseText).message;

                me.controller.unmask();
                Ext.Msg.alert('Ошибка!', msg || 'Ошибка во время выполнения операции');
            }
        });
    }
});