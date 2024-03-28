Ext.define('B4.controller.ActCheck', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActCheck',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.YesNoNotSet',
        'B4.aspects.GkhBlobText'
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
        'violationgroup.ViolationGroup',
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
        'dict.InspectedPartGjiForSelected',
        'violationgroup.ViolationGroupForSelect',
        'violationgroup.ViolationGroupForSelected',
        'actcheck.ViolationGroup'
    ],

    views: [
        'actcheck.EditPanel',
        'actcheck.RealityObjectGrid',
        'actcheck.RealityObjectEditWindow',
        'actcheck.RealityObjectEditPanel',
        'actcheck.ViolationGrid',
        'actcheck.ViolationGroupEditWindow',
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
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            //Аспект кнопки печати акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actViewPrintAspect',
            buttonSelector: '#actCheckEditPanel #btnPrintActView',
            codeForm: 'ActView',
            getUserParams: function (reportId) {
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
            getUserParams: function (reportId) {
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
                actions[this.editPanelSelector + ' #sfResolPros'] = {
                    'beforeload': { fn: this.onResProsBeforeLoad, scope: this },
                    'change': { fn: this.onResProsChange, scope: this }
                };

                actions[this.editPanelSelector + ' #actCheckRealityObjectEditPanel #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this} };
            },
            
            saveRecord: function (rec) {
                // делаем запрос, так как  поле ActCheckGjiRealityObject  догоняется в get
                var model = this.getModel(rec);
                model.load(rec.getId(), {
                    success: function (result) {
                            //Если это акт на 1 дом то тогда сохраняем сначала Результаты проверки,
                            //а затем саму сущность
                        if (result.get('ActCheckGjiRealityObject') != null && rec.get('TypeActCheck') != 10) {
                                var recordObj = result.get('ActCheckGjiRealityObject'),
                                    panel = this.getPanel(),
                                    storeViolation = panel.down('#actCheckRealityObjectEditPanel #actCheckViolationGrid').getStore(),
                                    haveViolation = panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').getValue(),
                                    description = panel.down('#actCheckRealityObjectEditPanel #taDescription').getValue(),
                                    notRevViol = panel.down('#actCheckRealityObjectEditPanel #taNotRevealedViolations').getValue(),
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

                                this.saveRealityObjectViolation(recordObj.Id, haveViolation, description, storeViolation, deferred, notRevViol);
                            } else {
                                this.saveActCheck(rec);
                            }
                    },
                    scope: this
                });
            },

            saveActCheck: function (rec) {
                var me = this,
                    panel = me.getPanel(),
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
                var me = this,
                    panel = me.getPanel(),
                    violGridAddButton = panel.down('actCheckViolationGrid b4addbutton');

                violGridAddButton.setDisabled(false);
                // Нельзя добавлять если не 'да' блокируем кнопку добавления
                if (newValue != 10) {
                    violGridAddButton.setDisabled(true);
                }
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                
                var addButtonViol = panel.down('#actCheckViolationGrid').down('b4addbutton');
                if (rec.get('ActCheckGjiRealityObject') && rec.get('ActCheckGjiRealityObject').HaveViolation == 10) {
                    addButtonViol.setDisabled(true);
                } else {
                    addButtonViol.setDisabled(false);
                }
                
                var title = 'Акт проверки';
                
                if (rec.get('DocumentNumber'))
                    panel.setTitle(title + ' ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle(title);

                panel.down('#actCheckTabPanel').setActiveTab(0);

                panel.down('#btnPrintActView').hide();

                if (rec.get('TypeActCheck') == 30) {

                    //Если Акт проверки с типом = Акт проверки предписания то название вкладки меняем 
                    //То должны быть кнопки Акт Общий, Акт на 1 дом и Акт обследования
                    panel.down('#actCheckViolationTab').setTitle('Новые нарушения');
                    panel.down('#actCheckTabPanel').child('#actCheckActRemovalTab').tab.show();

                    asp.controller.getStore('actcheck.ActRemoval').load();

                    //запрос на получение пермишена на просмотр вкладки новые нарушения
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
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
                        asp.controller.unmask();
                    }).error(function() {
                        asp.controller.unmask();
                    });
                }
                else if (rec.get('TypeActCheck') == 40) {
                    if (rec.get('DocumentNumber'))
                        panel.setTitle('Акт осмотра' + ' ' + rec.get('DocumentNumber'));
                    else
                        panel.setTitle('Акт осмотра');
                    panel.down('#btnPrint').hide();
                    panel.down('#btnPrintActView').show();
                }
                else {
                    panel.down('#actCheckViolationTab').setTitle('Результаты проверки');
                    panel.down('#actCheckViolationTab').tab.show();
                    panel.down('#actCheckTabPanel').child('#actCheckActRemovalTab').tab.hide();
                }
                
                this.controller.setCurrentId(null, null);
                
                //Если акт на 1 дом то открываем панель и скрываем грид для Результатов проверки
                //Иначе скрываем панель и открываем грид Результатов проверки
                if (rec.get('ActCheckGjiRealityObject')) {
                    panel.down('#actCheckRealityObjectGrid').hide();
                    panel.down('#actCheckRealityObjectEditPanel').show();

                    if (rec.get('ActCheckGjiRealityObject') != null) {
                        var actRo = rec.get('ActCheckGjiRealityObject'),
                            objectId = actRo.Id,
                            roId = actRo.RealityObject != null && actRo.RealityObject > 0 ? actRo.RealityObject : 0;

                        this.controller.setCurrentId(objectId, roId);
                        
                        panel.down('#actCheckRealityObjectEditPanel #cbHaveViolation').setValue(actRo.HaveViolation);
                        panel.down('#actCheckRealityObjectEditPanel #taDescription').setValue(actRo.Description);
                        panel.down('#actCheckRealityObjectEditPanel #taNotRevealedViolations').setValue(actRo.NotRevealedViolations);
                        
                        this.controller.getAspect('actCheckRoBlobTextAspect').doInjection();
                    }
                } else {
                    panel.down('#actCheckRealityObjectGrid').show();
                    panel.down('#actCheckRealityObjectEditPanel').hide();

                    //Обновляем таблицу Проверяемых домов акта
                    this.controller.getStore('actcheck.RealityObject').load();
                }

                //Делаем запросы на получение Инспекторов
                //и обновляем соответсвующие Тригер филды
                var me = this;
                
                B4.Ajax.request(B4.Url.action('GetInfo', 'ActCheck', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var taRealityObjAddress = panel.down('#actCheckRealityObjectEditPanel #taRealityObjAddress');
                    if (obj.realityObjAddress) {
                        taRealityObjAddress.show();
                        taRealityObjAddress.setValue(obj.realityObjAddress);
                    } else {
                        taRealityObjAddress.hide();
                        taRealityObjAddress.setValue(null);
                    }
                    
                    var fieldInspectors = panel.down('#trigfInspectors');

                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldset = panel.down('#fsActResol');
                    if (obj.typeBase == 50) {
                        if (fieldset)
                            fieldset.show();
                    } else {
                        if (fieldset) {
                            fieldset.hide();
                        }
                    }

                    var actToPres = panel.down('[name = chbActToPres]');
                    if (actToPres) {
                        if (obj.typeCheck != 5) {
                            actToPres.hide();
                        } else {
                            actToPres.show();
                        }
                    }

                    me.disableButtons(false);
                    
                    panel.doLayout();
                    
                    asp.controller.unmask();
                }).error(function () {
                    asp.controller.unmask();
                });
                
                //загружаем стор отчетов
                this.controller.getAspect('actCheckPrintAspect').loadReportStore();

                //загружаем стор отчетов
                this.controller.getAspect('actViewPrintAspect').loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('actCheckStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));

                // обновляем кнопку Сформирвоать
                this.controller.getAspect('actCheckCreateButtonAspect').setData(rec.get('Id'));
            },

            /*
            * метод сохранения нарушений по дому
            */
            saveRealityObjectViolation: function (actObjectId, haveViolation, description, violationStore, deferred, notRevViol) {
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
                                DatePlanRemoval: data.DatePlanRemoval,
                                ViolationWording: data.ViolationWording
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
                        description: description,
                        notRevViol: notRevViol
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
            multiSelectWindowWidth: 1024,
            storeSelect: 'violationgroup.ViolationGroupForSelect',
            storeSelected: 'violationgroup.ViolationGroupForSelected',
            columnsGridSelect: [
                { header: 'Пункты нормативных документов', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 2, filter: { xtype: 'textfield' } },
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
                { header: 'Пункты', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор описаний',
            titleGridSelect: 'Описания для отбора',
            titleGridSelected: 'Выбранные описания',
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
                            listIds.push(item.get('Id'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем описания)
                        params = creationAspect.getParams(btn);
                        params.violationGroupIds = listIds;
                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать описания');
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
            storeSelect: 'violationgroup.ViolationGroupForSelect',
            storeSelected: 'violationgroup.ViolationGroupForSelected',
            columnsGridSelect: [
                { header: 'Пункты нормативных документов', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Описание', xtype: 'gridcolumn', dataIndex: 'Description', flex: 2, filter: { xtype: 'textfield' } },
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункты', xtype: 'gridcolumn', dataIndex: 'PointCodes', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор описаний',
            titleGridSelect: 'Описания для отбора',
            titleGridSelected: 'Выбранные описания',
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
                            listIds.push(item.get('Id'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCheckCreateButtonAspect');
                        // еще раз получаем параметры по умолчанию и добавляем к уже созданным еще один (Выбранные пользователем описания)
                        params = creationAspect.getParams(btn);
                        params.violationGroupIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать описания');
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
            Аспект взаимодействия домов акта и формы редактирования нарушений
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
                    var objectId = record.getId(),
                        roObj = record.get('RealityObject'),
                        roId = 0;

                    if (roObj != null && roObj > 0) {
                        roId = roObj;
                    }
                    else if (roObj != null && roObj.Id > 0) {
                        roId = roObj.Id;
                    }
                    
                    asp.controller.setCurrentId(objectId, roId);
                }
            },

            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this} };
            },

            changeHaveViolation: function (combobox, newValue) {
                var me = this,
                    form = me.getForm(),
                    violGridAddButton = form.down('actCheckViolationGrid b4addbutton');

                violGridAddButton.setDisabled(false);

                //Нельзя добавлять если не 'да' блокируем кнопку добавления
                if (newValue != 10) {
                    violGridAddButton.setDisabled(true);
                }
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после созранения
            },
            //переопределен метод сохранения. Сохраняется форма редактирования и таблица дочерних нарушений
            saveRecord: function (rec) {
                var me = this,
                    editWindow = me.getForm(),
                    storeViolation = editWindow.down('#actCheckViolationGrid').getStore(),
                    cbHaveViolation = editWindow.down('#cbHaveViolation').getValue(),
                    notRevViol = editWindow.down('#taNotRevealedViolations').getValue(),
                    description = editWindow.down('#taDescription').getValue(),
                    deferred = new Deferred();

                editWindow.setDisabled(true);

                deferred.next(function(res) {
                    editWindow.setDisabled(false);
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
                    .saveRealityObjectViolation(this.controller.objectId, cbHaveViolation, description, storeViolation, deferred, notRevViol);
            },
            closeWindowHandler: function () {
                this.getForm().close();
                this.updateGrid();
            }
        },
        {
            // Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            // При добавлении открывается форма массового выбора нарушений. После выбора список получается через подписку 
            // на событие getdata идет добавление записей в сторе
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actCheckViolationAspect',
            gridSelector: '#actCheckViolationGrid',
            saveButtonSelector: '#actCheckViolationGrid #actCheckViolationSaveButton',
            storeName: 'actcheck.Violation',
            modelName: 'actcheck.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckViolationMultiSelectWindow',
            multiSelectWindowWidth: 1024,
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Пункт нормативного документа', flex: 1,xtype: 'gridcolumn', dataIndex: 'CodePin', filter: { xtype: 'textfield' }, sortable: false },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }, sortable: false }
            ],
            columnsGridSelected: [
                { header: 'Пункт нормативного документа', xtype: 'gridcolumn', flex: 1, dataIndex: 'CodePin', sortable: false }
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
                                    ActObject: asp.controller.objectId,
                                    ViolationGjiPin: rec.get('CodePin'),
                                    ViolationGjiName: rec.get('Name'),
                                    ViolationGjiId: rec.get('Id'),
                                    ActionsRemovViolName: rec.get('ActRemViol'),
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
            /*
            * Аспект взаимодействия Таблицы Описаний с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actCheckViolGroupAspect',
            gridSelector: 'actCheckViolationGroupGrid',
            editFormSelector: 'actCheckViolationGroupEditWindow',
            storeName: 'actcheck.ViolationGroup',
            modelName: 'violationgroup.ViolationGroup',
            editWindowView: 'actcheck.ViolationGroupEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после созранения
            },
            closeWindowHandler: function () {
                this.getForm().close();
                this.updateGrid();
            },
            saveRecord: function (rec) {
                var me = this,
                    form = me.getForm(),
                    description = form.down('textarea[name=Description]').getValue(),
                    points = form.down('gkhtriggerfield[name=Points]').getValue();
                    
                me.controller.mask('Сохранение', form);
                B4.Ajax.request({
                    url: B4.Url.action('/DocumentViolGroup/SavePoints'),
                    method: 'POST',
                    params: {
                        documentId: me.controller.params.documentId,
                        roId: me.controller.roId,
                        description: description,
                        pointIds: points,
                        groupId: rec.get('Id')
                    }
                })
                .next(function(response) {
                    me.controller.unmask();
                    var data = Ext.decode(response.responseText).data;
                    if (!rec.get('Id')) {
                        me.setData(data.Id);
                    }
                })
                .error(function(e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка сохранения', e.message || e);
                });
                    
            },
            setData: function(id) {
                var me = this,
                    model = me.getModel();
                    
                model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                });
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    
                    var fieldPoints = form.down('gkhtriggerfield[name=Points]');

                    fieldPoints.updateDisplayedText(record.get('PointCodes'));
                    fieldPoints.setValue(record.get('PointIds'));
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля Пункты нарушений описания с массовой формой выбора
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actCheckPointsMultiSelectWindowAspect',
            fieldSelector: 'actCheckViolationGroupEditWindow gkhtriggerfield[name=Points]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckPointsSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            textProperty: 'ViolationGjiPin',
            columnsGridSelect: [
                { header: 'Пункт нормативного нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Пункт нормативного нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор пунктов нормативных документов',
            titleGridSelect: 'Пункты нормативных дкоументов для отбора',
            titleGridSelected: 'Выбранные пункты',
            onBeforeLoad: function (store, operation) {
                if (this.controller.objectId > 0) {
                    operation.params.objectId = this.controller.objectId;
                }
            },
            listeners: {
                getdata: function () {
                    // В этом методе ничег онеделаем поскольку сохранение пунктов выбраных идет после сохранения основной карточки
                    return true;
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckBlobDescriptionAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'actCheckViolGroupAspect',
            controllerName: 'DocumentViolGroup',
            valueFieldName: 'Description',
            previewLength: 150,
            autoSavePreview: true,
            previewField: 'Description'
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'actCheckRoBlobTextAspect',
            fieldSelector: '#taNotRevealedViolations',
            editPanelAspectName: 'actCheckEditPanelAspect',
            controllerName: 'ActCheckRealityObject',
            valueFieldName: 'NotRevealedViolations',
            previewLength: 1000,
            autoSavePreview: true,
            previewField: false,
            getParentPanel: function () {
                return this.componentQuery('#actCheckRealityObjectEditPanel');
            },

            getParentRecordId: function () {
                return this.controller.objectId;
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            fieldSelector: '#taNotRevealedViolations',
            editPanelAspectName: 'actCheckRealityObjectAspect',
            controllerName: 'ActCheckRealityObject',
            valueFieldName: 'NotRevealedViolations',
            previewField: false
        }
    ],

    init: function () {
        this.getStore('actcheck.RealityObject').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.Violation').on('beforeload', this.onViolationBeforeLoad, this);
        this.getStore('actcheck.ActRemoval').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.Witness').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.Definition').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.Annex').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.Period').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.InspectedPart').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actcheck.ViolationGroup').on('beforeload', this.onViolationGroupBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('actCheckEditPanelAspect').setData(this.params.documentId);

            //Обновляем таблицу Лиц присутсвующих при проверке
            this.getStore('actcheck.Witness').load();

            //Обновляем таблицу Дата и время проведения проверки
            this.getStore('actcheck.Period').load();

            //Обновляем таблицу Приложений
            this.getStore('actcheck.Annex').load();

            //Обновляем таблицу определений
            this.getStore('actcheck.Definition').load();

            //Обновляем таблицу инспектируемых частей
            this.getStore('actcheck.InspectedPart').load();
        }
    },

    onObjectBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    },

    onViolationBeforeLoad: function (store, operation) {
        if (this.objectId > 0)
            operation.params.objectId = this.objectId;
    },
    
    onViolationGroupBeforeLoad: function (store, operation) {
        operation.params.documentId = this.params.documentId;
        if (this.roId > 0)
            operation.params.roId = this.roId;
    },

    setCurrentId: function (id, roId) {
        var me = this,
            storeViol = me.getStore('actcheck.Violation'),
            storeViolGroup = me.getStore('actcheck.ViolationGroup');

        me.objectId = id;
        me.roId = roId;
        
        if (me.objectId > 0) {
            storeViol.load();
        } else {
            storeViol.removeAll();
        }
        
        if (me.roId > 0) {
            storeViolGroup.load();
        } else {
            storeViolGroup.removeAll();
        }
    }
});