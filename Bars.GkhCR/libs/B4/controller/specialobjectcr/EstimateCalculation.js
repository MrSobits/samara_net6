﻿Ext.define('B4.controller.specialobjectcr.EstimateCalculation', {
    /*
    * Контроллер раздела сметный расчет
    */
    
    extend: 'B4.controller.MenuItemController',
   
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateContextButton',
        'B4.aspects.permission.specialobjectcr.EstimateCalculation',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu',
        'B4.enums.EstimationTypeParam'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'specialobjectcr.estimate.EstimateCalculation',
        'specialobjectcr.Estimate'
    ],
    
    stores: [
        'specialobjectcr.estimate.EstimateRegisterDetail',
        'specialobjectcr.Estimate'
    ],
    
    views: [
        'specialobjectcr.estimate.EstimateCalculationEditWindow',
        'specialobjectcr.estimate.EstimateCalculationGrid',
        'specialobjectcr.estimate.Grid',
        'specialobjectcr.estimate.ResStatGrid',
    ],

    mainView: 'specialobjectcr.estimate.EstimateCalculationGrid',
    mainViewSelector: 'specialobjectcrestimateregdetailgrid',

    estimateCalculationEditWindow: 'specialobjectcrestimatecalcwin',
    estimateGrid: 'specialobjectcrestimategrid',

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    refs: [
        {
            ref: 'mainView',
            selector: 'specialobjectcrestimateregdetailgrid'
        },
        {
            ref: 'editWindow',
            selector: 'specialobjectcrestimatecalcwin'
        }
    ],

    aspects: [
        /* пермишшены по статусы Сметного расчета */
        {
            xtype: 'estimatecalcspecialobjectcrstateperm',
            editFormAspectName: 'estimateGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'estimateCalculationStatePerm'
        },
        /* пермишшен по статусу Объекта КР */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'estimateCalculationCreatePerm',
            permissions: [
                  { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate.Create', applyTo: 'b4addbutton', selector: 'specialobjectcrestimateregdetailgrid' }
            ]
        },
        /* пермишен по удалению записи Сметного расчета(по его статусы), вынесен в отдельный аспект для  удобства */
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'GkhCr.SpecialObjectCr.Register.EstimateCalculation.Delete' }
            ],
            name: 'deleteEstimateCalculationStatePerm'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'estimateCalculationStateTransferAspect',
            gridSelector: 'specialobjectcrestimateregdetailgrid',
            stateType: 'special_cr_obj_estimate_calc',
            menuSelector: 'estimateCalculationGridStateMenu'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statecontextbuttonaspect',
            name: 'estimateCalculationStateButtonAspect',
            stateButtonSelector: 'specialobjectcrestimatecalcwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('estimateGridEditWindowAspect');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('specialobjectcr.estimate.EstimateRegisterDetail');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('estimateCalculationStatePerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('estimateCalculationStatePerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела сметный расчет
            */
            xtype: 'grideditctxwindowaspect',
            name: 'estimateGridEditWindowAspect',
            gridSelector: 'specialobjectcrestimateregdetailgrid',
            editFormSelector: 'specialobjectcrestimatecalcwin',
            modelName: 'specialobjectcr.estimate.EstimateCalculation',
            editWindowView: 'specialobjectcr.estimate.EstimateCalculationEditWindow',
            otherActions: function (actions) {
                actions[this.editFormSelector + ' [name=TypeWorkCr]'] = { 'beforeload': { fn: this.onBeforeLoadTypeWork, scope: this } };
                actions[this.editFormSelector + ' id [actionName=cbPrimary]'] = { 'change': { fn: this.onChangePrimary, scope: this } };
                actions[this.editFormSelector] = { 'change': { fn: this.onChangePrimary, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var sumWithoutNdsField = form.down('specialobjectcrresstatgrid [name=IsSumWithoutNds]');

                    asp.controller.setCurrentId(record.getId());
                    asp.controller.getAspect('estimateCalculationStateButtonAspect').setStateData(record.get('Id'), record.get('State'));

                    if (sumWithoutNdsField) {
                        sumWithoutNdsField.setValue(record.get('IsSumWithoutNds'));
                    }

                    B4.Ajax.request({
                        url: B4.Url.action('GetParams', 'GkhParam', {
                            prefix: 'GkhCr'
                        })
                    }).next(function (resp) {
                        var response = Ext.decode(resp.responseText);
                        var estimationType = asp.getForm().down('[name=EstimationType]');
                        if (estimationType) {
                            if (response.data && response.data.EstimationTypeParam == B4.enums.EstimationTypeParam.Use) {
                                estimationType.show();
                            }
                        }
                    }).error(function () {
                    });
                },
                getdata: function (asp, record) {
                    var me = this;

                    if (!record.data.Id) {
                        record.set('ObjectCr', me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId'));
                    }
                }
            },
            onBeforeLoadTypeWork: function (field, options) {
                var me = this;
                options.params = {};
                options.params.objectCrId = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId');
            },
            onChangePrimary: function (field, newValue) {
                var me = this,
                    estimateGrid = me.controller.getAspect('EstimateInlineGridAspect').getGrid(),
                    editWindow = me.controller.getEditWindow(),
                    record = editWindow.getForm().getRecord(),
                    cbPrimary = estimateGrid.down('[actionName=cbPrimary]'),
                    store = estimateGrid.getStore();

                store.clearFilter(true);
                store.filter([
                            { property: 'estimateCalculationId', value: record.getId() },
                            { property: 'showPrimary', value: cbPrimary.getValue() }
                ]);
            },
            deleteRecord: function (record) {
                var me = this;

                if (record.getId()) {
                    me.controller.getAspect('deleteEstimateCalculationStatePerm').loadPermissions(record)
                        .next(function(response) {
                                var grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись? Возможно имется зависимые записи в разделах Сметы и Ведомость ресурсов', function(result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', B4.getBody());
                                        rec.destroy()
                                            .next(function() {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function(result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }

                        }, this);
                }
            }
        },
        {
            /*
            * Аспект взаимодействия субтаблицы смет и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'EstimateInlineGridAspect',
            gridSelector: 'specialobjectcrestimategrid',
            storeName: 'specialobjectcr.Estimate',
            modelName: 'specialobjectcr.Estimate',
            saveButtonSelector: 'specialobjectcrestimategrid [actionName=btnSaveRecs]',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this,
                        editWindow = me.controller.getEditWindow(),
                        record = editWindow.getForm().getRecord();

                    if (record && record.getId()) {
                        Ext.each(store.data.items, function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('EstimateCalculation', record.getId());
                            }
                        });
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия субтаблицы ведомости ресурсов и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'resStatInlineGridAspect',
            gridSelector: 'specialobjectcrresstatgrid',
            modelName: 'specialobjectcr.estimate.ResStat',
            saveButtonSelector: 'specialobjectcrresstatgrid #btnSaveRecs',
            otherActions: function (actions) {
                actions['specialobjectcrresstatgrid [name=IsSumWithoutNds]'] = { 'change': { fn: this.onChangeSumWithoutNds, scope: this } };
            },
            listeners: {
                beforesave: function (asp, store) {
                    var me = this,
                        editWindow = me.controller.getEditWindow(),
                        record = editWindow.getForm().getRecord();

                    if (record && record.getId()) {
                        Ext.each(store.data.items, function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('EstimateCalculation', record.getId());
                            }
                        });
                    }
                }
            },
            onChangeSumWithoutNds: function (fld, newValue) {
                var me = this,
                    estRecAspect = this.controller.getAspect('estimateGridEditWindowAspect'),
                    editWin = fld.up('specialobjectcrestimatecalcwin'),
                    rec = editWin.getRecord();
                
                var frm = estRecAspect.getForm();
                if (newValue != rec.get('IsSumWithoutNds')) {
                    rec.set('IsSumWithoutNds', newValue);
                    
                    me.controller.mask('Сохранение', editWin);
                    frm.submit({
                        url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                        params: { records: Ext.encode([rec.getData()]) },
                        success: function () {
                            me.controller.unmask();
                        },
                        failure: function (form, action) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                        }
                    });
                }
            }
        }
    ],

    init: function () {
        var me = this;
        var actions = {};

        actions[me.mainViewSelector] = { 'afterrender': { fn: me.onMainViewAfterRender, scope: me } };
        actions['specialobjectcrresstatgrid'] = { 'store.beforeload': { fn: me.onBeforeLoadParent, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },
    
    onMainViewAfterRender: function () {
        var me = this;
        var mainView = me.getMainView();
        var estimationType = mainView.down('[dataIndex=EstimationType]');

        if (estimationType) {
            B4.Ajax.request({
                url: B4.Url.action('GetParams', 'GkhParam', {
                    prefix: 'GkhCr'
                })
            }).next(function (resp) {
                var response = Ext.decode(resp.responseText);
                if (response.data && response.data.EstimationTypeParam == B4.enums.EstimationTypeParam.Use) {
                    estimationType.show();
                } else {
                    estimationType.hide();
                }
            }).error(function () {
                estimationType.hide();
            });
        }
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrestimateregdetailgrid'),
            store;

        this.getAspect('estimateCalculationCreatePerm').setPermissionsByRecord({ getId: function() { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    },

    setCurrentId: function (id) {
        var me = this,
            editWindow = me.getEditWindow(),
            estimateGrid = editWindow.down('specialobjectcrestimategrid'),
            resStatGrid = editWindow.down('specialobjectcrresstatgrid'),
            cbPrimary = estimateGrid.down('[actionName=cbPrimary]'),
            sourceResStatStore,
            sourceEstStore;

        if (editWindow) {
            editWindow.down('.tabpanel').setActiveTab(0);
        }

        sourceEstStore = estimateGrid.getStore();
        sourceEstStore.removeAll();

        sourceResStatStore = resStatGrid.getStore();
        sourceResStatStore.removeAll();

        if (id > 0) {
            estimateGrid.setDisabled(false);
            resStatGrid.setDisabled(false);
            
            sourceEstStore.clearFilter(true);
            sourceEstStore.filter([
                { property: 'estimateCalculationId', value: id },
                { property: 'showPrimary', value: cbPrimary.getValue() }
            ]);

            sourceResStatStore.clearFilter(true);
            sourceResStatStore.filter('estimateCalculationId', id);
        } else {
            estimateGrid.setDisabled(true);
            resStatGrid.setDisabled(true);
        }
    },

    onBeforeLoadParent: function (store, operation) {
        var me = this;

        operation.params.objectCrId = me.getContextValue(me.getMainView(), 'objectcrId');
    }
});