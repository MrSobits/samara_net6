Ext.define('B4.controller.ActCheck', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActCheck',
        'B4.aspects.permission.ChelyabinskActCheck',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet',
        'B4.aspects.GkhBlobText',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.aspects.StateContextMenu',
        'B4.aspects.StateGridWindowColumn',
        'B4.view.actcheck.ControlListTestWindow'
    ],

    models: [
        'ActCheck',
        'actcheck.Annex',
        'actcheck.Period',
        'actcheck.Witness',
        'actcheck.Violation',
        'actcheck.Definition',
        'actcheck.RealityObject',
        'actcheck.InspectedPart',
        'actcheck.ProvidedDoc',
        'actcheck.ControlListAnswer',
        'actcheck.ControlMeasures',
        'Prescription',
        'ProtocolGji',
        'Disposal'
    ],

    stores: [
        'ActCheck',
        'actcheck.Annex',
        'actcheck.Period',
        'actcheck.Witness',
        'actcheck.Violation',
        'actcheck.ActRemoval',
        'actcheck.ProvidedDoc',
        'actcheck.Definition',
        'actcheck.InspectedPart',
        'actcheck.RealityObject',
        'actcheck.ViolationForSelect',
        'actcheck.ControlMeasures',
        'actcheck.ViolationForSelected',
        'actcheck.ControlListAnswer',
        'dict.ControlListForSelect',
        'dict.ControlListForSelected',
        'prescription.ForSelect',
        'prescription.ForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'dict.ViolationGjiForTreeSelect',
        'dict.Inspector'
    ],

    views: [
        'actcheck.EditPanel',
        'actcheck.RealityObjectGrid',
        'actcheck.RealityObjectEditWindow',
        'actcheck.RealityObjectEditPanel',
        'actcheck.ViolationGrid',
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
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree',
        'actcheck.ProvidedDocGrid',
        'actcheck.ViolationEditWindow',
        'actcheck.Grid',
        'actcheck.ControlMeasuresGrid'
    ],

    mainView: 'actcheck.EditPanel',
    mainViewSelector: '#actCheckEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: '#actCheckAnnexGrid',
            controllerName: 'ActCheckAnnex',
            name: 'actCheckAnnexSignatureAspect',
            signedFileField: 'SignedFile'
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'actCheckDefinitionGrid',
            controllerName: 'ActCheckDefinition',
            name: 'actCheckDefinitionSignatureAspect',
            signedFileField: 'SignedFile'
        },
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
                if (params.ruleId == 'ActCheckToDisposalRule'
                    || params.ruleId == 'ActCheckToProtocolRule'
                    || params.ruleId == 'ActCheckToPrescriptionRule') {
                    return false;
                }
                return true;
            }
        },
        {
            xtype: 'actcheckperm',
            editFormAspectName: 'actCheckEditPanelAspect',
            name: "actCheckEditPanelPermissionAspect"
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.Area_Rqrd', applyTo: '#nfArea', selector: '#actCheckEditPanel' },
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.Acquainted_With_The_Order', applyTo: '#nfAcquaintedWithDisposalCopy', selector: '#actCheckEditPanel' }
            ]
        },
        {
            xtype: 'nsoactcheckperm',
            editFormAspectName: 'actCheckEditPanelAspect'
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
            xtype: 'b4_state_contextmenu',
            name: 'actCheckStateTransferAspect',
            gridSelector: '#actCheckGrid',
            stateType: 'gji_document_actcheck',
            menuSelector: 'actCheckStateTransferStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('ActCheck');
                    model.load(record.getId());
                }
            }
        },
        {
            //Аспект кнопки печати акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actCheckPrintAspect',
            buttonSelector: '#actCheckEditPanel #btnPrint',
            codeForm: 'ActCheck',
            displayField: 'Description',
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            //кнопка печати определения акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actCheckDefinitionPrintAspect',
            buttonSelector: '#actCheckDefinitionEditWindow #btnPrint',
            codeForm: 'ActCheckDefinition',
            getUserParams: function () {
                var param = { DefinitionId: this.controller.params.definitionId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /* 
            Аспект  для Акта проверки (в нем идет сохранение основных сведений + формирование дочерних документов)
            */
            xtype: 'gjidocumentaspect',
            name: 'actCheckEditPanelAspect',
            editPanelSelector: '#actCheckEditPanel',
            modelName: 'ActCheck',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbToPros'] = { 'change': { fn: this.onToProsecutorChange, scope: this } };
                actions[this.editPanelSelector + ' #cbUnavaliable'] = { 'change': { fn: this.oncbUnavaliableChange, scope: this } };
                actions[this.editPanelSelector + ' #sfResolPros'] = {
                    'beforeload': { fn: this.onResProsBeforeLoad, scope: this },
                    'change': { fn: this.onResProsChange, scope: this }
                };

                actions[this.editPanelSelector + ' #actCheckRealityObjectEditPanel #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this} };
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var addButton = panel.down('#actCheckViolationGrid').down('b4addbutton');
                    if (rec.get('ActCheckGjiRealityObject') && rec.get('ActCheckGjiRealityObject').HaveViolation == 10) {
                        addButton.setDisabled(true);
                    } else {
                        addButton.setDisabled(false);
                    }
                }
            }, 
            oncbUnavaliableChange: function (field, newValue) {
                var panel = this.getPanel();
                var tfUnavaliableComment = panel.down('#tfUnavaliableComment')
                if (newValue == true) {
                    tfUnavaliableComment.show();
                    tfUnavaliableComment.allowBlank = false;
                }
                else {
                    tfUnavaliableComment.hide();
                    tfUnavaliableComment.allowBlank = true;
                }
            },

            saveRecord: function (rec) {
                // делаем запрос, так как  поле ActCheckGjiRealityObject  догоняется в get
                var me = this,
                    model = me.getModel(rec);
                model.load(rec.getId(), {
                    success: function(result) {
                        //Если это акт на 1 дом то тогда сохраняем сначала Результаты проверки,
                        //а затем саму сущность
                        if (result.get('ActCheckGjiRealityObject') != null) {
                            var recordObj = result.get('ActCheckGjiRealityObject'),
                                panel = me.getPanel(),
                                storeViolation = panel.down('#actCheckRealityObjectEditPanel #actCheckViolationGrid').getStore(),
                                haveViolation = panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').getValue(),
                                description = panel.down('#actCheckRealityObjectEditPanel #taDescription').getValue(),
                                officialsGuiltyActions = panel.down('#actCheckRealityObjectEditPanel #taOfficialsGuiltyActions').getValue(),
                                personsWhoHaveViolated = panel.down('#actCheckRealityObjectEditPanel #taPersonsWhoHaveViolated').getValue(),
                                deferred = new Deferred();

                            panel.setDisabled(true);

                            deferred.next(function() {
                                me.saveActCheck(rec);
                            }, me)
                                .error(function(e) {
                                    panel.setDisabled(false);
                                    if (e.message) {
                                        Ext.Msg.alert('Ошибка сохранения!', e.message);
                                        return false;
                                    } else {
                                        throw e;
                                    }
                                }, me);

                            me.saveRealityObjectViolation(recordObj.Id, haveViolation, description, storeViolation, deferred, officialsGuiltyActions, personsWhoHaveViolated);
                        } else {
                            me.saveActCheck(rec);
                        }
                    },
                    scope: me
                });
            },

            saveActCheck: function (rec) {
                var me = this,
                    panel = this.getPanel(),
                    recId = rec.getId();

                panel.setDisabled(true);
                rec.save({ id: recId })
                    .next(function () {
                        panel.setDisabled(false);
                        me.setData(recId);
                        B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                    })
                    .error(function(result) {
                        panel.setDisabled(false);
                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    });
            },

            changeHaveViolation: function (combobox, newValue) {
                var actViolationGridAddButton = this.getPanel().down('#actCheckRealityObjectEditPanel #actViolationGridAddButton');

                actViolationGridAddButton.setDisabled(false);
                //Нельзя добавлять если не 'да' блокируем кнопку добавления
                if(newValue != 10)
                    actViolationGridAddButton.setDisabled(true);
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this;
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                asp.controller.mask('Загрузка', panel);
                
                var addButton = panel.down('#actCheckViolationGrid').down('b4addbutton');
                if (rec.get('ActCheckGjiRealityObject') && rec.get('ActCheckGjiRealityObject').HaveViolation == 10) {
                    addButton.setDisabled(true);
                } else {
                    addButton.setDisabled(false);
                }
                
                //После проставления данных обновляем title у вкладки
                var title = rec.get('TypeActCheck') == 10 ? 'Акт проверки (общий)' : 'Акт проверки';
                
                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('#actCheckTabPanel').setActiveTab(0);

                if (rec.get('TypeActCheck') == 30) {
                    //Если Акт проверки с типом = Акт проверки предписания то название вкладки меняем 
                    //То должны быть кнопки Акт Общий, Акт на 1 дом и Акт обследования
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
                me.controller.documentId = rec.getId();
                
                if (rec.get('ActCheckGjiRealityObject') != null) {
                    panel.down('actcheckinspectionresultpanel').hide();
                    panel.down('#actCheckRealityObjectEditPanel').show();

                    var realObj = rec.get('ActCheckGjiRealityObject');
                    me.controller.currentRoId = realObj.Id;
                    panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').setValue(realObj.HaveViolation);
                    panel.down('#actCheckRealityObjectEditPanel #taDescription').setValue(realObj.Description);
                    panel.down('#actCheckRealityObjectEditPanel #taOfficialsGuiltyActions').setValue(realObj.OfficialsGuiltyActions);
                    panel.down('#actCheckRealityObjectEditPanel #taPersonsWhoHaveViolated').setValue(realObj.PersonsWhoHaveViolated);
                    panel.down('#actCheckRealityObjectEditPanel #taNotRevealedViolations').setValue(realObj.NotRevealedViolations);

                    me.controller.getStore('actcheck.Violation').load();
                    me.controller.getAspect('actCheckRoBlobTextAspect').doInjection();
                    me.controller.getAspect('actCheckNotRevealedViolationsAspect').doInjection();
                } else {
                    panel.down('actcheckinspectionresultpanel').show();
                    panel.down('#actCheckRealityObjectEditPanel').hide();

                    //Обновляем таблицу Проверяемых домов акта
                    me.controller.getStore('actcheck.RealityObject').load();
                }

                B4.Ajax.request(B4.Url.action('GetInfo', 'ActCheck', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    
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

                    var fieldInspectors = panel.down('#trigfInspectors');

                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldset = panel.down('#fsActResol');
                    if (obj.typeBase == 50) {
                        if (fieldset)
                            fieldset.show();
                    } else {
                        if (fieldset)
                            fieldset.hide();
                    }

                    me.disableButtons(false);
                    
                    asp.controller.unmask();
                }).error(function () {
                    asp.controller.unmask();
                });
                
                //Обновляем таблицу Лиц присутсвующих при проверке
                me.controller.getStore('actcheck.Witness').load();

                //Обновляем таблицу Дата и время проведения проверки
                me.controller.getStore('actcheck.Period').load();

                //Обновляем таблицу Приложений
                me.controller.getStore('actcheck.Annex').load();

                //Обновляем таблицу определений
                me.controller.getStore('actcheck.Definition').load();

                //Обновляем таблицу инспектируемых частей
                me.controller.getStore('actcheck.InspectedPart').load();

                //Обновляем таблицу предосталвяемые докуенты 
                me.controller.getStore('actcheck.ProvidedDoc').load();
                me.controller.getStore('actcheck.ControlListAnswer').load();

                
                //загружаем стор отчетов
                me.controller.getAspect('actCheckPrintAspect').loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('actCheckStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем кнопку Сформирвоать
                me.controller.getAspect('actCheckCreateButtonAspect').setData(rec.get('Id'));

                me.controller.isAnyHasViolation();
                me.controller.getStore('actcheck.ControlMeasures').load();
            },

            /*
            * метод сохранения нарушений по дому
            */
            saveRealityObjectViolation: function (actObjectId, haveViolation, description, violationStore, deferred, officialsGuiltyActions, personsWhoHaveViolated) {
                var me = this,
                    panel = me.controller.getMainComponent(),
                    actDate = panel.down('datefield[name=DocumentDate]').value,
                    actDatePlus6 = new Date(new Date(actDate).setMonth(actDate.getMonth() + 6));
                
                //Блокируем сохранение если не выполняется ряд условий
                if (violationStore.getCount() == 0 && haveViolation == 10) {
                    deferred.fail({ message: 'Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений' });
                    return false;
                }

                if (violationStore.getCount() != 0 && haveViolation != 10) {
                    deferred.fail({ message: 'Записи в таблице нарушений должны быть только если нарушения выявлены' });
                    return false;
                }

                /*
                //по умолчанию считаем что даты в сторе в гриде забиты по всем рекордам
                var notExistDate = false;
                
                violationStore.each(function (rec) {
                    var datePlanRemoval = rec.get('DatePlanRemoval');
                    if (Ext.isEmpty(datePlanRemoval)) {
                        notExistDate = true;
                    }
                });

                if (violationStore.getCount() != 0 && notExistDate) {
                    deferred.fail({ message: 'Необходимо для всех нарушений проставить срок устранения' });
                    return false;
                }
                */

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

                            if (datePlanRemoval.getTime() > actDatePlus6.getTime()) {
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
                    deferred.fail({ message: 'Срок устранения нарушения не должен превышать 6 месяцев.' });
                    return false;
                }

                me.controller.mask('Сохранение', panel);
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('SaveParams', 'ActCheckRealityObject'),
                    params: {
                        actObjectId: actObjectId,
                        haveViolation: haveViolation,
                        actViolationJson: Ext.encode(actCheckViolationRecords),
                        description: description,
                        officialsGuiltyActions: officialsGuiltyActions,
                        personsWhoHaveViolated: personsWhoHaveViolated
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

            onToProsecutorChange: function (field, newValue) {
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

            onResProsBeforeLoad: function (field, options) {
                options = options || {};
                options.params = {};

                options.params.documentType = 80;

                return true;
            },

            disableButtons: function (value) {
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
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield'} },
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
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'CodesPin', flex: 1, sortable: false },
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
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'CodesPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'CodesPin', flex: 1, sortable: false },
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
                { header: 'Номер документа', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата', xtype: 'datecolumn', dataIndex: 'DocumentDate', format: 'd.m.Y', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер документа', dataIndex: 'DocumentNumber', sortable: false },
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
                        });

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
                aftersetformdata: function(asp, record, form) {
                    asp.controller.setCurrentRoId(record.getId());

                    asp.controller.getAspect('actCheckEditPanelPermissionAspect').setPermissionsByRecord({ getId: function () { return asp.controller.documentId; } });

                    var field = form.down('textarea[name=TechPassportChars]');

                    if (!field) return;

                    B4.Ajax.request(B4.Url.action('GetRobjectCharacteristics', 'ActCheckRealityObject', {
                        actRoId: record.getId()
                    })).next(function(res) {
                        var obj = Ext.decode(res.responseText);

                        field.setValue(obj.chars);
                    });
                }
            },

            editRecord: function (record) {
                var me = this,
                    panel = me.controller.getMainComponent(),
                    actDate = panel.down('datefield[name=DocumentDate]').value,
                    id, model;
                
                if (!actDate) {
                    Ext.Msg.alert('Ошибка!', 'Для того, чтобы работать с формой, необходимо заполнить поле Дата в акте.');
                    return;
                }

                id = record ? record.getId() : null;
                model = me.getModel(record);

                if (id) {
                    model.load(id, {
                        success: function (rec) {
                            me.setFormData(rec);
                        }
                    });
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },

            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #cbHaveViolation'] = { 'change': { fn: me.changeHaveViolation, scope: me } };
            },

            changeHaveViolation: function (combobox, newValue) {
                var form = this.getForm(),
                    addButton = form.down('#actViolationGridAddButton'),
                    addCharsField = form.down('textarea[name=AdditionalChars]');

                //если не 'да' блокируем кнопку добавления
                addButton.setDisabled(newValue != 10);
                addCharsField.setDisabled(newValue != 10);
            },

            //переопределен метод сохранения. Сохраняется форма редактирования и таблица дочерних нарушений
            saveRecord: function () {
                var me = this,
                    editWindow = me.getForm(),
                    storeViolation = editWindow.down('#actCheckViolationGrid').getStore(),
                    cbHaveViolation = editWindow.down('#cbHaveViolation').getValue(),
                    description = editWindow.down('#taDescription').getValue(),
                    deferred = new Deferred();

                editWindow.setDisabled(true);

                deferred
                    .next(function() {
                        editWindow.setDisabled(false);
                        editWindow.close();
                        me.getGrid().getStore().load();
                        Ext.Msg.alert('Сохранение!', 'Результаты проверки сохранены успешно');
                        me.controller.isAnyHasViolation();
                    })
                    .error(function(e) {
                        editWindow.setDisabled(false);
                        if (e.message) {
                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                        } else {
                            throw e;
                        }
                    });

                me.controller.getAspect('actCheckEditPanelAspect')
                    .saveRealityObjectViolation(
                        me.controller.currentRoId,
                        cbHaveViolation,
                        description,
                        storeViolation,
                        deferred,
                        null, null);
            }
        },
        {
           /*
            * Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            * При добавлении открывается форма массового выбора нарушений. После выбора список получается через подписку 
            * на событие getdata идет добавление записей в сторе
            */
            xtype: 'gkhmultiselectwindowtreeaspect',
            name: 'actCheckViolationAspect',
            gridSelector: '#actCheckViolationGrid',
            saveButtonSelector: '#actCheckViolationGrid #actCheckViolationSaveButton',
            storeName: 'actcheck.Violation',
            modelName: 'actcheck.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindowTree',
            multiSelectWindowSelector: '#multiSelectWindowTree',
            storeSelect: 'dict.ViolationGjiForTreeSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            isTbar: true,
            tbarCmp: [
                {
                    xtype: 'textfield',
                    ident: 'searchfield',
                    width: 350,
                    emptyText: 'Поиск',
                    enableKeyEvents: true
                },
                {
                    xtype: 'button',
                    text: 'Искать',
                    iconCls: 'icon-page-white-magnify',
                    ident: 'searchbtn'
                }
            ],
            otherActions: function (actions) {
                var me = this;
                
                actions[me.multiSelectWindowSelector + ' [ident=searchbtn]'] = { 'click': { fn: me.goFilter, scope: me } };
                actions[me.multiSelectWindowSelector + ' [ident=searchfield]'] = {
                    'keypress': {
                        fn: function (scope, e) {
                            if (e.getKey() == 13) {
                                me.goFilter(scope);
                            }
                }, scope: me } };
            },
            goFilter: function (btn) {
                var filterData = btn.up('#multiSelectWindowTree').down('[ident=searchfield]').getValue(),
                    treepanel = btn.up('#multiSelectWindowTree').down('treepanel');
                treepanel.getStore().reload({
                    params: { filter: filterData }
                });
            },
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'treecolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'Code', width: 140, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', width: 80, sortable: false }
            ],
            listeners: {
                getdata: function(asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    currentViolationStore.removeAll();

                    Ext.Array.each(records.items, function(rec) {
                        currentViolationStore.add({
                            Id: 0,
                            ActObject: asp.controller.currentRoId,
                            CodesPin: rec.get('ViolationGjiPin'),
                            ViolationGjiName: rec.get('ViolationGjiName'),
                            ViolationGjiId: rec.get('ViolationGjiId'),
                            Features: rec.get('FeatViol'),
                            DatePlanRemoval: null
                        });
                    });

                    Ext.Array.each(range, function(rec) {
                        currentViolationStore.add(rec);
                    });

                    asp.controller.unmask();

                    return true;
                }
            },
            onCheckRec: function (node, checked) {
                var me = this,
                    grid = me.getSelectedGrid(),
                    storeSelected = grid.getStore(),
                    model = me.controller.getModel(me.modelName);
                
                if (grid && node.get('leaf')) {
                    if (checked) {
                        if (storeSelected.find('Id', node.get('Id'), 0, false, false, true) == -1) {
                            storeSelected.add(new model({ Id: node.get('Id'), ViolationGjiPin: node.get('Code'), ViolationGjiName: node.get('Name'), ViolationGjiId: node.get('ViolationGjiId') }));
                        }
                    } else {
                        storeSelected.remove(storeSelected.getById(node.get('Id')));
                    }
                }
            },
            getSelectGrid: function () {
                var me = this;
                if (me.componentQuery) {
                    win = me.componentQuery(me.multiSelectWindowSelector);
                }

                if (!win) {
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
                }

                if (win) {
                    return win.down('#tpSelect');
                }
            },
            selectedGridRowActionHandler: function (action, record) {
                var me = this,
                    gridSelect = me.getSelectGrid(),
                    gridSelected = me.getSelectedGrid();

                if (gridSelect && gridSelected) {
                    gridSelected.fireEvent('rowaction', gridSelected, action, record);
                    
                    var node = gridSelect.getRootNode().findChild('Id', record.getId(), true);
                    if (node) {
                        node.set('checked', false);
                    }
                }
            },
            getForm: function () {
                var me = this,
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0],
                    stSelected,
                    stSelect;

                if (win && !win.getBox().width) {
                    win = win.destroy();
                }

                if (!win) {
                    stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);
                    stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

                    stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
                    stSelect.on('beforeload', me.onBeforeLoad, me);
                    stSelect.on('load', me.onLoad, me);

                    win = me.controller.getView(me.multiSelectWindow).create({
                        itemId: me.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: me.columnsGridSelect,
                        columnsGridSelected: me.columnsGridSelected,
                        title: me.titleSelectWindow,
                        titleGridSelect: me.titleGridSelect,
                        titleGridSelected: me.titleGridSelected,
                        selModelMode: me.selModelMode,
                        isTbar: me.isTbar,
                        tbarCmp: me.tbarCmp,
                        constrain: true,
                        modal: false,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    });

                    win.on('afterrender', me.formAfterrender, me);

                    if (Ext.isNumber(me.multiSelectWindowWidth) && win.setWidth) {
                        win.setWidth(me.multiSelectWindowWidth);
                    }

                    stSelected.sorters.clear();
                    stSelect.sorters.clear();
                }

                return win;
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
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        });

                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
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
            },
            onAfterSetFormData: function (asp, rec, form) {
                if (form) {
                    form.show();
            }
                asp.controller.setDisplayTimeField('#tfTimeStart');
                asp.controller.setDisplayTimeField('#tfTimeEnd');
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
                aftersetformdata: function (asp, record) {
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
        {////
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckEditViolationAspect',
            gridSelector: 'actCheckViolationGrid',
            editFormSelector: 'actcheckviolationwin',
            storeName: 'actcheck.Violation',
            modelName: 'actcheck.Violation',
            editWindowView: 'actcheck.ViolationEditWindow',
            editRecord: function(record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (!id) return;

                model = me.getModel(record);

                model.load(id, {
                    success: function(rec) {
                        me.setFormData(rec);
                    },
                    scope: me
                });

                me.getForm().getForm().isValid();

                me.controller.currentViolId = id;
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var field = form.down('textarea[name=TechPassportChars]');

                    if (!field) return;

                    B4.Ajax.request(B4.Url.action('GetRobjectCharacteristics', 'ActCheckRealityObject', {
                        actRoId: rec.getId()
                    })).next(function (res) {
                        var obj = Ext.decode(res.responseText);

                        field.setValue(obj.data);
                    });
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
                        function(item) {
                            recordIds.push(item.get('Id'));
                        });
                    
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddInspectedParts', 'ActCheckInspectedPart', {
                            partIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
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
            //Аспект взаимодействия инлайн таблицы Предоставляемые документы и массовой формой выбора документов
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actCheckControlListAnswerAspect',
            gridSelector: 'actcheckcontrollistanswergrid',
            saveButtonSelector: 'actcheckcontrollistanswergrid #actcheckcontrollistanswerSaveButton',
            storeName: 'actcheck.ControlListAnswer',
            modelName: 'actcheck.ControlListAnswer',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckControlListAnswerMultiSelectWindow',
            storeSelect: 'dict.ControlListForSelect',
            storeSelected: 'dict.ControlListForSelected',
            titleSelectWindow: 'Выбор проверочных листов',
            titleGridSelect: 'Проверочные листы для отбора',
            titleGridSelected: 'Выбранные проверочные листы',
            isPaginable: false,


            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            otherActions: function (actions) {
                var me = this;
                actions['actcheckcontrollistanswergrid #actcheckcontrollistanswerupdateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
                actions['actcheckcontrollistanswergrid #clistAnswers'] = {
                    'click': {
                        fn: this.onLoadTest, scope: this
                    }
                };

                actions['controllistwindow #btnExamNextQuestion'] = {
                    'click': {
                        fn: this.saveAndGoNextQuestion, scope: this
                    }
                };

                actions['controllistwindow #btnExamClose'] = {
                    'click': {
                        fn: this.closeExamForm, scope: this
                    }
                };


            },

            saveAndGoNextQuestion: function (btn) {
                var me = this;
                var clistAnswersWindow = btn.up('controllistwindow');
                if (clistAnswersWindow.qid == 0) {
                    var questionFieldId = clistAnswersWindow.down('#questionFieldId');
                    var NameCCLId = clistAnswersWindow.down('#NameCCLId');
                    NameCCLId.setText('Проверка закончена');
                    questionFieldId.setText('Нажмите кнопку "Закрыть"');
                    var btnExamNextQuestion = clistAnswersWindow.down('#btnExamNextQuestion');
                    var btnExamClose = clistAnswersWindow.down('#btnExamClose');
                    btnExamNextQuestion.hide();
                    btnExamClose.show();
                }
                var radGrp = clistAnswersWindow.down('#radGroupId');
                var ans = radGrp.getChecked();

                if (ans.length == 0) {
                    Ext.Msg.alert('Ошибка!', 'Выберите один из вариантов');
                }
                else {
                    var choise = ans[0].inputValue;
                    var descripitonfield = clistAnswersWindow.down('#infoFieldId'),
                        descripiton = descripitonfield.getValue();
                    B4.Ajax.request({
                        url: B4.Url.action('SaveAndGetNextQuestion', 'ActCheckProvidedDoc'),
                        params: {
                            documentId: me.controller.params.documentId,
                            choise: choise,
                            descripiton: descripiton,
                            answerId: clistAnswersWindow.qid
                        }
                    }).next(function (resp) {
                        var tryDecoded;

                        //asp.unmask();
                        try {
                            tryDecoded = Ext.JSON.decode(resp.responseText);
                        } catch (e) {
                            tryDecoded = {};
                        }
                        if (tryDecoded.success == false) {
                            throw new Error(tryDecoded.message);
                        }

                        var questionFieldId = clistAnswersWindow.down('#questionFieldId');
                        var NameCCLId = clistAnswersWindow.down('#NameCCLId');
                        radGrp.reset();
                        descripitonfield.setValue(null);
                        clistAnswersWindow.qid = tryDecoded.data.qid;;
                        if (tryDecoded.data.Question != "" && tryDecoded.data.QlistName != "") {
                            questionFieldId.setText(tryDecoded.data.Question);
                            NameCCLId.setText(tryDecoded.data.QlistName);
                        }
                        else {
                            NameCCLId.setText('Проверка закончена');
                            questionFieldId.setText('Нажмите кнопку "Закрыть"');
                            var btnExamNextQuestion = clistAnswersWindow.down('#btnExamNextQuestion');
                            var btnExamClose = clistAnswersWindow.down('#btnExamClose');
                            btnExamNextQuestion.hide();
                            btnExamClose.show();
                        }
                    }).error(function (err) {
                        //asp.unmask();
                        Ext.Msg.alert('Ошибка', err.message);
                    });
                }

                


            },

            closeExamForm: function (btn) {
                var me = this;
                var window = btn.up('controllistwindow');
                if (window.task) window.task.destroy();
                window.destroy();

                B4.Ajax.request({
                    url: B4.Url.action('PrintReport', 'ActCheckProvidedDoc'),
                    params: {
                        documentId: me.controller.params.documentId,                      
                    }
                }).next(function (resp) {
                   
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });


                Ext.Msg.alert('Внимание!', 'Результаты проверочного листа размещены на вкладке "Приложения"');

            },
           
            onLoadTest: function (btn) {
                var me = this;
                var clistAnswersWindow = Ext.create('B4.view.actcheck.ControlListTestWindow');
                B4.Ajax.request({
                    url: B4.Url.action('GetNextQuestion', 'ActCheckProvidedDoc'),
                    params: {
                        documentId: me.controller.params.documentId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }

                    var questionFieldId = clistAnswersWindow.down('#questionFieldId');
                    var NameCCLId = clistAnswersWindow.down('#NameCCLId');
                   // radGrp.removeAll();
                    clistAnswersWindow.qid = tryDecoded.data.qid;;
                    if (tryDecoded.data.Question != "" && tryDecoded.data.QlistName != "") {                       
                        questionFieldId.setText(tryDecoded.data.Question);
                        NameCCLId.setText(tryDecoded.data.QlistName);
                    }
                    else {
                        NameCCLId.setText('Проверка закончена');
                        questionFieldId.setText('Нажмите кнопку "Закрыть"');
                        var btnExamNextQuestion = clistAnswersWindow.down('#btnExamNextQuestion');
                        var btnExamClose = clistAnswersWindow.down('#btnExamClose');
                        btnExamNextQuestion.hide();
                        btnExamClose.show();
                    }
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                });
                clistAnswersWindow.show();


            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddCTListAnswers', 'ActCheckProvidedDoc', {
                            ctlistIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать проверочный лист');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckRoBlobTextAspect',
            fieldSelector: '#taDescription',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ActCheckRealityObject',
            valueFieldName: 'Description',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: 'Description',
            getParentPanel: function () {
                return this.componentQuery('#actCheckRealityObjectEditPanel');
            },
            getParentRecordId: function () {
                return this.controller.currentRoId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckNotRevealedViolationsAspect',
            fieldSelector: '#taNotRevealedViolations',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ActCheckRealityObject',
            getAction: 'GetNotRevealedViolations',
            saveAction: 'SaveNotRevealedViolations',
            valueFieldName: 'NotRevealedViolations',
            previewLength: 200,
            autoSavePreview: true,
            previewField: 'NotRevealedViolations',
            getParentPanel: function () {
                return this.componentQuery('#actCheckRealityObjectEditPanel');
            },
            getParentRecordId: function () {
                return this.controller.currentRoId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckRoEditWindowBlobTextAspect',
            fieldSelector: '#taDescription',
            editPanelAspectName: 'actCheckRealityObjectAspect',
            controllerName: 'ActCheckRealityObject',
            valueFieldName: 'Description',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            getParentRecordId: function () {
                return this.controller.currentRoId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckRoAdditionalCharsEditWindowBlobTextAspect',
            fieldSelector: 'textarea[name=AdditionalChars]',
            editPanelAspectName: 'actCheckRealityObjectAspect',
            controllerName: 'ActCheckRealityObject',
            valueFieldName: 'AdditionalChars',
            getAction: 'GetAdditionalChars',
            saveAction: 'SaveAdditionalChars',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            getParentRecordId: function () {
                return this.controller.currentRoId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckPersonViolationInfoEditWindowBlobTextAspect',
            fieldSelector: 'actcheckinspectionresultpanel textarea[name=PersonViolationInfo]',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ChelyabinskDocumentGji',
            valueFieldName: 'PersonViolationInfo',
            getAction: 'GetPersonViolationInfo',
            saveAction: 'SavePersonViolationInfo',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            injectionEvent: 'aftersetpaneldata',
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckPersonViolationActionInfoEditWindowBlobTextAspect',
            fieldSelector: 'actcheckinspectionresultpanel textarea[name=PersonViolationActionInfo]',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ChelyabinskDocumentGji',
            valueFieldName: 'PersonViolationActionInfo',
            getAction: 'GetPersonViolationActionInfo',
            saveAction: 'SavePersonViolationActionInfo',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            injectionEvent: 'aftersetpaneldata',
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckViolationDescriptionEditWindowBlobTextAspect',
            fieldSelector: 'actcheckinspectionresultpanel textarea[name=ViolationDescription]',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ChelyabinskDocumentGji',
            valueFieldName: 'ViolationDescription',
            getAction: 'GetViolationDescription',
            saveAction: 'SaveViolationDescription',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            injectionEvent: 'aftersetpaneldata',
            getParentRecordId: function () {
                return this.controller.params.documentId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckEditViolationEditWindowBlobTextAspect',
            fieldSelector: 'textarea[name=Description]',
            editPanelAspectName: 'actCheckEditViolationAspect',
            controllerName: 'ActCheckViolation',
            valueFieldName: 'Description',
            getAction: 'GetDescription',
            saveAction: 'SaveDescription',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            getParentRecordId: function () {
                return this.controller.currentViolId;
            }
        },
        {
            /*
           Переводим грид на мультиселект с инлайн редактированием
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actcheckControlMeasuresAspect',
            modelName: 'actcheck.ControlMeasures',
            storeName: 'actcheck.ControlMeasures',
            gridSelector: 'actcheckcontrolmeasuresgrid',
            saveButtonSelector: 'actcheckcontrolmeasuresgrid #actMeasuresGridSaveButton',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actcheckControlMeasuresSelectWindow',
            storeSelect: 'dict.ControlActivityGji',
            storeSelected: 'dict.ControlActivityGji',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор мероприятий по контролю',
            titleGridSelect: 'Мероприятия по контролю для отбора',
            titleGridSelected: 'Выбранные мероприятия по контролю',

            //onBeforeLoad: function (store, operation) {
            //    var me = this;
            //    if (me.controller.params) {
            //        operation.params.documentId = me.controller.params.kindCheckId;
            //    }
            //},

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddActCheckControlMeasures', 'ActCheck', {
                        controlMeasuresIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Мероприятия по контролю сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
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
        me.getStore('actcheck.ControlListAnswer').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Annex').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.Period').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.InspectedPart').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.ProvidedDoc').on('beforeload', me.onObjectBeforeLoad, me);
        me.getStore('actcheck.ControlMeasures').on('beforeload', me.onBeforeLoad, me);

        me.control({
            '#actCheckEditPanel #btnMerge': { click: { fn: me.mergeActs, scope: me } }
        });

        me.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId)
            operation.params.documentId = this.params.documentId;
    },

    onLaunch: function () {
        var me = this;
      //  me.setDisplayTimeField('#tfDocumentTime');
        
        if (me.params) {
            me.getAspect('actCheckEditPanelAspect').setData(me.params.documentId);
        }
    },

    onObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.params.documentId;
        }
    },

    onViolationBeforeLoad: function (store, operation) {
        var me = this;
        
        if (me.currentRoId > 0) {
            operation.params.objectId = me.currentRoId;
        } else if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.documentId;
        }
    },

    setCurrentRoId: function (id) {
        var me = this;
        
        me.currentRoId = id;
        me.getStore('actcheck.Violation').load();
    },
    
    isAnyHasViolation: function() {
        var me = this;
        
        B4.Ajax.request(B4.Url.action('IsAnyHasViolation', 'ChelyabinskActCheck', {
            actId: me.documentId
        })).next(function (resp) {
            var obj = Ext.decode(resp.responseText);
            me.disableViolationInfo(!obj.success);
        }).error(function (error) {
            me.disableViolationInfo(true);
            Ext.Msg.alert('Ошибка', error.message || error.message || error);
        });
    },

    disableViolationInfo: function(isDisabled) {
        var me = this,
            mainView = me.getMainView(),
            field1 = mainView.down('textarea[name=PersonViolationInfo]'),
            field2 = mainView.down('textarea[name=PersonViolationActionInfo]'),
            field3 = mainView.down('textarea[name=ViolationDescription]');

        field1.setDisabled(isDisabled);
        field2.setDisabled(isDisabled);
        field3.setDisabled(isDisabled);
    },

    mergeActs: function(btn) {
        var me = this,
            panel = btn.up('actCheckEditPanel');

        me.mask('Объединение актов...', panel);

        B4.Ajax.request({
            url: B4.Url.action('MergeActs', 'ActCheck'),
            timeout: 9999999,
            params: {
                documentId: me.params.documentId
            }
        }).next(function (resp) {
            var obj = Ext.decode(resp.responseText);
            var editPanelAspect = me.getAspect('actCheckEditPanelAspect');

            editPanelAspect.setData(me.params.documentId);
            editPanelAspect.reloadTreePanel();

            if (obj && obj.message) {
                B4.QuickMsg.msg('Объединение актов', obj.message, 'success');
            }

            me.unmask();
        }).error(function (error) {
            me.unmask();
            Ext.Msg.alert('Ошибка', error.message || error.message || error);
        });
    },
    setDisplayTimeField: function (selectorTimeField) {
        var timeField = Ext.ComponentQuery.query(selectorTimeField)[0];
        if (timeField && timeField.getStore()) {
            var store = timeField.getStore(),
            format = timeField.format,
            altFormat = timeField.altFormats;

            //На вкладке Реквизиты в поле "Время составления Акта" на выбор пользователю в выпадающем списке предоставлять только полные часы.
            //Список должен содержать только часы, которые входят в рабочий день, исключая обед.
            //store.filterBy(function (record) {
            //    var date = Ext.Date.parse(record.get('disp'), format),
            //        min = Ext.Date.parse(Gkh.config.HousingInspection.SettingsOfTheDay.BeginningOfTheDay, altFormat),
            //        max = Ext.Date.parse(Gkh.config.HousingInspection.SettingsOfTheDay.EndOfTheDay, altFormat),
            //        startLaunch = Ext.Date.parse(Gkh.config.HousingInspection.SettingsOfTheDay.BeginLunchtime, altFormat),
            //        endLaunch = Ext.Date.parse(Gkh.config.HousingInspection.SettingsOfTheDay.EndLunchtime, altFormat);

            //    return date >= min && date <= max && !(date >= startLaunch && date < endLaunch);
            //});
    }
    },

});