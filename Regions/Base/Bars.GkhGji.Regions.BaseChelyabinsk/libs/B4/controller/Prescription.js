Ext.define('B4.controller.Prescription', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    officialReportId: 0,
    courtId:0,

    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.Prescription',
        'B4.aspects.permission.ChelyabinskPrescription',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'ProtocolGji',
        'Prescription',
        'prescription.Annex',
        'prescription.Cancel',
        'prescription.Violation',
        'prescription.ArticleLaw',
        'courtpractice.CourtPractice',
        'prescription.BaseDocument',
        'prescription.PrescriptionOfficialReportViolation',
        'prescription.PrescriptionOfficialReport',
        'actcheck.Violation'
    ],

    stores: [
        'Contragent',
        'Prescription',
        'prescription.Violation',
        'prescription.RealityObjViolation',
        'prescription.ViolationForSelect',
        'prescription.ViolationForSelected',
        'prescription.Annex',
        'prescription.ArticleLaw',
        'prescription.Cancel',
        'prescription.BaseDocument',
        'dict.ActivityDirection',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.PrescriptionDirectionsForSelected',
        'dict.ExecutantDocGji',
        'dict.Municipality',
        'actcheck.ViolationForSelect',
        'prescription.PrescriptionOfficialReport',
        'prescription.PrescriptionOfficialReportViolation',
        'actcheck.ViolationForSelected',
        'prescription.PrescriptionViolationForSelect',
        'prescription.PrescriptionViolationForSelected'
    ],

    views: [
        'prescription.EditPanel',
        'prescription.RealityObjViolationGrid',
        'prescription.AnnexEditWindow',
        'prescription.AnnexGrid',
        'prescription.ArticleLawGrid',
        'prescription.CancelEditWindow',
        'B4.view.prescription.PlanDateSetWindow',
        'prescription.CancelGrid',
        'prescription.RealityObjListPanel',
        'prescription.BaseDocumentGrid',
        'prescription.BaseDocumentEditWindow',
        'prescription.OfficialReportGrid',
        'prescription.OfficialReportEditWindow',
        'prescription.OfficialReportViolationGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'prescription.EditPanel',
    mainViewSelector: '#prescriptionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: '#prescriptionfficialReportStateAspect',
            gridSelector: 'prescriptionofficialreportgrid',
            stateType: 'prescription_official_report',
            menuSelector: 'prescriptionfficialReportGridStateMenu'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'officialReportPrintAspect',
            buttonSelector: '#prescriptionfficialReportEditWindow #btnPrint',
            codeForm: 'PrescriptionOfficialReport',
            getUserParams: function () {
                var param = { Id: officialReportId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#prescriptionAnnexGrid',
            controllerName: 'PrescriptionAnnex',
            name: 'prescriptionAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            /*
            Аспект формирвоания документов для Акта проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'prescriptionCreateButtonAspect',
            buttonSelector: '#prescriptionEditPanel gjidocumentcreatebutton',
            containerSelector: '#prescriptionEditPanel',
            typeDocument: 50, // Тип документа предписания
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил для которых необходимы пользовательские параметры
                // при созданни документов из предписания все правила ожидают пользовательские параметры
                if (params.ruleId == 'PrescriptionToProtocolRule') {
                    return false;
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'prescriptionStateButtonAspect',
            stateButtonSelector: '#prescriptionEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('prescriptionEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /*
             * аспект кнопки печати предписания
             */
            xtype: 'gkhbuttonprintaspect',
            name: 'prescriptionPrintAspect',
            buttonSelector: '#prescriptionEditPanel #btnPrint',
            codeForm: 'Prescription',
            getUserParams: function(reportId) {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
             * аспект кнопки печати решения об отмене предписания
             */
            xtype: 'gkhbuttonprintaspect',
            name: 'prescriptionCancelPrintAspect',
            buttonSelector: '#prescriptionCancelEditWindow #btnPrint',
            codeForm: 'PrescriptionCancel',
            getUserParams: function(reportId) {
                var param = { CancelId: this.controller.params.CancelId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
             * аспект пермишенов предписания
             */
            xtype: 'prescriptionperm',
            editFormAspectName: 'prescriptionEditPanelAspect'
        },
        {
            /*
             * аспект пермишенов предписания котоыре расширены в модуле НСО ГЖИ
             */
            xtype: 'nsoprescriptionperm',
            editFormAspectName: 'prescriptionEditPanelAspect'
        },
       {
            /*
            * Апект для основной панели Предписания
            */
            xtype: 'gjidocumentaspect',
            name: 'prescriptionEditPanelAspect',
            editPanelSelector: '#prescriptionEditPanel',
            modelName: 'Prescription',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #btnCourtPractice'] = { 'click': { fn: this.goToCourtPractice, scope: this } };
                actions['#prescriptionRealityObjViolationGrid'] = { 'select': { fn: this.onSelectRealityObjViolationGrid, scope: this } };
            },

            onSelectRealityObjViolationGrid: function() {
                this.controller.getStore('prescription.Violation').load();
           },

           goToCourtPractice: function () {
               debugger;
               var me = this,
                   portal = me.controller.getController('PortalController'),
                   controllerEditName,
                   params = {};
               controllerEditName = 'B4.controller.CourtPractice';
               params.recId = me.controller.courtId;
               if (controllerEditName) {
                   portal.loadController(controllerEditName, params);
               }
           },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    gridBaseDoc = panel.down('prescriptionBaseDocumentGrid'),
                    storeBaseDoc = gridBaseDoc.getStore();
                
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                var contragent = asp.controller.getMainView().down('#sfContragent');
                
                //После проставления данных обновляем title вкладки
                panel.setTitle(asp.controller.params.title);

                if (rec.get('DocumentNumber')) {
                    panel.setTitle('Предписание ' + rec.get('DocumentNumber'));
                } else {
                    panel.setTitle('Предписание');
                }

                if (!Ext.isObject(contragent.value)) {
                    contragent.setDisabled(true);
                }
                
                panel.down('#prescriptionTabPanel').setActiveTab(0);

                //Делаем запросы на получение Инспекторов и документа основания
                //и обновляем соответствующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Prescription', {
                    documentId: asp.controller.params.documentId
                })).next(function(response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#prescriptionInspectorsTrigerField');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldBaseName = panel.down('#prescriptionBaseNameTextField');
                    fieldBaseName.setValue(obj.baseName);
                    
                    var fieldDirections = panel.down('#prescriptionDirectionsTrigerField');
                    fieldDirections.updateDisplayedText(obj.directionNames);
                    fieldDirections.setValue(obj.directionIds);

                    asp.controller.params.parentId = obj.parentId;

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetDocInfo', 'CourtPracticeOperations', {
                    docId: asp.controller.params.documentId
                })).next(function (response) {
                    asp.controller.unmask();
                    debugger;
                    //десериализуем полученную строку             
                    var data = Ext.decode(response.responseText);
                    var btnGoTo = panel.down('#btnCourtPractice');
                    if (data.data.courtId > 0) {
                       
                        btnGoTo.setDisabled(false);
                    }
                    else {
                        btnGoTo.setDisabled(true);
                    }
                    asp.controller.courtId = data.data.courtId;
                }).error(function () {
                    asp.controller.unmask();
                });
                
                storeBaseDoc.clearFilter(true);
                storeBaseDoc.filter('documentId', rec.get('Id'));
                
                //загружаем стор меню кнопки печати
                me.controller.getAspect('prescriptionPrintAspect').loadReportStore();

                //обновляем статусы
                me.controller.getAspect('prescriptionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('prescriptionCreateButtonAspect').setData(rec.get('Id'));
            },

            setTypeExecutantPermission: function(typeExec) {
                var me = this,
                    panel = this.getPanel(),
                    permissions = [
                        'GkhGji.DocumentsGji.Prescription.Field.Contragent_Edit',
                        'GkhGji.DocumentsGji.Prescription.Field.PhysicalPerson_Edit',
                        'GkhGji.DocumentsGji.Prescription.Field.PhysicalPersonInfo_Edit'
                    ];

                panel.down('#sfContragent').setDisabled(true);
                panel.down('#tfPhysPerson').setDisabled(true);
                panel.down('#taPhysPersonInfo').setDisabled(true);
                panel.down('#sfContragent').allowBlank = true;

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        })
                    }).next(function(response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        switch (typeExec.Code) {
                        //Активны все поля                                                                    
                        case "1":
                        case "3":
                        case "5":
                        case "11":
                        case "13":
                        case "16":
                        case "18":
                        case "19":
                            panel.down('#sfContragent').setDisabled(!perm[0]);

                            panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                            panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                            panel.down('#sfContragent').allowBlank = false;
                            break;
                        //Активно поле Юр.лицо                                                                    
                        case "0":
                        case "2":
                        case "4":
                        case "10":
                        case "12":
                        case "6":
                        case "7":
                        case "15":
                        case "21": //ИП
                            panel.down('#sfContragent').setDisabled(!perm[0]);

                            panel.down('#tfPhysPerson').setDisabled(true);
                            panel.down('#taPhysPersonInfo').setDisabled(true);

                            panel.down('#sfContragent').allowBlank = false;
                            break;
                        //Активны поля Физ.лица                                                                    
                        case "8":
                        case "9":
                        case "14": 
                            panel.down('#sfContragent').setDisabled(true);

                            panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                            panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                            panel.down('#sfContragent').allowBlank = true;
                            break;
                        }
                    }).error(function() {
                        me.controller.unmask();
                    });
                }
            },
            
            onChangeTypeExecutant: function(field, value) {
                var me = this,
                    data = field.getRecord(value);

                if (data) {
                    if (me.controller.params) {
                        me.controller.params.typeExecutant = data.Code;
                    }
                    me.setTypeExecutantPermission(data);
                }
            },
            onBeforeLoadContragent: function(field, options) {
                var executantField = this.getPanel().down('#cbExecutant');

                var typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

                return true;
            },

            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {
                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            }
        },
       {
           xtype: 'gkhgridmultiselectwindowaspect',
           name: 'officialReportViolationGridAspect',
           gridSelector: 'prescriptionoffrepviolationgrid',
           storeName: 'prescription.PrescriptionOfficialReportViolation',
           modelName: 'prescription.PrescriptionOfficialReportViolation',
           multiSelectWindow: 'SelectWindow.MultiSelectWindow',
           multiSelectWindowSelector: '#officialReportViolationMultiSelectWindow',
           storeSelect: 'prescription.PrescriptionViolationForSelect',
           storeSelected: 'prescription.PrescriptionViolationForSelected',
           titleSelectWindow: 'Выбор нарушений',
           titleGridSelect: 'Нарушения для отбора',
           titleGridSelected: 'Выбранные нарушения',
           onBeforeLoad: function (store, operation) {
               operation.params.documentId = this.controller.params.documentId;
           },
           columnsGridSelect: [
               { header: 'МКД', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } },
               { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } },
               { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
               { header: 'Факт устранения', xtype: 'datecolumn', dataIndex: 'DateFactRemoval', format: 'd.m.Y', flex: 0.5, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
               
           ],
           columnsGridSelected: [
               { header: 'МКД', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } },
               { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, sortable: false }
           ],

           listeners: {
               getdata: function (me, records) {
                   var recordIds = [];
                   debugger;
                   Ext.each(records.items, function (item) {
                       recordIds.push(item.get('Id'));
                   });

                   if (recordIds[0] > 0) {
                       me.controller.mask('Сохранение', me.controller.getMainComponent());
                       B4.Ajax.request(B4.Url.action('AddPrescriptionViolations', 'PrescriptionViol', {
                           objectIds: recordIds,
                           documentId: officialReportId
                       })).next(function () {
                           me.controller.unmask();
                           me.controller.getStore(me.storeName).load();
                           return true;
                       }).error(function () {
                           me.controller.unmask();
                       });
                   }
                   else {
                       Ext.Msg.alert('Ошибка!', 'Необходимо выбрать цели проверки');
                       return false;
                   }
                   return true;
               }
           }
       },
        {
            /*
            Аспект инлайн таблицы нарушений
            */
            xtype: 'gkhinlinegridaspect',
            name: 'prescriptionViolationAspect',
            storeName: 'prescription.Violation',
            modelName: 'prescription.Violation',
            gridSelector: '#prescriptionViolationGrid',
            saveButtonSelector: '#prescriptionRealityObjListPanel #prescriptionViolationSaveButton',
            otherActions: function(actions) {
                var me = this;
                actions['#prescriptionViolationGrid #updateButton'] = {
                    click: {
                        fn: function() {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };

                actions['#prescriptionViolationGrid #setCommonRemovalDate'] = {
                    click: {
                        fn: function () {
                            var gisinfo = Ext.create('B4.view.prescription.PlanDateSetWindow');
                            gisinfo.show();
                        }
                    }
                };

                actions['#prescriptionPlanDateSetWindow #btnSetPlanRemovat'] = {
                                click: {
                                    fn: function (btn) {
                                        var gisinfo = btn.up('#prescriptionPlanDateSetWindow');
                                        var paramdate = gisinfo.down('#dfSetPlanRemoval').getValue();
                                        debugger;
                                        var docId = this.params.documentId;
                                        me.mask('Установка плановой даты устранения нарушения', this.getMainComponent());
                                        var result = B4.Ajax.request(B4.Url.action('SetNewDatePlanRemoval', 'PrescriptionViol', {
                                            paramdate: paramdate, documentId: docId
                                        }))
                                            .next(function (response) {
                                                me.unmask();
                                                var data = Ext.decode(response.responseText);
                                                Ext.Msg.alert('Сообщение', data.data);

                                                return true;
                                            }).error(function () {
                                                me.unmask();

                                            });
                                        gisinfo.close();
                                        me.controller.getStore(me.storeName).load();
                                    }
                                }
                            };
            },
            listeners: {
                beforesave: {
                    fn: function(asp, store) {
                        var result = true;

                        Ext.each(store.data.items, function(rec) {
                            if (Ext.isEmpty(rec.get('DatePlanRemoval'))) {
                                result = false;
                                return;
                            }
                        });

                        if (!result) {
                            Ext.Msg.alert('Ошибка!', 'У всех записей должен быть заполнен срок исполнения!');
                            return false;
                        }

                        return true;
                    }
                }
            },
            save: function () {
                var me = this;
                var store = this.getStore();
                var objViolationStore = this.controller.getStore('prescription.RealityObjViolation');
                debugger;
                // очищаем, так как записи уже удалили с сервера
                store.removed = [];
                var modifiedRecs = store.getModifiedRecords();
                if (modifiedRecs.length > 0) {
                    if (this.fireEvent('beforesave', this, store) !== false) {
                        me.mask('Сохранение', this.getGrid());
                        store.sync({
                            callback: function () {
                                me.unmask();
                                store.load();
                                objViolationStore.load();
                            },
                            failure: me.handleDataSyncError,
                            scope: me
                        });
                    }
                }
            },
            deleteRecord: function (record) {
                var me = this,
                    store = me.getStore(),
                    message = 'Вы действительно хотите удалить запись?',
                    realObjViolationStore = me.controller.getStore('prescription.RealityObjViolation'),
                    violationStore = me.controller.getStore('prescription.Violation'),
                    countViolation = violationStore.data.length,
                    countObjViolation = realObjViolationStore.data.length,
                    tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0],
                    prescriptionGrid = Ext.ComponentQuery.query('#prescriptionGrid')[0];


                if (record.getId()) {
                    // перед удалением проверяем наличие нарушений по дому
                    // если удаляется последняя запись по дому и/или остался последний дом, то выдаем предупреждение

                    if (countViolation == 1) {
                        if (countObjViolation == 1) {
                            message = 'Удаление последнего нарушения приведет к удалению Предписания. Вы действительно хотите удалить нарушение?';
                        } else {
                            message = 'Удаление последнего нарушения по дому приведет к удалению этого дома из Предписания. Вы действительно хотите удалить нарушение?';
                        }
                    }

                    Ext.Msg.confirm('Удаление записи!', message, function (result) {
                        if (result == 'yes') {
                            var model = me.controller.getModel(me.modelName);

                            var rec = new model({ Id: record.getId() });
                            me.controller.mask('Удаление', B4.getBody());
                            rec.destroy()
                                .next(function () {
                                    if (countObjViolation == 1 && countViolation == 1) {
                                        if (tree) {
                                            tree.getStore().load();
                                        };

                                        if (prescriptionGrid) {
                                            prescriptionGrid.getStore().load();
                                        }
                                        me.getGrid().up('prescriptionEditPanel').close();
                                    } else {
                                        realObjViolationStore.load();
                                        store.remove(record);
                                    }
                                    me.controller.unmask();
                                }, this)
                                .error(function (e) {
                                    Ext.Msg.alert('Ошибка!', e.responseData.message);
                                    me.controller.unmask();
                                }, this);
                        }
                    }, me);
                }
            }
        },
        {   /* 
               Аспект взаимодействия кнопки Добавить в таблице нарушения с массовой формой выбора нарушений
               По нажатию на кнопку Добавить будет открыта форма массовго выбора нарушений
               а после отбора будет вызван метод у главного аспекта createPrescription с передачей выбранных Ids нарушений
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'addObjViolationToPrescriptionAspect',
            gridSelector: '#prescriptionRealityObjViolationGrid',
            buttonSelector: '#prescriptionRealityObjViolationGrid #btnAddObjViolation',
            storeName: 'prescription.Violation',
            modelName: 'prescription.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionViolationMultiSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                {
                    header: 'Муниципальное образование',
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
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', dataIndex: 'ViolationGji', flex: 1, sortable: false },
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушения',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function (store, operation) {
                operation.params.documentId = this.controller.params.parentId;
                operation.params.forSelect = true;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('InspectionViolationId')); });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddViolations', 'PrescriptionViol'),
                            method: 'POST',
                            params: {
                                insViolIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.getStore('prescription.RealityObjViolation').load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки Протокол с массовой формой выборка
            По нажатию на кнопку Протокол открывается форма массовго выбора нарушений
            По нажатию на применить у главного аспекта вызывается метод createProtocol
            в который передаются Id нарушений
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'prescriptionToProtocolAspect',
            buttonSelector: '#prescriptionEditPanel [ruleId=PrescriptionToProtocolRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionToProtocolSelectWindow',
            storeSelectSelector: '#prescriptionViolationForSelected',
            storeSelect: 'prescription.ViolationForSelect',
            storeSelected: 'prescription.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                {
                    header: 'Муниципальное образование',
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
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты НПД', dataIndex: 'CodesPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', dataIndex: 'ViolationGji', flex: 1, sortable: false },
                { header: 'Адрес', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function(store, operation) {
                if (this.controller.params)
                    operation.params.documentId = this.controller.params.documentId;
            },

            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function(rec) {
                        listIds.push(rec.get('InspectionViolationId'));
                    });

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('prescriptionCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /PrescriptionGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'prescriptionInspectorMultiSelectWindowAspect',
            fieldSelector: '#prescriptionEditPanel #prescriptionInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function() {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            /*
            аспект для поля с массовым добавлением Направлений дейстелньости
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'prescriptionDirectionMultiSelectWindowAspect',
            fieldSelector: '#prescriptionEditPanel #prescriptionDirectionsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionDirectionSelectWindow',
            storeSelect: 'dict.ActivityDirection',
            storeSelected: 'dict.PrescriptionDirectionsForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор направлений деятельности',
            titleGridSelect: 'Направления для отбора',
            titleGridSelected: 'Выбранные направления',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['documentId'] = this.controller.params.documentId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddDirections', 'Prescription', {
                        directionIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Направления деятельности сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionAnnexAspect',
            gridSelector: '#prescriptionAnnexGrid',
            editFormSelector: '#prescriptionAnnexEditWindow',
            storeName: 'prescription.Annex',
            modelName: 'prescription.Annex',
            editWindowView: 'prescription.AnnexEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                }           
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'prescriptionOfficialReportAspect',
            gridSelector: 'prescriptionofficialreportgrid',
            editFormSelector: '#prescriptionfficialReportEditWindow',
            storeName: 'prescription.PrescriptionOfficialReport',
            modelName: 'prescription.PrescriptionOfficialReport',
            editWindowView: 'prescription.OfficialReportEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#prescriptionfficialReportEditWindow #cbOfficialReportType'] = { 'change': { fn: this.onChangeReportType, scope: this } };             
            },
            onChangeReportType: function (field, newValue) {
                var form = this.getForm(),                 
                    cbYesNo = form.down('#cbYesNo'),
                    dfViolationDate = form.down('#dfViolationDate'),                 
                    dfExtensionViolationDate = form.down('#dfExtensionViolationDate');

                if (newValue == B4.enums.OfficialReportType.Removal) {
                    cbYesNo.show();
                    cbYesNo.setDisabled(false);
                    dfViolationDate.show();
                    dfViolationDate.setDisabled(false);
                    dfExtensionViolationDate.hide();
                    dfExtensionViolationDate.setDisabled(true);                  
                }
                else if (newValue == B4.enums.OfficialReportType.Extension) {
                    cbYesNo.hide();
                    cbYesNo.setDisabled(true);
                    dfViolationDate.hide();
                    dfViolationDate.setDisabled(true);
                    dfExtensionViolationDate.show();
                    dfExtensionViolationDate.setDisabled(false);   
                }              
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    officialReportId = record.getId();
                    me.controller.getAspect('officialReportPrintAspect').loadReportStore();
                    if (officialReportId > 0) {
                        var grid = form.down('prescriptionoffrepviolationgrid'),
                            store = grid.getStore();
                        grid.setDisabled(false);
                        store.filter('OfficialReport', record.getId());
                    }
                    else {
                        var grid = form.down('prescriptionoffrepviolationgrid');
                        grid.setDisabled(true);
                    }

                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы решений об отмены с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionCancelAspect',
            gridSelector: '#prescriptionCancelGrid',
            editFormSelector: '#prescriptionCancelEditWindow',
            storeName: 'prescription.Cancel',
            modelName: 'prescription.Cancel',
            editWindowView: 'prescription.CancelEditWindow',
            onSaveSuccess: function(asp, record) {
                asp.setCancelId(record.getId());
            },
            setCancelId: function(id) {
                this.controller.params.CancelId = id;
                if (id) {
                    this.controller.getAspect('prescriptionCancelPrintAspect').loadReportStore();
                }
            },
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function(asp, record) {
                    asp.setCancelId(record.getId());
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            По нажатию на Добавить открывается форма выбора статей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение статей
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'prescriptionArticleLawAspect',
            gridSelector: '#prescriptionArticleLawGrid',
            saveButtonSelector: '#prescriptionArticleLawGrid #prescriptionSaveButton',
            storeName: 'prescription.ArticleLaw',
            modelName: 'prescription.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#prescriptionArticleLawMultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи для отбора',
            titleGridSelected: 'Выбранные статьи',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'PrescriptionArticleLaw'),
                            method: 'POST',
                            params: {
                                articleIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы документов оснований с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'prescriptionBaseDocumentAspect',
            gridSelector: 'prescriptionBaseDocumentGrid',
            editFormSelector: '#prescriptionBaseDocumentEditWindow',
            modelName: 'prescription.BaseDocument',
            editWindowView: 'prescription.BaseDocumentEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Prescription', this.controller.params.documentId);
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {
                '#prescriptionRealityObjListPanel': {
                    afterrender: me.onAfterRealityObjListPanelRender,
                    scope: me
                }
            };

        me.getStore('prescription.Violation').on('beforeload', me.onBeforeLoadRealityObjViol, me);
        me.getStore('prescription.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('prescription.PrescriptionOfficialReport').on('beforeload', me.onBeforeLoad, me);
        me.getStore('prescription.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('prescription.Cancel').on('beforeload', me.onBeforeLoad, me);
        me.getStore('prescription.RealityObjViolation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('prescription.RealityObjViolation').on('load', me.onLoadRealityObjectViolation, me);

        me.control(actions);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('prescriptionEditPanelAspect').setData(me.params.documentId);

            //Обновляем стор нарушений предписания
            me.getStore('prescription.RealityObjViolation').load();

            //Обновляем стор приложений
            me.getStore('prescription.Annex').load();

            //Обновляем стор приложений
            me.getStore('prescription.PrescriptionOfficialReport').load();

            //Обновляем стор статьи закона
            me.getStore('prescription.ArticleLaw').load();

            //Обновляем стор решений об отмене
            me.getStore('prescription.Cancel').load();
        }
    },

    onAfterRealityObjListPanelRender: function () {
        //var me = this,
        //    store = me.getStore('prescription.RealityObjViolation'),
        //    objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
        //    countRecords = store.getCount();

        //if (countRecords === 1) {
        //    objGrid.up('#prescriptionWestPanel').collapse();
        //} else {
        //    objGrid.up('#prescriptionWestPanel').expand();
        //}
    },

    onLoadRealityObjectViolation: function(store) {
        var me = this,
            storeViol = me.getStore('prescription.Violation'),
            objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
            countRecords = store.getCount();
        
        if (storeViol.getCount() > 0) {
            storeViol.removeAll();
        }
        
        if (countRecords > 0) {
            objGrid.getSelectionModel().select(0);
        } else {
            me.getStore('prescription.Violation').load();
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params)
            operation.params.documentId = this.params.documentId;
    },

    onBeforeLoadRealityObjViol: function(store, operation) {
        var objGrid = Ext.ComponentQuery.query('#prescriptionRealityObjViolationGrid')[0],
            violGrid = Ext.ComponentQuery.query('#prescriptionViolationGrid')[0],
            rec = objGrid.getSelectionModel().getSelection()[0];

        operation.params.documentId = this.params.documentId;
        if (rec) {
            operation.params.realityObjId = rec.getId();
            violGrid.setTitle(rec.get('RealityObject'));
        }
    }
});