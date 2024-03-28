Ext.define('B4.controller.HeatInputPeriod', {

    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: [
        'HeatInputPeriod',
        'heatinputperiod.Information',
        'heatinputperiod.Boiler'
    ],
    stores: [
        'HeatInputPeriod',
        'heatinputperiod.Information',
        'heatinputperiod.Boiler'
    ],
    views: [
        'heatinputperiod.EditWindow',
        'HeatInputPeriodGrid',
        'heatinputperiod.Panel',
        'heatinputperiod.HeatInputInfoGrid',
        'heatinputperiod.BoilerGrid'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },


    refs: [
        {
            ref: 'mainView',
            selector: 'heatinputperiodgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditctxwindowaspect',
            name: 'heatinputperiodGridAspect',
            gridSelector: 'heatinputperiodgrid',
            editFormSelector: 'heatinputperiodEditWindow',
            storeName: 'HeatInputPeriod',
            modelName: 'HeatInputPeriod',
            editWindowView: 'heatinputperiod.EditWindow',
            infEditWindowView: 'heatinputperiod.Panel',
            onSaveSuccess: function () {
                this.closeWindowHandler(this.getForm());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (!id) {
                    model = this.getModel(record);

                    me.setFormData(new model({ Id: 0 }));

                    me.getForm().getForm().isValid();
                } else {
                    me.showInfWindow(record);
                }
            },

            showInfWindow: function(record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    mainGrid = me.controller.getMainView(),
                    editWindow = me.controller.getView(me.infEditWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy'
                    }),
                    form = editWindow.down('form'),
                    infoStore = editWindow.down('heatInputInfoGrid').getStore(),
                    boilerStore = editWindow.down('boilerGrid').getStore();

                form.loadRecord(record);
                editWindow.down('textfield[name=MonthStr]').setValue(mainGrid.months[record.get('Month')]);

                form.getForm().isValid();

                infoStore.filter('hipId', id);

                boilerStore.filter('hipId', id);
                
                editWindow.show();
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'copyToMunicipalityMultiSelectWindowAspect',
            fieldSelector: 'copyworkpricewindow #copyToMunicipalitiesTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#copyToMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ListByParamAndOperator',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальных образований для отбора',
            titleGridSelected: 'Выбранные муниципальные образования'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'heatinputinfoPanel b4savebutton': { click: { fn: me.onSaveInfo, scope: me } },
            'heatinputinfoPanel b4closebutton': { click: { fn: me.closeInfo, scope: me } },
            'boilerGrid #boilerChangeBtn': { click: { fn: me.boilerChange, scope: me } }
        });
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('heatinputperiodgrid');
        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('HeatInputPeriod').load();
    },

    boilerChange: function (btn) {
        Ext.History.add('boilerrooms/');
    },

    closeInfo: function (btn) {
        var panel = btn.up('heatinputinfoPanel');

        panel.close();
    },

    onSaveInfo: function (btn) {
        var me = this,
            panel = btn.up('heatinputinfoPanel'),
            recordGrid = panel.down('heatInputInfoGrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];
        Ext.each(store.getModifiedRecords(), function(rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Сохранение', panel);

        B4.Ajax.request({
            url: B4.Url.action('SaveHeatInputInfo', 'HeatInputInformation'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData)
            }
        }).next(function (res) {
            me.unmask();
            store.load();
            Ext.Msg.alert('Сохранение', 'Данные успешно сохранены');

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка сохранения!', e.message);
        });
    }
});