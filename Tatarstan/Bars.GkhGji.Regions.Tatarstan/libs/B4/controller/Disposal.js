Ext.define('B4.controller.Disposal', {
    extend: 'B4.base.Controller',
    params: null,

    requires: [
        'B4.Ajax',
        'B4.DisposalTextValues',
        'B4.Url',
        'B4.QuickMsg',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.Disposal',
        'B4.aspects.permission.TatDisposal',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhBlobText',
        'B4.form.SelectWindow',
        'B4.ux.grid.column.Enum',
        'B4.enums.ExpertType',
        'B4.enums.TypeBase',
        'B4.enums.TypeCheck',
        'B4.enums.TypeAgreementResult',
        'B4.enums.TypeAgreementProsecutor',
        'B4.enums.DecisionTypeAgreementProsecutor',
        'B4.enums.TypeDocumentGji',
        'B4.enums.YesNoNotSet'
    ],

    models: [
        'Disposal',
        'Decision',
        'ActCheck',
        'ActSurvey',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.DisposalVerificationSubject',
        'disposal.TypeSurvey',
        'disposal.SurveyPurpose',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
        'disposal.ControlObjectInfo',
        'disposal.InspectionBase',
        'disposal.KnmReason',
        //возможно, переместится в акты проверок
        'controllist.ControlList',
        'controllist.ControlListQuestion'
    ],

    stores: [
        'RealityObjectGjiForSelect',
        'RealityObjectGjiForSelected',
        'Disposal',
        'disposal.Expert',
        'disposal.ProvidedDoc',
        'disposal.Annex',
        'disposal.TypeSurvey',
        'disposal.DisposalVerificationSubject',
        'disposal.SurveyPurpose',
        'disposal.InspFoundationCheck',
        'disposal.SurveyObjective',
        'disposal.InspectionBase',
        'disposal.KnmReason',
        'disposal.ControlObjectInfo',
        'dict.SurveyPurposeForSelect',
        'dict.SurveyPurposeForSelected',
        'dict.SurveyObjectiveForSelect',
        'dict.SurveyObjectiveForSelected',
        'dict.NormativeDocForSelect',
        'dict.NormativeDocForSelected',
        'dict.SurveySubjectForSelect',
        'dict.SurveySubjectForSelected',
        'dict.TypeSurveyGjiForSelect',
        'dict.TypeSurveyGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ExpertGjiForSelect',
        'dict.ExpertGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'dict.Municipality',
        'dict.InspectionBaseType',
        //возможно, переместится в акты проверок
        'controllist.ControlList',
        'controllist.ControlListQuestion'
    ],

    views: [
        'disposal.EditPanel',
        'disposal.InspFoundationCheckGrid',
        'disposal.SubjectVerificationGrid',
        'disposal.SurveyPurposeGrid',
        'disposal.SurveyObjectiveGrid',

        'disposal.KnmReasonGrid',
        'disposal.KnmReasonWindow',

        'disposal.AnnexGrid',
        'disposal.AnnexEditWindow',
        'disposal.ExpertGrid',
        'disposal.TypeSurveyGrid',
        'disposal.ProvidedDocGrid',
        'disposal.inspectionbase.Grid',
        'disposal.inspectionbase.EditWindow',
        'SelectWindow.MultiSelectWindow',
        'disposal.controlobjectinfo.EditWindow',
        'disposal.controlobjectinfo.Grid',

        //возможно, переместится в акты проверок
        'controllist.Grid',
        'controllist.editwindow.EditWindow',
        'controllist.editwindow.ControlListQuestionGrid'
    ],

    mainView: 'disposal.EditPanel',
    mainViewSelector: '#disposalEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    getCurrentContextKey: function() {
        return 'B4.controller.Disposal';
    },

    aspects: [
        {
            /*
            Аспект формирования документов для Распоряжения/Решения
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'disposalCreateButtonAspect',
            buttonSelector: 'disposaleditpanel #btnCreateDisposal',
            containerSelector: 'disposaleditpanel',
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId === 'DisposalToActCheckByRoRule' ||
                    params.ruleId === 'DisposalToActSurveyRule') {
                    return false;
                }

                return true;
            }
        },
        {
            xtype: 'disposalperm',
            editFormAspectName: 'disposalEditPanelAspect',
            name: 'DisposalPermissionAspect'
        },
        {
            xtype: 'tatdisposalperm',
            editFormAspectName: 'disposalEditPanelAspect',
            name: 'TatDisposalPermissionAspect'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.DocumentsGji.Disposal.Field.SendToErp_View',
                    applyTo: '#btnSendToErp',
                    selector: 'disposaleditpanel',
                    applyBy: this.setVisible
                }
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitStart', applyTo: '[name=TimeVisitStart]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitEnd', applyTo: '[name=TimeVisitEnd]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDate', applyTo: '[name=NcDate]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNum', applyTo: '[name=NcNum]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcDateLatter', applyTo: '[name=NcDateLatter]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcNumLatter', applyTo: '[name=NcNumLatter]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcSent', applyTo: '[name=NcSent]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.NcObtained', applyTo: '[name=NcObtained]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.CountDays', applyTo: '[name=CountDays]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.CountHours', applyTo: '[name=CountHours]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.Prosecutor', applyTo: '[name=Prosecutor]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Disposal.Field.PlannedActions', applyTo: 'gkhtriggerfield[name=PlannedActions]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Decision.Field.SubmissionDate', applyTo: '[name=SubmissionDate]', selector: '#disposalEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.Decision.Field.ReceiptDate', applyTo: '[name=ReceiptDate]', selector: '#disposalEditPanel' },
                {
                    name: 'GkhGji.DocumentReestrGji.Decision.Field.Annex.DocumentType', applyTo: '[name=ErknmTypeDocument]', selector: '#disposalAnnexEditWindow',
                    applyBy: function (component, allowed) {
                        if (this.controller.params.typeDocument == B4.enums.TypeDocumentGji.Decision) {
                            component.allowBlank = !allowed;
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'disposalPrintAspect',
            buttonSelector: '#disposalEditPanel #btnPrint',
            getUserParams: function() {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'disposalStateButtonAspect',
            stateButtonSelector: '#disposalEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('disposalEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            /*
            Аспект основной панели Карточки распоряжения
            В нем вешаемся на событие aftersetpaneldata, чтобы загрузить подчиенные сторы
            А также проставить дополнительные значения
            Вешаемся на savesuccess чтобы после сохранения сразу получить Номер и обновить Вкладку
            */
            xtype: 'gjidocumentaspect',
            name: 'disposalEditPanelAspect',
            editPanelSelector: '#disposalEditPanel',
            listeners: {
                beforesave: function (asp, rec) {
                    var panel = asp.getPanel(),
                        tfPlannedActions = panel.down('#tfPlannedActions');

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddDecisionPlannedActions', 'KnmAction'),
                        method: 'POST',
                        params: {
                            actionId: tfPlannedActions.getValue(),
                            documentId: asp.controller.params.documentId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            },
            setData: function(objectId) {
                var me = this,
                    panel = me.getPanel(),
                    model, 
                    isDisposal = me.controller.params.typeDocument === B4.enums.TypeDocumentGji.Disposal;

                panel.setDisabled(true);
                isDisposal ? this.modelName = 'Disposal' : this.modelName = 'Decision';

                panel.down('combobox[name=TypeAgreementProsecutor]').bindStore(
                    isDisposal
                        ? B4.enums.TypeAgreementProsecutor.getStore()
                        : B4.enums.DecisionTypeAgreementProsecutor.getStore()
                );

                if (me.fireEvent('beforesetdata', me, objectId) !== false) {
                    me.objectId = objectId;

                    model = me.getModel();

                    objectId > 0 ? model.load(objectId, {
                        params: {
                            typeDocument: me.controller.params.typeDocument
                        },
                        success: function(rec) {
                            me.setPanelData(rec);
                        },
                        scope: me
                    }) : me.setPanelData(new model({ Id: 0 }));
                }
            },
            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #sfIssuredDisposal'] = { beforeload: { fn: me.onBeforeLoadInspectorManager, scope: me } };
                actions[me.editPanelSelector + ' button[name=btnSendTo]'] = { click: { fn: me.onSendToButtonClick, scope: me } };
                actions[me.editPanelSelector + ' #sfControlType'] = { change: { fn: me.onControlTypeChange, scope: me } };
            },
            saveRecord: function(rec) {
                var me = this;

                Ext.Msg.confirm('Внимание!', 'Убедитесь, что вид проверки указан правильно', function(result) {
                    if (result === 'yes') {
                        if (me.fireEvent('beforesave', me, rec) !== false) {
                            if (me.hasUpload()) {
                                me.saveRecordHasUpload(rec);
                            } else {
                                me.saveRecordHasNotUpload(rec);
                            }
                        }
                    }
                });
            },
            //переопределение удаления с целью показа зависимостей пользователю
            btnDeleteClick: function () {
                var me = this,
                    panel = me.getPanel(),
                    record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
                    if (result == 'yes') {
                        B4.Ajax.request({
                            url: B4.Url.action('GetDependenciesString', 'TatarstanDisposal',
                                { disposalId: record.getId() }
                            )
                        }).next(function (response) {
                            var data = response.responseText;
                            if (data != null && data.length > 0) {
                                Ext.Msg.confirm('Удаление записи!', data,
                                    function(result) {
                                        if (result == 'yes') {
                                            me.deleteRecord(record, panel);
                                        }
                                    }, this);
                            } else {
                                me.deleteRecord(record, panel);
                            }
                        }).error(function (result) {
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            this.unmask();
                        });
                    }
                }, this);
            },
            deleteRecord: function (record, panel) {
                this.mask('Удаление', B4.getBody());
                record.destroy() 
                    .next(function () {

                        //Обновляем дерево меню
                        var tree = Ext.ComponentQuery.query(this.controller.params.treeMenuSelector)[0];
                        tree.getStore().load();

                        Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                        panel.close();
                        this.unmask();
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        this.unmask();
                    }, this);
            },
            onBeforeLoadInspectorManager: function(field, operation) {
                var me = this;

                operation.params = operation.params || {};
                operation.params.headOnly = true;

                if (me.controller.params.typeDocument === B4.enums.TypeDocumentGji.Decision) {
                    operation.params.controlTypeId = me.controller.params.controlTypeId;
                    operation.params.issuerOnly = true;
                }

                return true;
            },
            onControlTypeChange: function (field, value) {
                var me = this;

                me.controller.params = me.controller.params || {};
                me.controller.params.controlTypeId = value?.Id ?? 0;
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx]) {
                        break;
                    }

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function(asp, rec, panel) {
                var me = this,
                    docType = rec.get('TypeDocumentGji'),
                    isDisposal = docType === B4.enums.TypeDocumentGji.Disposal,
                    title = isDisposal ? 'Распоряжение' : 'Решение',
                    callbackUnMask,
                    cbTypeCheck = panel.down('#cbTypeCheck'),
                    sfControlType = panel.down('#sfControlType'),
                    printAspect = me.controller.getAspect('disposalPrintAspect'),
                    createDocAspect = me.controller.getAspect('disposalCreateButtonAspect'),
                    tabPanel = panel.down('#disposalTabPanel');

                asp.controller.params = asp.controller.params || {};
                asp.controller.params.documentTitle = title;
                asp.controller.params.singleRegistries = [];

                asp.controller.params.singleRegistries.push(isDisposal ? 'Erp' : 'Erknm');

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber')) {
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                } else {
                    panel.setTitle(title);
                }

                tabPanel.setActiveTab(0);
                panel.down('[name=IssuedDisposal]').setFieldLabel('ДЛ, вынесшее ' + title);

                var submissionDateField = panel.down('[name=SubmissionDate]'),
                    receiptDateField = panel.down('[name=ReceiptDate]'),
                    usingRemoteField = panel.down('[name=UsingMeansRemoteInteraction]'),
                    infoRemoteField = panel.down('[name=InfoUsingMeansRemoteInteraction]'),
                    decisionPlace = panel.down('[name=DecisionPlace]'),
                    kindCheckTypes = [B4.enums.TypeCheck.PlannedDocumentation, B4.enums.TypeCheck.NotPlannedDocumentation],
                    sfInspectionBase = panel.down('[name=InspectionBase]'),
                    insBasePanel = panel.down('disposalinspectionbasegrid'),
                    controlObjInfoPanel = panel.down('disposalcontrolobjectinfogrid'),
                    erknmTypeDocumentField = panel.down('#disposalAnnexGrid').down('gridcolumn[dataIndex=ErknmTypeDocument]'),
                    plannedActionField = panel.down('#tfPlannedActions');
                
                // Скрываем все поля, принадлежащие только документу Решение,
                // потом проверяем условия их отображения
                var decisionFields = [
                    erknmTypeDocumentField,
                    submissionDateField,
                    receiptDateField,
                    usingRemoteField,
                    infoRemoteField,
                    plannedActionField,
                    decisionPlace
                ];

                Ext.Array.forEach(decisionFields, function (cmp) {
                    cmp.hide();
                    cmp.allowBlank = true;
                });

                if (isDisposal) {
                    tabPanel.hideTab(insBasePanel);
                    tabPanel.hideTab(controlObjInfoPanel);
                    sfInspectionBase.show();
                }
                else {
                    tabPanel.showTab(insBasePanel);
                    tabPanel.showTab(controlObjInfoPanel);
                    sfInspectionBase.hide();
                    decisionPlace.show();
                    submissionDateField.allowBlank = !submissionDateField.permissionRequired;
                    receiptDateField.allowBlank = !receiptDateField.permissionRequired;
                    erknmTypeDocumentField.show();

                    if (kindCheckTypes.includes(rec.getData().KindCheck.Code)) {
                        submissionDateField.show();
                        receiptDateField.show();
                    }
                    else {
                        submissionDateField.allowBlank = true;
                        receiptDateField.allowBlank = true;
                    }

                    kindCheckTypes = [
                        B4.enums.TypeCheck.PlannedExit,
                        B4.enums.TypeCheck.NotPlannedExit,
                        B4.enums.TypeCheck.PlannedInspectionVisit,
                        B4.enums.TypeCheck.NotPlannedInspectionVisit
                    ];

                    if (kindCheckTypes.includes(rec.getData().KindCheck.Code)) {
                        usingRemoteField.show();

                        if (usingRemoteField.getValue() === B4.enums.YesNoNotSet.Yes) {
                            infoRemoteField.show();
                            infoRemoteField.allowBlank = false;
                        }
                    }
                }

                if (asp.controller.params) {
                    //получаем вид проверки
                    asp.controller.params.kindCheckId = cbTypeCheck.getValue();
                    asp.controller.params.controlTypeId = sfControlType.getValue();

                    Ext.ComponentQuery.query('button[name=btnSendTo]').forEach(function (btn) {
                        //указать дополнительные параметры, которые необходимы для методов отправки
                        if (btn.additionalParams && btn.additionalParams.length > 1) {
                            btn.additionalParams.forEach(function (param) {
                                asp.controller.params[param] = panel.down('[name=' + param + ']').getValue();
                            })
                        }
                    });

                    asp.rebrendPanel(panel, rec.get('TypeBase'));
                }

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                const controllerName = isDisposal
                    ? 'Disposal'
                    : 'TatarstanDecision';
                B4.Ajax.request({
                    url: B4.Url.action('GetInfo', controllerName, { documentId: asp.controller.params.documentId }),
                    //для IE, чтобы не кэшировал GET запрос
                    cache: false
                }).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    const obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors = panel.down('#trigFInspectors'),
                        fieldPlannedActions = panel.down('#tfPlannedActions');

                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    if (!fieldPlannedActions.hidden) {
                        fieldPlannedActions.updateDisplayedText(obj.plannedActionNames);
                        fieldPlannedActions.setValue(obj.plannedActionIds);
                    }

                    panel.down('#tfBaseName').setValue(obj.baseName);
                    panel.down('#tfPlanName').setValue(obj.planName);

                    asp.disableButtons(false);
                }).error(function() {
                    asp.controller.unmask();
                });

                me.controller.getStore('disposal.Expert').load();
                me.controller.getStore('disposal.ProvidedDoc').load();
                me.controller.getStore('disposal.Annex').load();
                me.controller.getStore('disposal.TypeSurvey').load();
                me.controller.getStore('disposal.DisposalVerificationSubject').load();
                me.controller.getStore('disposal.SurveyPurpose').load();
                me.controller.getStore('disposal.SurveyObjective').load();
                me.controller.getStore('disposal.InspFoundationCheck').load();
                me.controller.getStore('disposal.ControlObjectInfo').load();
                me.controller.getStore('controllist.ControlList').load();
                me.controller.getStore('disposal.InspectionBase').load();
                me.controller.getStore('disposal.KnmReason').load();

                me.controller.getAspect('disposalNoticeDescriptionAspect').doInjection();

                // Значение по умолчанию "Внеплановая документарная" для поля "Вид проверки", если не задано
                if (cbTypeCheck.getValue() == null) {
                    panel.down('#cbTypeCheck').setValue(B4.enums.TypeCheck.NotPlannedDocumentation);
                }

                // Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('disposalStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем отчеты
                printAspect.codeForm = B4.enums.TypeDocumentGji.getMeta(docType).Name;
                printAspect.loadReportStore();

                // обновляем кнопку Сформирвоать
                createDocAspect.typeDocument = docType;
                createDocAspect.setData(rec.get('Id'));

                //вкладка "Проверочные листы" неактивна, если Вид проверки в разделе Распоряжение не "Плановая выездная" и не "Плановая документарная"
                panel.down('[name=controlListGrid]').setDisabled(asp.controller.params.kindCheckId != B4.enums.TypeCheck.PlannedExit
                    && asp.controller.params.kindCheckId != B4.enums.TypeCheck.PlannedDocumentation);
            },
            onSaveSuccess: function(asp, rec) {
                asp.getPanel().setTitle(asp.controller.params.documentTitle + ' ' + rec.get('DocumentNumber'));

                if (asp.controller.params) {
                    asp.controller.params.kindCheckId = asp.getPanel().down('#cbTypeCheck').getValue();
                }
            },
            onSendToButtonClick: function (btn) {
                var me = this,
                    params = { id: me.controller.params.documentId };
                
                if (btn.additionalParams && btn.additionalParams.length > 1) {
                    btn.additionalParams.forEach(function (param) {
                        params[param] = me.controller.params[param];
                    })
                }

                if (me.controller.params.typeDocument === B4.enums.TypeDocumentGji.Decision) {
                    me.controller.mask('Проверка перед отправкой в ЕРКНМ', me.controller.getMainComponent());

                    var msgTemplate = "<br/>Отсутствующие данные не будут отправлены в ЕКРНМ<br/> Хотите продолжить?";
                    
                    B4.Ajax.request({
                        url: B4.Url.action('BeforeSendCheck', btn.controllerName),
                        params: params
                    }).next(function () {
                        me.sendToRegistry(btn, params);
                    }).error(function (response){
                        Ext.Msg.confirm('Внимание!', response.message.split(';').join('<br/>') + msgTemplate, 
                            function(result) {
                                if (result === 'yes') {
                                    me.sendToRegistry(btn, params);
                                }
                            });
                    });

                    me.controller.unmask();
                }
                else {
                    me.sendToRegistry(btn, params);
                }
            },
            sendToRegistry: function (btn, params){
                var me = this;
                
                me.controller.mask('Отправление в ' + btn.singleRegistryTitle, me.controller.getMainComponent());

                B4.Ajax.request({
                    url: B4.Url.action(btn.sendMethodName, btn.controllerName, params),
                    timeout: 9999999
                }).next(function (response) {
                    me.controller.unmask();

                    Ext.Msg.alert('Отправка в ' + btn.singleRegistryTitle, response.message || 'Выполнено успешно');
                    return true;
                }).error(function (e) {
                    me.controller.unmask();

                    Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
                });
            },
            /**
             * Кастомизируем панель в зависимости от типа проверки
             * @param {any} panel
             * @param {B4.enums.TypeBase} typeInspection Тип проверки
             */
            rebrendPanel: function(panel, typeInspection) {
                var me = this,
                    countDays = panel.down('[name=CountDays]'),
                    countHours = panel.down('[name=CountHours]'),
                    prosecutor = panel.down('[name=Prosecutor]'),
                    reasonErpChecking = panel.down('[name=ReasonErpChecking]'),
                    btnSendToTor = panel.down('#btnSendToTor'),
                    notificationType = panel.down('[name=NotificationType]'),
                    components = [
                        btnSendToTor,
                        countDays,
                        countHours,
                        prosecutor,
                        notificationType,
                        reasonErpChecking
                    ],
                    setCmpShowDisabled = function (cmp, registryName) {
                        cmp.showDisabled = !me.controller.params.singleRegistries.includes(registryName);
                    };

                me.controller.params.typeInspection = typeInspection;

                setCmpShowDisabled(reasonErpChecking, 'Erp');

                // Схожие компоненты реестров с указанием возможности отображения
                [ 'Erp', 'Erknm' ].forEach(function (name) {
                    [
                        "SendTo",
                        "RegistrationNumber",
                        "Id",
                        "RegistrationDate"
                    ].forEach(function (field) {
                        var cmp = panel.down('[name=' + name + field + ']') ??
                            panel.down('[name=' + field + name + ']');

                        components.push(cmp);
                        setCmpShowDisabled(cmp, name);
                    });

                    var sendButton = panel.down('#btnSendTo' + name);
                    components.push(sendButton);
                    setCmpShowDisabled(sendButton, name);
                })

                // сначала всё скрываем, а потом показываем только нужное
                var allowBlankObj = {};
                Ext.Array.forEach(components, function(cmp) {
                    cmp.hide();
                    // сохраняем состояние allowBlank полей 'CountHours', 'CountDays' и 'Prosecutor'
                    // так как их обязательность определяется в настройках прав доступа 
                    if (cmp.name === 'CountHours' || cmp.name === 'CountDays' || cmp.name === 'Prosecutor') {
                        allowBlankObj[cmp.name] = cmp.allowBlank;
                    }
                    cmp.allowBlank = true;
                });

                var allowBlankTuples = [
                    {
                        field: prosecutor,
                        allowBlank: allowBlankObj.Prosecutor
                    },
                    {
                        field: countDays,
                        allowBlank: allowBlankObj.CountDays
                    },
                    {
                        field: countHours,
                        allowBlank: allowBlankObj.CountHours
                    }
                ];
                
                switch (typeInspection) {
                    case B4.enums.TypeBase.PlanJuridicalPerson:
                        me.setFullPermissionOptions(allowBlankTuples);
                        break;
                    case B4.enums.TypeBase.ProsecutorsClaim:
                    case B4.enums.TypeBase.DisposalHead:
                    case B4.enums.TypeBase.CitizenStatement:
                    case B4.enums.TypeBase.InspectionActionIsolated:
                    case B4.enums.TypeBase.InspectionPreventiveAction:
                        reasonErpChecking.allowBlank = reasonErpChecking.showDisabled;
                        me.setFullPermissionOptions(allowBlankTuples);
                        break;
                    default:
                        components = []; // здесь должно быть пусто, по умолчанию не показываем ничего из того, что скрыли
                        break;
                }

                Ext.Array.forEach(components, function (cmp) {
                    if (cmp.showDisabled === undefined || cmp.showDisabled !== true) {
                        cmp.show();
                    }
                });
            },
            setFullPermissionOptions: function(permissionTuples){
                permissionTuples.forEach(function(tuple){
                    tuple.field.allowBlank = tuple.allowBlank;
                });
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на 1 дом и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'disposalToActCheck1HouseGJIAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActCheckByRoRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActCheckByRoRuleSelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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
                //спрятать панель выбранных домов
                var wnd = Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #multiSelectedPanel')[0];
                if (wnd) {
                    wnd.hide();
                }

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;
            },
            onRowSelect: function(rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },
            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;
                        
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дом');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акт на обследование и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActSurvey и передаются выбранные Id домов

            метод onRowSelect перекрываем потомучто необходимо изменить поведение выделения строки, чтобы можно было
            Выбрать только одну запись а не множество
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'disposalToActSurveyAspect',
            buttonSelector: '#disposalEditPanel [ruleId=DisposalToActSurveyRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalToActSurveySelectWindow',
            storeSelectSelector: '#realityobjForSelectStore',
            storeSelect: 'RealityObjectGjiForSelect',
            storeSelected: 'RealityObjectGjiForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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

            onBeforeLoad: function(store, operation) {
                //получаем текущую запись документа Распоряжения
                var record = this.controller.getAspect('disposalEditPanelAspect').getRecord();

                if (this.controller.params && this.controller.params.inspectionId > 0)
                    operation.params.inspectionId = this.controller.params.inspectionId;

                if (record.get('TypeDisposal') === 20) {
                    //Если акт обследования делается из Распоряжения на проверку Предписания то
                    //получаем все дома из предписаний по которым создано это распоряжение
                    operation.params.documentId = record.get('Id');
                }
            },

            onRowSelect: function(rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    records.each(function(rec) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('disposalCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дом)
                        params = creationAspect.getParams(btn);
                        params.realityIds = recordIds;
                        
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
            /*
            аспект взаимодействия грида типов обследований с массовой формой выбора типов обслуживания
            по нажатию на кнопку добавления показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем Типы обслуживания через серверный метод /DisposalTypeSurvey/AddTypeSurveys
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalTypeSurveyAspect',
            modelName: 'disposal.TypeSurvey',
            storeName: 'disposal.TypeSurvey',
            gridSelector: '#disposalTypeSurveyGrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalTypeSurveySelectWindow',
            storeSelect: 'dict.TypeSurveyGjiForSelect',
            storeSelected: 'dict.TypeSurveyGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор типов обследования',
            titleGridSelect: 'Типы обследования для отбора',
            titleGridSelected: 'Выбранные типы обследования',

            onBeforeLoad: function(store, operation) {
                if (this.controller.params) {
                    operation.params.kindCheckId = this.controller.params.kindCheckId;
                }
            },

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTypeSurveys', 'DisposalTypeSurvey', {
                        typeIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function() {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Типы обследований сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /DocumentGjiInspector/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'disposalInspectorMultiSelectWindowAspect',
            fieldSelector: '#disposalEditPanel #trigFInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
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

                    records.each(function(rec) { recordIds.push(rec.get('Id')); });

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

                    return true;
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this;

                if (me.controller.params.typeDocument === B4.enums.TypeDocumentGji.Decision) {
                    operation.params = operation.params || {};
                    operation.params.controlTypeId = me.controller.params.controlTypeId;
                    operation.params.memberOnly = true;
                }

                me.setIgnoreChanges(true);
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы экспертов с массовой формой выбора экспертов
            По нажатию на Добавить открывается форма выбора экспертов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение экспертов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalExpertAspect',
            gridSelector: '#disposalExpertGrid',
            storeName: 'disposal.Expert',
            modelName: 'disposal.Expert',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalExpertMultiSelectWindow',
            storeSelect: 'dict.ExpertGjiForSelect',
            storeSelected: 'dict.ExpertGjiForSelected',
            titleSelectWindow: 'Выбор экспертов',
            titleGridSelect: 'Эксперты для отбора',
            titleGridSelected: 'Выбранные эксперты',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Тип эксперта', xtype: 'b4enumcolumn', dataIndex: 'ExpertType', enumName: 'B4.enums.ExpertType', flex: 0.5, filter: true }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddExperts', 'DisposalExpert', {
                            expertIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать экспертов');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalProvidedDocumentAspect',
            gridSelector: '#disposalProvidedDocGrid',
            storeName: 'disposal.ProvidedDoc',
            modelName: 'disposal.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Документы для выбора',
            titleGridSelected: 'Выбранные документы',
            isPaginable: false,
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    renderer: function(value, metaData) {
                        metaData.style = 'white-space: normal;';
                        return value;
                    }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    sortable: false,
                    renderer: function(value, metaData) {
                        metaData.style = 'white-space: normal;';
                        return value;
                    }
                }
            ],
            onBeforeLoad: function (store, operation) {
                operation.start = undefined;
                operation.page = undefined;
                operation.limit = undefined;
                operation.params.disposalId = this.controller.params.documentId;
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'DisposalProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * Аспект взаимодействия Таблицы Приложений с формой редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'disposalAnnexAspect',
            gridSelector: '#disposalAnnexGrid',
            editFormSelector: '#disposalAnnexEditWindow',
            storeName: 'disposal.Annex',
            modelName: 'disposal.Annex',
            editWindowView: 'disposal.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Disposal', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function () {
                    var docTypeField = this.getForm().down('b4selectfield[name=ErknmTypeDocument]');

                    if (this.controller.params.typeDocument === B4.enums.TypeDocumentGji.Decision) {
                        docTypeField.show();
                    }
                    else {
                        docTypeField.hide();
                    }
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'disposalNoticeDescriptionAspect',
            fieldSelector: '[name=NoticeDescription]',
            editPanelAspectName: 'disposalEditPanelAspect',
            getAction: 'GetNoticeDescription',
            saveAction: 'SaveNoticeDescription',
            controllerName: 'Disposal',
            valueFieldName: 'NoticeDescription',
            previewLength: 200,
            autoSavePreview: true,
            previewField: 'NoticeDescription'
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveySubjectAspect',
            modelName: 'disposal.DisposalVerificationSubject',
            storeName: 'disposal.DisposalVerificationSubject',
            gridSelector: 'disposalsubjectverificationgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveySubjectSelectWindow',
            storeSelect: 'dict.SurveySubjectForSelect',
            storeSelected: 'dict.SurveySubjectForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Предметы проверки',
            titleGridSelect: 'Предметы проверки для отбора',
            titleGridSelected: 'Выбранные предметы проверки',
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                if (me.controller.params) {
                    var existingIds = me.getGrid().getStore().collect('SurveySubjectId', false, true);
                    operation.params.ids = Ext.encode(existingIds);
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveySubjects', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();

                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.getStore('disposal.TypeSurvey').load();
                        asp.controller.getStore('disposal.SurveyPurpose').load();
                        asp.controller.getStore('disposal.SurveyObjective').load();
                        asp.controller.getStore('disposal.InspFoundationCheck').load();
                        asp.controller.getStore('disposal.ProvidedDoc').load();

                        Ext.Msg.alert('Сохранение!', 'Предметы проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveyPurposeAspect',
            modelName: 'disposal.SurveyPurpose',
            storeName: 'disposal.SurveyPurpose',
            gridSelector: 'disposalsurveypurposegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyPurposeSelectWindow',
            storeSelect: 'dict.SurveyPurposeForSelect',
            storeSelected: 'dict.SurveyPurposeForSelected',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор целей проверки',
            titleGridSelect: 'Цели проверки для отбора',
            titleGridSelected: 'Выбранные цели проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyPurposes', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Цели проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalSurveyObjectiveAspect',
            modelName: 'disposal.SurveyObjective',
            storeName: 'disposal.SurveyObjective',
            gridSelector: 'disposalsurveyobjectivegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalSurveyObjectiveSelectWindow',
            storeSelect: 'dict.SurveyObjectiveForSelect',
            storeSelected: 'dict.SurveyObjectiveForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор задач проверки',
            titleGridSelect: 'Задачи проверки для отбора',
            titleGridSelected: 'Выбранные задачи проверки',

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddSurveyObjectives', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Задачи проверки сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'disposalInspFoundationCheckAspect',
            modelName: 'disposal.InspFoundationCheck',
            storeName: 'disposal.InspFoundationCheck',
            gridSelector: 'disposalinspfoundationcheckgrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#disposalInspFoundationCheckSelectWindow',
            storeSelect: 'dict.NormativeDocForSelect',
            storeSelected: 'dict.NormativeDocForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нормативных документов',
            titleGridSelect: 'Нормативные документы для отбора',
            titleGridSelected: 'Выбранные нормативные документы',
            onBeforeLoad: function (store, operation) {
                var view = this.controller.getMainView();
                operation.params.documentDate = view.down('[name=DocumentDate]').getValue();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspFoundationChecks', 'Disposal', {
                        ids: Ext.encode(recordIds),
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Нормативные документы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },

        //аспект редактирования грида проверочных листов
        {
            xtype: 'grideditwindowaspect',
            name: 'controllistGridWindowAspect',
            gridSelector: 'controllistgrid',
            storeName: 'controllist.ControlList',
            modelName: 'controllist.ControlList',
            editWindowView: 'controllist.editwindow.EditWindow',
            editFormSelector: 'controllisteditwindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        controlListId = record.get('Id'),
                        store = asp.getForm().down('controllistquestiongrid').getStore();
                    if (controlListId) {
                        asp.controller.params['controlListId'] = controlListId;
                        asp.controller.setContextValue(asp.controller.getView('controllist.editwindow.EditWindow'), 'controlListId', controlListId);
                        store.on('beforeload', me.onBeforeLoadControlListQuestion, me);
                        asp.controller.getStore('controllist.ControlListQuestion').load();
                        store.load();
                    }

                    asp.getForm().down('controllistquestiongrid').setDisabled(controlListId == 0);
                },

                beforesave: function(asp, record) {
                    var fileField = asp.getForm().down('[name=File]'),
                        fileId = fileField.fileIsLoad ? fileField.fileInputEl.dom.files[0] : null;
                    record.set('File', fileId);
                    record.set('Disposal', asp.controller.params.documentId);
                },

                beforegridaction: function (asp, grid, action) {
                    if (action.toLowerCase() == 'add' && grid.getStore().count() > 0) {
                        Ext.Msg.alert('Ошибка', asp.controller.params.documentTitle + ' может содержать только один проверочный лист!');
                        return false;
                    }
                }
            },

            onBeforeLoadControlListQuestion: function(store, operation) {
                if (this.controller.params) {
                    operation.params.controlListId = this.controller.params['controlListId'];
                }
            }
        },

        //аспект грида редактирования вопросов
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'controllistquestiongrid',
            storeName: 'controllist.ControlListQuestion',
            modelName: 'controllist.ControlListQuestion',
            saveButtonSelector: '[name=questionsSaveButton]',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validQuestionContent = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (!me.validate(rec, 'QuestionContent')) {
                            validQuestionContent = false;
                            return false;
                        }
                    });

                    if (!(validQuestionContent)) {
                        var commonMessagePart = ' Это поле обязательно для заполнения<br>'
                        var errormessage = 'Следующие поля содержат ошибки:<br>' +
                            (validQuestionContent ? '' : '<b>Вопрос проверочного листа:</b>' + commonMessagePart);

                        Ext.Msg.alert('Ошибка сохранения!', errormessage);
                        return false;
                    }

                    var controlListId = asp.controller.getContextValue(asp.controller.getView('controllist.editwindow.EditWindow'),
                        'controlListId');
                    Ext.each(modifiedRecords, function (rec) {
                        rec.set('ControlList', controlListId);
                    });
                    return true;
                }
            },

            validate: function (rec, field) {
                return !Ext.isEmpty(rec.get(field));
            }
        },
        {
            /* Аспект взаимодействия таблицы Основания проведения с формой редактирования */
            xtype: 'grideditwindowaspect',
            gridSelector: 'disposalinspectionbasegrid',
            storeName: 'disposal.InspectionBase',
            modelName: 'disposal.InspectionBase',
            editWindowView: 'disposal.inspectionbase.EditWindow',
            editFormSelector: 'disposalinspectionbaseeditwindow',
            listeners: {
                beforesetformdata: function (asp, record, form) {
                    var me = this,
                        recordIds = [],
                        sfInspBaseType = form.down('b4selectfield[name=InspectionBaseType]'),
                        store = sfInspBaseType.getStore();
                    
                    if (record.data['Id'])
                        sfInspBaseType.setDisabled(true);
                    
                    if (asp.controller.params) {
                        // Получаем идентификаторы всех записией в текущем решении и после отпарвляем в качестве параметров
                        Ext.Array.each(me.getGrid().getStore().data.items, function (item) {
                            recordIds.push(item.data.Id);
                        });
                        
                        // Получаем kindCheckId с панели реквизитов и отправляем в качестве параметра
                        store.on('beforeload', function(s, operation) {
                            operation.params.kindCheckId = asp.controller.params.kindCheckId;
                            operation.params.recordIds = recordIds;
                        });
                        store.load();
                    }
                },
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Decision', asp.controller.params.documentId);
                    }
                }
            },
            otherActions: function (actions) {
                var me = this;
                
                actions[me.editFormSelector + ' b4selectfield[name=RiskIndicator]'] = { 'beforeload': { fn: me.OnLoadRiskIndicators, scope: me } };
                actions[me.editFormSelector + ' b4selectfield[name=InspectionBaseType]'] = { 'change': { fn: me.OnChangeInspectionBaseTypeField, scope: me } };
            },
            OnLoadRiskIndicators: function(field, operation) {
                var asp = this.controller.getAspect('disposalEditPanelAspect');
                
                // Получаем controlTypeId с панели реквизитов и отправляем в качестве параметра
                if (asp.controller.params.controlTypeId)
                    operation.params.controlTypeId = asp.controller.params.controlTypeId;
            },
            OnChangeInspectionBaseTypeField: function (field, selectedInspBaseType) {
                var me =  this,
                    form = me.getForm(),
                    otherInspBaseTextField = form.down('textfield[name=OtherInspBaseType]'),
                    dateField = form.down('datefield[name=FoundationDate]'),
                    riskIndicatorField = form.down('b4selectfield[name=RiskIndicator]');
                
                // Устанавливаем видимость и обязательность полей в зависимости от значений в выбраной записи в поле Основание Проведения
                me.setFieldOptions(otherInspBaseTextField, selectedInspBaseType.HasTextField);
                me.setFieldOptions(dateField, selectedInspBaseType.HasDate);
                me.setFieldOptions(riskIndicatorField, selectedInspBaseType.HasRiskIndicator);
            },
            setFieldOptions: function (field, value) {
                field.setVisible(value);
                field.allowBlank = !value;
            }
        },

        //аспект редактирования грида "Основания проведения КНМ"
        {
            xtype: 'grideditwindowaspect',
            name: 'disposalKnmReasonAspect',
            gridSelector: '#disposalKnmReasonGrid',
            editFormSelector: '#disposalKnmReasonWindow',
            storeName: 'disposal.KnmReason',
            modelName: 'disposal.KnmReason',
            editWindowView: 'disposal.KnmReasonWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('Decision', this.controller.params.documentId);
                    }
                },
            }
        },
        {
            /* Аспект взаимодействия таблицы 'Сведения об объектах контроля' с формой редактирования */
            xtype: 'grideditwindowaspect',
            name: 'disposalControlObjectInfoAspect',
            gridSelector: '#disposalControlObjectInfoGrid',
            storeName: 'disposal.ControlObjectInfo',
            modelName: 'disposal.ControlObjectInfo',
            editFormSelector: '#disposalControlObjectEditWindow',
            editWindowView: 'disposal.controlobjectinfo.EditWindow',
            listeners: {
                beforesetformdata: function (asp, record, form) {
                    var me = this,
                        realityObjIds = [],
                        sfInspRealityObj = form.down('b4selectfield[name=InspGjiRealityObject]'),
                        rObjectStore = sfInspRealityObj.getStore();
    
                    if (record.data['Id'])
                        sfInspRealityObj.setDisabled(true);
    
                    if (asp.controller.params) {
                        //Получаем идентификаторы всех записией в текущем решении и после отпарвляем в качестве параметров
                        Ext.Array.each(me.getGrid().getStore().data.items, function (item) {
                            realityObjIds.push(item.data.Id);
                        });

                        // Получаем kindCheckId с панели реквизитов и отправляем в качестве параметра
                        rObjectStore.on('beforeload', function (s, operation) {
                            operation.params.inspectionId = asp.controller.params.inspectionId;
                            operation.params.realityObjIds = realityObjIds;
                        });
                        rObjectStore.load();
                    }
                },
                getdata: function (asp, record){
                    if(!record.get('Id')){
                        record.set('Decision', this.controller.params.documentId);
                    }
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' b4selectfield[name=ControlObjectKind]'] = {
                    beforeload: {
                        fn: function (field, operation) {
                            var asp = this.controller.getAspect('disposalEditPanelAspect');

                            if (asp.controller.params['controlTypeId'])
                                operation.params['controlTypeId'] = asp.controller.params['controlTypeId'];
                        },
                        scope: me
                    }
                };
            }
        },

        {
            /*
            аспект взаимодействия триггер-поля "Запланированные действия" с массовой формой выбора действий
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем выбранные запалнированные действия
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'plannedActionsMultiSelectWindowAspect',
            fieldSelector: '#disposalEditPanel gkhtriggerfield[name=PlannedActions]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#PlannedActionsSelectWindow',
            storeSelect: 'dict.KnmAction',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
            ],
            titleSelectWindow: 'Запланированные действия',
            titleGridSelect: 'Запланированные действия',
            titleGridSelected: 'Выбранные запланированные действия',
            onBeforeLoad: function (store, operation) {
                var me = this;

                me.setIgnoreChanges(true);
                me.setDefaultKnmActionStoreParams(store, operation);
            },
            onSelectedBeforeLoad: function (store, operation) {
                var me = this,
                    field = me.getSelectField();

                if (field) {
                    operation.params[me.valueProperty] = field.getValue();

                    me.setDefaultKnmActionStoreParams(store, operation);
                }
            },
            setDefaultKnmActionStoreParams: function(store, operation) {
                var me = this,
                    disposalEditPanelAspect = me.controller.getAspect('disposalEditPanelAspect'),
                    panel = disposalEditPanelAspect.getPanel();

                operation.params.kindCheckId = panel.down('[name=KindCheck]').getValue();
                operation.params.controlTypeId = panel.down('[name=ControlType]').getValue();
            }
        },

    ],

    init: function () {
        var me = this,
            actions = {};

        me.getStore('disposal.Expert').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.TypeSurvey').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.DisposalVerificationSubject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyObjective').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyPurpose').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.InspFoundationCheck').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.InspectionBase').on('beforeload', me.onBeforeLoad, me);
        me.getStore('controllist.ControlList').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.SurveyPurpose').on('datachanged', me.storeDataChanged, me);
        me.getStore('dict.InspectionBaseType').on('beforeload', me.onBeforeLoadInspect, me);
        me.getStore('disposal.KnmReason').on('beforeload', me.onBeforeLoad, me);
        me.getStore('disposal.ControlObjectInfo').on('beforeload', me.onBeforeLoad, me);
        me.control({
            '#disposalEditPanel  #cbTypeAgreementProsecutor, #cbTypeAgreementResult': { 'change': me.onChangeTypeAgreement },
            'controllistgrid actioncolumn[name=getFile]': {
                'click': function (gridView, rowIndex, colIndex, el, e, rec) {
                    var file = rec.get('File');
                    if (file != null && file.Id > 0) {
                        window.open(B4.Url.action('/FileUpload/Download?id=' + file.Id));
                        return;
                    }
                    Ext.Msg.alert('Ошибка', 'Отсутсвует файл для загрузки');
                }
            },
            'disposaleditpanel b4enumcombo[name=UsingMeansRemoteInteraction]': {
                'change': function (cmp, value) {
                    me.changeVisibleFields(value);
                }
            },
            '#disposalAnnexEditWindow b4selectfield[name=ErknmTypeDocument]': {
                beforeload: { fn: me.onBeforeLoadInspect, scope: me }
            }
        });

        me.callParent(arguments);

        me.control(actions);
    },

    onLaunch: function () {
        var me = this;

        me.bindContext(me.getMainView());

        if (me.params) {
            me.getAspect('disposalEditPanelAspect').setData(me.params.documentId);
        }
    },

    onBeforeLoad: function(store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    },
    
    onBeforeLoadInspect: function(store, operation) {
        var me = this;
        if (me.params && me.params.kindCheckId) {
            operation.params.kindCheckId = me.params.kindCheckId;
        }
    },

    storeDataChanged: function (store, operation) {
        var button = this.getMainView().down('disposalsurveypurposegrid').down('b4addbutton');
        button.setDisabled(store.count() > 0);
    },

    onChangeTypeAgreement: function () {
        var me = this,
            view = me.getMainView(),
            record = view.getRecord(),
            knmGrid = view.down('disposalknmreasongrid'),
            docDate = view.down('#cbDocumentDateWithResultAgreement'),
            docNum = view.down('#cbDocumentNumberWithResultAgreement'),
            typeAgreementProsecutor = view.down('#cbTypeAgreementProsecutor').getValue(),
            typeAgreementResult = view.down('#cbTypeAgreementResult').getValue(),
            isEnabledocdate = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed)
                && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docDate.allowedEdit,
            isEnabledocnum = ((typeAgreementResult === B4.enums.TypeAgreementResult.Agreed || typeAgreementResult === B4.enums.TypeAgreementResult.NotAgreed)
                && typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement) && docNum.allowedEdit,
            informationAboutHarm = view.down('textfield[name=InformationAboutHarm]'),
            isImmediateInspection = typeAgreementProsecutor === B4.enums.DecisionTypeAgreementProsecutor.ImmediateInspection;
        
        informationAboutHarm.setVisible(isImmediateInspection);

        if(record){
            var kindCheckCode = record.data?.KindCheck?.Code,
                isDecision = me.params.typeDocument === B4.enums.TypeDocumentGji.Decision,
                isKind = kindCheckCode === B4.enums.TypeCheck.NotPlannedExit
                    || kindCheckCode === B4.enums.TypeCheck.NotPlannedDocumentation
                    || kindCheckCode === B4.enums.TypeCheck.NotPlannedInspectionVisit,
                isTypeAgreement = typeAgreementProsecutor === B4.enums.TypeAgreementProsecutor.RequiresAgreement
                    || isImmediateInspection;

            knmGrid.setVisible(isDecision && isKind && isTypeAgreement);
        }
        
        docDate.setDisabled(!isEnabledocdate);
        docNum.setDisabled(!isEnabledocnum);
    },

    changeVisibleFields: function (value) {
        var me = this,
            view = me.getMainView(),
            elInfo = view.down('[name=InfoUsingMeansRemoteInteraction]'),
            elUsing = view.down('[name=UsingMeansRemoteInteraction]');

        if (value === B4.enums.YesNoNotSet.Yes && !elUsing.hidden) {
            elInfo.show();
            elInfo.allowBlank = false;
        }
        else {
            elInfo.hide();
            elInfo.allowBlank = true;
        }
    },
    
    isDisposal: function () {
        return this.params.typeDocument === B4.enums.TypeDocumentGji.Disposal;
    }
});