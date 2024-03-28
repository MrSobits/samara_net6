Ext.define('B4.controller.actionisolated.motivatedpresentation.MotivatedPresentation', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.permission.actionisolated.MotivatedPresentation',
        'B4.form.ComboBox',
        'B4.enums.TypeDocumentGji',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'actionisolated.MotivatedPresentation',
        'inspectionactionisolated.InspectionActionIsolated',
        'inspectionpreventiveaction.InspectionPreventiveAction'
    ],

    stores: [
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'actionisolated.motivatedpresentation.Annex',
        'actionisolated.motivatedpresentation.InspectionInfo',
        'actionisolated.motivatedpresentation.ViolationInfo',
        'actionisolated.motivatedpresentation.RealityObjectForSelect',
        'actionisolated.motivatedpresentation.RealityObjectForSelected',
    ],

    views: [
        'actionisolated.motivatedpresentation.AnnexEditWindow',
        'actionisolated.motivatedpresentation.AnnexGrid',
        'actionisolated.motivatedpresentation.EditPanel',
        'actionisolated.motivatedpresentation.InspectionAddWindow'
    ],

    mainView: 'actionisolated.motivatedpresentation.EditPanel',
    mainViewSelector: '#motivatedPresentationEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'actionisolatedmotivatedpresentationperm',
            editFormAspectName: 'motivatedPresentationEditPanelAspect'
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'motivatedPresentationCreateButtonAspect',
            buttonSelector: '#motivatedPresentationEditPanel gjidocumentcreatebutton',
            containerSelector: '#motivatedPresentationEditPanel',
            typeDocument: 230, //B4.enums.TypeDocumentGji.MotivatedPresentation
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'MotivatedPresentationToWarningDocRule') {
                    return false;
                }
                return true;
            }
        },
        {
            xtype: 'statebuttonaspect',
            name: 'motivatedPresentationStateButtonAspect',
            stateButtonSelector: '#motivatedPresentationEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('motivatedPresentationEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#actionIsolatedNavigationPanel').getComponent('actionisolatedMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'motivatedPresentationEditPanelAspect',
            editPanelSelector: '#motivatedPresentationEditPanel',
            modelName: 'actionisolated.MotivatedPresentation',
            otherActions: function(actions) {
            },
            onSaveSuccess: function (asp, rec) {
                //исключение изменения заголовка панели
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                var callbackUnMask = asp.controller.params?.callbackUnMask,
                    parentDocumentType = rec.get('ParentDocumentType');

                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                asp.controller.params = asp.controller.params || {};
                asp.controller.params.parentDocumentType = parentDocumentType;
                asp.controller.params.isActionIsolated = parentDocumentType === B4.enums.TypeDocumentGji.ActActionIsolated;

                panel.down('#motivatedPresentationTabPanel').setActiveTab(0);
                panel.down('motivatedpresentationinspectioninfogrid').on('rowaction', asp.controller.onInspectionInfoGridRowAction, asp.controller);
                panel.down('#btnCreateUnscheduledInspection').on('click', asp.controller.onInspectionCreateButtonClick, asp.controller);

                asp.controller.getStore('actionisolated.motivatedpresentation.InspectionInfo').load();
                asp.controller.getStore('actionisolated.motivatedpresentation.ViolationInfo').load();

                this.disableButtons(false);
                this.updateFieldValue(panel, rec);

                this.controller.getAspect('motivatedPresentationStateButtonAspect').setStateData(rec.getId(), rec.get('State'));
                this.controller.getAspect('motivatedPresentationCreateButtonAspect').setData(rec.getId());
                this.controller.getAspect('motivatedPresentationPrintAspect').loadReportStore();
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            updateFieldValue: function (panel, rec) {
                var fieldInspectors = panel.down('#trigfInspectors');

                fieldInspectors.updateDisplayedText(rec.data.Inspectors);
                fieldInspectors.setValue(rec.data.InspectorIds);
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'motivatedPresentationInspectorMultiSelectWindowAspect',
            fieldSelector: '#motivatedPresentationEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#motivatedPresentationInspectorsSelectWindow',
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
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'DocumentGjiInspector'),
                        method: 'POST',
                        params: {
                            inspectorIds: (recordIds),
                            documentId: asp.controller.params.documentId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'motivatedPresentationPrintAspect',
            buttonSelector: '#motivatedPresentationEditPanel #btnPrint',
            codeForm: 'MotivatedPresentationActionIsolated',
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'motivatedPresentationToWarningDocRuleHouseAspect',
            buttonSelector: '#motivatedPresentationEditPanel [ruleId=MotivatedPresentationToWarningDocRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#motivatedPresentationActionToWarningDocRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'actionisolated.motivatedpresentation.RealityObjectForSelect',
            storeSelected: 'actionisolated.motivatedpresentation.RealityObjectForSelected',
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
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',

            onBeforeLoad: function (store, operation) {
                var me = this,
                    record = me.controller.getAspect('motivatedPresentationEditPanelAspect').getRecord();

                if (record) {
                    operation.params.documentId = record.get('Id');
                }
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('motivatedPresentationCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.houseIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'motivatedPresentationActionIsolatedAnnexGridWindowAspect',
            gridSelector: '#motivatedPresentationActionIsolatedAnnexGrid',
            editFormSelector: '#motivatedPresentationActionIsolatedAnnexEditWindow',
            storeName: 'actionisolated.motivatedpresentation.Annex',
            modelName: 'actionisolated.motivatedpresentation.Annex',
            editWindowView: 'actionisolated.motivatedpresentation.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('MotivatedPresentation', asp.controller.params.documentId);
                    }
                    return true;
                }
            }
        },
    ],

    init: function () {
        var me = this;

        me.getStore('actionisolated.motivatedpresentation.InspectionInfo').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.motivatedpresentation.ViolationInfo').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actionisolated.motivatedpresentation.Annex').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#motivatedPresentationActionIsolatedAnnexGrid': {
                afterrender: function(grid) {
                    grid.getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('motivatedPresentationEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    },
    
    onInspectionInfoGridRowAction: function (grid, action, rec) {
        if (action.toLowerCase() === 'gotoinspection') {
            var me = this,
                model = me.getModel('inspectionactionisolated.InspectionActionIsolated');

            me.mask('Загрузка', me.getMainComponent());
            
            model.load(rec.get('InspectionId'), {
                success: function(rec) {
                    me.openInspectionActionIsolatedEditPanel(rec);
                },
                failure: function() {
                    me.unmask();
                },
                scope: me
            });
        }
    },
    
    onInspectionCreateButtonClick: function () {
        var me = this,
            inspectionAddWindow = Ext.ComponentQuery.query('#inspectionAddWindow')[0];

        if (!inspectionAddWindow) {
            inspectionAddWindow = me.getView('actionisolated.motivatedpresentation.InspectionAddWindow').create(
                {
                    renderTo: B4.getBody().getActiveTab().getEl()
                });

            inspectionAddWindow.down('b4savebutton').on('click', function () {
                if (!inspectionAddWindow.getForm().isValid()) {
                    var fields = inspectionAddWindow.getForm().getFields(),
                        invalidFields = '';

                    Ext.each(fields.items, function (field) {
                        if (!field.isValid()) {
                            invalidFields += '<b><br>' + field.fieldLabel + '</b>';
                        }
                    });

                    Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
                    return false;
                }

                inspectionAddWindow.getForm().updateRecord();

                me.mask('Сохранение', inspectionAddWindow);
                inspectionAddWindow.getRecord().save({
                    success: function(rec) {
                        inspectionAddWindow.close();

                        me.openInspectionActionIsolatedEditPanel(rec);
                    },
                    failure: function (res) {
                        Ext.Msg.alert('Ошибка!', res.message);
                        me.unmask();
                    },
                    scope: me
                });
            });

            inspectionAddWindow.down('b4closebutton').on('click', function () {
                Ext.Msg.confirm('Внимание', 'Вы действительно хотите закрыть окно? Проверка не будет сохранена', function (result) {
                    if (result === 'yes') {
                        inspectionAddWindow.close();
                    }
                });
            });
        }
        
        me.mask('Загрузка', inspectionAddWindow);

        B4.Ajax.request(B4.Url.action('GetNewInspectionBasementInfo', 'MotivatedPresentation', {
            documentId: me.params.documentId,
            parentDocumentType: me.params.parentDocumentType
        })).next(function (response) {
            me.unmask();
            var obj = Ext.JSON.decode(response.responseText),
                tfNumberAndDocumentDate = inspectionAddWindow.down('[name=NumberAndDocumentDate]'),
                record, modelPath;

            if (me.params.isActionIsolated)
            {
                modelPath = 'inspectionactionisolated.InspectionActionIsolated';

                tfNumberAndDocumentDate.setFieldLabel('КНМ без взаимодействия');
            }
            else
            {
                modelPath = 'inspectionpreventiveaction.InspectionPreventiveAction';

                tfNumberAndDocumentDate.setFieldLabel('Профилактическое мероприятие');
            }

            record = me.getModel(modelPath).create(obj)
            record.set('NumberAndDocumentDate', obj.NumberAndDocumentDate);

            inspectionAddWindow.loadRecord(record);
            inspectionAddWindow.getForm().isValid();
            inspectionAddWindow.show();
        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', e.message);
            me.unmask();
        });
    },

    openInspectionActionIsolatedEditPanel: function (record) {
        var me = this,
            portal = me.getController('PortalController'),
            navigationController = me.params.isActionIsolated
                ? 'B4.controller.inspectionactionisolated.Navigation'
                : 'B4.controller.inspectionpreventiveaction.Navigation',
            params;

        params = record;
        params.defaultController = me.params.isActionIsolated
            ? 'B4.controller.inspectionactionisolated.Edit'
            : 'B4.controller.inspectionpreventiveaction.Edit';

        params.defaultParams = { inspectionId: record.getId() };

        if (!me.hideMask) {
            me.hideMask = function () { me.unmask(); };
        }

        me.loadController(navigationController, params, portal.containerSelector, me.hideMask);
    }
});