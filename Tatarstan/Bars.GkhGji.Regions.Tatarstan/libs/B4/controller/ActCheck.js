Ext.define('B4.controller.ActCheck', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GridDictEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActCheck',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.ActCheckActionViolation',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.ActCheckActionType',
        'B4.enums.ActCheckActionCarriedOutEventType',
        'B4.enums.TypeExecutantProtocol',
        'B4.enums.YesNoNotSet',
        'B4.enums.HasValuesNotSet',
        'B4.enums.TypeBase'
    ],

    models: [
        'ActCheck',
        'actcheck.Action',
        'actcheck.ActionCarriedOutEvent',
        'actcheck.ActionFile',
        'actcheck.ActionInspector',
        'actcheck.ActionRemark',
        'actcheck.ActionViolation',
        'actcheck.DocRequestAction',
        'actcheck.DocRequestActionRequestInfo',
        'actcheck.ExplanationAction',
        'actcheck.InspectionAction',
        'actcheck.InstrExamAction',
        'actcheck.InstrExamActionNormativeDoc',
        'actcheck.SurveyAction',
        'actcheck.SurveyActionQuestion',
        'actcheck.Annex',
        'actcheck.Period',
        'actcheck.Witness',
        'actcheck.Violation',
        'actcheck.Definition',
        'actcheck.RealityObject',
        'actcheck.InspectedPart',
        'actcheck.ProvidedDoc',
        'Prescription',
        'ProtocolGji',
        'Disposal'
    ],

    stores: [
        'ActCheck',
        'actcheck.Action',
        'actcheck.ActionCarriedOutEvent',
        'actcheck.ActionFile',
        'actcheck.ActionInspector',
        'actcheck.ActionRemark',
        'actcheck.ActionViolation',
        'actcheck.Annex',
        'actcheck.Period',
        'actcheck.Witness',
        'actcheck.Violation',
        'actcheck.ActRemoval',
        'actcheck.Definition',
        'actcheck.InspectedPart',
        'actcheck.RealityObject',
        'actcheck.ViolationForSelect',
        'actcheck.ViolationForSelected',
        'actcheck.ProvidedDoc',
        'actcheck.DocRequestActionRequestInfo',
        'actcheck.InstrExamActionNormativeDoc',
        'actcheck.SurveyActionQuestion',
        'prescription.ForSelect',
        'prescription.ForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected'
    ],

    views: [
        'actcheck.EditPanel',
        'actcheck.RealityObjectGrid',
        'actcheck.RealityObjectEditWindow',
        'actcheck.RealityObjectEditPanel',
        'actcheck.ViolationGrid',
        'actcheck.ActionAddWindow',
        'actcheck.ActionFileEditWindow',
        'actcheck.AnnexGrid',
        'actcheck.AnnexEditWindow',
        'actcheck.WitnessGrid',
        'actcheck.PeriodGrid',
        'actcheck.PeriodEditWindow',
        'actcheck.DefinitionGrid',
        'actcheck.DefinitionEditWindow',
        'actcheck.ActRemovalGrid',
        'actcheck.InspectedPartGrid',
        'actcheck.InspectedPartEditWindow',
        'actcheck.ProvidedDocGrid',
        'actcheck.docrequestaction.ActionEditWindow',
        'actcheck.docrequestaction.RequestInfoEditWindow',
        'actcheck.explanationaction.ActionEditWindow',
        'actcheck.inspectionaction.ActionEditWindow',
        'actcheck.instrexamaction.ActionEditWindow',
        'actcheck.surveyaction.ActionEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'actcheck.EditPanel',
    mainViewSelector: '#actCheckEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        actCheck: 'B4.mixins.ActCheck'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Акта проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'actCheckCreateButtonAspect',
            buttonSelector: '#actCheckEditPanel gjidocumentcreatebutton',
            containerSelector: '#actCheckEditPanel',
            typeDocument: 20, // Тип документа Акт проверки
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил для которых необходимы пользовательские параметры
                // при созданни документов из акта проверки все правила ожидают пользовательские параметры
                if (params.ruleId === 'ActCheckToDisposalRule'
                    || params.ruleId === 'ActCheckToProtocolRule'
                    || params.ruleId === 'ActCheckToPrescriptionRule'
                    || params.ruleId === 'ActCheckToResolutionRospotrebnadzorRule'
                    || params.ruleId === 'ActCheckToWarningDocRule') {
                        return false;
                }
            },
            onAfterMenuLoad: function (menu) {
                var me = this,
                    cbNeedReferral = me.controller.getMainView().down('b4combobox[name=NeedReferral]'),
                    resolutionRospotrebnadzor = me.controller.getMenuElementByRuleId(menu, 'ActCheckToResolutionRospotrebnadzorRule');

                // Если не 'Да', то скрываем пункт меню
                if (resolutionRospotrebnadzor && (!cbNeedReferral.manualAllowed || (cbNeedReferral && cbNeedReferral.value !== 10))) {
                    resolutionRospotrebnadzor.hide();
                }
            }
        },
        {
            xtype: 'actcheckperm',
            editFormAspectName: 'actCheckEditPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.Area_Rqrd', applyTo: '#nfArea', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.DocumentPlace', applyTo: '[name=DocumentPlaceFias]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.DocumentTime', applyTo: '[name=DocumentTime]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintState', applyTo: '[name=AcquaintState]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.RefusedToAcquaintPerson', applyTo: '[name=RefusedToAcquaintPerson]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedPerson', applyTo: '[name=AcquaintedPerson]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedPersonTitle', applyTo: '[name=AcquaintedPersonTitle]', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedDate', applyTo: '[name=AcquaintedDate]', selector: '#actCheckEditPanel' }
            ]
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actCheckStateButtonAspect',
            stateButtonSelector: '#actCheckEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель

                    var editPanelAspect = asp.controller.getAspect('actCheckEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            //Аспект кнопки печати акта проверки
            //codeForm задается в аспекте 'actCheckEditPanelAspect'
            xtype: 'gkhbuttonprintaspect',
            name: 'actCheckPrintAspect',
            buttonSelector: '#actCheckEditPanel #btnPrint',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'actionPrintAspect',
            buttonSelector: '#actCheckActionAddWindow #btnPrint',
            codeForm: 'ProtocolActAction',
            getUserParams: function() {
                var param = { DocumentId: this.controller.getCurrentActCheckAction() };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            //кнопка печати определения акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actCheckDefinitionPrintAspect',
            buttonSelector: '#actCheckDefinitionEditWindow #btnPrint',
            codeForm: 'ActCheckDefinition',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DefinitionId: me.controller.params.definitionId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /* 
            Аспект  для Акта проверки
            */
            xtype: 'gjidocumentaspect',
            name: 'actCheckEditPanelAspect',
            editPanelSelector: '#actCheckEditPanel',
            modelName: 'ActCheck',
            otherActions: function (actions) {
                var me = this,
                    realityObjectEditPanelSelector = me.editPanelSelector + ' #actCheckRealityObjectEditPanel';

                actions[me.editPanelSelector + ' #cbToPros'] = { 'change': { fn: me.onToProsecutorChange, scope: me } };
                actions[me.editPanelSelector + ' #sfResolPros'] = {
                    'beforeload': { fn: me.onResProsBeforeLoad, scope: me },
                    'change': { fn: me.onResProsChange, scope: me }
                };

                actions[realityObjectEditPanelSelector + ' b4combobox[name=HaveViolation]'] = {
                    'change': { fn: me.changeHaveViolation, scope: me }
                };
                actions[realityObjectEditPanelSelector + ' b4combobox[name=NeedReferral]'] = {
                    'change': { fn: me.changeNeedReferral, scope: me }
                };
            },
            saveRecord: function (rec) {
                // делаем запрос, так как  поле ActCheckGjiRealityObject  догоняется в get
                var model = this.getModel(rec);
                model.load(rec.getId(), {
                    success: function (result) {
                            //Если это акт на 1 дом то тогда сохраняем сначала Результаты проверки,
                            //а затем саму сущность
                            if (result.get('ActCheckGjiRealityObject') != null) {
                                var recordObj = result.get('ActCheckGjiRealityObject'),
                                    panel = this.getPanel(),
                                    storeViolation = panel.down('#actCheckRealityObjectEditPanel #actCheckViolationGrid').getStore(),
                                    haveViolation = panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').getValue(),
                                    description = panel.down('#actCheckRealityObjectEditPanel #taDescription').getValue(),
                                    deferred = new Deferred();

                                panel.setDisabled(true);

                                deferred.next(function(res) {
                                    this.saveActCheck(rec);
                                }, this)
                                    .error(function(e) {
                                        panel.setDisabled(false);
                                        if (e.message) {
                                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                                            return false;
                                        } else {
                                            throw e;
                                        }
                                    }, this);

                                this.saveRealityObjectViolation(recordObj.Id, haveViolation, description, storeViolation, deferred);
                            } else {
                                this.saveActCheck(rec);
                            }
                    },
                    scope: this
                });
            },
            saveActCheck: function (rec) {
                var me = this,
                    panel = this.getPanel(),
                    recId = rec.getId();

                panel.setDisabled(true);
                rec.save({ id: recId })
                    .next(function (result) {
                        panel.setDisabled(false);
                        me.setData(recId);
                        B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                    }, this)
                    .error(function(result) {
                        panel.setDisabled(false);
                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
            },
            changeHaveViolation: function (combobox, newValue) {
                var actViolationGridAddButton = this.getPanel().down('#actCheckRealityObjectEditPanel #actViolationGridAddButton');

                actViolationGridAddButton.setDisabled(false);
                // Нельзя добавлять, если не 'Да' блокируем кнопку добавления
                if(newValue !== 10)
                    actViolationGridAddButton.setDisabled(true);
            },
            changeNeedReferral: function (combobox, newValue) {
                var me = this,
                    menu = combobox.up('#actCheckEditPanel').down('gjidocumentcreatebutton').menu,
                    resolutionRospotrebnadzor = me.controller.getMenuElementByRuleId(menu, 'ActCheckToResolutionRospotrebnadzorRule');

                if (resolutionRospotrebnadzor) {
                    resolutionRospotrebnadzor.hide();

                    // Если 'Да', то отображаем пункт меню
                    if (combobox.manualAllowed && newValue === 10) {
                        resolutionRospotrebnadzor.show();
                    }
                }
            },
            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask,
                    addButton,
                    title,
                    typeBase = rec.get('TypeBase'),
                    resolutionRospotrebnadzor = rec.get('ReferralResolutionToRospotrebnadzor');
                
                asp.controller.params= asp.controller.params || {};

                asp.controller.params.typeBase = typeBase;
                me.controller.params.isExistsWarningDoc = rec.get('IsExistsWarningDoc');
                
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                asp.controller.mask('Загрузка', panel);
                
                addButton = panel.down('#actCheckViolationGrid b4addbutton');
                if (rec.get('ActCheckGjiRealityObject') && rec.get('ActCheckGjiRealityObject').HaveViolation !== 10) {
                    addButton.setDisabled(true);
                } else {
                    addButton.setDisabled(false);
                }

                if (resolutionRospotrebnadzor) {
                    panel.down('#actCheckRealityObjectEditPanel b4combobox[name=NeedReferral]').setValue(resolutionRospotrebnadzor);
                }
                //После проставления данных обновляем title у вкладки
                title = rec.get('TypeActCheck') == 10 ? 'Акт проверки (общий)' : 'Акт проверки';
                
                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('#actCheckTabPanel').setActiveTab(0);

                if (rec.get('TypeActCheck') == 30) {
                    //Если Акт проверки с типом = Акт проверки предписания то название вкладки меняем 
                    panel.down('#actCheckViolationTab').setTitle('Новые нарушения');
                    panel.down('#actCheckTabPanel').child('#actCheckActRemovalTab').tab.show();

                    asp.controller.getStore('actcheck.ActRemoval').load();

                    //запрос на получение пермишена на просмотр вкладки новые нарушения
                    B4.Ajax.request({
                            url: B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                                permissions: Ext.encode(['GkhGji.DocumentsGji.ActCheck.Register.Violation.View']),
                                ids: Ext.encode([rec.getId()])
                            }),
                            method: 'POST'
                        }
                    ).next(function(response) {
                        var tabViolPermission = Ext.JSON.decode(response.responseText);
                        if (!tabViolPermission[0][0]) {
                            panel.down('#actCheckViolationTab').tab.hide();
                        } else {
                            panel.down('#actCheckViolationTab').tab.show();
                        }
                    });
                }
                else {
                    panel.down('#actCheckViolationTab').setTitle('Результаты проверки');
                    panel.down('#actCheckViolationTab').tab.show();
                    panel.down('#actCheckTabPanel').child('#actCheckActRemovalTab').tab.hide();
                }

                //Если акт на 1 дом то открываем панель и скрываем грид для Результатов проверки
                //Иначе скрываем панель и открываем грид Результатов проверки

                me.controller.currentRoId = 0;
                
                me.controller.params.documentId = rec.getId();
                
                if (rec.get('ActCheckGjiRealityObject') != null) {
                    panel.down('#actCheckRealityObjectGrid').hide();
                    panel.down('#actCheckRealityObjectEditPanel').show();

                    var realObj = rec.get('ActCheckGjiRealityObject');
                    me.controller.currentRoId = realObj.Id;
                    panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').setValue(realObj.HaveViolation);
                    panel.down('#actCheckRealityObjectEditPanel #taDescription').setValue(realObj.Description);
                    
                    me.controller.getStore('actcheck.Violation').load();
                } else {
                    panel.down('#actCheckRealityObjectGrid').show();
                    panel.down('#actCheckRealityObjectEditPanel').hide();

                    //Обновляем таблицу Проверяемых домов акта
                    me.controller.getStore('actcheck.RealityObject').load();
                }

                B4.Ajax.request(B4.Url.action('GetInfo', 'ActCheck', {
                        documentId: asp.controller.params.documentId
                    }))
                    .next(function(response) {

                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var taRealityObjAddress = panel.down('#actCheckRealityObjectEditPanel #taRealityObjAddress');
                        if (obj.realityObjAddress) {
                            if (taRealityObjAddress) {
                                taRealityObjAddress.show();
                                taRealityObjAddress.setValue(obj.realityObjAddress);
                                panel.doLayout();
                            }
                        } else {
                            if (taRealityObjAddress) {
                                taRealityObjAddress.hide();
                                taRealityObjAddress.setValue(null);
                                panel.doLayout();
                            }
                        }

                        asp.controller.params.controlTypeId = obj.controlTypeId;
                        
                        var signerStore = panel.down('b4selectfield[name=Signer]').getStore();
                        signerStore.on({
                            'beforeload': function (store, operation) {
                                if (asp.controller.params.controlTypeId > 0) {
                                    operation.params = operation.params || {};
                                    operation.params.memberOnly = true;
                                    operation.params.controlTypeId = asp.controller.params.controlTypeId;
                                }
                            }
                        });

                        var fieldInspectors = panel.down('#trigfInspectors');

                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);

                        var fieldset = panel.down('#fsActResol');
                        if (obj.typeBase == B4.enums.TypeBase.ProsecutorsClaim && (!obj.prosClaimTypeBase || obj.prosClaimTypeBase == 10)) {
                            if (fieldset)
                                fieldset.show();
                        } else {
                            if (fieldset)
                                fieldset.hide();
                        }

                        me.disableButtons(false);

                        asp.controller.unmask();
                    })
                    .error(function() {
                        asp.controller.unmask();
                    });

                if (typeBase === B4.enums.TypeBase.GjiWarning) {
                    var acquaintInfoFieldset = panel.down('fieldset[name=AcquaintInfo]');
                    Ext.each(acquaintInfoFieldset.query('field'), function (f) {
                        f.setDisabled(true);
                    });
                    acquaintInfoFieldset.setVisible(false);
                }

                var actCheckPrintAspect = me.controller.getAspect('actCheckPrintAspect');
                actCheckPrintAspect.codeForm = '';
                
                if(rec.get('DocumentDate')){
                    if (Date.parse(rec.get('DocumentDate')) < Date.parse('01.01.2022')){
                        actCheckPrintAspect.codeForm = 'ActCheck'
                    }
                    else {
                        actCheckPrintAspect.codeForm = 'ActCheckTat,ProtocolNotice'
                    }
                }
                //загружаем стор отчетов
                actCheckPrintAspect.loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('actCheckStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('actCheckCreateButtonAspect').setData(rec.get('Id'));
            },
            //метод сохранения нарушений по дому
            saveRealityObjectViolation: function (actObjectId, haveViolation, description, violationStore, deferred) {
                var me = this,
                    panel = me.controller.getMainComponent(),
                    actDate = panel.down('datefield[name=DocumentDate]').getValue(),
                    timeLine = me.controller.getTimeline(),
                    actDatePlusTimeLine,
                    timelineName;
                
                //Блокируем сохранение если не выполняется ряд условий
                if (!Ext.isDate(actDate)) {
                    deferred.fail({ message: 'Не заполнена дата документа' });
                    return false;
                }

                if (violationStore.getCount() == 0 && haveViolation == 10) {
                    deferred.fail({ message: 'Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений' });
                    return false;
                }

                if (violationStore.getCount() != 0 && haveViolation != 10) {
                    deferred.fail({ message: 'Записи в таблице нарушений должны быть только если нарушения выявлены' });
                    return false;
                }

                switch (timeLine.kind) {
                    case B4.enums.PeriodKind.Day:
                    {
                        actDatePlusTimeLine = new Date(new Date(actDate).setDate(actDate.getDate() + timeLine.period));
                        timelineName = 'д.';
                        break;
                    }
                    case B4.enums.PeriodKind.Month:
                    {
                        actDatePlusTimeLine = new Date(new Date(actDate).setMonth(actDate.getMonth() + timeLine.period));
                        timelineName = 'мес.';
                        break;
                    }
                    default :
                    {
                        deferred.fail({ message: 'Указана некорректная единица периода в единых настройках приложения, в секции "Ограничение срока устранения нарушений"' });
                        return false;
                    }
                }

                //формируем записи нарушений из стора для последующей обработки на сервере
                var actCheckViolationRecords = [];
                var isCorrectDate = true;
                Ext.Array.each(violationStore.getRange(0, violationStore.getCount()),
                    function(item) {
                        var data = item.getData();

                        if (data.DatePlanRemoval) {
                            var datePlanRemoval;
                            if (data.DatePlanRemoval instanceof Date) {
                                datePlanRemoval = data.DatePlanRemoval;
                            } else {
                                var dateParts = data.DatePlanRemoval.split("-");
                                datePlanRemoval = new Date(dateParts[0], dateParts[1] - 1, dateParts[2].substring(0, dateParts[2].lenght - 9));
                            }

                            if (datePlanRemoval.getTime() > actDatePlusTimeLine.getTime()) {
                                isCorrectDate = false;
                            }
                        }


                        actCheckViolationRecords.push(
                            {
                                Id: data.Id || 0,
                                ViolationGjiId: data.ViolationGjiId,
                                DatePlanRemoval: data.DatePlanRemoval
                            });
                    });

                if (!isCorrectDate) {
                    deferred.fail({ message: Ext.String.format('Указанная дата превышает допустимый период ограничения срока устранения нарушений. Дата не может превышать период {0} {1} начиная с установленной даты', timeLine.period, timelineName) });
                    return false;
                }

                me.controller.mask('Сохранение', me.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('SaveParams', 'ActCheckRealityObject'),
                    params: {
                        actObjectId: actObjectId,
                        haveViolation: haveViolation,
                        actViolationJson: Ext.encode(actCheckViolationRecords),
                        description: description
                    } 
                }).next(function () {
                    me.controller.unmask();
                    deferred.call({ message: 'Сохранение результатов проверки прошло успешно' });
                }).error(function (e) {
                    me.controller.unmask();
                    deferred.fail(e);
                });               
                return true;
            },
            onToProsecutorChange: function (field, newValue, oldValue) {
                var panel = this.getPanel();

                if (newValue == 10) {
                    panel.down('#dfToPros').setDisabled(false);
                    panel.down('#sfResolPros').setDisabled(false);
                } else {
                    panel.down('#dfToPros').setDisabled(true);
                    panel.down('#dfToPros').setValue(null);
                    panel.down('#sfResolPros').setDisabled(true);
                    panel.down('#sfResolPros').setValue(null);
                }
            },
            onResProsChange: function (field, data) {
                if (data) {
                    var dateStr = '';
                    if (data.DocumentDate) {
                        var date = new Date(data.DocumentDate);
                        dateStr = date.toLocaleDateString();
                    }

                    field.updateDisplayedText('№' + data.DocumentNumber + ' от ' + dateStr);
                } else {
                    field.updateDisplayedText('');
                }
            },
            onResProsBeforeLoad: function (field, options, store) {
                options = options || {};
                options.params = {};

                options.params.documentType = 80;

                return true;
            },
            disableButtons: function (value) {
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
            }
        },
        {   /* 
               Аспект взаимодействия кнопки Предписание с массовой формой выбора нарушений
               и последующей отправке на выполнение
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actCheckToPrescriptionAspect',
            buttonSelector: '#actCheckEditPanel [ruleId=ActCheckToPrescriptionRule]',
            multiSelectWindowSelector: '#actCheckToPrescriptionMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq} },
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('InspectionViolationId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /* 
               Аспект взаимодействия для кнопки Протокол с массовой формой выбора Нарушений
               По нажатию на кнопку Протокол будет открыта форма массовго выбора
               а после выбранные Id будут отправлены на формирование докумета Протокола
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actCheckToProtocolAspect',
            buttonSelector: '#actCheckEditPanel [ruleId=ActCheckToProtocolRule]',
            multiSelectWindowSelector: '#actCheckToProtocolMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq} },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('InspectionViolationId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /* 
             *  Аспект взаимодействия для кнопки Распоряжения с массовой формой выбора предписаний
             *  По нажатию на кнопку Распоряжение будет открыта форма массовго выбора Предписаний
             *  а после выбранные Id будут отправлены на формирование нового документа Распоряжение 
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actCheckToDisposalAspect',
            buttonSelector: '#actCheckEditPanel [ruleId=ActCheckToDisposalRule]',
            multiSelectWindowSelector: '#actCheckToDisposalMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'prescription.ForSelect',
            storeSelected: 'prescription.ForSelected',
            columnsGridSelect: [
                    { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield'} },
                    { header: 'Дата', xtype: 'datecolumn', dataIndex: 'DocumentDate', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq} }
                ],
            columnsGridSelected: [
                    { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', sortable: false },
                    { header: 'Дата', xtype: 'datecolumn', dataIndex: 'DocumentDate', format: 'd.m.Y', flex: 1, sortable: false }
                ],
            titleSelectWindow: 'Выбор предписаний',
            titleGridSelect: 'Предписания для отбора',
            titleGridSelected: 'Выбранные предписания',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0)
                    operation.params.parentId = this.controller.params.documentId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('Id'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем предписания)
                        params = creationAspect.getParams(btn);
                        params.parentIds = listIds;
                        
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предписания');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /*
             *  Аспект взаимодействия для кнопки Постановление Роспотребнадзора с массовой формой выбора предписаний
             *  По нажатию на кнопку Постановление Роспотребнадзора будет открыта форма массовго выбора Предписаний
             *  а после выбранные Id будут отправлены на формирование нового документа Постановление Роспотребнадзора
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actCheckToResolutionRospotrebnadzorAspect',
            buttonSelector: '#actCheckEditPanel [ruleId=ActCheckToResolutionRospotrebnadzorRule]',
            multiSelectWindowSelector: '#actCheckToResolutionRospotrebnadzorMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('InspectionViolationId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {   /*
             *  Аспект взаимодействия для кнопки Предостережение
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actCheckToWarningDocAspect',
            buttonSelector: '#actCheckEditPanel [ruleId=ActCheckToWarningDocRule]',
            multiSelectWindowSelector: '#actCheckToWarningDocMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actcheck.RealityObject',
            storeSelected: 'actcheck.RealityObject',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                    operation.params.onlyHasViolations = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('RealityObjectId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дома)
                        params = creationAspect.getParams(btn);
                        params.realityIds = listIds;

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
            Аспект взаимодействия Таблицы Проверяемых домов акта и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckRealityObjectAspect',
            gridSelector: '#actCheckRealityObjectGrid',
            editFormSelector: '#actCheckRealityObjectEditWindow',
            storeName: 'actcheck.RealityObject',
            modelName: 'actcheck.RealityObject',
            editWindowView: 'actcheck.RealityObjectEditWindow',

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentRoId(record.getId());
                }
            },

            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this} };
            },

            changeHaveViolation: function (combobox, newValue) {
                var me = this,
                    actViolationGridAddButton = me.getForm().down('#actViolationGridAddButton');

                actViolationGridAddButton.setDisabled(false);

                // Нельзя добавлять, если не 'Да' блокируем кнопку добавления
                if (newValue !== 10) {
                    actViolationGridAddButton.setDisabled(true);
                }
            },

            //переопределен метод сохранения. Сохраняется форма редактирования и таблица дочерних нарушений
            saveRecord: function (rec) {
                var me = this,
                    editWindow = me.getForm(),
                    storeViolation = editWindow.down('#actCheckViolationGrid').getStore(),
                    cbHaveViolation = editWindow.down('#cbHaveViolation').getValue(),
                    description = editWindow.down('#taDescription').getValue(),
                    deferred = new Deferred();

                editWindow.setDisabled(true);

                deferred.next(function(res) {
                    editWindow.setDisabled(false);
                    editWindow.close();
                    me.getGrid().getStore().load();
                    Ext.Msg.alert('Сохранение!', 'Результаты проверки сохранены успешно');
                }, me)
                    .error(function(e) {
                        editWindow.setDisabled(false);
                        if (e.message) {
                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                        } else {
                            throw e;
                        }
                    }, me);

                me.controller.getAspect('actCheckEditPanelAspect')
                    .saveRealityObjectViolation(this.controller.currentRoId, cbHaveViolation, description, storeViolation, deferred);
            }
        },
        {
            //Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            //При добавлении открывается форма массового выбора нарушений. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actCheckViolationAspect',
            gridSelector: '#actCheckViolationGrid',
            saveButtonSelector: '#actCheckViolationGrid #actCheckViolationSaveButton',
            storeName: 'actcheck.Violation',
            modelName: 'actcheck.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckViolationMultiSelectWindow',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, sortable: false },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида нарушений без сохранения
                getdata: function (asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    //Очищаем стор потомучто там буд
                    currentViolationStore.removeAll();
                    
                    //сначала добавлем вверх новые нарушения
                    Ext.Array.each(records.items,
                        function (rec) {
                            //Tесли уже среди существующих записей нет таких записей до добавляем в стор
                            //if (voilationIds.indexOf(rec.get('Id')) == -1) {
                                currentViolationStore.add({
                                    Id: 0,
                                    ActObject: asp.controller.currentRoId,
                                    ViolationGjiPin: rec.get('CodePin'),
                                    ViolationGjiName: rec.get('Name'),
                                    ViolationGjiId: rec.get('Id'),
                                    DatePlanRemoval: null
                                });
                            //}
                        }, this);

                    //теперь добавляем старые вконец
                    Ext.Array.each(range,
                        function (rec) {
                            currentViolationStore.add(rec);
                        }, this);
                   
                    asp.controller.unmask();
                    
                    return true;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /ActCheckGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actCheckInspectorMultiSelectWindowAspect',
            fieldSelector: '#actCheckEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.controlTypeId > 0) {
                    operation.params = operation.params || {};
                    operation.params.memberOnly = true;
                    operation.params.controlTypeId = this.controller.params.controlTypeId;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                    
                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы актов проверки предписаний
            По нажатию на Редактирование Идет открытие карточки Акта проверки предписания в боковой панели
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckActRemovalAspect',
            gridSelector: '#actCheckActRemovalGrid',
            storeName: 'actcheck.ActRemoval',
            modelName: 'ActRemoval',
            editRecord: function (record) {
                var me = this;
                //Перекрываем метод редактирования и открываем в боковой панели вкладку с Актом проверки предписания
                var params = {};
                params.inspectionId = me.controller.params.inspectionId;
                params.documentId = record.getId();
                params.title = 'Акт проверки предписания';
                params.containerSelector = me.controller.params.containerSelector;
                params.treeMenuSelector = me.controller.params.treeMenuSelector;
                
                //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                if (!me.controller.hideMask) {
                    me.controller.hideMask = function () { me.controller.unmask(); };
                }

                me.controller.mask('Загрузка', me.controller.getMainComponent());
                me.controller.loadController('B4.controller.ActRemoval', params, me.controller.params.containerSelector, me.controller.hideMask);
            }
        },
        {
            /*
            Аспект взаимодействия таблицы Лица присутсвующие при проверке, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'actCheckWitnessAspect',
            storeName: 'actcheck.Witness',
            modelName: 'actcheck.Witness',
            gridSelector: '#actCheckWitnessGrid',
            saveButtonSelector: '#actCheckEditPanel #actCheckWitnessSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        //Для новых  записей присваиваем родительский документ
                        if (!rec.get('Id')) {
                            rec.set('ActCheck', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckPeriodAspect',
            gridSelector: '#actCheckPeriodGrid',
            editFormSelector: '#actCheckPeriodEditWindow',
            storeName: 'actcheck.Period',
            modelName: 'actcheck.Period',
            editWindowView: 'actcheck.PeriodEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActCheck', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckAnnexAspect',
            gridSelector: '#actCheckAnnexGrid',
            editFormSelector: '#actCheckAnnexEditWindow',
            storeName: 'actcheck.Annex',
            modelName: 'actcheck.Annex',
            editWindowView: 'actcheck.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActCheck', this.controller.params.documentId);
                    }
                },
                beforesave: function (me, rec) {
                    if (rec.get('SendFileToErknm') === B4.enums.YesNoNotSet.Yes) {
                        var store = me.getGrid().getStore(),
                            foundRecord = store.findRecord('SendFileToErknm', B4.enums.YesNoNotSet.Yes);
                        if (foundRecord && foundRecord.get('Id') !== rec.get('Id')) {
                            Ext.Msg.alert('Ошибка сохранения!', 'Уже добавлены приложения с признаком "Передавать файл в ФГИС ЕРКНМ = Да". В ФГИС ЕРКНМ допускается передавать только один файл акта проверки. Просьба скорректировать значение текущей или ранее добавленной записи.');
                            return false;
                        }
                    }
                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы определений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckDefinitionAspect',
            gridSelector: '#actCheckDefinitionGrid',
            editFormSelector: '#actCheckDefinitionEditWindow',
            storeName: 'actcheck.Definition',
            modelName: 'actcheck.Definition',
            editWindowView: 'actcheck.DefinitionEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.setDefinitionId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActCheck', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    asp.setDefinitionId(record.getId());
                }
            },
            setDefinitionId: function (id) {
                this.controller.params.definitionId = id;
                if (id) {
                    this.controller.getAspect('actCheckDefinitionPrintAspect').loadReportStore();
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы инспектируемых частей с массовой формой выбора инсп. частей
            По нажатию на Добавить открывается форма выбора инсп. частей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение инсп. частей
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actCheckInspectedPartAspect',
            gridSelector: '#actCheckInspectedPartGrid',
            storeName: 'actcheck.InspectedPart',
            modelName: 'actcheck.InspectedPart',
            editFormSelector: '#actCheckInspectedPartEditWindow',
            editWindowView: 'actcheck.InspectedPartEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckInspectedPartMultiSelectWindow',
            storeSelect: 'dict.InspectedPartGjiForSelect',
            storeSelected: 'dict.InspectedPartGjiForSelected',
            titleSelectWindow: 'Выбор инспектируемых частей',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);
                    
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddInspectedParts', 'ActCheckInspectedPart', {
                            partIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспектируемые части');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            //Аспект взаимодействия инлайн таблицы Предоставляемые документы и массовой формой выбора документов
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actCheckProvidedDocAspect',
            gridSelector: '#actcheckProvidedDocGrid',
            saveButtonSelector: '#actcheckProvidedDocGrid #actProvidedDocGridSaveButton',
            storeName: 'actcheck.ProvidedDoc',
            modelName: 'actcheck.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckProvidedDocMultiSelectWindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор дкоументов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            isPaginable: false,
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            otherActions: function (actions) {
                var me = this;
                actions['#actcheckProvidedDocGrid #actProvidedDocGridUpdateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            },
            onBeforeLoad: function (store, operation) {
                operation.start = undefined;
                operation.page = undefined;
                operation.limit = undefined;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddProvidedDocuments', 'ActCheckProvidedDoc', {
                            providedDocIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранено!', 'Документы сохранены успешно');
                            return true;
                        }).error(function () {
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
            * Аспект взаимодействия грида "Действия" с формой редактирования
            */
            xtype: 'griddicteditwindowaspect',
            name: 'actCheckActionAspect',
            gridSelector: '#actCheckActionGrid',
            entityPropertyName: 'ActionType',
            listeners: {
                beforesetformdata: function (asp, record, form) {
                    var actionCarriedOutEvents = form.down('#actCheckActionCarriedOutEventSelectField'),
                        actcheckActionFileGrid = form.down('#actCheckActionFileGrid'),
                        trigfActionInspectors = form.down('#trigfActionInspectors'),
                        actCheckActionRemarkGrid = form.down('#actCheckActionRemarkGrid'),
                        actCheckActionViolationGrid = form.down('#actCheckActionViolationGrid'),
                        instrExamActionNormativeDocGrid = form.down('#instrExamActionNormativeDocGrid'),
                        surveyActionQuestionGrid = form.down('#surveyActionQuestionGrid'),
                        docRequestActionRequestInfoGrid = form.down('#docRequestActionRequestInfoGrid'),
                        stores = [];
                    
                    asp.currentActCheckActionId = record.getId();
                    asp.actCheckActionInspectorIds = null;
                    
                    if (actionCarriedOutEvents) {
                        stores.push(asp.controller.getActcheckActionCarriedOutEventStore());
                    }

                    if (actcheckActionFileGrid) {
                        stores.push(asp.controller.getActcheckActionFileStore());
                    }

                    if (trigfActionInspectors) {
                        stores.push(asp.controller.getActcheckActionInspectorStore());
                    }

                    if (actCheckActionRemarkGrid) {
                        stores.push(asp.controller.getActcheckActionRemarkStore());
                    }

                    if (actCheckActionViolationGrid) {
                        stores.push(asp.controller.getActcheckActionViolationStore());
                    }
                    
                    if (instrExamActionNormativeDocGrid) {
                        stores.push(asp.controller.getActcheckInstrExamActionNormativeDocStore());
                    }
                    
                    if (surveyActionQuestionGrid) {
                        stores.push(asp.controller.getActcheckSurveyActionQuestionStore());
                    }
                    
                    if (docRequestActionRequestInfoGrid) {
                        stores.push(asp.controller.getActcheckDocRequestActionRequestInfoStore());
                    }
                    
                    stores.forEach(function (store) {
                        store.removeAll();
                        store.load();
                    });
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.getAspect('actionPrintAspect').loadReportStore();
                },
                beforesave: function (asp, record) {
                    var form = asp.getForm(),
                        actionCarriedOutEvent = form.down('#actCheckActionCarriedOutEventSelectField'),
                        fillBasedOnAnotherAction = form.down('[name=FillBasedOnAnotherAction]');

                    if (actionCarriedOutEvent){
                        var enumValues = [], fieldValue = actionCarriedOutEvent.value;

                        if (fieldValue) {
                            var enumItemsMeta = fieldValue;

                            if (fieldValue === 'All'){
                                enumItemsMeta = B4.enums.ActCheckActionCarriedOutEventType.getItemsMeta();
                            }

                            enumItemsMeta.forEach(function (enumItemMeta) {
                                enumValues.push(enumItemMeta.Value);
                            })
                        }

                        B4.Ajax.request(B4.Url.action('AddCarriedOutEvents', 'ActCheckAction', {
                            enumValues: enumValues.length === 0 ? '' : enumValues,
                            actCheckActionId: record.getId()
                        })).error(function (err) {
                            Ext.Msg.alert('Ошибка!', err.message);
                            return false;
                        });
                    }

                    if (asp.actCheckActionInspectorIds) {
                        B4.Ajax.request(B4.Url.action('AddInspectors', 'ActCheckAction', {
                            inspectorIds: asp.actCheckActionInspectorIds,
                            actCheckActionId: record.getId()
                        })).error(function (err) {
                            Ext.Msg.alert('Ошибка!', err.message);
                            return false;
                        });
                    }

                    if (fillBasedOnAnotherAction && !fillBasedOnAnotherAction.isHidden()) {
                        var prototype = fillBasedOnAnotherAction.value;

                        if (prototype) {
                            record.set('PrototypeId', prototype.Id);
                            record.set('PrototypeActionType', prototype.ActionType);
                        }
                    }
                }
            },
            setModelAndEditWindowDict: function () {
                var me = this;

                // Для добавления новой записи
                me.modelAndEditWindowMap.set(0, ['actcheck.Action', '#actCheckActionAddWindow', 'actcheck.ActionAddWindow']);

                B4.enums.ActCheckActionType.getItems().forEach(function (item) {
                    var key, properties;
                    switch (item[0]) {
                        case B4.enums.ActCheckActionType.Inspection:
                            key = B4.enums.ActCheckActionType.Inspection;
                            properties = ['actcheck.InspectionAction','actcheckinspectionactioneditwindow','actcheck.inspectionaction.ActionEditWindow'];
                            break;
                        case B4.enums.ActCheckActionType.Survey:
                            key = B4.enums.ActCheckActionType.Survey;
                            properties = ['actcheck.SurveyAction','actchecksurveyactioneditwindow','actcheck.surveyaction.ActionEditWindow'];
                            break;
                        case B4.enums.ActCheckActionType.InstrumentalExamination:
                            key = B4.enums.ActCheckActionType.InstrumentalExamination;
                            properties = ['actcheck.InstrExamAction','actcheckinstrexamactioneditwindow','actcheck.instrexamaction.ActionEditWindow'];
                            break;
                        case B4.enums.ActCheckActionType.RequestingDocuments:
                            key = B4.enums.ActCheckActionType.RequestingDocuments;
                            properties = ['actcheck.DocRequestAction','actcheckdocrequestactioneditwindow','actcheck.docrequestaction.ActionEditWindow'];
                            break;
                        case B4.enums.ActCheckActionType.GettingWrittenExplanations:
                            key = B4.enums.ActCheckActionType.GettingWrittenExplanations;
                            properties = ['actcheck.ExplanationAction','actcheckexplanationactioneditwindow','actcheck.explanationaction.ActionEditWindow'];
                            break;
                        default:
                            return;
                    }
                    me.modelAndEditWindowMap.set(key, properties);
                });
            },
            getRecordBeforeSave: function (record) {
                var asp = this;

                if (!record.get('ActCheck')){
                    record.set('ActCheck', asp.ActCheckId);
                }
                
                return record;
            },
            // Получить окно редактирования действия (без создания, в отличие от .getForm())
            getActionEditForm: function () {
                var me = this;

                if (me.editFormSelector) {
                    return me.componentQuery(me.editFormSelector);
                }

                return null;
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Нормативно-правовые акты" 
            // в окне редактирования действия акта проверки с типом "Инструментальное обследование"
            xtype: 'gkhinlinegridaspect',
            name: 'instrExamActionNormativeDocAspect',
            storeName: 'actcheck.InstrExamActionNormativeDoc',
            modelName: 'actcheck.InstrExamActionNormativeDoc',
            gridSelector: '#instrExamActionNormativeDocGrid',
            saveButtonSelector: '#instrExamActionNormativeDocGrid #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store, 'InstrExamAction');
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Вопросы" 
            // в окне редактирования действия акта проверки с типом "Опрос"
            xtype: 'gkhinlinegridaspect',
            name: 'surveyActionQuestionAspect',
            storeName: 'actcheck.SurveyActionQuestion',
            modelName: 'actcheck.SurveyActionQuestion',
            gridSelector: '#surveyActionQuestionGrid',
            saveButtonSelector: '#surveyActionQuestionGrid #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store, 'SurveyAction');
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Запрошенные сведения" 
            // в окне редактирования действия акта проверки с типом "Истребование документов"
            xtype: 'grideditwindowaspect',
            name: 'docRequestActionRequestInfoAspect',
            gridSelector: '#docRequestActionRequestInfoGrid',
            editFormSelector: '#docRequestActionRequestInfoEditWindow',
            storeName: 'actcheck.DocRequestActionRequestInfo',
            modelName: 'actcheck.DocRequestActionRequestInfo',
            editWindowView: 'actcheck.docrequestaction.RequestInfoEditWindow',
            listeners: {
                beforerowaction: function (asp, grid, action, rec) {
                    if (action.toLowerCase() === 'doubleclick') {
                        return false;
                    }
                },
                getdata: function (asp, record) {
                    asp.controller.setActCheckActionForRecord(record, 'DocRequestAction');
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Нарушения" 
            // в окне редактирования действия акта проверки
            xtype: 'gkhinlinegridaspect',
            name: 'actCheckActionViolationAspect',
            storeName: 'actcheck.ActionViolation',
            modelName: 'actcheck.ActionViolation',
            gridSelector: '#actCheckActionViolationGrid',
            saveButtonSelector: '#actCheckActionViolationGrid #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store);
                    return true;
                }
            }
        },
        {
            xtype: 'actcheckactionviolationaspect',
            buttonSelector: '#actCheckActionViolationGrid #addButton',
            multiSelectWindowSelector: '#actCheckActionViolationGridSelector',
            inlineGridAspectName: 'actCheckActionViolationAspect'
        },
        {
            // Аспект, который обеспечивает работу грида "Замечания" 
            // в окне редактирования действия акта проверки
            xtype: 'gkhinlinegridaspect',
            name: 'actCheckActionRemarkAspect',
            storeName: 'actcheck.ActionRemark',
            modelName: 'actcheck.ActionRemark',
            gridSelector: '#actCheckActionRemarkGrid',
            saveButtonSelector: '#actCheckActionRemarkGrid #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store);
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Файлы" 
            // в окне редактирования действия акта проверки
            xtype: 'grideditwindowaspect',
            name: 'actCheckActionFileAspect',
            gridSelector: '#actCheckActionFileGrid',
            editFormSelector: '#actCheckActionFileEditWindow',
            storeName: 'actcheck.ActionFile',
            modelName: 'actcheck.ActionFile',
            editWindowView: 'actcheck.ActionFileEditWindow',
            listeners: {
                beforerowaction: function (asp, grid, action, rec) {
                    if (action.toLowerCase() === 'doubleclick') {
                        return false;
                    }
                },
                getdata: function (asp, record) {
                    asp.controller.setActCheckActionForRecord(record);
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает добавление инспекторов 
            // в окне редактирования действия акта проверки
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actCheckActionInspectorMultiSelectWindowAspect',
            fieldSelector: '#actCheckActionAddWindow #trigfActionInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckActionInspectorSelectWindow',
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
                    var actionEditAspect = asp.controller.getAspect('actCheckActionAspect');

                    actionEditAspect.actCheckActionInspectorIds = [];

                    records.each(function (rec) { 
                        actionEditAspect.actCheckActionInspectorIds.push(rec.get('Id')); 
                    });

                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('actcheck.RealityObject').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Violation').on('beforeload', me.onViolationBeforeLoad, me);
        me.getStore('actcheck.ActRemoval').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Witness').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Definition').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Annex').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Period').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.InspectedPart').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.ProvidedDoc').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Action').on('beforeload', me.onActionBeforeLoad, me);
        me.getStore('actcheck.ActionCarriedOutEvent').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.ActionCarriedOutEvent').on('load', me.onActionCarriedOutEventLoad, me);
        me.getStore('actcheck.ActionFile').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.ActionInspector').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.ActionInspector').on('load', me.onActionInspectorStoreLoad, me);
        me.getStore('actcheck.ActionRemark').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.ActionViolation').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.InstrExamActionNormativeDoc').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.SurveyActionQuestion').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actcheck.DocRequestActionRequestInfo').on('beforeload', me.onActionObjectBeforeLoad, me);
        
        me.control({
            '#actCheckActionAddWindow [name=FillBasedOnAnotherAction]' : {
                beforeload: {fn: me.onActionBeforeLoad, scope: me},
                change: {fn: me.fillActionAddWindow}
            },
            '#actCheckActionAddWindow b4combobox[name=ActionType]': {
                storebeforeload: { fn: me.onActionTypeBeforeLoad, scope: me },
                change: { fn: me.fillActionAddWindow }
            },
            '#actCheckActionAddWindow' : {
                afterrender: {fn: me.onAddWindowAfterRender}
            }
        });

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('actCheckEditPanelAspect').setData(me.params.documentId);

            //Обновляем таблицу Лиц присутсвующих при проверке
            me.getStore('actcheck.Witness').load();

            //Обновляем таблицу Дата и время проведения проверки
            me.getStore('actcheck.Period').load();

            //Обновляем таблицу Приложений
            me.getStore('actcheck.Annex').load();

            //Обновляем таблицу определений
            me.getStore('actcheck.Definition').load();

            //Обновляем таблицу инспектируемых частей
            me.getStore('actcheck.InspectedPart').load();

            //Обновляем таблицу действий
            me.getStore('actcheck.Action').load();
        }

        me.getMainView().down('b4enumcombo[name=AcquaintState]')
            .on('change', me.acquaintStateChanged, me);
    },

    onObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0)
            operation.params.documentId = me.params.documentId;
    },

    onViolationBeforeLoad: function (store, operation) {
        var me = this;

        if (me.currentRoId > 0) {
            operation.params.objectId = me.currentRoId;
        }
        else if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.documentId;
        }
    },

    onActionBeforeLoad: function (store, operation) {
        var me = this,
            actionEditAspect = me.getAspect('actCheckActionAspect');

        if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.params.documentId;
            actionEditAspect.ActCheckId = me.params.documentId;
        }
    },

    onActionTypeBeforeLoad: function (field, store, operation) {
        operation.params.documentId = this.params.documentId;
    },

    onActionObjectBeforeLoad: function (store, operation) {
        var me = this,
            actionEditAspect = me.getAspect('actCheckActionAspect');
        
        if (actionEditAspect.currentActCheckActionId &&
            actionEditAspect.currentActCheckActionId > 0) {
            operation.params.actCheckActionId = actionEditAspect.currentActCheckActionId;
        }
    },

    onActionCarriedOutEventLoad: function (store, records) {
        var me = this,
            actionEditAspect = me.getAspect('actCheckActionAspect'),
            form = actionEditAspect.getActionEditForm();
        
        if (form) {
            var carriedOutEventSelectField = form.down('#actCheckActionCarriedOutEventSelectField'),
                eventType, value = [];

            if (records.length === B4.enums.ActCheckActionCarriedOutEventType.getItems().length) {
                carriedOutEventSelectField.updateDisplayedText('Выбраны все');
                carriedOutEventSelectField.value = 'All';
            }
            else {
                records.forEach(function (rec) {
                    eventType = rec.get('EventType');

                    if (eventType) {
                        value.push(B4.enums.ActCheckActionCarriedOutEventType.getMeta(eventType));
                    }
                });

                if (value.length > 0) {
                    carriedOutEventSelectField.setValue(value);
                }
            }
        }
    },
    
    onActionInspectorStoreLoad: function (store, records) {
        var me = this,
            actionEditAspect = me.getAspect('actCheckActionAspect'),
            form = actionEditAspect.getActionEditForm();
        
        if (form) {
            var trigfActionInspectors = form.down('#trigfActionInspectors'),
                inspector, inspectorIds = [], inspectorNames = [];

            records.forEach(function (rec) {
                inspector = rec.get('Inspector');

                if (inspector) {
                    inspectorIds.push(inspector.Id);
                    inspectorNames.push(inspector.Fio);
                }
            });

            if (inspectorIds.length > 0) {
                trigfActionInspectors.updateDisplayedText(inspectorNames.join(', '));
                trigfActionInspectors.setValue(inspectorIds.join(', '));
            }
        }
    },

    setCurrentRoId: function (id) {
        var me = this;

        me.currentRoId = id;
        me.getStore('actcheck.Violation').load();
    },

    /*
    Принимает '#actCheckEditPanel gjidocumentcreatebutton'.menu,
    */
    getMenuElementByRuleId: function (menu, ruleId) {
        if (menu && 'items' in menu && menu.items && 'items' in menu.items && menu.items.items) {
            return menu.items.items.find(function(obj) {
                return 'ruleId' in obj && obj.ruleId === ruleId;
            });
        } else {
            return null;
        }
    },
    
    acquaintStateChanged: function(component, newValue) {
        var fieldset = component.up('fieldset[name=AcquaintInfo]'),
            refusedToAcquaintPersonField = fieldset.down('textfield[name=RefusedToAcquaintPerson]'),
            acquaintedPersonField = fieldset.down('textfield[name=AcquaintedPerson]'),
            acquaintedPersonTitleField = fieldset.down('textfield[name=AcquaintedPersonTitle]');

        refusedToAcquaintPersonField.hide();
        acquaintedPersonField.hide();
        acquaintedPersonTitleField.hide();
        refusedToAcquaintPersonField.disable();
        acquaintedPersonField.disable();
        acquaintedPersonTitleField.disable();

        switch (newValue) {
            case B4.enums.AcquaintState.NotAcquainted:
                break;

            case B4.enums.AcquaintState.RefuseToAcquaint:
                refusedToAcquaintPersonField.setVisible(refusedToAcquaintPersonField.allowedView);
                refusedToAcquaintPersonField.setDisabled(!refusedToAcquaintPersonField.allowedEdit);
                break;

            case B4.enums.AcquaintState.Acquainted:
                acquaintedPersonField.setVisible(acquaintedPersonField.allowedView);
                acquaintedPersonField.setDisabled(!acquaintedPersonField.allowedEdit);
                acquaintedPersonTitleField.setVisible(acquaintedPersonField.allowedView);
                acquaintedPersonTitleField.setDisabled(!acquaintedPersonField.allowedEdit);
                break;
        }
    },

    setActCheckActionForRecord: function (record, propertyName) {
        var me = this;

        if (!propertyName){
            propertyName = 'ActCheckAction';
        }

        if (!record.get('Id')) {
            record.set(propertyName, me.getCurrentActCheckAction());
        }
    },
    
    setActCheckActionForStoreRecords: function (store, propertyName) {
        var me = this;
        
        store.each(function (record) {
            me.setActCheckActionForRecord(record, propertyName);
        });
    },
    
    getCurrentActCheckAction: function () {
        var me = this,
            asp = me.getAspect('actCheckActionAspect');
        
        return asp.currentActCheckActionId;
    },
    
    fillActionAddWindow: function(cmp, data){
        var window = cmp.up('window'),
            dateField = window.down('[name=Date]'),
            creationPlaceField = window.down('[name=CreationPlace]')
        
        if(data) {
            var creationPlace = data['CreationPlace'];

            creationPlace.Id = 0;
            dateField.setValue(new Date(data['Date']));
            creationPlaceField.setValue(creationPlace);
        } else {
            dateField.setValue(null);
            creationPlaceField.setValue(null);
        }
        
        dateField.validate();
        creationPlaceField.validate();
    },

    onAddWindowAfterRender: function(window){
        var me = this,
            store = me.getStore('actcheck.Action'),
            fillBasedOnAnotherActionField = window.down('[name=FillBasedOnAnotherAction]');
        
        if(fillBasedOnAnotherActionField) {
            store.load(
                {
                    callback: function () {
                        if (store.getCount() > 0) {
                            fillBasedOnAnotherActionField.show();
                        } else {
                            fillBasedOnAnotherActionField.hide();
                        }
                    }
                });
        }
    }
});