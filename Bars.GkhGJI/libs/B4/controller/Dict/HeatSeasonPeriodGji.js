Ext.define('B4.controller.dict.HeatSeasonPeriodGji', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow', 'B4.aspects.permission.dict.HeatSeasonPeriod'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.HeatSeasonPeriodGji', 'dict.HeatingSeasonResolution'],
    stores: ['dict.HeatSeasonPeriodGji'],
    views: [
        'dict.heatseasonperiodgji.EditWindow',
        'dict.heatseasonperiodgji.Grid',
        'dict.heatseasonperiodgji.ResolutionEditWindow'
    ],

    mainView: 'dict.heatseasonperiodgji.Grid',
    mainViewSelector: 'heatSeasonPeriodGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'heatSeasonPeriodGjiGrid'
        }, {
            ref: 'periodIdField',
            selector: 'heatseason_period_edit hidden[name=Id]'
        },
        {
            ref: 'resolutionGrid',
            selector: 'heatseason_period_edit grid'
        }
    ],

    aspects: [
        {
            xtype: 'heatseasonperiodperm'
        },
        {
            /*
            Аспект взаимодействия таблицы периодов отопительного сезона и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'heatSeasonPeriodGjiGridWindowAspect',
            gridSelector: 'heatSeasonPeriodGjiGrid',
            editFormSelector: '#heatSeasonPeriodGjiEditWindow',
            storeName: 'dict.HeatSeasonPeriodGji',
            modelName: 'dict.HeatSeasonPeriodGji',
            editWindowView: 'dict.heatseasonperiodgji.EditWindow',
            listeners: {
                'aftersetformdata': function(asp) {
                    var ctrl = asp.controller,
                        store = ctrl.getResolutionGrid().getStore();
                    store.on('beforeload', ctrl.onBeforeResolutionStoreLoad, ctrl);
                    store.load();
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'resolutioneditaspect',
            gridSelector: 'heatseason_period_edit b4grid',
            editFormSelector: 'resolutioneditwin',
            modelName: 'dict.HeatingSeasonResolution',
            editWindowView: 'dict.heatseasonperiodgji.ResolutionEditWindow',
            listeners: {
                'beforesetformdata': function (asp, rec, form) {
                    form.setTitle(rec.get('Municipality').Name);
                },
                'aftersetformdata': function (asp, rec, form) {
                    form.down('hiddenfield[name=Municipality]').setValue(rec.get('Municipality').Id);
                    form.down('hiddenfield[name=HeatSeasonPeriodGji]').setValue(asp.controller.getPeriodIdField().getValue());
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                model = me.getModel(record);

                id ? me.setFormData(record) : me.setFormData(new model({ Id: 0 }));

                me.getForm().getForm().isValid();
            },
            setFormData: function (rec) {
                var form = this.getForm();
                if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
                    form.loadRecord(rec);
                    form.getForm().isValid();
                }

                this.fireEvent('aftersetformdata', this, rec, form);
            },
            saveRecordHasUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                var data = rec.getData();
                if (rec.get('Phantom')) {
                    data['Id'] = 0;
                }
                frm.submit({
                    url: rec.getProxy().getUrl({ action: rec.get('Phantom') ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([data])
                    },
                    success: function (form, action) {
                        me.unmask();
                        me.updateGrid();

                        var model = me.getModel(rec);

                        if (action.result.data.length > 0) {
                            var id = action.result.data[0] instanceof Object ? action.result.data[0].Id : action.result.data[0];
                            model.load(id, {
                                success: function (newRec) {
                                    me.setFormData(newRec);
                                    me.fireEvent('savesuccess', me, newRec);
                                }
                            });
                        }
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            }
        }
    ],
    
    index: function () {
        var view = this.getMainView() || Ext.widget('heatSeasonPeriodGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.HeatSeasonPeriodGji').load();
    },

    onBeforeResolutionStoreLoad: function(store, operation) {
        operation.params = operation.params || {};
        operation.params.periodId = this.getPeriodIdField().getValue();
    }
});