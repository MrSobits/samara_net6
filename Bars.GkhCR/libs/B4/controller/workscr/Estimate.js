Ext.define('B4.controller.workscr.Estimate', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.typeworkcr.EstimateCalculation',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'ObjectCr',
        'objectcr.estimate.EstimateCalculation',
        'objectcr.Estimate'
    ],

    stores: [
        'objectcr.estimate.EstimateRegisterDetail',
        'objectcr.estimate.ResStat',
        'objectcr.Estimate'
    ],

    views: [
        'workscr.EstimateEditWindow',
        'objectcr.estimate.EstimateCalculationGrid',
        'objectcr.estimate.Grid',
        'objectcr.estimate.ResStatGrid',
        'B4.view.Import.Window'
    ],

    mainView: 'objectcr.estimate.EstimateCalculationGrid',
    mainViewSelector: 'objectcrestimateregdetailgrid',

    estimateCalculationEditWindow: 'estimatecalcwin',
    estimateGrid: 'estimategrid',

    refs: [
        {
            ref: 'EstimateGrid',
            selector: 'objectcrestimateregdetailgrid'
        },
        {
            ref: 'EstimateWindow',
            selector: 'workscrestimatewin'
        },
        {
            ref: 'DetailGrid',
            selector: 'estimatecalcwin estimategrid'
        },
        {
            ref: 'ResourceGrid',
            selector: 'estimatecalcwin resstatgrid'
        }
    ],

    aspects: [
        /**
        * пермишшены по статусы Сметного расчета
        */
        {
            xtype: 'estimatecalculationtypeworkcrstateperm',
            editFormAspectName: 'estimateGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'estimateCalculationStatePerm'
        },
        /**
        * пермишшен по статусу Объекта КР
        */
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'estimateCalculationCreatePerm',
            permissions: [
                  { name: 'GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate.Create', applyTo: 'b4addbutton', selector: 'objectcrestimateregdetailgrid' }
            ]
        },
        /**
        * пермишшен по удалению записи Сметного расчета(по его статусы), вынесен в отдельный аспект для удобства
        */
        {
            //
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'GkhCr.TypeWorkCr.Register.EstimateCalculation.Delete' }],
            name: 'deleteEstimateCalculationStatePerm'
        },
        {
            //Аспект смены статуса
            xtype: 'b4_state_contextmenu',
            name: 'estimateCalculationStateTransferAspect',
            gridSelector: 'objectcrestimateregdetailgrid',
            stateType: 'cr_obj_estimate_calculation',
            menuSelector: 'estimateCalculationGridStateMenu'
        },
        {
            //Аспект взаимодействия таблицы и формы редактирования раздела сметный расчет
            xtype: 'grideditctxwindowaspect',
            name: 'estimateGridEditWindowAspect',
            gridSelector: 'objectcrestimateregdetailgrid',
            modelName: 'objectcr.estimate.EstimateCalculation',
            editFormSelector: 'workscrestimatewin',
            editWindowView: 'workscr.EstimateEditWindow',
            onSaveSuccess: function(me, record) {
                me.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function(me, record) {
                    me.setCurrentId(record.getId());
                },
                getdata: function (asp, record) {
                    if (!+record.get('Id')) {
                        
                        record.set('TypeWorkCr', asp.controller.getTypeWorkId());
                        record.set('ObjectCr', asp.controller.getObjectId());
                    }
                }
            },
            setCurrentId: function(id) {
                var me = this,
                    win = me.controller.getEstimateWindow(),
                    detailGrid = win.down('estimategrid'),
                    resGrid = win.down('resstatgrid'),
                    btnImport = win.down('#btnImport');

                win.down('.tabpanel').setActiveTab(0);

                detailGrid.setDisabled(!id);
                resGrid.setDisabled(!id);
                btnImport.setDisabled(!id);
                if (id > 0) {
                    detailGrid.getStore().load();
                    resGrid.getStore().load();
                } else {
                    detailGrid.getStore().removeAll();
                    resGrid.getStore().removeAll();
                }
            }
        },
        {
            /**
            * Аспект взаимодействия субтаблицы смет и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'EstimateInlineGridAspect',
            gridSelector: 'workscrestimatewin estimategrid',
            modelName: 'objectcr.Estimate',
            saveButtonSelector: 'workscrestimatewin estimategrid #btnSaveRecs',
            listeners: {
                beforesave: function (asp, store) {
                    var estimateId = asp.controller.getEstimateId();
                    
                    Ext.each(store.data.items, function(rec) {
                        if (!rec.get('Id')) {
                            rec.set('EstimateCalculation', estimateId);
                        }
                    });
                }
            }
        },
        {
            /**
            * Аспект взаимодействия субтаблицы ведомости ресурсов и формы редактирования
            */
            xtype: 'gkhinlinegridaspect',
            name: 'resStatInlineGridAspect',
            gridSelector: 'workscrestimatewin resstatgrid',
            storeName: 'objectcr.estimate.ResStat',
            modelName: 'objectcr.estimate.ResStat',
            saveButtonSelector: 'workscrestimatewin resstatgrid #btnSaveRecs',
            otherActions: function (actions) {
                var me = this;
                actions[me.gridSelector + ' [name=IsSumWithoutNds]'] = { 'change': { fn: me.onChangeSumWithoutNds, scope: me } };
            },
            listeners: {
                beforesave: function (asp, store) {
                    var estimateId = asp.controller.getEstimateId();
                    
                    Ext.each(store.data.items, function(rec) {
                        if (!rec.get('Id')) {
                            rec.set('EstimateCalculation', estimateId);
                        }
                    });
                }
            },
            onChangeSumWithoutNds: function(fld, newValue) {
                var me = this,
                    estRecAspect = me.controller.getAspect('estimateGridEditWindowAspect'),
                    editWin = fld.up('workscrestimatewin'),
                    rec = editWin.getRecord(),
                    frm = estRecAspect.getForm();

                if (newValue != rec.get('IsSumWithoutNds')) {
                    rec.set('IsSumWithoutNds', newValue);

                    me.controller.mask('Сохранение', editWin);
                    frm.submit({
                        url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                        params: { records: Ext.encode([rec.getData()]) },
                        success: function() {
                            me.controller.unmask();
                        },
                        failure: function(form, action) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                        }
                    });
                }
            }
        }
    ],

    index: function(id, objectId) {
        var me = this,
            view = me.getMainView(),
            model = me.getModel('ObjectCr');

        if (!view) {
            view = Ext.widget('objectcrestimateregdetailgrid');

            view.getStore().on('beforeload',
                function(s, operation) {
                    operation.params.twId = id;
                    operation.params.objectId = objectId;
                });
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();
        
        me.getAspect('estimateImportAspect').loadImportStore();
        me.getAspect('estimateCalculationCreatePerm').setPermissionsByRecord(new model({ Id: objectId }));
        me.getAspect('deleteEstimateCalculationStatePerm').setPermissionsByRecord(new model({ Id: objectId }));
    },

    init: function() {
        var me = this;
        me.control({
            'workscrestimatewin estimategrid': {
                'afterrender': function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.estimateCalculationId = me.getEstimateId();
                        operation.params.showPrimary = grid.down('#cbPrimary').getValue();
                    });
                }
            },
            'workscrestimatewin resstatgrid': {
                'afterrender': function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.estimateCalculationId = me.getEstimateId();
                    });
                }
            }
        });

        me.callParent(arguments);
    },

    getEstimateId: function() {
        var win = this.getEstimateWindow();

        if (win) return win.down('hidden[name=Id]').getValue();

        return null;
    },

    getObjectId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    },

    getTypeWorkId: function() {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    }
});