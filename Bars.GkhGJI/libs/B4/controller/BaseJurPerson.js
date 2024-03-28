Ext.define('B4.controller.BaseJurPerson', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.BaseJurPerson',
        'B4.aspects.FieldRequirementAspect',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['BaseJurPerson'],
    stores: [
        'BaseJurPerson',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ZonalInspectionForSelect',
        'dict.ZonalInspectionForSelected'
    ],
    views: [
        'basejurperson.AddWindow',
        'SelectWindow.MultiSelectWindow',
        'basejurperson.Grid',
        'basejurperson.FilterPanel',
        'basejurperson.MainPanel'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'basejurperson.MainPanel',
    mainViewSelector: 'baseJurPersonPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseJurPersonPanel'
        }
    ],
    
    params : {},

    aspects: [
         {
             xtype: 'gkhpermissionaspect',
             permissions: [
                { name: 'GkhGji.Inspection.BaseJurPerson.Create', applyTo: 'b4addbutton', selector: '#baseJurPersonGrid' },
                { name: 'GkhGji.Inspection.BaseJurPerson.Edit', applyTo: 'b4savebutton', selector: '#baseJurPersonEditPanel' },
                { name: 'GkhGji.Inspection.BaseJurPerson.Delete', applyTo: 'b4deletecolumn', selector: '#baseJurPersonGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                   name: 'GkhGji.Inspection.BaseJurPerson.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: '#baseJurPersonGrid',
                   applyBy: function (component, allowed) {
                       var me = this;

                       if (allowed) {
                           if (me.controller.params) {
                               me.controller.params.showCloseInspections = false;
                           }
                           component.show();
                       } else {
                           if (me.controller.params) {
                               me.controller.params.showCloseInspections = true;
                           }
                           component.hide();
                       }
                   }
               }
             ]
         },
         {
             xtype: 'requirementaspect',
             applyOn: { event: 'show'},
             requirements: [
                 {
                     name: 'GkhGji.Inspection.BaseJurPerson.Field.JurPersonInspectors',
                     applyTo: '[name=JurPersonInspectors]',
                     selector: '#baseJurPersonAddWindow'
                 },
                 {
                     name: 'GkhGji.Inspection.BaseJurPerson.Field.JurPersonZonalInspections',
                     applyTo: '[name=JurPersonZonalInspections]',
                     selector: '#baseJurPersonAddWindow'
                 }
             ]
         },
         {
            xtype: 'b4_state_contextmenu',
            name: 'baseJurPersonStateTransferAspect',
            gridSelector: '#baseJurPersonGrid',
            menuSelector: 'baseJurPersonStateMenu',
            stateType: 'gji_inspection'
        },
        {
            /*
            * Аспект экспорта из реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'baseJurPersonButtonExportAspect',
            gridSelector: '#baseJurPersonGrid',
            buttonSelector: '#baseJurPersonGrid #btnExport',
            controllerName: 'BaseJurPerson',
            actionName: 'Export'
        },
        {
            /*
             * аспект взаимодействия таблицы плановых проверок юр лиц формы добавления и Панели редактирования,
             * открывающейся в боковой вкладке
             */
            xtype: 'gkhgrideditformaspect',
            name: 'baseJurPersonGridWindowAspect',
            gridSelector: '#baseJurPersonGrid',
            editFormSelector: '#baseJurPersonAddWindow',
            storeName: 'BaseJurPerson',
            modelName: 'BaseJurPerson',
            editWindowView: 'basejurperson.AddWindow',
            controllerEditName: 'B4.controller.basejurperson.Navigation',
            deleteWithRelatedEntities: true,

            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: me.onChangeType, scope: me } };
                actions[me.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: me.onBeforeLoadContragent, scope: me } };

                actions['baseJurPersonPanel #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
                actions['baseJurPersonPanel #dfDateEnd'] = { 'change': { fn: me.onChangeDateEnd, scope: me } };
                actions['baseJurPersonPanel #trigfInspectors'] = { 'triggerClear': { fn: me.onClearInspectors, scope: me } };
                actions['baseJurPersonPanel #trigfZonalInspections'] = { 'triggerClear': { fn: me.onClearZonalInspections, scope: me } };
                actions['baseJurPersonPanel #sfPlan'] = { 'change': { fn: me.onChangePlan, scope: me } };
                actions['#baseJurPersonFilterPanel #updateGrid'] = { 'click': { fn: me.onUpdateGrid, scope: me } };
                actions['#baseJurPersonGrid #cbShowCloseInspections'] = { 'change': { fn: me.onChangeCheckbox, scope: me } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseJurPerson');
                str.currentPage = 1;
                str.load();
            },
            onAfterSetFormData: function (aspect, rec, form) {
                var fieldInspectors = form.down('#trigfInspectors');
                fieldInspectors.setValue(null);
                fieldInspectors.updateDisplayedText(null);

                var fieldZonalInspections = form.down('#trigfZonalInspections');
                fieldZonalInspections.setValue(null);
                fieldZonalInspections.updateDisplayedText(null);

                form.show();
            },

            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.params.typeJurOrg;
            },

            onChangeType: function (field, newValue) {
                var me = this;

                me.controller.params = this.controller.params || {};
                me.getForm().down('#sfContragent').setValue(null);
                me.controller.params.typeJurOrg = newValue;
            },

            //-----Фильтры

            onClearInspectors: function () {
                var me = this;
                if (me.controller.params) {
                    me.controller.params.inspectorIds = [];
                }
            },

            onClearZonalInspections: function () {
                var me = this;
                if (me.controller.params) {
                    me.controller.params.zonalInspectionIds = [];
                }
            },

            onChangePlan: function (field, newValue) {
                var me = this;
                if (me.controller.params) {
                    if (newValue) {
                        me.controller.params.planId = newValue.Id;
                    }
                    else {
                        me.controller.params.planId = null;
                    }
                } 
            },

            onChangeDateStart: function (field, newValue) {
                var me = this;
                if (me.controller.params) {
                    me.controller.params.dateStart = newValue;
                }
            },

            onChangeDateEnd: function (field, newValue) {
                var me = this;
                if (me.controller.params) {
                    me.controller.params.dateEnd = newValue;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                var me = this;
                me.controller.params.showCloseInspections = newValue;
                me.controller.getStore('BaseJurPerson').load();
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /InspectionGJIInspector/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonAddWindowMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonAddWindow #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseJurPersonInspectorAddWindowSelectWindow',
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
                operation.params.zonalInspectionIds = Ext.ComponentQuery.query('#trigfZonalInspections')[1].value
            },
            updateSelectedGrid: function () {
                var me = this;

                me.updateSelectGrid();
                //чтобы грид выбранных не перезагружался
            },
            onLoad: function (store, operation) {
                this.setIgnoreChanges(false);

                var me = this,
                    grid = me.getSelectGrid(),
                    selectedInspectorIds = me.getSelectField().value;

                if (grid && selectedInspectorIds) {
                    var selectedInspectorIdsSplited = selectedInspectorIds.toString().split(", "),
                        i;
                    for (i = 0; i < selectedInspectorIdsSplited.length; i++) {
                        var index = store.find('Id', selectedInspectorIdsSplited[i]);
                        if (index != -1) {
                            grid.getSelectionModel().select(index);
                        }
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения 
            и формируем строку идентификаторов инспекторов
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonFilterInspectorMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonFilterPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseJurPersonFilterPanelInspectorsSelectWindow',
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
                operation.params.zonalInspectionIds = Ext.ComponentQuery.query('#trigfZonalInspections')[1].value
            },
            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    if (me.controller.params) {
                        me.controller.params.inspectorIds = recordIds;
                    }
                }
            },
            onTrigger2Click: function () {
                var me = this;

                me.setValue(null);
                me.updateDisplayedText();
                me.controller.params.inspectorIds = [];
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /InspectionGJIInspector/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonZonalInspectionsAddWindowMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonAddWindow #trigfZonalInspections',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseJurPersonZonalInspectionAddWindowSelectWindow',
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
                operation.params.inspectorIds = Ext.ComponentQuery.query('#trigfInspectors')[1].value
            },
            updateSelectedGrid: function () {
                var me = this;

                me.updateSelectGrid();
                //чтобы грид выбранных не перезагружался
            },
            onLoad: function (store, operation) {
                this.setIgnoreChanges(false);

                var me = this,
                    grid = me.getSelectGrid(),
                    selectedZonalInspectionIds = me.getSelectField().value;

                if (grid && selectedZonalInspectionIds) {
                    var selectedZonalInspectionIdsSplited = selectedZonalInspectionIds.toString().split(", "),
                        i;
                    for (i = 0; i < selectedZonalInspectionIdsSplited.length; i++) {
                        index = store.find('Id', selectedZonalInspectionIdsSplited[i]);
                        if (index != -1) {
                            grid.getSelectionModel().select(index);
                        }
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения 
            и формируем строку идентификаторов инспекторов
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseJurPersonFilterZonalInspectionMultiSelectWindowAspect',
            fieldSelector: '#baseJurPersonFilterPanel #trigfZonalInspections',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseJurPersonFilterPanelZonalInspectionsSelectWindow',
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
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор отделов',
            titleGridSelect: 'Отделы для отбора',
            titleGridSelected: 'Выбранные отделы',
            onBeforeLoad: function (store, operation) {
                operation.params.inspectorIds = Ext.ComponentQuery.query('#trigfInspectors')[1].value
            },
            listeners: {
                getdata: function(asp, records) {
                    var me = this,
                        recordIds = [];

                    records.each(function(rec) {
                        recordIds.push(rec.getId());
                    });

                    if (me.controller.params) {
                        me.controller.params.zonalInspectionIds = recordIds;
                    }
                }
            },
            onTrigger2Click: function () {
                var me = this;

                me.setValue(null);
                me.updateDisplayedText();
                me.controller.params.zonalInspectionIds = [];
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        me.getStore('BaseJurPerson').on('beforeload', me.onBeforeLoad, me);

        actions[me.mainViewSelector] = { 'afterrender': { fn: me.onMainViewAfterRender, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('baseJurPersonPanel');
        me.bindContext(view);
        me.application.deployView(view);
    },

    onMainViewAfterRender: function () {
        var me = this,
            mainView = me.getMainComponent();

        me.params = {};
        me.params.planId = null;
        me.params.inspectorIds = [];

        if (mainView) {
            me.mask('Загрузка', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetStartFilters', 'BaseJurPerson'))
                .next(function (response) {
                    var obj, planId, planName,
                        dateStart, dateEnd, panel;

                    me.unmask();
                    //десериализуем полученную строку
                    if (!Ext.isEmpty(response.responseText)) {
                        obj = Ext.JSON.decode(response.responseText);

                        planId = obj.planId;
                        planName = obj.planName;
                        dateStart = obj.dateStart;
                        dateEnd = new Date();

                        dateEnd.setDate(new Date().getDate() + 7);

                        panel = me.getMainView();

                        panel.down('#sfPlan').setValue(planId);
                        panel.down('#sfPlan').updateDisplayedText(planName);
                        panel.down('#dfDateStart').setValue(dateStart);
                        panel.down('#dfDateEnd').setValue(dateEnd);

                        me.params.planId = planId;
                        me.params.dateStart = dateStart;
                        me.params.dateEnd = dateEnd;
                        me.getStore('BaseJurPerson').load();
                    }
                    
                }).error(function () {
                    me.unmask();
                });
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params) {
            operation.params.dateStart = me.params.dateStart;
            operation.params.dateEnd = me.params.dateEnd;
            operation.params.planId = me.params.planId;
            operation.params.inspectorIds = Ext.encode(me.params.inspectorIds);
            operation.params.zonalInspectionIdsIds = Ext.encode(me.params.zonalInspectionIds);
            operation.params.showCloseInspections = me.params.showCloseInspections;
        }
    }
});