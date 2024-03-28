Ext.define('B4.controller.dict.ZonalInspection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.dict.ZonalInspection'
    ],

    models: [
        'dict.ZonalInspection',
        'dict.ZonalInspectionMunicipality',
        'dict.ZonalInspectionInspector'
    ],
    stores: [
        'dict.ZonalInspection',
        'dict.ZonalInspectionMunicipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ZonalInspectionInspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],
    views: [
        'dict.zonalinspection.Grid',
        'dict.zonalinspection.EditWindow',
        'SelectWindow.MultiSelectWindow',
        'dict.zonalinspection.MunicipalityGrid',
        'dict.zonalinspection.InspectorGrid'
    ],

    editWindowSelector : '#zonalInspectionEditWindow',
    refs: [
        {
            ref: 'mainView',
            selector: 'zonalInspectionGrid'
        }
    ],

    mainView: 'dict.zonalinspection.Grid',
    mainViewSelector: 'zonalInspectionGrid',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'zonalinspectiondictperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'zonalInspectionGridWindowAspect',
            gridSelector: 'zonalInspectionGrid',
            editFormSelector: '#zonalInspectionEditWindow',
            storeName: 'dict.ZonalInspection',
            modelName: 'dict.ZonalInspection',
            editWindowView: 'dict.zonalinspection.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.getId(), asp);
            },
            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.controller.setCurrentId(record.getId(), asp);
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'zonalInspectionMunicipalityAspect',
            gridSelector: '#zonalInspectionMunicipalityGrid',
            storeName: 'dict.ZonalInspectionMunicipality',
            modelName: 'dict.ZonalInspectionMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#zonalInspectionMunicipalityMultiSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(item) {
                        recordIds.push(item.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipalites', 'ZonalInspection'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                zonalInspectionId: asp.controller.zonalInspectionId
                            }
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'zonalInspectionInspectorAspect',
            gridSelector: '#zonalInspectionInspectorsGrid',
            storeName: 'dict.ZonalInspectionInspector',
            modelName: 'dict.ZonalInspectionInspector',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#zonalInspectionInspectorMultiSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1 }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (item) {
                        recordIds.push(item.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddInspectors', 'ZonalInspection'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                zonalInspectionId: asp.controller.zonalInspectionId
                            }
                        }).next(function() {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспекторов');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        this.getStore('dict.ZonalInspectionMunicipality').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.ZonalInspectionInspector').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('zonalInspectionGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ZonalInspection').load();
    },

    setCurrentId: function(id, aspect) {
        this.zonalInspectionId = id;

        var storeMunicipality = this.getStore('dict.ZonalInspectionMunicipality');
        var sourceInspector = this.getStore('dict.ZonalInspectionInspector');
        storeMunicipality.removeAll();
        sourceInspector.removeAll();
        
        aspect.componentQuery('zonalinspectionmunicipalitygrid').setDisabled(!(id > 0));
        aspect.componentQuery('zonalinspectioninspectorsgrid').setDisabled(!(id > 0));
        
        if (id > 0) {
            storeMunicipality.load();
            sourceInspector.load();
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.zonalInspectionId = this.zonalInspectionId;
    }
});