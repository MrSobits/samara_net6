Ext.define('B4.controller.basedisphead.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.BaseDispHead',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Disposal',
        'BaseDispHead',
        'RealityObjectGji'
    ],

    stores: [
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.Municipality',
        'ManagingOrganization',
        'basedisphead.RealityObject',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'basedisphead.EditPanel',
        'basedisphead.RealityObjectGrid',
        'inspectiongji.RiskPrevWindow',
        'SelectWindow.MultiSelectWindow',
        'basedisphead.ContragentGrid'
    ],

    mainView: 'basedisphead.EditPanel',
    mainViewSelector: '#baseDispHeadEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseDispHeadEditPanelSelector: '#baseDispHeadEditPanel',

    aspects: [
        {
            xtype: 'requirementaspect',
            name: 'requirementAspect',
            applyOn: { event: 'show', selector: '#baseDispHeadEditPanel' },
            requirements: [
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentName',
                    applyTo: '[name=DocumentName]',
                    selector: '#baseDispHeadEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentNumber',
                    applyTo: '[name=DocumentNumber]',
                    selector: '#baseDispHeadEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.DocumentDate',
                    applyTo: '[name=DocumentDate]',
                    selector: '#baseDispHeadEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.CheckDate',
                    applyTo: '[name=CheckDate]',
                    selector: '#baseDispHeadEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.InnOfficial',
                    applyTo: '[name=Inn]',
                    selector: '#baseDispHeadEditPanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.Official);
                    }
                },
                {
                    name: 'GkhGji.Inspection.BaseDispHead.MainInfo.Field.InnIndividual',
                    applyTo: '[name=Inn]',
                    selector: '#baseDispHeadEditPanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.PhysPerson);
                    }
                }
            ],
            checkInnRequires: function(component, required, objectType){
                if (component) {
                    var inspectionObjectType = component.up('#baseDispHeadEditPanel').down('[name=PersonInspection]').getValue();

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
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseDispHeadCreateButtonAspect',
            buttonSelector: '#baseDispHeadEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseDispHeadEditPanel',
            typeBase: 40, // Тип проверка по поручению руководства
            onValidateUserParams: function () {
                var check = this.controller.getBasedispheadRealityObjectStore().data.length > 0;
                if (check) {
                    return true;
                }
                Ext.Msg.alert('Ошибка формирования документа', 'Отсуствуют проверяемые дома');
                return false;
            }
        },
        {
            /*
            * аспект прав доступа
            */
            xtype: 'basedispheadperm',
            editFormAspectName: 'baseDispHeadEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseDispHeadStateButtonAspect',
            stateButtonSelector: '#baseDispHeadEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseDispHeadEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели проверки по поручению руководства
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseDispHeadEditPanelAspect',
            editPanelSelector: '#baseDispHeadEditPanel',
            modelName: 'BaseDispHead',
            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #dispHeadPrevDocumentSelectField'] = {
                    'beforeload': { fn: me.onPrevDocumentBeforeLoad, scope: me },
                    'change': { fn: me.onPrevDocumentChange, scope: me }
                };

                actions[me.editPanelSelector + ' #cbTypeBase'] = { 'change': { fn: me.onTypeBaseChange, scope: me } };
                actions[me.editPanelSelector + ' #cbPersonInspection'] = { 'change': { fn: me.onChangePerson, scope: me } };
                actions[me.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };
                actions[me.editPanelSelector + ' #sflHead'] = { 'beforeload': { fn: me.onBeforeLoadHead, scope: me } };
                actions[me.editPanelSelector + ' #dispHeadPrevDocumentSelectField'] = { 'beforeload': { fn: me.onBeforeLoadPrevDocs, scope: me } };
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
            onBeforeLoadPrevDocs: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.listForInspection = true;
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.typeJurOrg = this.getPanel().down('#cbTypeJurPerson').getValue();
            },
            onBeforeLoadHead: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.headOnly = true;
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
            getDisposal: function () {
                /*
                перекрываем метод формирования распоряжения потому что
                если предыдущий документ не выбран, то формируем обычное распоряжение
                если предыдущий документ выбран то формируем
                */

                var model = this.controller.getModel('Disposal');
                var disp = new model({ Id: 0 });
                disp.set('Inspection', this.controller.params.inspectionId);
                disp.set('TypeDocumentGji', 10);

                var record = this.getRecord();

                if (record.get('PrevDocument')) {
                    disp.set('TypeDocumentGji', 20);
                    disp.set('ParentDocumentsList', []);

                    var prev = record.get('PrevDocument');
                    if (typeof prev == 'object') {
                        disp.data.ParentDocumentsList.push(prev.Id);
                    } else {
                        disp.data.ParentDocumentsList.push(prev);
                    }
                }
                else {
                    disp.set('TypeDocumentGji', 10);
                    disp.set('ParentDocumentsList', []);
                }

                return disp;
            },

            onTypeBaseChange: function (field, newValue) {
                this.getPanel().down('#dispHeadPrevDocumentSelectField').setDisabled(newValue === 10);
            },

            onPrevDocumentChange: function (field, data) {
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

            onPrevDocumentBeforeLoad: function (field, options) {
                options = options || {};
                options.params = {};

                var typeBase = this.getPanel().down('#cbTypeBase').getValue();

                if (typeBase === 10) {
                    options.params.prescriptionsForDispHeadId = 0;
                }
                else if (typeBase === 20) {
                    options.params.prescriptionsForDispHeadId = this.controller.params.inspectionId;
                }
                else if (typeBase === 30) {
                    options.params.protocolsForDispHeadId = this.controller.params.inspectionId;
                }

                options.params.prescriptionsForDispHead = 80;

                return true;
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
                    
                    //Делаем запросы на получение Инспекторов
                    //и обновляем соответсвующие Тригер филды
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseDispHead', {
                        inspectionId: asp.controller.params.inspectionId
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var fieldInspectors = panel.down('#trfInspectors');
                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    //Обновляем Статучы на панели
                    asp.controller.getAspect('baseDispHeadStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //Обновляем кнопку Сформировать
                    asp.controller.getAspect('baseDispHeadCreateButtonAspect').setData(rec.get('Id'));
                    //Обновляем обязательность полей
                    asp.controller.getAspect('requirementAspect').onAfterRender();
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'baseDispHeadRealityObjectAspect',
            gridSelector: '#baseDispHeadRealityObjectGrid',
            storeName: 'basedisphead.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseDispHeadRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation) {
                var panel = Ext.ComponentQuery.query(this.controller.baseDispHeadEditPanelSelector)[0];
                operation.params.contragentId = panel.down('#sfContragent').getValue();
                operation.params.date = panel.down('#dfDispHeadDate').getValue();
                operation.params.typeJurPerson = panel.down('#cbTypeJurPerson').getValue();
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
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /DisposalGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseDispHeadInspectorMultiSelectWindowAspect',
            fieldSelector: '#baseDispHeadEditPanel #trfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseDispHeadInspectorsSelectWindow',
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

                    records.each(function(rec) {
                         recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'InspectionGjiInspector'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        }
                    }).next(function () {
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
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'basedispheadcontragentaspect',
            gridSelector: '#baseDispHeadEditPanel basedispheadcontragentgrid',
            storeName: 'inspectiongji.InspectionBaseContragent',
            modelName: 'inspectiongji.InspectionBaseContragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseDispHeadInspectionContragentMultiSelectWindow',
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

        actions['basedispheadcontragentgrid'] = { 'basedispheadstore.beforeload': { fn: me.onBeforeLoad, scope: me } };
        me.control(actions);

        me.getStore('basedisphead.RealityObject').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseDispHeadEditPanelAspect').setData(this.params.inspectionId);

            var mainView = this.getMainComponent();
            if (mainView) {
                mainView.setTitle(this.params.title);
                mainView.down('basedispheadcontragentgrid').getStore().load();
            }

            //Обновляем список домов в проверке
            this.getStore('basedisphead.RealityObject').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    }
});