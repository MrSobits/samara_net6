Ext.define('B4.controller.warninginspection.Edit', {
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
        'B4.aspects.permission.WarningInspection',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.InspectionCreationBasis'
    ],

    models: [
        'Disposal',
        'WarningInspection',
    ],

    stores: [
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.Municipality',
        'ManagingOrganization',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'warninginspection.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'warninginspection.EditPanel',
    mainViewSelector: '#warninginspectionEditPanel',

    refs: [
        { ref: 'mainView', selector: '#warninginspectionEditPanel' },
        { ref: 'realityObjectGrid', selector: '#warninginspectionEditPanel realityobjectgjigrid' },
        { ref: 'inspectionContragentGrid', selector: '#warninginspectionEditPanel inspectiongjicontragentgrid' }
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseDispHeadEditPanelSelector: '#warninginspectionEditPanel',

    disabledFields: [],

    aspects: [
        {
            xtype: 'requirementaspect',
            name: 'requirementAspect',
            requirements: [
                {
                    name: 'GkhGji.Inspection.WarningInspection.Field.InnOfficial',
                    applyTo: '[name=Inn]',
                    selector: 'warninginspectioneditpanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.Official);
                    }
                },
                {
                    name: 'GkhGji.Inspection.WarningInspection.Field.InnIndividual',
                    applyTo: '[name=Inn]',
                    selector: 'warninginspectioneditpanel',
                    applyBy: function (component, required) {
                        this.checkInnRequires(component, required, B4.enums.PersonInspection.PhysPerson);
                    }
                }
            ],
            checkInnRequires: function(component, required, objectType){
                if (component) {
                    var inspectionObjectType = component.up('warninginspectioneditpanel').down('[name=PersonInspection]').getValue();

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
            name: 'createButtonAspect',
            buttonSelector: '#warninginspectionEditPanel gjidocumentcreatebutton',
            containerSelector: '#warninginspectionEditPanel',
            typeBase: 41, // Тип предостережение
            onValidateUserParams: function (params) {
                var check = this.controller.getRealityObjectGrid().getStore().data.length > 0;
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих правил необходимы пользовательские параметры
                if (params.ruleId == 'GjiWarningToWarningDocRule') {
                    return false;
                }
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
            xtype: 'warninginspectionperm',
            editFormAspectName: 'editPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'stateButtonAspect',
            stateButtonSelector: '#warninginspectionEditPanel button[action=State]',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('editPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели проверки по поручению руководства
            */
            xtype: 'gjiinspectionaspect',
            name: 'editPanelAspect',
            editPanelSelector: '#warninginspectionEditPanel',
            modelName: 'WarningInspection',

            otherActions: function (actions) {
                var asp = this;
                actions[asp.editPanelSelector + ' [name=InspectionBasis]'] = { change: { fn: asp.onChangeInspectionBasis, scope: asp } };
                actions[asp.editPanelSelector + ' [name=PersonInspection]'] = { change: { fn: asp.onChangePerson, scope: asp } };
                actions[asp.editPanelSelector + ' [name=Contragent]'] = { beforeload: { fn: asp.onBeforeLoadContragent, scope: asp } };
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.typeJurOrg = this.getPanel().down('[name=TypeJurPerson]').getValue();
            },
            onChangePerson: function (field, newValue, oldValue) {
                var me= this,
                    form = this.getPanel(),
                    sfContragent = form.down('[name=Contragent]'),
                    tfPhysicalPerson = form.down('[name=PhysicalPerson]'),
                    cbTypeJurPerson = form.down('[name=TypeJurPerson]'),
                    innField = form.down('[name=Inn]');

                switch (newValue) {
                    case 10://физлицо
                        sfContragent.setValue(null);
                        cbTypeJurPerson.setValue(null);
                        sfContragent.setDisabled(true);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(true);
                        innField.setDisabled(false);
                        innField.show();
                        break;
                    case 20://организация
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(true);
                        cbTypeJurPerson.setDisabled(false);
                        innField.setDisabled(true);
                        innField.hide();
                        break;
                    case 30://должностное лицо
                        sfContragent.setDisabled(false);
                        tfPhysicalPerson.setDisabled(false);
                        cbTypeJurPerson.setDisabled(false);
                        innField.setDisabled(false);
                        innField.show();
                        break;
                }
                if (oldValue > 0 && newValue > 0) {
                    tfPhysicalPerson.setValue(null);
                }
                me.controller.disabledFields['Contragent'] = sfContragent.disabled;
                me.controller.disabledFields['PhysicalPerson'] = tfPhysicalPerson.disabled;
                me.controller.disabledFields['TypeJurPerson'] = cbTypeJurPerson.disabled;
            },
            onChangeInspectionBasis: function (field, newValue, oldValue) {
                var form = this.getPanel(),
                    appealCits = form.down('[name=citizensAppeal]');

                switch (newValue) {
                    case B4.enums.InspectionCreationBasis.AppealCitizens:
                        //Обращение граждан
                        appealCits.show();
                        return;
                    case B4.enums.InspectionCreationBasis.AnotherBasis:
                        //Иное основание
                        appealCits.hide();
                        break;
                }
            },
            getDisposal: function () {
                /*
                перекрываем метод формирования распоряжения потому что
                если предыдущий документ не выбран, то формируем обычное распоряжение
                если предыдущий документ выбран то формируем
                */

                var model = this.controller.getModel('TaskDisposal');
                var disp = new model({ Id: 0 });
                disp.set('Inspection', this.controller.params.inspectionId);
                disp.set('TypeDocumentGji', 10);
                disp.set('ParentDocumentsList', []);

                return disp;
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    
                    asp.controller.params = asp.controller.params || {};

                    asp.onChangePerson(null, rec.get('PersonInspection'));
                    
                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
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

                        var fieldInspectors = panel.down('[name=Inspectors]');
                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    var appCits = rec.get('AppealCits');

                    if (appCits) {
                        var docNumber = appCits.NumberGji ? '№ ' + appCits.NumberGji : '',
                            docDate = appCits.DateFrom ? ' от ' + asp.controller.convertDate(appCits.DateFrom) + ' г.' : '',
                            citizensAppeal = panel.down('[name=citizensAppeal]');

                        citizensAppeal.setValue(docNumber + docDate);
                    }

                    this.controller.getAspect('stateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    this.controller.getAspect('createButtonAspect').setData(rec.get('Id'));
                    this.controller.getAspect('requirementAspect').onAfterRender();
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Акта без взаимодействия и массовой формы выборка домов
            По нажатию на кнопку открывается массовая форма выбора после нажатия на форме Применить
            у главного аспекта вызывается метод создания документа createActCheck1House и передаются выбранные Id домов
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'taskWarningToWarningDocGJIAspect',
            buttonSelector: '#warninginspectionEditPanel [ruleId=GjiWarningToWarningDocRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#taskWarningToWarningDocRuleSelectWindow',
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
                    operation.params.fromWarningDoc = true;
            },

            onRowSelect: function (rowModel, record, index, opt) {
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
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('RealityObjectId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('createButtonAspect');
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
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'realityObjectAspect',
            gridSelector: '#warninginspectionEditPanel realityobjectgjigrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#warninginspectionRealityObjectMultiSelectWindow',
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
                var panel = this.controller.getMainView();
                operation.params.contragentId = panel.down('[name=Contragent]').getValue();
                operation.params.date = panel.down('[name=Date]').getValue();
                operation.params.typeJurPerson = panel.down('[name=TypeJurPerson]').getValue();
                operation.params.isPhysicalPerson = panel.down('[name=PersonInspection]').getValue() === 10 ? true : false;
            },

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) {
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
                            asp.controller.getRealityObjectGrid().getStore().load();
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
            name: 'inspectorMultiSelectWindowAspect',
            fieldSelector: '#warninginspectionEditPanel [name=Inspectors]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#warninginspectionInspectorsSelectWindow',
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
            name: 'inspectionContragentAspect',
            gridSelector: '#warninginspectionEditPanel inspectiongjicontragentgrid',
            storeName: 'inspectiongji.InspectionBaseContragent',
            modelName: 'inspectiongji.InspectionBaseContragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspectionContragentMultiSelectWindow',
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

        me.control(actions);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            mainView = me.getMainComponent(),
            realityObjectGridStore = me.getRealityObjectGrid().getStore(),
            inspectionContragentGridStore = me.getInspectionContragentGrid().getStore();

        if (me.params && mainView) {
            me.getAspect('editPanelAspect').setData(me.params.inspectionId);

            realityObjectGridStore.on('beforeload', me.onBeforeLoad, me);
            realityObjectGridStore.load();

            inspectionContragentGridStore.on('beforeload', me.onBeforeLoad, me);
            inspectionContragentGridStore.load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    },

    convertDate: function (datetime) {
        if (datetime) {
            var a = datetime.split('T')[0],
                b = a.split('-').reverse();

            return b.join('.');
        }

        return '';
    }
});