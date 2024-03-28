Ext.define('B4.controller.BusinessActivity', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.BusinessActivity',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    models: [
        'BusinessActivity',
        'businessactivity.ServiceJuridicalGji',
        'Contragent'
    ],
    stores: [
        'BusinessActivity',
        'businessactivity.ServiceJuridicalGji',
        'dict.KindWorkNotifGjiForSelect',
        'dict.KindWorkNotifGjiForSelected'
    ],
    views: [
        'businessactivity.Grid',
        'businessactivity.EditWindow',
        'businessactivity.ServiceJuridicalGjiGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    businessActivityEditWindowSelector : '#businessActivityEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'businessActivityGrid'
        }
    ],

    mainView: 'businessactivity.Grid',
    mainViewSelector: 'businessActivityGrid',

    aspects: [
         /* пермишшен Уведомление о начале предпринимательской деятельности по роли */
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                  { name: 'GkhGji.BusinessActivityViewCreate.Create', applyTo: 'b4addbutton', selector: 'businessActivityGrid' }
            ]
        },
         /* пермишшены по статусу Уведомления о начале предпринимательской деятельности*/
        {
            xtype: 'businessactivityperm',
            editFormAspectName: 'businessActivityNotifGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'businessActivityStatePerm'
        },
         /* пермишшен по удалению записи Уведомления о начале предпринимательской деятельности(по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhGji.BusinessActivity.Delete' }],
            name: 'deleteBusinessActivityStatePerm'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'businessActivityNotifButtonExportAspect',
            gridSelector: 'businessActivityGrid',
            buttonSelector: 'businessActivityGrid #btnExport',
            controllerName: 'BusinessActivity',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'businessActivityStateTransferAspect',
            gridSelector: 'businessActivityGrid',
            stateType: 'gji_business_activity',
            menuSelector: 'businessActivityGridStateMenu'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'businessActivityStateButtonAspect',
            stateButtonSelector: '#businessActivityEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var aspect = asp.controller.getAspect('businessActivityNotifGridWindowAspect');
                    aspect.updateGrid();

                    var model = this.controller.getModel('BusinessActivity');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('businessActivityStatePerm').setPermissionsByRecord(rec);
                            aspect.getForm().down('#tfRegNum').setValue(rec.get('RegNum'));
                        },
                        scope: this
                    }) : this.controller.getAspect('businessActivityStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },

        {
            /*
            Аспект взаимодействия таблицы справочника орг правовые формы и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'businessActivityNotifGridWindowAspect',
            gridSelector: 'businessActivityGrid',
            editFormSelector: '#businessActivityEditWindow',
            storeName: 'BusinessActivity',
            modelName: 'BusinessActivity',
            editWindowView: 'businessactivity.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.get('Id'));
            },
            saveRecord: function(rec) {
                var me = this;
                B4.Ajax.request(B4.Url.action('CheckDateNotification', 'BusinessActivity', {
                    dateNotification: rec.get('DateNotification'),
                    businessActivityId: rec.get('Id')
                })).next(function(response) {
                    var obj = Ext.JSON.decode(response.responseText);
                    rec.set('Registered', obj.registered);
                    rec.set('State', obj.state);
                    if (!obj.success) {
                        Ext.Msg.confirm('Cохранение записи!', 'Дата уведомления должна быть больше или равна текущей дате! Продолжить?', function(result) {
                            if (result == 'yes') {
                                me.saveRecordHasUpload(rec);
                            }
                        });
                    } else {
                        me.saveRecordHasUpload(rec);
                    }
                });
            },
            listeners: {
                aftersetformdata: function(asp, record, form) {
                    asp.controller.currentRecord = record;
                    asp.controller.setCurrentId(record.get('Id'));
                    asp.controller.getAspect('businessActivityStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            },

            otherActions: function(actions) {
                actions[this.editFormSelector + ' #sfContragent'] = { 'change': { fn: this.onChangeContractor, scope: this } };
                actions[this.editFormSelector + ' #btnUpdate'] = { 'click': { fn: this.onBtnUpdateClick, scope: this } };
                actions[this.editFormSelector + ' #btnEditContragent'] = { 'click': { fn: this.onBtnEditContragentClick, scope: this } };

                actions[this.gridSelector] = {
                    'rowaction': { fn: this.rowAction, scope: this },
                    'itemdblclick': { fn: this.rowDblClick, scope: this },
                    'gridaction': { fn: this.gridAction, scope: this },
                    'destroy': { fn: this.gridDestroy, scope: this }
                };
            },

            gridDestroy: function() {
                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                if (editWindow) {
                    editWindow.close();
                }
            },

            getForm: function() {
                var editWindow = Ext.ComponentQuery.query(this.editFormSelector)[0];

                if (!editWindow) {
                    editWindow = this.controller.getView(this.editWindowView).create();

                    var cmp = this.controller.getMainComponent();

                    if (cmp) {
                        cmp.add(editWindow);
                    }
                }

                return editWindow;
            },

            //При смене контрагента подгружаем его данные в readOnly поля
            onChangeContractor: function(obj, newValue) {
                var editWindow = this.getForm();

                var cbOrganizationFormName = editWindow.down('#cbOrganizationFormName');
                var tfMailingAddress = editWindow.down('#tfMailingAddress');
                var tfInn = editWindow.down('#tfInn');
                var tfOgrn = editWindow.down('#tfOgrn');

                if (!Ext.isEmpty(newValue)) {
                    cbOrganizationFormName.setValue(newValue.OrganizationForm);
                    tfMailingAddress.setValue(newValue.MailingAddress);
                    tfInn.setValue(newValue.Inn);
                    tfOgrn.setValue(newValue.Ogrn);
                } else {
                    cbOrganizationFormName.setValue(null);
                    tfMailingAddress.setValue(null);
                    tfInn.setValue(null);
                    tfOgrn.setValue(null);
                }
            },
            //Заполняем форму последними сохраненными данными
            onBtnUpdateClick: function() {
                Ext.Msg.confirm('Обновление записи!', 'Вы действительно хотите обновить запись? Все несохраненные данные при этом пропадут', function(result) {
                    if (result == 'yes') {
                        this.setFormData(this.controller.currentRecord);
                    }
                }, this);
            },
            onBtnEditContragentClick: function() {
                var me = this;
                var editWindow = Ext.ComponentQuery.query(me.controller.businessActivityEditWindowSelector)[0];

                var contragent = editWindow.down('#sfContragent').getValue();

                if (contragent) {
                    contragent = Ext.isNumeric(contragent) ? contragent : contragent.Id;
                    Ext.History.add('contragentedit/' + contragent + '/');
                }
            },
            deleteRecord: function(record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteBusinessActivityStatePerm').loadPermissions(record)
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
            }
        },
        {
            /*
            Аспект взаимодействия таблицы услуг оказываемых юр. лицом и формы массового выбора элементов(видов работ уведомлений)
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'serviceJuridicalGkhGridMultiSelectWindow',
            gridSelector: '#serviceJuridicalGjiGrid',
            storeName: 'businessactivity.ServiceJuridicalGji',
            modelName: 'businessactivity.ServiceJuridicalGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#serviceJuridalNotificationMultiSelectWindow',
            storeSelect: 'dict.KindWorkNotifGjiForSelect',
            storeSelected: 'dict.KindWorkNotifGjiForSelected',
            titleSelectWindow: 'Выбор видов работ',
            titleGridSelect: 'Виды работ для отбора',
            titleGridSelected: 'Выбранные виды работ',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                //сохраняем выбранные строки
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddKindWorkNotification', 'ServiceJuridicalGji', {
                            workIds: recordIds,
                            buisnesId: asp.controller.buisnesId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать виды работ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {  
            xtype: 'gkhbuttonprintaspect',
            name: 'businessActivityPrintAspect',
            buttonSelector: '#businessActivityEditWindow #btnPrint',
            codeForm: 'BusinessActivity',
            getUserParams: function (reportId) {
                var param = { DocumentId: this.controller.buisnesId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    init: function() {
        this.getStore('businessactivity.ServiceJuridicalGji').on('beforeload', this.onBeforeLoad, this);
        B4.Url.loadCss('content/css/b4Gji.css');
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('businessActivityGrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('BusinessActivity').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.buisnesId = this.buisnesId;
        this.getAspect('businessActivityPrintAspect').loadReportStore();
    },

    setCurrentId: function(id) {
        this.buisnesId = id;

        var editWindow = Ext.ComponentQuery.query(this.businessActivityEditWindowSelector)[0];
        editWindow.down('#businessActivityPanel').setActiveTab(0);

        var sourceStore = this.getStore('businessactivity.ServiceJuridicalGji');
        sourceStore.removeAll();

        if (id > 0) {
            editWindow.down('#serviceJuridicalGjiGrid').setDisabled(false);
            editWindow.down('#btnEditContragent').setDisabled(false);
            sourceStore.load();
        } else {
            editWindow.down('#serviceJuridicalGjiGrid').setDisabled(true);
            editWindow.down('#btnEditContragent').setDisabled(true);
        }
    }
});