Ext.define('B4.controller.baseomsu.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.BaseOMSU',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Disposal',
        'BaseOMSU'
    ],

    stores: [
      
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ZonalInspectionForSelect',
        'dict.ZonalInspectionForSelected'
    ],

    views: [
        'baseomsu.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'baseomsu.EditPanel',
    mainViewSelector: '#baseOMSUEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: '#baseOMSUEditPanel' },
            requirements: [
                {
                    name: 'GkhGji.Inspection.BaseOMSU.Field.UriRegistrationNumber',
                    applyTo: '[name=UriRegistrationNumber]',
                    selector: '#baseOMSUEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseOMSU.Field.UriRegistrationDate',
                    applyTo: '[name=UriRegistrationDate]',
                    selector: '#baseOMSUEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseOMSU.Field.JurPersonInspectors',
                    applyTo: '[name=JurPersonInspectors]',
                    selector: '#baseOMSUEditPanel'
                },
                {
                    name: 'GkhGji.Inspection.BaseOMSU.Field.JurPersonZonalInspections',
                     applyTo: '[name=JurPersonZonalInspections]',
                     selector: '#baseOMSUEditPanel'
                }
            ]
        },
        {
            /*
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseOMSUCreateButtonAspect',
            buttonSelector: '#baseOMSUEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseOMSUEditPanel',
            typeBase: 31 // Тип плановая проверка ЮЛ
        },
        {
            xtype: 'baseomsuperm',
            editFormAspectName: 'baseOMSUEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseOMSUStateButtonAspect',
            stateButtonSelector: '#baseOMSUEditPanel #btnState'
        },
        {
            /*
            Аспект основной панели Плановой проверки юр лиц
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseOMSUEditPanelAspect',
            editPanelSelector: '#baseOMSUEditPanel',
            modelName: 'BaseOMSU',
            otherActions: function (actions) {
                var me = this;
                actions[me.editPanelSelector + ' #cbTypeFactInspection'] = { 'select': { fn: me.onSelectTypeFactInspectionCombobox, scope: me } };
                actions[me.editPanelSelector + ' #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
            },
            onChangeDateStart: function (df, newValue) {
                this.controller.params.dateInspection = newValue;
            },
            onSelectTypeFactInspectionCombobox: function (combobox) {
                var me = this;
                if (combobox.value == 30) {
                    me.getPanel().down('#tfReason').setDisabled(false);
                } else {
                    me.getPanel().down('#tfReason').setDisabled(true);
                }
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var me = this,
                        callbackUnMask;
                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    callbackUnMask = asp.controller.params.callbackUnMask;

                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }
                    
                    asp.controller.params.contragentId = rec.get('Contragent').Id;
                    asp.controller.params.dateStart = rec.get('Plan').DateStart;
                    asp.controller.params.dateEnd = rec.get('Plan').DateEnd;
                    asp.controller.params.typeJurOrg = rec.get('TypeJurPerson');
                    asp.controller.params.dateInspection = rec.get('DateStart');
                    asp.controller.params.omsuPerson = rec.get('OmsuPerson');

                    if (panel.down('#cbTypeFactInspection').getValue() == 30) {
                        panel.down('#tfReason').setDisabled(false);
                    }
                    else {
                        panel.down('#tfReason').setDisabled(true);
                    }

                    //Делаем запрос на получение жилых домов управляющей организации по которой проверка
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    debugger;
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseJurPerson', {
                        inspectionId: asp.controller.params.inspectionId
                    })).next(function (response) {
                        asp.controller.unmask();
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        tfInspectors = panel.down('#trigfInspectors');
                        tfInspectors.updateDisplayedText(obj.inspectorNames);
                        tfInspectors.setValue(obj.inspectorIds);

                        tfZonalInspections = panel.down('#trigfZonalInspections');
                        tfZonalInspections.updateDisplayedText(obj.zonalInspectionNames);
                        tfZonalInspections.setValue(obj.zonalInspectionIds);

                    }).error(function () {
                        asp.controller.unmask();
                    });
                    debugger;
                    //Обновляем Статусы
                    me.controller.getAspect('baseOMSUStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    debugger;
                    //Обновляем кнопку Сформировать
                   me.controller.getAspect('baseOMSUCreateButtonAspect').setData(rec.get('Id'));
                    debugger;
                    
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
            name: 'baseOMSUMultiSelectWindowAspect',
            fieldSelector: '#baseOMSUEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#OMSUInspectorSelectWindow',
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
                operation.params.zonalInspectionIds = this.controller.getMainView().down('#trigfZonalInspections').getValue()
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

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
            name: 'baseOMSUZonalInspectionsAddWindowMultiSelectWindowAspect',
            fieldSelector: '#baseOMSUEditPanel #trigfZonalInspections',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#OMSUZonalInspectionSelectWindow',
            storeSelect: 'dict.ZonalInspectionForSelect',
            storeSelected: 'dict.ZonalInspectionForSelected',
            textProperty: 'ZoneName',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Зональное наименование', xtype: 'gridcolumn', dataIndex: 'ZoneName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Зональное наименование', xtype: 'gridcolumn', dataIndex: 'ZoneName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор отделов',
            titleGridSelect: 'Отделы для отбора',
            titleGridSelected: 'Выбранные отделы',
            onBeforeLoad: function (store, operation) {
                operation.params.inspectorIds = this.controller.getMainView().down('#trigfInspectors').getValue()
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddZonalInspections', 'InspectionGjiZonalInspection'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inspectionId: asp.controller.params.inspectionId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Отделы сохранены успешно');
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
        debugger;
    //    me.getStore('basejurperson.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            mainView;

        if (me.params) {
            me.getAspect('baseOMSUEditPanelAspect').setData(me.params.inspectionId);

            mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.params.title);
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params)
            operation.params.inspectionId = me.params.inspectionId;
    }
});