Ext.define('B4.controller.EstimateRegister', {
    /*
    * Контроллер реестра Сметы кр
    */
    extend: 'B4.base.Controller',
    params: null,
    requires:
    [
        'B4.aspects.GridEditWindow',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateGridWindowColumn',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    models:
    [
        'ObjectCr',
        'EstimateRegister'
    ],

    stores: ['EstimateRegister'],

    views:
    [
        'estimateregister.AddWindow',
        'estimateregister.Panel',
        'estimateregister.Grid'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    addWindowSelector: '#estimateRegisterAddWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'estimateRegisterPanel'
        }
    ],

    mainView: 'estimateregister.Panel',
    mainViewSelector: 'estimateRegisterPanel',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.ObjectCr.Register.EstimateCalculationViewCreate.View' }],
            name: 'viewEstimateCalcPermission'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'estimateRegisterStateTransferAspect',
            gridSelector: 'estimateregistergrid',
            stateType: 'cr_obj_estimate_calculation',
            menuSelector: 'estimateRegisterGridStateMenu'
        },
        {
             xtype: 'b4buttondataexportaspect',
             name: 'EstimateCalculationButtonExportAspect',
             gridSelector: 'estimateregistergrid',
             buttonSelector: 'estimateregistergrid #btnEstimateExport',
             controllerName: 'EstimateCalculation',
             actionName: 'Export'
        },
        {
            /*
            * Аспект взаимодействия таблицы акта вып. работ и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'estimateRegisterGridEditWindowAspect',
            gridSelector: 'estimateregistergrid',
            storeName: 'EstimateRegister',
            modelName: 'EstimateRegister',
            editFormSelector: '#estimateRegisterAddWindow',
            editWindowView: 'estimateregister.AddWindow',
            otherActions: function (actions) {
                actions['#estimateRegisterAddWindow' + ' #sfObjectCr'] = {
                    'beforeload': { fn: this.onBeforeLoadObjectCr, scope: this },
                    'change': { fn: this.onChangeAddWindowObjectCr, scope: this }
                };
                actions['#estimateRegisterAddWindow' + ' #sfTypeWorkCr'] = { 'beforeload': { fn: this.onBeforeLoadTypeWorkCr, scope: this } };
                actions['#estimateRegisterAddWindow' + ' #sfProgramCr'] = { 'change': { fn: this.onChangeAddWindowProgramCr, scope: this } };
                actions['#estimateFilterPanel #sfProgramCr'] = { 'change': { fn: this.onChangeProgramCr, scope: this } };
            },
            onSaveSuccess: function (aspect, rec) {
                var me = aspect;
                aspect.getForm().close();
                var objCrId = rec.get('ObjectCr').Id;

                if (objCrId) {
                    Ext.History.add('objectcredit/' + objCrId + '/estimatecalculation');
                }
            },
            editRecord: function (rec) {
                var me = this;

                var portal, model;
                if (rec != undefined) {
                    
                    // проверяем пермишшен просмотра пункта меню сметные расчеты
                    me.controller.getAspect('viewEstimateCalcPermission').loadPermissions(rec)
                      .next(function (response) {
                          var grants = Ext.decode(response.responseText);

                          if (grants && grants[0]) {
                              grants = grants[0];
                          }

                          // проверяем пермишшен просмотра пункта меню сметные расчеты
                          if (grants[0] == 0) {
                              Ext.Msg.alert('Сообщение', 'Просмотр сметных расчетов на данном статусе Объекта КР запрещен');
                          } else {
                              var objCrId = rec.get('ObjectCrId');

                              if (objCrId) {
                                  Ext.History.add('objectcredit/' + objCrId + '/estimatecalculation');
                              }
                          }
                      }, this);
                }
                else {
                    var id = rec ? rec.getId() : null;
                    model = me.controller.getModel(me.modelName);

                    if (id) {
                        if (me.controllerEditName) {
                            portal = me.controller.getController('PortalController');
                            //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                            if (!me.controller.hideMask) {
                                me.controller.hideMask = function () { me.controller.unmask(); };
                            }
                            me.controller.mask('Загрузка', me.controller.getMainComponent());
                            portal.loadController(me.controllerEditName, rec, portal.containerSelector, me.controller.hideMask);
                        }
                        else {
                            model.load(id, {
                                success: function (rec) {
                                    me.setFormData(rec);
                                },
                                scope: this
                            });
                        }
                    }
                    else {
                        me.setFormData(new model({ Id: 0 }));
                    }
                }
            },
            onChangeAddWindowObjectCr: function (field, newValue) {
                var addWindow = Ext.ComponentQuery.query(this.controller.addWindowSelector)[0];
                var sfTypeWorkCr = addWindow.down('#sfTypeWorkCr');
                if (newValue) {
                    this.controller.objectCrId = newValue.Id;
                    sfTypeWorkCr.setDisabled(false);
                } else {
                    sfTypeWorkCr.setValue(null);
                    sfTypeWorkCr.setDisabled(true);
                }
            },
            onChangeAddWindowProgramCr: function (field, newValue) {
                var addWindow = Ext.ComponentQuery.query(this.controller.addWindowSelector)[0];
                var sfObjectCr = addWindow.down('#sfObjectCr');
                if (newValue) {
                    this.controller.programId = newValue.Id;
                    sfObjectCr.setDisabled(false);
                } else {
                    sfObjectCr.setValue(null);
                    sfObjectCr.setDisabled(true);
                }
            },
            onBeforeLoadObjectCr: function (field, options) {
                options.params = {};
                options.params.programId = this.controller.programId;
            },
            
            onBeforeLoadTypeWorkCr: function (field, options) {
                options.params = {};
                options.params.objectCrId = this.controller.objectCrId;
            },
            
            onChangeProgramCr: function (field, newValue, options) {
                if (this.controller.params && newValue) {
                    this.controller.params.programmCrId = newValue.Id;
                } else {
                    this.controller.params.programmCrId = null;
                }
                this.controller.getStore('EstimateRegister').load();
            }
        }
    ],

    init: function () {
        this.getStore('EstimateRegister').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('estimateRegisterPanel');
        me.params = {};
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('EstimateRegister').load();

    },
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.programmCrId = this.params.programmCrId;
        }
    }
});