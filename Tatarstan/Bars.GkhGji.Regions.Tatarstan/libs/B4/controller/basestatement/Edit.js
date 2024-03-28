Ext.define('B4.controller.basestatement.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.BaseStatement',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.enums.BaseStatementRequestType'
    ],

    models: [
        'Disposal',
        'BaseStatement',
        'RealityObjectGji'
    ],

    stores: [
        'basestatement.RealityObject',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'motivationconclusion.ForBaseStatement'
    ],

    views: [
        'basestatement.RealityObjectGrid',
        'basestatement.EditPanel',
        'inspectiongji.RiskPrevWindow',
        'SelectWindow.MultiSelectWindow',
        'basestatement.ContragentGrid'
    ],

    mainView: 'basestatement.EditPanel',
    mainViewSelector: '#baseStatementEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseStatementEditPanelSelector: '#baseStatementEditPanel',

    aspects: [
        {
            /*
            Аспект формирвоания документов для данного основания по обращению
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseStatementCreateButtonAspect',
            buttonSelector: '#baseStatementEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseStatementEditPanel',
            typeBase: 20 // Тип проверка обращения
        },
        {
            xtype: 'basestatementperm',
            editFormAspectName: 'baseStatementEditPanelAspect'
        },
        {
            xtype: 'requirementaspect',
            name: 'requirementAspect',
            applyOn: { event: 'show', selector: '#baseStatementEditPanel' },
            requirements: [
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.CheckDate',
                    applyTo: '[name=CheckDate]',
                    selector: '#baseStatementEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseStatement.MainInfo.Field.InnOfficial',
                    applyTo: '[name=Inn]',
                    selector: '#baseStatementEditPanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.Official);
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseStatement.MainInfo.Field.InnIndividual',
                    applyTo: '[name=Inn]',
                    selector: '#baseStatementEditPanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.PhysPerson);
                    }
                }
            ],
            checkInnRequires: function(component, required, objectType){
                if (component) {
                    var inspectionObjectType = component.up('#baseStatementEditPanel').down('[name=PersonInspection]').getValue();

                    if(inspectionObjectType == objectType) {
                        component.allowBlank = !required;

                        if (Ext.isEmpty(component.getValue())) {

                            if (!component.allowBlank) {
                                component.markInvalid();
                            } else {
                                component.clearInvalid();
                            }
                        }
                    }
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseStatementStateButtonAspect',
            stateButtonSelector: '#baseStatementEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseStatementEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели Проверки по обращению граждан
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseStatementEditPanelAspect',
            editPanelSelector: '#baseStatementEditPanel',
            modelName: 'BaseStatement',
            otherActions: function (actions) {
                var me = this;

                //В данном методе добавляем дополнительные обработчики для получения Должностей у SelectField Инспекторов
                actions[me.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: me.onPersonInspectionChange, scope: me } };
                actions[me.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
                actions[me.editPanelSelector + ' #trigfAppealCitizens'] = { 'triggerClear': { fn: me.onTriggerClearAppealCitizens, scope: me } };

                actions[me.editPanelSelector + ' #btnDelete'] = { 'click': { fn: me.btnDeleteClick, scope: me } };
                actions[me.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: me.onChangePerson, scope: me } };
                actions[me.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
                actions[me.editPanelSelector + ' #cbTypeJurPerson'] = { 'change': { fn: me.onChangeType, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategory]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
                actions[me.editPanelSelector + ' [name=RiskCategoryStartDate]'] = { change: { fn: me.onChangeRiskCategoryData, scope: me } };
                actions[me.editPanelSelector + ' [name=AllCategory]'] = {
                    click: {
                        fn: function () {
                            var record = me.getRecord(),
                                contragentId = record.get('Contragent');

                            Ext.History.add(Ext.String.format('contragentedit/{0}/risk', contragentId));
                        },
                        scope: me
                    }
                };
                actions[me.editPanelSelector + ' [name=PrevCategory]'] = {
                    click: {
                        fn: function () {
                            var prevWindow = Ext.create('B4.view.inspectiongji.RiskPrevWindow', {
                                renderTo: B4.getBody().getActiveTab().getEl(),
                                inspectionId: me.controller.params.inspectionId
                            });

                            prevWindow.on('beforeclose', function (win) {
                                if (win.saved) {
                                    me.setData(me.controller.params.inspectionId);
                                }
                            });

                            prevWindow.show();
                        },
                        scope: me
                    }
                };
            },
            onChangeType: function (field, newValue, oldValue) {
                this.controller.params = this.controller.params || {};
                
                if (oldValue != null && newValue !== oldValue) {
                    this.getPanel().down('#sfContragent').setValue(null);
                }
                
                this.controller.params.typeJurOrg = newValue;
            },
            onChangePerson: function (field, newValue) {
                var panel = this.getPanel(),
                    sfContragent = panel.down('#sfContragent'),
                    tfPhysicalPerson = panel.down('#tfPhysicalPerson'),
                    cbTypeJurPerson = panel.down('#cbTypeJurPerson'),
                    innField = panel.down('[name=Inn]');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.hide();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.hide();
                        innField.setDisabled(false);
                        innField.show();
                        break;
                    case 20://организация
                        sfContragent.show();
                        tfPhysicalPerson.hide();
                        cbTypeJurPerson.show();
                        innField.setDisabled(true);
                        innField.hide();
                        break;
                    case 30://должностное лицо
                        sfContragent.show();
                        tfPhysicalPerson.show();
                        cbTypeJurPerson.show();
                        innField.setDisabled(false);
                        innField.show();
                        break;
                }
            },

            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },

            onSaveSuccess: function (asp, record) {
                asp.controller.setInspectionId(record.get('Id'));
            },

            onTriggerClearAppealCitizens: function () {
                this.controller.mask('Сохранение', this.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'BaseStatement', {
                    objectIds: '',
                    inspectionId: this.controller.params.inspectionId
                })).next(function () {
                    this.controller.unmask();
                    Ext.Msg.alert('Сохранение!', 'Обращения граждан сохранены успешно');

                    this.controller.getStore('basestatement.RealityObject').load();
                    return true;
                }, this).error(function () {
                    this.controller.unmask();
                }, this);
            },

            /**
             * Обработка изменения полей "Категория" и "Дата начала" категории
             * @param {any} field Поле категории или даты
             */
            onChangeRiskCategoryData: function (field) {
                var riskCategory = field.up().down('[name=RiskCategory]'),
                    riskCategoryStartDate = field.up().down('[name=RiskCategoryStartDate]'),
                    allowBlank = Ext.isEmpty(riskCategory.getValue()) && Ext.isEmpty(riskCategoryStartDate.getValue());

                riskCategory.allowBlank = allowBlank;
                riskCategoryStartDate.allowBlank = allowBlank;

                riskCategory.validate();
                riskCategoryStartDate.validate();
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var statementId = rec.get('Id'),
                        requestType = rec.get('RequestType'),
                        fieldAppealCitizens = panel.down('#trigfAppealCitizens'),
                        motivationConclusions = panel.down('field[name=motivationConclusions]'),
                        isMotivationConclusion = requestType === B4.enums.BaseStatementRequestType.MotivationConclusion;
                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }

                    if (rec.get('Contragent')) {
                        panel.down('[name=RiskSet]').show();
                    } else {
                        panel.down('[name=RiskSet]').hide();
                    }
                    
                    asp.controller.setInspectionId(statementId);

                    fieldAppealCitizens.setDisabled(isMotivationConclusion);
                    fieldAppealCitizens.setVisible(!isMotivationConclusion);
                    motivationConclusions.setDisabled(!isMotivationConclusion);
                    motivationConclusions.setVisible(isMotivationConclusion);

                    //Делаем запросы на получение Обращения граждан и обновляем соответсвующий Тригер филд
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseStatement', {
                        inspectionId: statementId,
                        requestType: requestType
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        if (requestType === B4.enums.BaseStatementRequestType.MotivationConclusion) {
                            motivationConclusions.updateDisplayedText(obj.documentNumbers);
                            motivationConclusions.setValue(obj.documentIds);
                        } else {
                            fieldAppealCitizens.updateDisplayedText(obj.appealCitizensNames);
                            fieldAppealCitizens.setValue(obj.appealCitizensIds);
                        }
                    }).error(function () {
                        asp.controller.unmask();
                    });
                    
                    //Обновляем статусы
                    asp.controller.getAspect('baseStatementStateButtonAspect').setStateData(statementId, rec.get('State'));
                    //Обновляем кнопку Сформировать
                    asp.controller.getAspect('baseStatementCreateButtonAspect').setData(statementId);
                    //Обновляем обязательность полей
                    asp.controller.getAspect('requirementAspect').onAfterRender();
                }
            },
            btnDeleteClick: function () {
                var panel = this.getPanel();
                var record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить проверку?', function (result) {
                    if (result == 'yes') {
                        Ext.Msg.confirm('Удаление записи!',
                            'При удалении данной записи произойдет удаление всех связанных объектов. Продолжить удаление?',
                            function (result) {
                                if (result == 'yes') {
                                    this.mask('Удаление', B4.getBody());
                                    record.destroy()
                                        .next(function (result) {
                                            //Обновляем дерево меню
                                            this.unmask();
                                            var tree =
                                                Ext.ComponentQuery.query(this.controller.params.treeMenuSelector)[0];
                                            tree.getStore().load();
                                            panel.close();
                                        }, this)
                                        .error(function (result) {
                                            Ext.Msg.alert('Ошибка удаления!',
                                                Ext.isString(result.responseData)
                                                ? result.responseData
                                                : result.responseData.message);
                                            this.unmask();
                                        }, this);
                                }
                            }, this)
                    }
                }, this);
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля Обращения граждан с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /StatementGJI/AddAppealCitizens
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'appealCitizensMultiSelectWindowAspect',
            fieldSelector: '#baseStatementEditPanel #trigfAppealCitizens',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата документа', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DateFrom', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Количество вопросов', xtype: 'gridcolumn', dataIndex: 'QuestionsCount', flex: 1, filter: { xtype: 'numberfield', hideTrigger: true, operand: CondExpr.operands.eq } }
            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'Number', flex: 1, sortable: false },
                { header: 'Номер ГЖИ', xtype: 'gridcolumn', dataIndex: 'NumberGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор обращения граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        field = asp.getSelectField(),
                        val = field.getValue(),
                        rawval = field.getRawValue();

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddAppealCitizens', 'BaseStatement', {
                            objectIds: recordIds,
                            inspectionId: asp.controller.params.inspectionId
                        })).next(function () {
                            asp.controller.unmask();  
                            Ext.Msg.alert('Сохранение!', 'Обращения граждан сохранены успешно');
                            asp.controller.getStore('basestatement.RealityObject').load();
                            return true;
                        }).error(function (response) {
                            asp.controller.unmask();
                            field.updateDisplayedText(rawval);
                            field.setValue(val);
                            Ext.Msg.alert('Ошибка!', response.message);
                            return false;
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'motivationConclusionMultiSelectWindowAspect',
            fieldSelector: '#baseStatementEditPanel field[name=motivationConclusions]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementMotivConclSelectWindow',
            storeSelect: 'motivationconclusion.ForBaseStatement',
            textProperty: 'DocumentNumber',
            columnsGridSelect: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата', xtype: 'datecolumn', format: 'd.m.Y', dataIndex: 'DocumentDate', flex: 1, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Управляющая организация', xtype: 'gridcolumn', dataIndex: 'ManagingOrganization', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Номер документа', xtype: 'gridcolumn', dataIndex: 'DocumentNumber', flex: 1, sortable: false },
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'DocumentNum', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор мотивировочного заключения',
            titleGridSelect: 'Мотивировочные заключения для выбора',
            titleGridSelected: 'Мотивировочные заключения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [],
                        field = asp.getSelectField(),
                        val = field.getValue(),
                        rawval = field.getRawValue();

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddBasisDocs', 'BaseStatement', {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        })).next(function () {
                            asp.controller.unmask();  
                            Ext.Msg.alert('Сохранение', 'Мотивировочные заключения сохранены успешно');
                            //asp.controller.getStore('basestatement.RealityObject').load();
                            return true;
                        }).error(function (response) {
                            field.updateDisplayedText(rawval);
                            field.setValue(val);
                            Ext.Msg.alert('Ошибка', response.message);
                            asp.controller.unmask();
                            return false;
                        });
                    } else {
                        Ext.Msg.alert('Ошибка', 'Необходимо мотивировочные заключения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'baseStatementRealityObjectAspect',
            gridSelector: '#baseStatementRealityObjectGrid',
            storeName: 'basestatement.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
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
                var panel = Ext.ComponentQuery.query(this.controller.baseStatementEditPanelSelector)[0];
                operation.params.typeJurPerson = panel.down('#cbTypeJurPerson').getValue();
                operation.params.contragentId = panel.down('#sfContragent').getValue();
                operation.params.isPhysicalPerson = panel.down('#cbPersonInspection').getValue() === 10 ? true : false;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddRealityObjects', 'InspectionGjiRealityObject'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
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
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspectionContragentAspect',
            gridSelector: '#baseStatementEditPanel basestatementcontragentgrid',
            storeName: 'inspectiongji.InspectionBaseContragent',
            modelName: 'inspectiongji.InspectionBaseContragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseStatementInspectionContragentMultiSelectWindow',
            storeSelect: 'Contragent',
            storeSelected: 'Contragent',
            titleSelectWindow: 'Выбор органов проверки',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Id',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddContragents', 'InspectionBaseContragent'),
                            method: 'POST',
                            params: {
                                contragentIds: Ext.encode(recordIds),
                                inspectionId: asp.controller.params.inspectionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать контрагентов');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['basestatementcontragentgrid'] = { 'basestatementstore.beforeload': { fn: me.onBeforeLoad, scope: me } };
        me.control(actions);

        me.getStore('basestatement.RealityObject').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseStatementEditPanelAspect').setData(this.params.inspectionId);

            var mainView = this.getMainComponent();
            if (mainView) {
                mainView.setTitle(this.params.title);
                mainView.down('basestatementcontragentgrid').getStore().load();
            }
        }
        this.getStore('basestatement.RealityObject').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.inspectionId > 0)
            operation.params.inspectionId = this.params.inspectionId;
    },

    setInspectionId: function (id) {
        if (this.params) {
            this.params.inspectionId = id;
        }
    }
});