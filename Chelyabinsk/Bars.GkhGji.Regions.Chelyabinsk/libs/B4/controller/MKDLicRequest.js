Ext.define('B4.controller.MKDLicRequest', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.form.FileField',
        'B4.mixins.Context',
        'B4.data.Connection',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.permission.AppealCits',
        'B4.aspects.permission.AppealCitsAnswer',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.view.PreviewFileWindow'
    ],
    requestId: null,
    answerId: null,
    currentPerson: null,
    author: null,
    executor: null,
    fileId: null,

    models: [
        'dict.Subj',
        'mkdlicrequest.Executant',
        'mkdlicrequest.HeadInspector',
        'administration.Operator',
        'mkdlicrequest.MKDLicRequestFile',
        'mkdlicrequest.MKDLicRequestRealityObject'
    ],

    stores: [
        'mkdlicrequest.MKDLicRequest',
        'mkdlicrequest.HeadInspector',
        'mkdlicrequest.MKDLicRequestRealityObject',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'mkdlicrequest.Executant',
        'mkdlicrequest.MKDLicRequestFile'
    ],

    views: [
        'mkdlicrequest.Grid',
        'mkdlicrequest.EditWindow',
        'mkdlicrequest.RealityObjectGrid',
        'mkdlicrequest.Panel',
        'mkdlicrequest.FilterPanel',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'mkdlicrequest.ExecutantGrid',
        'mkdlicrequest.ExecutantEditWindow',
        'mkdlicrequest.MultiSelectWindowExecutant',
        'mkdlicrequest.FileEditWindow',
        'mkdlicrequest.FileGrid',
        'mkdlicrequest.HeadInspectorGrid',
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'mkdlicrequest.Panel',
    mainViewSelector: 'mkdLicRequestPanel',

    editWindowSelector: '#mkdLicRequestEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'mkdLicRequestPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'mkdlicrequestfilegrid',
            controllerName: 'MKDLicRequestFile',
            name: 'mkdLicRequestFileSignatureAspect',
            signedFileField: 'SignedFile'
        },
        //{
        //    xtype: 'requirementaspect',
        //    applyOn: { event: 'show', selector: '#appealCitsExecutantMultiSelectWindowExecutant' },
        //    requirements: [
        //        {
        //            name: 'GkhGji.AppealCits.Executant.Field.PerformanceDate',
        //            applyTo: 'datefield[name=PerformanceDate]',
        //            selector: '#appealCitsExecutantMultiSelectWindowExecutant'
        //        }
        //    ]
        //},
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'mkdLicRequestExecutantStatePerm',
            editFormAspectName: 'mkdLicRequestExecutantGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Surety_Edit', applyTo: '[name=Author]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyResolve_Edit', applyTo: '[name=Resolution]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.SuretyDate_Edit', applyTo: '[name=OrderDate]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Executant_Edit', applyTo: '[name=Executant]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.Tester_Edit', applyTo: '[name=Controller]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ExecuteDate_Edit', applyTo: '[name=PerformanceDate]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ZonalInspection_Edit', applyTo: '[name=ExecutantZji]', selector: '#amkdLicRequestExecutantEditWindow' },
                { name: 'GkhGji.AppealCitizensState.Field.Consideration.ApprovalContragent_View', applyTo: '[name=ApprovalContragent]', selector: '#amkdLicRequestExecutantEditWindow' }
            ]
        },
        //{
        //    xtype: 'requirementaspect',
        //    applyOn: { event: 'show', selector: '#mkdLicRequestRedirectExecutantSelectWindow' },
        //    requirements: [
        //        {
        //            name: 'GkhGji.AppealCits.Executant.Field.PerformanceDate',
        //            applyTo: 'datefield[name=PerformanceDate]',
        //            selector: '#mkdLicRequestRedirectExecutantSelectWindow'
        //        }
        //    ]
        //},
        //{
        //    xtype: 'gkhpermissionaspect',
        //    permissions: [
        //        { name: 'GkhGji.AppealCitizens.Create', applyTo: 'b4addbutton', selector: '#mkdLicRequestGrid' },
        //        {
        //            name: 'GkhGji.AppealCitizens.CheckBoxShowCloseApp',
        //            applyTo: '#cbShowCloseAppeals',
        //            selector: '#mkdLicRequestGrid',
        //            applyBy: function (component, allowed) {
        //                var me = this;

        //                me.controller.params = me.controller.params || {};
        //                if (allowed) {
        //                    component.show();
        //                }
        //                else {
        //                    component.hide();
        //                }

        //                // Проверка на выполнение функции preDisable в аспекте 
        //                if (!component.wasPreDisabled) {
        //                    component.wasPreDisabled = true;
        //                } else {
        //                    me.controller.params.showCloseAppeals = !allowed;
        //                    me.controller.getStore('mkdlicrequest.MKDLicRequest').load();
        //                }
        //            }
        //        },
        //        {
        //            name: 'GkhGji.AppealCitizens.ShowOnlyFromEais', applyTo: 'checkbox[name=ShowOnlyFromEais]', selector: '#mkdLicRequestGrid',
        //            applyBy: function (component, allowed) {
        //                if (component) {
        //                    component.setVisible(allowed);
        //                }
        //            }
        //        }
        //    ]
        //},
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhGji.AppealCitizensState.Delete' }],
            name: 'deleteMKDLicRequestStatePerm'
        },
        {
            xtype: 'appealcitsperm',
            name: 'mkdLicRequestStatePerm',
            editFormAspectName: 'mkdLicRequestWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            /**
            * Вешаем аспект смены статуса 
            */
            xtype: 'statebuttonaspect',
            name: 'mkdLicRequestStateButtonAspect',
            stateButtonSelector: '#mkdLicRequestEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var model = this.controller.getModel('MKDLicRequest');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            this.controller.getAspect('mkdLicRequestStatePerm').setPermissionsByRecord(rec);
                            this.controller.getAspect('mkdLicRequestWindowAspect').setFormData(rec);
                        },
                        scope: this
                    })
                    : this.controller.getAspect('mkdLicRequestStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        //{
        //    xtype: 'requirementaspect',
        //    requirements: [
        //        { name: 'GkhGji.AppealCits.Field.Department_Rqrd', applyTo: '#sflZonalInspection', selector: '#appealCitsEditWindow' }
        //    ]
        //},
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'mkdLicRequestStateTransferAspect',
            gridSelector: '#mkdLicRequestGrid',
            stateType: 'gji_appeal_citizens',
            menuSelector: 'mkdLicRequestGridStateMenu'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdLicRequestWindowAspect',
            gridSelector: '#mkdLicRequestGrid',
            editFormSelector: '#mkdLicRequestEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequest',
            modelName: 'mkdlicrequest.MKDLicRequest',
            editWindowView: 'mkdlicrequest.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId(), record.get('NumberGji'));
            },
            otherActions: function (actions) {
                actions['#mkdLicRequestFilterPanel #sfRealityObject'] = { 'change': { fn: this.onChangeRealityObject, scope: this } };
                actions['#mkdLicRequestFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#mkdLicRequestFilterPanel #dfDateFromStart'] = { 'change': { fn: this.onChangeDateFromStart, scope: this } };
                actions['#mkdLicRequestFilterPanel #dfDateFromEnd'] = { 'change': { fn: this.onChangeDateFromEnd, scope: this } };
                actions['#mkdLicRequestFilterPanel #dfCheckTimeStart'] = { 'change': { fn: this.onChangeCheckTimeStart, scope: this } };
                actions['#mkdLicRequestFilterPanel #dfCheckTimeEnd'] = { 'change': { fn: this.onChangeCheckTimeEnd, scope: this } };
                //actions[this.editFormSelector + ' #btnCopy'] = { 'click': { fn: this.onCopyClick, scope: this } };

                actions[this.editFormSelector + ' #cbRedtapeFlag'] = { 'change': { fn: this.onRedtapeFlagChange, scope: this } };
                actions[this.editFormSelector + ' #mkdLicRequestExecutantSelectField'] = { 'change': { fn: this.onExecutantChange, scope: this } };
                actions[this.gridSelector + ' #cbShowCloseAppeals'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
                //actions[this.editFormSelector + ' [name=SuretyResolve]'] = { 'change': { fn: this.onSuretyResolveChange, scope: this } };
                //actions[this.editFormSelector + ' #btnSOPR'] = { 'click': { fn: this.goToSOPR, scope: this } };

                actions['#mkdLicRequestFilterPanel #clear'] = { 'click': { fn: this.clearAllFilters, scope: this } };
                //actions[this.gridSelector + ' #cbShowExtensTimes'] = { 'change': { fn: this.onExtensTimesCheckbox, scope: this } };
                //actions[this.gridSelector + ' checkbox[name=ShowOnlyFromEais]'] = { 'change': { fn: this.onShowOnlyFromEais, scope: this } };

                /*actions[this.editFormSelector + ' #sfQuestionStatus'] = { 'change': { fn: this.onChangeQuestionStatus, scope: this } };*/

                //actions[this.editFormSelector + ' button[name=btnGetAttachmentArchive]'] = { 'click': { fn: this.onGetAttachmentArchiveClick, scope: this } };
            },

            //onChangeQuestionStatus: function (field, newValue) {
            //    var form = this.getForm(),
            //        sfSSTUTransferOrg = form.down('#sfSSTUTransferOrg');

            //    if (newValue == B4.enums.QuestionStatus.Transferred) {
            //        sfSSTUTransferOrg.setDisabled(false);
            //        sfSSTUTransferOrg.allowBlank = false;
            //    }
            //    else {
            //        sfSSTUTransferOrg.setValue(null);
            //        sfSSTUTransferOrg.setDisabled(true);
            //        sfSSTUTransferOrg.allowBlank = true;
            //    }
            //},

            //onSuretyResolveChange: function (sf, val) {
            //    var form = this.getForm(),
            //        contragentField = form.down('[name=ApprovalContragent]');

            //    contragentField.setDisabled(!val || val.Code != "4");
            //},

            onAfterSetFormData: function (aspect, rec, form) {
                // Изза того что в Gkh перекрыт аспект в нем 2 раза делается метод show у окна что приводит к повторному открытию окна
            },

            //Данный метод перекрываем для того чтобы вместо целого объекта Executant и Surety
            // передать только Id на сохранение, поскольку если на сохранение уйдет Executant или Surety целиком,
            //то это поле тоже сохраниться и поля для записи Executant и Surety будут потеряны
            getRecordBeforeSave: function (record) {

                if (record && record.data) {
                    var executant = record.data.Executant;
                    if (executant && executant.Id > 0) {
                        record.data.Executant = executant.Id;
                    }

                    var surety = record.data.Surety;
                    if (surety && surety.Id > 0) {
                        record.data.Surety = surety.Id;
                    }
                }

                return record;
            },
            onRedtapeFlagChange: function (field, newValue) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0];

                var previousRequestSelectField = wnd.down('#previousRequestSelectField');
                //TODO! Переделать
                previousRequestSelectField.setDisabled(newValue > 2 ? false : true);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    
                        var numberGji = record.get('NumberGji'),
                        requestId = record.getId();

                    //Передаем аспекту смены статуса необходимые параметры
                    asp.controller.getAspect('mkdLicRequestStateButtonAspect').setStateData(requestId, record.get('State'));

                    asp.controller.setCurrentId(requestId, numberGji);
                },
                beforesaverequest: function () {
                    var checkTime = this.getForm().down('[name=CheckTime]').getValue(),
                        extensTime = this.getForm().down('[name=ExtensTime]').getValue();

                    if (extensTime != null && checkTime > extensTime) {
                        Ext.Msg.alert('Ошибка сохранения!', "Дата продленного контрольного срока не может быть меньше Даты контрольного срока");
                        return false;
                    }
                    return true;
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteMKDLicRequestStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }
                        }, this);
                }
            },
            onSuretyChange: function (field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#mkdLicRequestSuretyPositionTextField');

                if (fieldPosition) {
                    fieldPosition.setValue(data && data.Position);
                }
            },

            onSuretyBeforeLoad: function (field, options) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;
            },

            onExecutantChange: function (field, data) {
                var wnd = Ext.ComponentQuery.query(this.controller.editWindowSelector)[0],
                    fieldPosition = wnd.down('#mkdLicRequestExecutantPositionTextField');
                if (fieldPosition) {
                    fieldPosition.setValue(data && data.Position);
                }

            },
            onChangeRealityObject: function (field, newValue) {
                if (newValue) {
                    this.controller.params.realityObjectId = newValue.Id;
                } else {
                    this.controller.params.realityObjectId = null;
                }
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('mkdlicrequest.MKDLicRequest');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateFromStart: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromStart = newValue;
                } else {
                    this.controller.params.dateFromStart = null;
                }
            },
            onChangeDateFromEnd: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.dateFromEnd = newValue;
                } else {
                    this.controller.params.dateFromEnd = null;
                }
            },
            onChangeCheckTimeStart: function (field, newValue, oldValue) {

                if (newValue) {
                    this.controller.params.checkTimeStart = newValue;
                } else {
                    this.controller.params.checkTimeStart = null;
                }
            },
            onChangeCheckTimeEnd: function (field, newValue, oldValue) {
                if (newValue) {
                    this.controller.params.checkTimeEnd = newValue;
                } else {
                    this.controller.params.checkTimeEnd = null;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            clearAllFilters: function (bt) {
                var panel = bt.up('mkdLicRequestFilterPanel');
                panel.down('#dfDateFromStart').setValue();
                panel.down('#dfDateFromEnd').setValue();
                panel.down('#dfCheckTimeStart').setValue();
                panel.down('#dfCheckTimeEnd').setValue();
                panel.down('#sfRealityObject').setValue();
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            onExtensTimesCheckbox: function (field, newValue) {
                this.controller.params.showExtensTimes = newValue;
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            onShowOnlyFromEais: function (field, newValue) {
                this.controller.params.showOnlyFromEais = newValue;
                this.controller.getStore('mkdlicrequest.MKDLicRequest').load();
            },
            onAfterSetFormData: function (aspect, rec, form) {
                var sfMExecutantDocGji = form.down('#sfMExecutantDocGji');
                if (!sfMExecutantDocGji.value) {
                    var newMKDLicTypeRequest = {
                        Id: 1,
                        Name: 'Управляющая компания'
                    };
                    sfMExecutantDocGji.setValue(newMKDLicTypeRequest); newMKDLicTypeRequest
                }
                var sfMKDLicTypeRequest = form.down('#sfMKDLicTypeRequest');
                if (!sfMKDLicTypeRequest.value) {
                    var newExecutantDocGji = {
                        Id: 1,
                        Name: 'Заключение договора управления'
                    };
                    sfMKDLicTypeRequest.setValue(newExecutantDocGji);
                }
            },
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdlicrequestFileGridAspect',
            gridSelector: 'mkdlicrequestfilegrid',
            editFormSelector: '#mkdlicrequestFileEditWindow',
            storeName: 'mkdlicrequest.MKDLicRequestFile',
            modelName: 'mkdlicrequest.MKDLicRequestFile',
            editWindowView: 'mkdlicrequest.FileEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MKDLicRequest', this.controller.requestId);
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия ВКЛАДКИ руководителя обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitsHeadInspector/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'mkdLicRequestHeadInspectorMultiselectWindowAspect',
            storeName: 'mkdlicrequest.HeadInspector',
            modelName: 'mkdlicrequest.HeadInspector',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdLicRequestHeadInspectorSelectWindow',
            storeSelect: 'dict.Inspector',
            storeSelected: 'dict.Inspector',
            gridSelector: 'mkdLicRequestHeadInspectorGrid',
            titleSelectWindow: 'Выбор руководителя',
            titleGridSelect: 'Руководители для выбора',
            titleGridSelected: 'Выбранные руководители',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    header: 'ФИО',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    header: 'Должность',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    header: 'ФИО'
                }
            ],
            
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRequests', 'MKDLicRequestHeadInspector', {
                            recordIds: Ext.encode(recordIds),
                            requestId: asp.controller.requestId
                        })).next(function (response) {
                            me.getGrid().getStore().load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Руководители сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать руководителей');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.mkdlicrequestId = this.controller.requestId;
            }
        },
        {
            /*
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'mkdlicrequestrogridAspect',
            gridSelector: 'mkdlicrequestrogrid',
            storeName: 'mkdlicrequest.MKDLicRequestRealityObject',
            modelName: 'mkdlicrequest.MKDLicRequestRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdlicrequestroMultiSelectWindow',
            storeSelect: 'realityobj.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.mkdlicrequestId = this.controller.requestId;
            },

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddStatementRealityObjects', 'AppealCitsRealObject', {
                            objectIds: recordIds,
                            mkdlicrequestId: asp.controller.requestId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();

                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            Аспект смены статуса в гриде исполнителей обращения
            */
            xtype: 'b4_state_contextmenu',
            name: 'mkdLicRequestExecutantStateTransferAspect',
            gridSelector: '#mkdLicRequestExecutantGrid',
            menuSelector: 'mkdLicRequestExecutantGridStateMenu',
            stateType: 'gji_appcits_executant'
        },
        {
            /*
            Аспект смены статуса в карточке редактирования исполнителя обращения
            */
            xtype: 'statebuttonaspect',
            name: 'mkdLicRequestExecutantStateButtonAspect',
            stateButtonSelector: '#mkdLicRequestExecutantEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                    var editWindowAspect = asp.controller.getAspect('mkdLicRequestExecutantGridWindowAspect');
                    editWindowAspect.updateGrid();

                    var model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            /* 
               Аспект взаимодействия таблицы Исполнителей и грида с массовым доабавлением
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'mkdLicRequestExecutantGridWindowAspect',
            gridSelector: '#mkdLicRequestExecutantGrid',
            storeName: 'mkdlicrequest.Executant',
            modelName: 'mkdlicrequest.Executant',
            multiSelectWindow: 'mkdlicrequest.MultiSelectWindowExecutant',
            multiSelectWindowSelector: '#mkdLicRequestExecutantMultiSelectWindowExecutant',
            editFormSelector: '#mkdLicRequestExecutantEditWindow',
            editWindowView: 'mkdlicrequest.ExecutantEditWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор исполнителей',
            titleGridSelect: 'Исполнители',
            titleGridSelected: 'Выбранные исполнители',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1 }
            ],
            otherActions: function (actions) {
                // actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions['#mkdLicRequestExecutantEditWindow #sflAuthor'] = { 'change': { fn: this.onChangeAutor, scope: this } };
                actions['#mkdLicRequestExecutantEditWindow #sflExecutant'] = { 'change': { fn: this.onChangeExecutant, scope: this }, 'beforeload': { fn: this.onBeforeLoadAuthor, scope: this } };
                actions['#mkdLicRequestExecutantEditWindow #sflController'] = { 'beforeload': { fn: this.onBeforeLoadExecutant, scope: this } };
            },
            onBeforeLoadExecutant: function (store, operation) {
                var me = this;
                operation.params.currentPerson = me.controller.Executant;
            },
            onBeforeLoadAuthor: function (store, operation) {
                var me = this;
                operation.params.currentPerson = me.controller.author;
            },
            onChangeAutor: function (field, newValue) {
                var me = this;
                if (newValue)
                    me.controller.author = newValue.Id;
            },
            onChangeExecutant: function (field, newValue) {
                var me = this;
                if (newValue)
                    me.controller.Executant = newValue.Id;
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.excludeInpectorId = this.controller.inpectorId;

                B4.Ajax.request(B4.Url.action('GetParamByKey', 'GjiParams', {
                    key: 'AutoSetSurety'
                }))
                    .next(function (resp) {
                        var win = me.getForm(),
                            fields = win.query('[name=Author]'),
                            field = fields ? fields[0] : null,
                            data = Ext.decode(resp.responseText);

                        field.setDisabled(data.data.toLowerCase() === 'true');
                    });
            },
            listeners: {
                beforesave: function (aspect, rec) { //Перекрываем для поддержки загрузки файла
                    var win = Ext.ComponentQuery.query('#mkdLicRequestExecutantEditWindow')[0];
                    var frm = win.getForm();
                    win.mask('Сохранение', frm);
                    frm.submit({
                        url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                        params: {
                            records: Ext.encode([rec.getData()])
                        },
                        success: function (form, action) {
                            win.unmask();
                            aspect.updateGrid();

                            win.close();
                        },
                        failure: function (form, action) {
                            win.unmask();
                            win.fireEvent('savefailure', rec, action.result.message);
                            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                        }
                    });

                    return false;
                },
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var dateField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfPerformanceDate')[0];
                    if (!dateField.allowBlank && !dateField.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать срок исполнения');
                        return false;
                    }

                    var authorField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #sflAuthor')[0];
                    if (!authorField.isDisabled() && (!authorField.value || authorField.value.Id <= 0)) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать поручителя');
                        return false;
                    }

                    if (recordIds[0] <= 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                        return false;
                    }
                    
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddExecutants', 'MKDLicRequestExecutant', {
                        inspectorIds: Ext.encode(recordIds),
                        requestId: asp.controller.requestId,
                        performanceDate: dateField.value,
                        authorId: authorField.value ? authorField.value.Id : 0
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Исполнители сохранены успешно');
                        return true;
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    this.controller.getAspect('mkdLicRequestExecutantStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                },
                panelrendered: function (asp, prm) {
                    var me = this,
                        autoPerformanceDate = Gkh.config.HousingInspection.SettingTheVerification.AutoPerformanceDate;

                    if (autoPerformanceDate) {
                        var performanceDateValue = me.controller.getStore('mkdlicrequest.MKDLicRequest').getById(me.controller.requestId).get('CheckTime'),
                            performanceDateEl = prm.window.down('#dfPerformanceDate');

                        performanceDateEl.setValue(performanceDateValue);
                        performanceDateEl.setDisabled(true);
                    }
                },
            }
        },
        {   /* 
               Аспект взаимодейсвтия кнопки Перенаравить с массовой формой выбора исполнителей
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'mkdLicRequestRedirectExecutantAspect',
            buttonSelector: '#mkdLicRequestExecutantEditWindow #btnRedirect',
            multiSelectWindowSelector: '#mkdLicRequestRedirectExecutantSelectWindow',
            multiSelectWindow: 'mkdlicrequest.MultiSelectWindowExecutant',
            storeName: 'mkdlicrequest.Executant',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор исполнителей',
            titleGridSelect: 'Исполнители',
            titleGridSelected: 'Выбранные исполнители',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Отдел', xtype: 'gridcolumn', dataIndex: 'ZonalInspection', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.excludeInpectorId = this.controller.inpectorId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        btn = Ext.ComponentQuery.query(this.buttonSelector)[0],
                        form = btn.up('#mkdLicRequestExecutantEditWindow').getForm(),
                        record = form.getRecord();

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    var dateField = Ext.ComponentQuery.query(asp.multiSelectWindowSelector + ' #dfPerformanceDate')[0];

                    if (!dateField.allowBlank && !dateField.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать срок исполнения');
                        return false;
                    }

                    if (recordIds.length == 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исполнителя');
                        return false;
                    }

                    asp.controller.mask('Перенаправленеи', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('RedirectExecutant', 'MKDLicRequestExecutant', {
                        objectIds: Ext.encode(recordIds),
                        executantId: record.getId(),
                        performanceDate: dateField.value
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Перенаправление выполнено успешно');
                        return true;
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка перенаправления', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                }
            }
        },
    ],

    setCurrentId: function (id, numberGji) {
        
        this.requestId = id;
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            tabpanel = editWindow.down('.tabpanel'),
            storeAttach = this.getStore('mkdlicrequest.MKDLicRequestFile'),
            storeRo = this.getStore('mkdlicrequest.MKDLicRequestRealityObject'),
            storeExecutant = this.getStore('mkdlicrequest.Executant'),
            storeHeadInspector = this.getStore('mkdlicrequest.HeadInspector');

        storeAttach.removeAll();
        storeRo.removeAll();
        storeExecutant.removeAll();
        storeHeadInspector.removeAll();

        tabpanel.down('#tabAttachment').tab.setDisabled(!id);
        tabpanel.down('#tabRealityObject').tab.setDisabled(!id);
        tabpanel.down('#tabDecision').tab.setDisabled(!id);
        tabpanel.down('#tabApproval').tab.setDisabled(!id);
        tabpanel.setActiveTab(0);

        if (id > 0) {
            storeAttach.load();
            storeRo.load();
            storeExecutant.load();
            storeHeadInspector.load();
        }
    },

    init: function () {
        this.getStore('mkdlicrequest.MKDLicRequest').on('beforeload', this.onBeforeLoadRequest, this);
        this.getStore('mkdlicrequest.MKDLicRequestFile').on('beforeload', this.onBeforeLoad, this);
        this.getStore('mkdlicrequest.MKDLicRequestRealityObject').on('beforeload', this.onBeforeLoad, this);
        this.getStore('mkdlicrequest.Executant').on('beforeload', this.onBeforeLoad, this);
        this.getStore('mkdlicrequest.HeadInspector').on('beforeload', this.onBeforeLoad, this);

        var actions = {};
        this.control({

            'mkdLicRequestExecutantGrid actioncolumn[action="previewfille"]': { click: { fn: this.onPreviewClick, scope: this } },
            'previewFileWindow button[name="Save"]': { click: { fn: this.downloadFile, scope: this } }
        });
        this.callParent(arguments);
    },

    onPreviewClick: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        var file = rec.get('Resolution');
        me.fileId = file.Id;
        if (me.fileId != null) {
            win = Ext.widget('previewFileWindow', {
                renderTo: B4.getBody().getActiveTab().getEl(),
                fileId: me.fileId,
            });
            win.show();
            var save = win.down('Save');
        }
    },
    downloadFile: function (params) {
        var me = this;
        var id = me.fileId;
        window.open(B4.Url.action('/FileUpload/Download?id=' + id));
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('mkdLicRequestPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('mkdlicrequest.MKDLicRequest').load();

        this.params = {};
        this.params.dateFromStart = null;
        this.params.dateFromEnd = null;
        this.params.checkTimeStart = null;
        this.params.checkTimeEnd = null;

        this.params.realityObjectId = null;
    },

    edit: function (id) {
        var view = this.getMainView() || Ext.widget('mkdLicRequestPanel');
        
        if (view && !view.rendered) {
            this.bindContext(view);
            this.application.deployView(view);

            this.params = {};
            this.params.dateFromStart = null;
            this.params.dateFromEnd = null;
            this.params.checkTimeStart = null;
            this.params.checkTimeEnd = null;

            this.params.realityObjectId = null;
        }

        var model = this.getModel('mkdlicrequest.MKDLicRequest');
        this.getAspect('mkdLicRequestWindowAspect').editRecord(new model({ Id: id }));
    },

    onBeforeLoad: function (store, operation) {
        operation.params.mkdlicrequestId = this.requestId;
    },

    onBeforeLoadRequest: function (store, operation) {
        if (this.params) {
            Ext.apply(operation.params, this.params);
        }
    }
});