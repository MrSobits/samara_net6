Ext.define('B4.controller.basejurperson.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.BaseJurPerson',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'Disposal',
        'BaseJurPerson',
        'RealityObjectGji',
        'RealityObject',
        'Contragent',
        'basejurperson.Contragent'
    ],

    stores: [
        'basejurperson.RealityObject',
        'basejurperson.Contragent',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ZonalInspectionForSelect',
        'dict.ZonalInspectionForSelected'
    ],

    views: [
        'basejurperson.EditPanel',
        'basejurperson.RealityObjectGrid',
        'basejurperson.ContragentGrid',
        'inspectiongji.RiskPrevWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'basejurperson.EditPanel',
    mainViewSelector: '#baseJurPersonEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#baseJurPersonEditPanel' },
            requirements: [
                {
                    name: 'GkhGji.Inspection.BaseJurPerson.Field.JurPersonInspectors',
                    applyTo: '[name=JurPersonInspectors]',
                    selector: '#baseJurPersonEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseJurPerson.Field.JurPersonZonalInspections',
                    applyTo: '[name=JurPersonZonalInspections]',
                    selector: '#baseJurPersonEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseJurPerson.Field.InspectionBaseType',
                    applyTo: '[name=InspectionBaseType]',
                    selector: '#baseJurPersonEditPanel'
                }
            ]
        },
        {
            /*
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseJurPersonCreateButtonAspect',
            buttonSelector: '#baseJurPersonEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseJurPersonEditPanel',
            typeBase: 30 // Тип плановая проверка ЮЛ
        },
        {
            xtype: 'basejurpersonperm',
            editFormAspectName: 'baseJurPersonEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseJurPersonStateButtonAspect',
            stateButtonSelector: '#baseJurPersonEditPanel #btnState'
        },
        {
            /*
            Аспект основной панели Плановой проверки юр лиц
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseJurPersonEditPanelAspect',
            editPanelSelector: '#baseJurPersonEditPanel',
            modelName: 'BaseJurPerson',
            otherActions: function (actions) {
                var me = this;

                actions[me.editPanelSelector + ' #cbTypeFactInspection'] = { 'select': { fn: me.onSelectTypeFactInspectionCombobox, scope: me } };
                actions[me.editPanelSelector + ' #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
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
                        fn: function() {
                            var prevWindow = Ext.create('B4.view.inspectiongji.RiskPrevWindow', {
                                renderTo: B4.getBody().getActiveTab().getEl(),
                                inspectionId: me.controller.params.inspectionId
                            });

                            prevWindow.on('beforeclose', function(win) {
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
            onChangeDateStart: function (_df, newValue) {
                this.controller.params.dateInspection = newValue;
            },
            onSelectTypeFactInspectionCombobox: function (combobox) {
                if (combobox.value === 30) {
                    this.getPanel().down('#tfReason').setDisabled(false);
                } else {
                    this.getPanel().down('#tfReason').setDisabled(true);
                }
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
                    
                    asp.controller.params.contragentId = rec.get('Contragent').Id;
                    asp.controller.params.dateStart = rec.get('Plan').DateStart;
                    asp.controller.params.dateEnd = rec.get('Plan').DateEnd;
                    asp.controller.params.typeJurOrg = rec.get('TypeJurPerson');
                    asp.controller.params.dateInspection = rec.get('DateStart');

                    if (panel.down('#cbTypeFactInspection').getValue() === 30) {
                        panel.down('#tfReason').setDisabled(false);
                    }
                    else {
                        panel.down('#tfReason').setDisabled(true);
                    }

                    //Делаем запрос на получение жилых домов управляющей организации по которой проверка
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseJurPerson', {
                        inspectionId: asp.controller.params.inspectionId
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText),
                            tfInspectors = panel.down('#trigfInspectors'),
                            tfZonalInspections = panel.down('#trigfZonalInspections');

                        tfInspectors.updateDisplayedText(obj.inspectorNames);
                        tfInspectors.setValue(obj.inspectorIds);
                        
                        tfZonalInspections.updateDisplayedText(obj.zonalInspectionNames);
                        tfZonalInspections.setValue(obj.zonalInspectionIds);
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    //Обновляем Статусы
                    asp.controller.getAspect('baseJurPersonStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //Обновляем кнопку Сформировать
                    asp.controller.getAspect('baseJurPersonCreateButtonAspect').setData(rec.get('Id'));
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'jurPersonRealityObjectAspect',
            gridSelector: '#baseJurPersonRealityObjectGrid',
            storeName: 'basejurperson.RealityObject',
            modelName: 'RealityObjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#jurPersonRealityObjectMultiSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],

            onBeforeLoad: function (store, operation) {
                if (this.controller.params) {
                    operation.params.contragentId = this.controller.params.contragentId;
                    operation.params.typeJurPerson = this.controller.params.typeJurOrg;
                    operation.params.date = this.controller.params.dateInspection;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
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
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
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
            Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'jurPersonContragentAspect',
            gridSelector: 'baseJurPersonContragentGrid',
            storeName: 'basejurperson.Contragent',
            modelName: 'basejurperson.Contragent',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#jurPersonContragentMultiSelectWindow',
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

            onBeforeLoad: function (store, operation) {
                if (this.controller.params) {
                    operation.params.contragentId = this.controller.params.inspectionId;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddContragents', 'BaseJurPersonContragent'),
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
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать контрагентов');
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
            и сохраняем инспекторов через серверный метод /InspectionGjiInspector/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#JurPersonInspectorSelectWindow',
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
            onBeforeLoad: function (store, operation) {
                operation.params.zonalInspectionIds = this.controller.getMainView().down('#trigfZonalInspections').getValue();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'InspectionGjiInspector'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        }
                    }).next(function() {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
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
            аспект взаимодействия триггер-поля отделы с массовой формой выбора отделов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем отделы через серверный метод /InspectionGjiZonalInspection/AddZonalInspections
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonZonalInspectionsAddWindowMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonEditPanel #trigfZonalInspections',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#JurPersonZonalInspectionSelectWindow',
            storeSelect: 'dict.ZonalInspectionForSelect',
            storeSelected: 'dict.ZonalInspectionForSelected',
            textProperty: 'ZoneName',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    header: 'Зональное наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'ZoneName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Зональное наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'ZoneName',
                    flex: 1,
                    sortable: false
                }
            ],
            titleSelectWindow: 'Выбор отделов',
            titleGridSelect: 'Отделы для отбора',
            titleGridSelected: 'Выбранные отделы',
            onBeforeLoad: function(store, operation) {
                operation.params.inspectorIds = this.controller.getMainView().down('#trigfInspectors').getValue();
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    records.each(function(rec) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddZonalInspections', 'InspectionGjiZonalInspection'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        }
                    }).next(function() {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Отделы сохранены успешно');
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('basejurperson.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('basejurperson.Contragent').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('baseJurPersonEditPanelAspect').setData(me.params.inspectionId);

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.params.title);
        }
        me.getStore('basejurperson.RealityObject').load();
        me.getStore('basejurperson.Contragent').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.inspectionId = this.params.inspectionId;
    }
});