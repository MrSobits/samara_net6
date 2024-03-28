Ext.define('B4.controller.BaseInsCheck', {
    extend: 'B4.base.Controller',
   
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.ButtonDataExport',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.aspects.permission.BaseInsCheck',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['BaseInsCheck'],

    stores: [
        'BaseInsCheck',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],

    views: [
        'baseinscheck.MainPanel',
        'baseinscheck.AddWindow',
        'baseinscheck.Grid',
        'baseinscheck.FilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },

    mainView: 'baseinscheck.MainPanel',
    mainViewSelector: 'baseInsCheckPanel',

    baseInsCheckAddWindowSelector : '#baseInsCheckAddWindow',
    baseInsCheckFilterPanelSelector : '#baseInsCheckFilterPanel',
   
    refs: [
        {
            ref: 'mainView',
            selector: 'baseInsCheckPanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseInsCheck.Create', applyTo: 'b4addbutton', selector: '#baseInsCheckGrid' },
                { name: 'GkhGji.Inspection.BaseInsCheck.Edit', applyTo: 'b4savebutton', selector: '#baseInsCheckEditPanel' },
                { name: 'GkhGji.Inspection.BaseInsCheck.Delete', applyTo: 'b4deletecolumn', selector: '#baseInsCheckGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                   name: 'GkhGji.Inspection.BaseInsCheck.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: '#baseInsCheckGrid',
                   applyBy: function (component, allowed) {
                       var mainViewParams = this.controller.getMainView().params;

                       if (!mainViewParams) {
                           mainViewParams = {}
                       }

                       if (allowed) {
                           mainViewParams.showCloseInspections = false;
                           this.controller.getStore('BaseInsCheck').load();
                           component.show();
                       } else {
                           mainViewParams.showCloseInspections = true;
                           this.controller.getStore('BaseInsCheck').load();
                           component.hide();
                       }
                   }
               }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseInsCheckStateTransferAspect',
            gridSelector: '#baseInsCheckGrid',
            menuSelector: 'baseInsCheckStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'baseInsCheckButtonExportAspect',
            gridSelector: '#baseInsCheckGrid',
            buttonSelector: '#baseInsCheckGrid #btnExport',
            controllerName: 'BaseInsCheck',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы инспекционных проверок и формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'baseInsCheckGridWindowAspect',
            gridSelector: '#baseInsCheckGrid',
            editFormSelector: '#baseInsCheckAddWindow',
            storeName: 'BaseInsCheck',
            modelName: 'BaseInsCheck',
            editWindowView: 'baseinscheck.AddWindow',
            controllerEditName: 'B4.controller.baseinscheck.Navigation',

            otherActions: function (actions) {
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this },
                                                                      'change': { fn: this.updateTrfRealityObject, scope: this },
                                                                      'triggerClear': { fn: this.updateTrfRealityObject, scope: this }
                                                                    };
                actions[this.editFormSelector + ' #dfDate'] = { 'change': { fn: this.updateTrfRealityObject, scope: this } };
                actions[this.editFormSelector + ' #cbTypeJurPerson'] = { 'change': { fn: this.updateTrfRealityObject, scope: this } };

                actions['baseInsCheckPanel' + ' #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['baseInsCheckPanel' + ' #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['baseInsCheckPanel' + ' #trfInspectors'] = { 'triggerClear': { fn: this.onClearInspectors, scope: this } };
                actions['baseInsCheckPanel' + ' #sfPlan'] = { 'change': { fn: this.onChangePlan, scope: this } };
                actions['baseInsCheckFilterPanel' + ' #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['baseInsCheckGrid #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('BaseInsCheck');
                str.currentPage = 1;
                str.load();
            },
            onAfterSetFormData: function (aspect, rec, form) {
                var fieldObject = form.down('#trfRealityObject');
                fieldObject.setValue(null);
                fieldObject.updateDisplayedText(null);
                form.show();
            },

            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = this.controller.getMainView().params.typeJurOrg;
            },

            updateTrfRealityObject: function () {
                var form = this.getForm();
                var contragent = form.down('#sfContragent').getValue();
                var date = form.down('#dfDate').getValue();
                var typeJurPerson = form.down('#cbTypeJurPerson').getValue();
                var trfRealityObject = form.down('#trfRealityObject');
                
                trfRealityObject.setValue(null);
                trfRealityObject.updateDisplayedText(null);
                contragent && date && typeJurPerson ? trfRealityObject.setDisabled(false) : trfRealityObject.setDisabled(true);
            },

            onChangeType: function (field, newValue) {
                var mainView = this.controller.getMainView();
                mainView.params = mainView.params || {};
                mainView.params.typeJurOrg = newValue;
                this.getForm().down('#sfContragent').setValue(null);
            },

            //-----Фильтры

            onClearInspectors: function () {
                var mainViewParams = this.controller.getMainView().params;
                if (mainViewParams) {
                    mainViewParams.inspectorIds = [];
                }
            },

            onChangePlan: function (field, newValue) {
                var mainViewParams = this.controller.getMainView().params;
                if (mainViewParams && newValue) {
                    mainViewParams.planId = newValue.Id;
                } else {
                    mainViewParams.planId = null;
                }
            },

            onChangeDateStart: function (field, newValue) {
                var mainViewParams = this.controller.getMainView().params;
                if (mainViewParams) {
                    mainViewParams.dateStart = newValue;
                }
            },

            onChangeDateEnd: function (field, newValue) {
                var mainViewParams = this.controller.getMainView().params;
                if (mainViewParams) {
                    mainViewParams.dateEnd = newValue;
                }
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.getMainView().params.showCloseInspections = newValue;
                this.controller.getStore('BaseInsCheck').load();
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля Дом с массовой формой выбора домов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            сохранение дома происходит в интерсепторе afterAction, в методе save
            
            тут есть специфический момент: в инспекционной проверке должен быть только один дом
            поэтому мы просто перекрываем метод onRowSelect и там контролируем чтобы был выбран только один дом
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseInsCheckAddWindowRealityObjectMultiSelectWindowAspect',
            fieldSelector: '#baseInsCheckAddWindow #trfRealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseInsCheckAddWindowRealityObjectSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            textProperty: 'Address',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            updateSelectedGrid: function () {
                //чтобы грид выбранных не загружался
            },
            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            onBeforeLoad: function (store, operation) {
                var addWindow = Ext.ComponentQuery.query(this.controller.baseInsCheckAddWindowSelector)[0];
                operation.params.contragentId = addWindow.down('#sfContragent').getValue();
                operation.params.date = addWindow.down('#dfDate').getValue();
                operation.params.typeJurPerson = addWindow.down('#cbTypeJurPerson').getValue();
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
            name: 'insCheckFilterInspectorMultiSelectWindowAspect',
            fieldSelector: '#baseInsCheckFilterPanel #trfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseInsCheckFilterInspectorsSelectWindow',
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

            updateSelectedGrid: function () {
                //чтобы грид выбранных не перезагружался
            },

            listeners: {
                getdata: function (asp, records) {
                    var result = [];

                    records.each(function (rec) {
                        result.push(rec.getId());
                    });

                    this.controller.getMainView().params.inspectorIds = result;
                }
            }
        }
    ],

    init: function () {
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);

        this.getStore('BaseInsCheck').on('beforeload', this.onBeforeLoad, this);

    },

    onMainViewAfterRender: function () {
        var me = this,
            mainView = me.getMainView();
        //делаем запрос на получение стартовых значений фильтра


        if (mainView) {
            me.mask('Загрузка', me.getMainView());
            mainView.params = {};
            mainView.params.planId = null;
            mainView.params.inspectorIds = [];

            B4.Ajax.request(B4.Url.action('GetStartFilters', 'BaseInsCheck'))
                .next(function (response) {
                    me.unmask();
                    if (!Ext.isEmpty(response.responseText)) {
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var dateEnd = new Date();

                        dateEnd.setDate(new Date().getDate() + 7);

                        var panel = me.getMainView();

                        panel.down('#sfPlan').setValue(obj.planId);
                        panel.down('#sfPlan').updateDisplayedText(obj.planName);
                        panel.down('#dfDateStart').setValue(obj.dateStart);
                        panel.down('#dfDateEnd').setValue(dateEnd);

                        me.params.planId = obj.planId;
                        me.params.dateStart = obj.dateStart;
                        me.params.dateEnd = dateEnd;
                    }

                }).error(function () {
                    me.unmask();
                });
        }
    },

    onBeforeLoad: function (store, operation) {
        var mainViewParams = this.getMainView().params,
            params = operation.params;     

        if (!mainViewParams) {
            mainViewParams = {}
        }

        var filterPanel = Ext.ComponentQuery.query(this.baseInsCheckFilterPanelSelector)[0];
        
        params.dateStart = filterPanel.down('#dfDateStart').getValue();
        params.dateEnd = filterPanel.down('#dfDateEnd').getValue();
        params.planId = filterPanel.down('#sfPlan').getValue();
        params.inspectorIds = Ext.encode(mainViewParams.inspectorIds);
        params.showCloseInspections = mainViewParams.showCloseInspections;
    },
    
    index: function () {
        var view = this.getMainView() || Ext.widget('baseInsCheckPanel');
        this.bindContext(view);
        this.application.deployView(view);
    }
});