Ext.define('B4.controller.dict.Inspector', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.Inspector',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.store.dict.ZonalInspectionForSelect',
        'B4.store.dict.ZonalInspectionForSelected',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['dict.Inspector', 'dict.InspectorSubscription', 'dict.ZonalInspection'],
    stores: ['dict.Inspector',
        'dict.InspectorSubscription',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ZonalInspection',
        'dict.inspector.ZonalInspection'
    ],
    views: [
        'dict.inspector.Grid',
        'SelectWindow.MultiSelectWindow',
        'dict.inspector.EditWindow',
        'dict.inspector.SubcriptionGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.inspector.Grid',
    mainViewSelector: 'inspectorGrid',

    //селектор окна котоырй потом используется при открытии
    editWindowSelector: '#inspectorEditWindow',

    refs: [{
        ref: 'mainView',
        selector: 'inspectorGrid'
    }],

    aspects: [
        {
            xtype: 'inspectordictperm'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.Dicts.Inspector.Fields.InspectorPosition', applyTo: '[name=InspectorPosition]', selector: '#inspectorEditWindow' },
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'inspectorGridWindowAspect',
            gridSelector: 'inspectorGrid',
            editFormSelector: '#inspectorEditWindow',
            storeName: 'dict.Inspector',
            modelName: 'dict.Inspector',
            editWindowView: 'dict.inspector.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.inpectorId = record.getId();
                this.updateControls(asp.controller.inpectorId);
            },
            updateControls: function (objectId) {
                var frm = this.getForm();

                var readOnly = objectId == 0;

                frm.down('#zonInspectorsTrigerField').setReadOnly(readOnly);

            },

            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentId(record.getId());
                    asp.controller.getStore('dict.InspectorSubscription').load();

                    var inpectorId = record.getId();
                    asp.controller.inpectorId = inpectorId;

                    this.updateControls(inpectorId);

                    var fieldInspectors = form.down('#zonInspectorsTrigerField');

                    if (inpectorId > 0) {
                        asp.controller.mask('Загрузка', asp.getForm());
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('GetInfo', 'Inspector'),
                            params: {
                                inpectorId: asp.controller.inpectorId
                            }
                        }).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            fieldInspectors.updateDisplayedText(obj.zonalInspectionNames);
                            fieldInspectors.setValue(obj.zonalInspectionIds);
                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        fieldInspectors.updateDisplayedText(null);
                        fieldInspectors.setValue(null);

                    }
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'inspectorSubscriptionAspect',
            gridSelector: '#inspectorSubcriptionGrid',
            storeName: 'dict.InspectorSubscription',
            modelName: 'dict.InspectorSubscription',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspectorSubscriptionMultiSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Испекторы',
            titleGridSelected: 'Выбранные инспекторы',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1 }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.excludeInpectorId = this.controller.inpectorId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('SubcribeToInspectors', 'Inspector', {
                            inpectorIds: Ext.encode(recordIds),
                            signedInpectorId: asp.controller.inpectorId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранено!', 'Подписки сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспектора');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            множественный выбор Жил инспекций
           аспект взаимодействия триггер-поля инспекторы с массовой формой выбора зон жи
           по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
           По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
           и сохраняем инспекторов через серверный метод /Inspector/AddZonalInspection
           */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'inspectorZonalMultiSelectWindowAspect',
            fieldSelector: '#inspectorEditWindow #zonInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#inspectorZonalSelectWindow',
            storeSelect: 'dict.ZonalInspectionForSelect',
            storeSelected: 'dict.inspector.ZonalInspection',
            textProperty: 'ZoneName',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ZoneName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ZoneName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор ЗЖИ',
            titleGridSelect: 'ЗЖИ для отбора',
            titleGridSelected: 'Выбранные ЗЖИ',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['inpectorId'] = this.controller.inpectorId;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddZonalInspection', 'Inspector'),
                        params: {
                            objectIds: Ext.encode(recordIds),
                            inpectorId: asp.controller.inpectorId
                        }
                    }).next(function () {
                        Ext.Msg.alert('Сохранение!', 'ЗЖИ сохранены успешно');
                        asp.controller.unmask();
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
        this.getStore('dict.InspectorSubscription').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.inpectorId = this.inpectorId;
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('inspectorGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.Inspector').load();
    },

    setCurrentId: function (id) {
        this.inpectorId = id;
        var store = this.getStore('dict.InspectorSubscription');
        store.removeAll();

        var editwindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        editwindow.down('#inspectorSubcriptionGrid').setDisabled(!id);

        if (id) {
            store.load();
        }
    }
});