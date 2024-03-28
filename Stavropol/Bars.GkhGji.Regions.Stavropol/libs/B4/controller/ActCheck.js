Ext.define('B4.controller.ActCheck', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
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
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet'
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
        'actcheck.Definition',
        'actcheck.InspectedPart',
        'actcheck.RealityObject',
        'actcheck.ViolationForSelect',
        'actcheck.ViolationForSelected',
        'prescription.ForSelect',
        'prescription.ForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected'
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
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'actcheck.EditPanel',
    mainViewSelector: '#actCheckEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
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
                if (params.ruleId == 'ActCheckToDisposalRule'
                    || params.ruleId == 'ActCheckToProtocolRule'
                    || params.ruleId == 'ActCheckToPrescriptionRule') {
                    return false;
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
                { name: 'GkhGji.DocumentReestrGji.ActCheck.Field.Area_Rqrd', applyTo: '#nfArea', selector: '#actCheckEditPanel' }
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
            xtype: 'gkhbuttonprintaspect',
            name: 'actCheckPrintAspect',
            buttonSelector: '#actCheckEditPanel #btnPrint',
            codeForm: 'ActCheck',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
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
                var me = this;

                actions[me.editPanelSelector + ' #cbToPros'] = { 'change': { fn: me.onToProsecutorChange, scope: me } };
                actions[me.editPanelSelector + ' #sfResolPros'] = {
                    'beforeload': { fn: me.onResProsBeforeLoad, scope: me },
                    'change': { fn: me.onResProsChange, scope: me }
                };

                actions[me.editPanelSelector + ' #actCheckRealityObjectEditPanel #cbHaveViolation'] = { 'change': { fn: me.changeHaveViolation, scope: me } };
            },
            saveRecord: function (rec) {
                // делаем запрос, так как  поле ActCheckGjiRealityObject  догоняется в get
                var model = this.getModel(rec);
                model.load(rec.getId(), {
                    success: function (result) {
                        // Дополение ддя Ставрополя: сохраняем "Время составления документа" 
                        this.saveActCheckCreateTime(rec.getId());

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
                //Нельзя добавлять если не 'да' блокируем кнопку добавления
                if(newValue != 10)
                    actViolationGridAddButton.setDisabled(true);
            },
            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    callbackUnMask,
                    addButton,
                    title;
                
                asp.controller.params= asp.controller.params || {};
                
                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                asp.controller.mask('Загрузка', panel);
                
                addButton = panel.down('#actCheckViolationGrid').down('b4addbutton');
                if (rec.get('ActCheckGjiRealityObject') && rec.get('ActCheckGjiRealityObject').HaveViolation == 10) {
                    addButton.setDisabled(true);
                } else {
                    addButton.setDisabled(false);
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
                    if (obj.typeBase == 50 && (!obj.prosClaimTypeBase || obj.prosClaimTypeBase == 10)) {
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
                
                //загружаем стор отчетов
                me.controller.getAspect('actCheckPrintAspect').loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                me.controller.getAspect('actCheckStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('actCheckCreateButtonAspect').setData(rec.get('Id'));
            },
            saveActCheckCreateTime: function (actCheckId) {
                var panel = this.getPanel(),
                    time = panel.down('textfield[name=CreationTime]').getValue(),
                    regExp = /^(?:(?:([01]?\d|2[0-3]):)?([0-5]?[0-9]))$/;
                if (regExp.test(time)) {
                    B4.Ajax.request(B4.Url.action('CreateActCheckTime', 'ActCheckTime', {
                        actCheckId: actCheckId,
                        creationtime: time
                    })).error(function () {
                        Ext.Msg.alert('Сохранение!', 'Возникла ошибка при сохранении значения "Время составления документа"');
                    });
                } else {
                    Ext.Msg.alert('Сохранение!', 'Значение поля "Время составления документа" не соответствует формату');
                }
            },
            //метод сохранения нарушений по дому
            saveRealityObjectViolation: function (actObjectId, haveViolation, description, violationStore, deferred) {
                var me = this;
                
                //Блокируем сохранение если не выполняется ряд условий
                if (violationStore.getCount() == 0 && haveViolation == 10) {
                    deferred.fail({ message: 'Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений' });
                    return false;
                }

                if (violationStore.getCount() != 0 && haveViolation != 10) {
                    deferred.fail({ message: 'Записи в таблице нарушений должны быть только если нарушения выявлены' });
                    return false;
                }

                //формируем записи нарушений из стора для последующей обработки на сервере
                var actCheckViolationRecords = [];
                Ext.Array.each(violationStore.getRange(0, violationStore.getCount()),
                    function(item) {
                        var data = item.getData();
                        actCheckViolationRecords.push(
                            {
                                Id: data.Id || 0,
                                ViolationGjiId: data.ViolationGjiId,
                                DatePlanRemoval: data.DatePlanRemoval
                            });
                    });

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
                var actViolationGridAddButton = this.getForm().down('#actViolationGridAddButton');

                actViolationGridAddButton.setDisabled(false);

                //Нельзя добавлять если не 'да' блокируем кнопку добавления
                if (newValue != 10) {
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

                    var numTxtFld = form.down('textfield[name=DocumentNum]'),
                        numberTxtFld = form.down('numberfield[name=DocumentNumber]');
                    if (!numTxtFld.isHidden() && !numberTxtFld.isHidden()) {
                        numTxtFld.hide();
                    }
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
        }
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

    setCurrentRoId: function (id) {
        var me = this;

        me.currentRoId = id;
        me.getStore('actcheck.Violation').load();
    }
});