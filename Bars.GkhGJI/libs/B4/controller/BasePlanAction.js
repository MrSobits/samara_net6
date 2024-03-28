Ext.define('B4.controller.BasePlanAction', {
    extend: 'B4.base.Controller',
   
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['BasePlanAction'],

    stores: ['BasePlanAction'],

    views: [
        'baseplanaction.MainPanel',
        'baseplanaction.AddWindow',
        'baseplanaction.Grid',
        'baseplanaction.FilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },

    mainView: 'baseplanaction.MainPanel',
    mainViewSelector: 'basePlanActionPanel',

    //baseInsCheckAddWindowSelector : '#baseInsCheckAddWindow',
    //baseInsCheckFilterPanelSelector : '#baseInsCheckFilterPanel',
   
    refs: [
        {
            ref: 'mainView',
            selector: 'basePlanActionPanel'
        }
    ],

    aspects: [
        //Аспект на добавление - Без статуса поскольку в момент добаления статус еще неизместен
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BasePlanAction.Create', applyTo: 'b4addbutton', selector: 'basePlanActionGrid' }
            ]
        },
        //Аспект на удаление по статусу, в реестре какието записи можн оудалять а какие то нельзя это
        // можно выяснить только в случае проверки в момент нажатия на удаление
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhGji.Inspection.BasePlanAction.Delete' }],
            name: 'deletePlanActionStatePerm'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BasePlanAction.Edit', applyTo: 'b4savebutton', selector: 'basePlanActionAddWindow' }
            ],
            name: 'editPlanActionStatePerm',
            editFormAspectName: 'basePlanActionGridWindowAspect',
            setPermissionEvent: 'aftersetformdata'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'basePlanActionStateTransferAspect',
            gridSelector: 'basePlanActionGrid',
            menuSelector: 'basePlanActionStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'basePlanActionButtonExportAspect',
            gridSelector: 'baseInsCheckGrid',
            buttonSelector: '#baseInsCheckGrid #btnExport',
            controllerName: 'BaseInsCheck',
            actionName: 'Export'
        },
        {
            /*
            аспект взаимодействия таблицы проверок и формы добавления и Панели редактирования,
            открывающейся в боковой вкладке
            */
            xtype: 'gkhgrideditformaspect',
            name: 'basePlanActionGridWindowAspect',
            gridSelector: 'basePlanActionGrid',
            editFormSelector: 'basePlanActionAddWindow',
            storeName: 'BasePlanAction',
            modelName: 'BasePlanAction',
            editWindowView: 'baseplanaction.AddWindow',
            controllerEditName: 'B4.controller.baseplanaction.Navigation',

            otherActions: function (actions) {
                actions[this.editFormSelector + ' [name=Contragent]'] = {
                    'beforeload': { fn: this.onBeforeLoadContragent, scope: this },
                    'triggerClear': { fn: this.onClearContragent, scope: this }
                };
                actions[this.editFormSelector + ' [name=PersonInspection]'] = { 'change': { fn: this.changeTypePersonInspection, scope: this } };

                actions['basePlanActionPanel' + ' b4selectfield[name=FilterPlan]'] = { 'change': { fn: this.onChangePlan, scope: this } };
                actions['basePlanActionFilterPanel' + ' b4updatebutton'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['basePlanActionGrid #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            
            onUpdateGrid: function () {
                var str = this.controller.getStore('BasePlanAction');
                str.currentPage = 1;
                str.load();
            },
            
            onBeforeLoadContragent: function (store, operation) {
                var form = this.getForm(),
                    typeJurOrg = form.down('[name=TypeJurPerson]').getValue();
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = typeJurOrg;
            },
            
            onClearContragent: function (fld) {
                fld.setValue(null);
            },
            
            changeTypePersonInspection: function (fld) {
                var form = fld.up(this.editFormSelector),
                    fldTypeJurPerson = form.down('[name=TypeJurPerson]'),
                    fldContragent = form.down('[name=Contragent]'),
                    fldPerson = form.down('[name=PhysicalPerson]');
                    
                if (fld.getValue() == 20) {
                    fldTypeJurPerson.setDisabled(false);
                    fldContragent.setDisabled(false);
                    fldPerson.setDisabled(true);
                } else {
                    fldTypeJurPerson.setDisabled(true);
                    fldContragent.setDisabled(true);
                    fldPerson.setDisabled(false);
                }
            },

            //-----Фильтры
            onChangePlan: function (field, newValue) {
                var mainViewParams = this.controller.getMainView().params;
                if (mainViewParams && newValue) {
                    mainViewParams.planId = newValue.Id;
                } else {
                    mainViewParams.planId = null;
                }
            },

            onChangeCheckbox: function (field, newValue) {
                this.controller.getMainView().params.showCloseInspections = newValue;
                this.onUpdateGrid();
            },
            
            deleteRecord: function (record) {
                var me = this,
                    grants,
                    model,
                    rec;
                
                if (record.getId()) {
                    this.controller.getAspect('deletePlanActionStatePerm').loadPermissions(record)
                        .next(function (response) {
                            grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        model = me.getModel(record);

                                        rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }
                        }, this);
                }
            }
        }
    ],

    init: function () {
        var actions = {};
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);

        this.getStore('BasePlanAction').on('beforeload', this.onBeforeLoad, this);
    },

    onMainViewAfterRender: function () {
        var me = this,
            mainView = me.getMainView();
           //делаем запрос на получение стартовых значений фильтра

        if (mainView) {
            me.mask('Загрузка', me.getMainView());
            mainView.params = {};
            mainView.params.planId = null;

            B4.Ajax.request(B4.Url.action('GetStartFilters', 'BasePlanAction'))
                .next(function (response) {
                    me.unmask();
                    if (!Ext.isEmpty(response.responseText)) {
                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var panel = me.getMainView();

                        var sfld = panel.down('b4selectfield[name=FilterPlan]');
                        
                        sfld.setValue(obj.planId);
                        sfld.updateDisplayedText(obj.planName);

                        mainView.params = mainView.params || {};
                        
                        mainView.params.planId = obj.planId;
                        
                        me.getStore('BasePlanAction').load();
                    }

                }).error(function () {
                    me.unmask();
                });
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            mainView = me.getMainView(),
            mainViewParams = mainView.params,
            params = operation.params;
        
        params.planId = mainView.down('b4selectfield[name=FilterPlan]').getValue();
        params.showCloseInspections = mainViewParams.showCloseInspections;
    },
    
    index: function () {
        var view = this.getMainView() || Ext.widget('basePlanActionPanel');
        this.bindContext(view);
        this.application.deployView(view);
    }
});