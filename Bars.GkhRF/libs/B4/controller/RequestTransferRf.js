Ext.define('B4.controller.RequestTransferRf', {
    /*
    * Контроллер раздела заявка на перечисление ден. средств
    */
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.RequestTransferRf',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.SelectField',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'transferrf.Request',
        'transferrf.Funds',
        'Contragent',
        'ManagingOrganization'
    ],
    stores: [
        'transferrf.Request',
        'transferrf.Funds',
        'transferrf.RealObjForSelect',
        'transferrf.RealObjForSelected',
        'transferrf.MunicipalityForSelect',
        'transferrf.MunicipalityForSelected',
        'transferrf.PersonalAccount',
        'contragent.Bank',
        'transferrf.PersonalAccount'
    ],
    views: [
        'transferrf.RequestEditWindow',
        'transferrf.RequestGrid',
        'SelectWindow.MultiSelectWindow',
        'transferrf.RequestPanel',
        'transferrf.FundsGrid'
    ],

    aspects: [
        /* пермишшен Заявки на перечисление средств по роли */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRf.RequestTransferRfViewCreate.Create', applyTo: 'b4addbutton', selector: '#requestTransferRfGrid' }
            ]
        },
        /* пермишшены по статусу Заявки на перечисление средств */
        {
            xtype: 'requesttransferrfstateperm',
            editFormAspectName: 'requestTransferRfGridEditWindow',
            setPermissionEvent: 'aftersetformdata',
            name: 'requestTransferRfPerm'
        },
        /* пермишшен по удалению записи Заявки на перечисление средств (по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhRf.RequestTransferRf.Delete' }],
            name: 'deleteRequestTransferRfStatePerm'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'requestTransferRfStateButtonAspect',
            stateButtonSelector: '#requestTransferRfEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('requestTransferRfGridEditWindow');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('transferrf.Request');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('requestTransferRfPerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('requestTransferRfPerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'requestTransferRfStateTransferAspect',
            gridSelector: '#requestTransferRfGrid',
            stateType: 'rf_request_transfer',
            menuSelector: 'requestTransferRfGridStateMenu'
        },
        {
            /*
            аспект взаимодействия таблицы заявок и окна редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'requestTransferRfGridEditWindow',
            gridSelector: '#requestTransferRfGrid',
            editFormSelector: '#requestTransferRfEditWindow',
            storeName: 'transferrf.Request',
            modelName: 'transferrf.Request',
            editWindowView: 'transferrf.RequestEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                /*
                * 1.Открываем грид если есть id родительской записи
                * 2.Проставляем статус
                */
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record); //1
                    this.controller.getAspect('requestTransferRfStateButtonAspect').setStateData(record.get('Id'), record.get('State')); //2
                }
               
            },
            otherActions: function (actions) {
                //вешаемся на события у поля контрагента банка
                actions[this.editFormSelector + ' #sfContragentBank'] = {
                    'enable': {
                        fn: this.onEnableContrBank,
                        scope: this
                    },
                    'change': {
                        fn: this.onChangeContrBank,
                        scope: this
                    }
                };
                actions[this.editFormSelector + ' #sfManagingOrganization'] = {
                    'change': {
                        fn: this.onChangeManOrg,
                        scope: this
                    }
                };
                actions['#requestTransferRfFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#requestTransferRfFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#requestTransferRfFilterPanel #tfMunicipality'] = { 'triggerClear': { fn: this.onClearMunicipalities, scope: this } };
                actions['#requestTransferRfFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions[this.editFormSelector + ' #btnEditContragentRf'] = { 'click': { fn: this.onBtnEditContragentClick, scope: this } };
                actions[this.editFormSelector + ' #btnEditContragentRf'] = { 'click': { fn: this.onBtnEditContragentClick, scope: this } };
                actions['#requestTransferRfEditWindow #transferFundsRfGrid'] = { 'beforeedit': { fn: this.onBeforeEditGrid, scope: this } };
                actions[this.editFormSelector + ' #sfContractRf'] = { 'enable': { fn: this.onEnableContractRf, scope: this } };
            },

            onChangeDateStart: function (field, newValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },

            onClearMunicipalities: function () {
                if (this.controller.params) {
                    this.controller.params.municipalities = null;
                }
            },

            onUpdateGrid: function () {
                var str = this.controller.getStore('transferrf.Request');
                str.currentPage = 1;
                str.load();

            },

            //при смене ук изменяем или чистим ее readonly поля (ИНН, КПП, Телефон)
            onChangeManOrg: function (obj, newValue, oldValue) {
                var editForm = this.getForm();
                if (editForm) {
                    var sfContragentBank = editForm.down('#sfContragentBank');
                    var sfContractRf= editForm.down('#sfContractRf');
                    var tfInn = editForm.down('#tfInn');
                    var tfKpp = editForm.down('#tfKpp');
                    var tfPhone = editForm.down('#tfPhone');

                    if (!Ext.isEmpty(newValue)) {

                        if (oldValue != newValue.Id) {
                            //запоминаем id выбранной упр. орг для дальнейшей передачи на сервер для получение банков контрагента текущей упр. орг.
                            //маноргу присваевам id контрагента
                            this.controller.ContragentId = newValue.ContragentId;
                            this.controller.ManagingOrganizationId = newValue.Id;
                            //заполняем readOnly поля реквизитами упр. орг-ии 
                            tfInn.setValue(newValue.ContragentInn);
                            tfKpp.setValue(newValue.ContragentKpp);
                            tfPhone.setValue(newValue.ContragentPhone);

                            //Делаем доступным для выбора поле банк контрагента
                            sfContragentBank.enable();
                            sfContractRf.enable();

                            //Поменяли упр. организацию затираем банк контрагента
                            if (oldValue) {
                                sfContragentBank.setValue(null);
                                sfContractRf.setValue(null);
                            }
                        }

                    }
                    else {
                        //Отработал метод SetValue(null) то есть нажали на очистить поле или не задано значение при заполнение данных
                        this.controller.ManagingOrganizationId = null;
                        this.controller.ContragentId = null;
                        //обнуляем readOnly поля реквизитами упр. орг-ии 
                        tfInn.setValue(null);
                        tfKpp.setValue(null);
                        tfPhone.setValue(null);
                        //обнуляем контрагента и лочим его
                        sfContragentBank.setValue(null);
                        sfContragentBank.disable();
                        sfContractRf.setValue(null);
                        sfContractRf.disable();
                    }
                }
            },
            //При смене банка заменяем или чистим поля (Расчетный счет, Корр. счет и БИК)
            onChangeContrBank: function (obj, newValue) {
                var editForm = this.getForm();

                var tfSettlementAccount = editForm.down('#tfSettlementAccount');
                var tfCorrAccount = editForm.down('#tfCorrAccount');
                var tfBik = editForm.down('#tfBik');

                if (editForm) {
                    if (!Ext.isEmpty(newValue)) {
                        //заполняем readOnly поля реквизитами упр. орг-ии 
                        tfSettlementAccount.setValue(newValue.SettlementAccount);
                        tfCorrAccount.setValue(newValue.CorrAccount);
                        tfBik.setValue(newValue.Bik);
                    }
                    else {
                        tfSettlementAccount.setValue(null);
                        tfCorrAccount.setValue(null);
                        tfBik.setValue(null);
                    }
                }
            },
            //После разрешения редактирования поля вешаемся на событие beforeload и передаем на серверный контроллер контрагента упр. орг выбранной ранее
            onEnableContrBank: function (obj) {
                var store = obj.getStore();
                if (store) {
                    store.on('beforeload', this.controller.onBeforeLoadContragentBank, this.controller);
                }
            },
            onEnableContractRf: function (obj) {
                var store = obj.getStore();
                if (store) {
                    store.on('beforeload', this.controller.onBeforeLoadContractRf, this.controller);
                }
            },
            onBtnEditContragentClick: function () {
                var me = this;
                var editWindow = Ext.ComponentQuery.query(me.controller.editWindowSelector)[0];

                var manorgId = editWindow.down('#sfManagingOrganization').getValue();
                var contragent;
                
                var manOrgModel = me.controller.getModel('ManagingOrganization');
                if (manorgId) {
                    manOrgModel.load(manorgId, {
                        success: function(rec) {
                            contragent = rec.get("Contragent");
                            if (contragent) {
                                Ext.History.add('contragentedit/' + contragent.Id + '/');
                            }
                        },
                        scope: this
                    });
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteRequestTransferRfStatePerm').loadPermissions(record)
                        .next(function(response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function(result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            },
            onBeforeEditGrid: function(editor, e) {
                if (e.column.dataIndex == 'PersonalAccount') {
                    e.column.field.getStore().load({
                        params: {
                            transferFundId: e.record.get('Id')
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'transferFundsRfGkhInlineGridMultiSelectWindowAspect',
            gridSelector: '#transferFundsRfGrid',
            saveButtonSelector: '#transferFundsRfGrid #transferFundsRfSaveButton',
            storeName: 'transferrf.Funds',
            modelName: 'transferrf.Funds',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#contractRFMultiSelectWindow',
            storeSelect: 'transferrf.RealObjForSelect',
            storeSelected: 'transferrf.RealObjForSelected',
            titleSelectWindow: 'Выбор домов',
            titleGridSelect: 'Дома для выбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код дома', xtype: 'gridcolumn', dataIndex: 'GkhCode', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.contractRfId = this.controller.contractRfId;
                operation.params.programCrId = this.controller.programCrId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddTransferFundsObjects', 'TransferFundsRf', {
                            objectIds: recordIds,
                            requestTransferRfId: asp.controller.requestTransferRfId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            },

            getForm: function () {
                var win = Ext.ComponentQuery.query(this.multiSelectWindowSelector)[0];

                if (!win) {

                    var stSelected = Ext.create('B4.store.' + this.storeSelected);
                    stSelected.on('beforeload', this.onSelectedBeforeLoad, this);

                    var stSelect = Ext.create('B4.store.' + this.storeSelect);
                    stSelect.on('beforeload', this.onBeforeLoad, this);

                    win = this.controller.getView(this.multiSelectWindow).create({
                        itemId: this.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: this.columnsGridSelect,
                        columnsGridSelected: this.columnsGridSelected,
                        title: this.titleSelectWindow,
                        titleGridSelect: this.titleGridSelect,
                        titleGridSelected: this.titleGridSelected,
                        height: 600,
                        maximized: false
                    });
                }

                return win;
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля фильтрации мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'requesttransferrfmultiselectwindowaspect',
            fieldSelector: '#requestTransferRfFilterPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySelectWindow',
            storeSelect: 'transferrf.MunicipalityForSelect',
            storeSelected: 'transferrf.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            listeners: {
                getdata: function (asp, records) {
                    var str = '';

                    records.each(function (rec) {
                        if (str)
                            str += ';';
                        str += rec.getId();
                    });

                    this.controller.params.municipalities = str;

                }
            },
            onTrigger2Click: function () {
                this.setValue(null);
                this.updateDisplayedText();
                this.controller.params.municipalities = null;
            }
        },
         {
             xtype: 'b4buttondataexportaspect',
             name: 'RequestTransferRfButtonExportAspect',
             gridSelector: '#requestTransferRfGrid',
             buttonSelector: '#requestTransferRfGrid #btnRequestTransferRfExport',
             controllerName: 'RequestTransferRf',
             actionName: 'Export'
         }
    ],


    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'transferrf.RequestPanel',
    mainViewSelector: 'requestTransferRfPanel',
    mainViewItemId: 'requestTransferRfPanel',

    editWindowSelector: '#requestTransferRfEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'requestTransferRfPanel'
        }
    ],

    init: function () {
        this.getStore('transferrf.Funds').on('beforeload', this.onBeforeLoad, this);
        this.getStore('transferrf.Request').on('beforeload', this.onBeforeLoadRequest, this);
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('requestTransferRfPanel');
        me.params = {};
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('transferrf.Request').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.requestTransferRfId = this.requestTransferRfId;
    },

    onBeforeLoadRequest: function (store, operation) {
        if (this.params) {
            operation.params.municipalities = this.params.municipalities;
            if (this.params.dateStart) {
                operation.params.dateStart = this.params.dateStart;
            }
            if (this.params.dateEnd) {
                operation.params.dateEnd = this.params.dateEnd;
            }
        }
    },
    
    onBeforeLoadContragentBank: function (store, operation) {
        operation.params.manorgId = this.ContragentId;
    },

    onBeforeLoadContractRf: function (store, operation) {
        operation.params.manorgId = this.ManagingOrganizationId;
    },

    setContragentBank: function (record, form) {
        form.down('#sfContragentBank').setValue(record);
        form.show();
    },

    setCurrentId: function (record) {
        this.requestTransferRfId = record.getId();
        this.contractRfId = record.get('ContractRf') && record.get('ContractRf').Id || record.getData().ContractRf;
        this.programCrId = record.get('ProgramCr') && record.get('ProgramCr').Id || record.getData().ProgramCr;

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        var store = this.getStore('transferrf.Funds');

        if (store.getCount() > 0) {
            store.removeAll();
        }

        if (this.requestTransferRfId > 0 && this.contractRfId && this.programCrId) {
            editWindow.down('#transferFundsRfGrid').setDisabled(false);
            store.load();
        } else {
            editWindow.down('#transferFundsRfGrid').setDisabled(true);
        }

    }
});