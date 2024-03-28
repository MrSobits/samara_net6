Ext.define('B4.controller.WorkWinterCondition', {

    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.view.workwintercondition.CopyWindow'
    ],

    models: [
        'HeatInputPeriod',
        'workwintercondition.Information'
    ],
    stores: [
        'HeatInputPeriod',
        'workwintercondition.Information'
    ],
    views: [
        'workwintercondition.EditWindow',
        'workwintercondition.WorkWinterConditionGrid',
        'workwintercondition.InfoGrid',
        'workwintercondition.InfoWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    refs: [
        {
            ref: 'mainView',
            selector: 'workWinterConditionGrid'
        },
         {
             ref: 'mainInfo',
             selector: 'workWinterConditionInfoGrid'
         },
         {
             ref: 'winmunicipality',
             selector: 'workWinterConditionInfoWindow [name="Municipality"]'
         },
        {
            ref: 'winmonth',
            selector: 'workWinterConditionInfoWindow [name="Month"]'
        },
        {
            ref: 'winyear',
            selector: 'workWinterConditionInfoWindow [name="Year"]'
        }
    ],

    months: {
        '1': 'Январь',
        '2': 'Февраль',
        '3': 'Март',
        '4': 'Апрель',
        '5': 'Май',
        '6': 'Июнь',
        '7': 'Июль',
        '8': 'Август',
        '9': 'Сентябрь',
        '10': 'Октябрь',
        '11': 'Ноябрь',
        '12': 'Декабрь'
    },

    aspects:
[
        { xtype: 'gkhpermissionaspect', permissions: [
            { name: 'GkhGji.WorkWinterCondition.CopyWorkWinterPeriod_Edit', applyTo: '[action=CopyWorkWinterPeriod]', selector: 'workWinterConditionInfoWindow' },  
            {
                name: 'GkhGji.WorkWinterCondition.CopyWorkWinterPeriod_View',
                applyTo: '[action=CopyWorkWinterPeriod]',
                selector: 'workWinterConditionInfoWindow',
                applyBy: function(component, allowed) {
                    if (component) {
                        component.setVisible(allowed);
                    }
                }
            }
           ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'workWinterInfoGridAspect',
            storeName: 'workwintercondition.Information',
            modelName: 'workwintercondition.Information',
            gridSelector: 'workWinterConditionInfoGrid'
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'workwinterconditionGridAspect',
            gridSelector: 'workWinterConditionGrid',
            editFormSelector: 'workwinterconditionEditWindow',
            storeName: 'HeatInputPeriod',
            modelName: 'HeatInputPeriod',
            editWindowView: 'workwintercondition.EditWindow',
            infoWindowView: 'workwintercondition.InfoWindow',
            onSaveSuccess: function (asp, rec) {
                this.closeWindowHandler(this.getForm());
                this.editRecord(rec);
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

            showInfWindow: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    municipality = record.data.Municipality,
                    editWindow = me.controller.getView(me.infoWindowView).create(
                    {
                        constrain: true,
                        renderTo: B4.getBody().getActiveTab().getEl(),
                        closeAction: 'destroy'
                    }),
                    form = editWindow.down('form'),
                    infoGrid = editWindow.down('workWinterConditionInfoGrid');

                editWindow.show();
                infoGrid.myId = id;
                infoGrid.municipalityId = municipality;
                form.loadRecord(record);
                form.getForm().isValid();

                infoGrid.getStore().on('beforeload', function (store, operation) {
                    operation.params.hipId = id;
                });
                infoGrid.getStore().load();
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
            'workWinterConditionInfoWindow b4savebutton': { click: { fn: me.onSaveInfo, scope: me } },
            'workWinterConditionInfoWindow b4closebutton': {click: { fn: me.closeWindow, scope: me } },
            'workwinterConditionCopyWindow b4closebutton': { click: { fn: me.closeCopyWindow, scope: me } },
            'workWinterConditionInfoWindow button[action=CopyWorkWinterPeriod]': { click: { fn: me.onCopyWorkWinterPeriod, scope: me } },
            'workwinterConditionCopyWindow b4savebutton': { click: { fn: me.onCopy, scope: me } }
            });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('workWinterConditionGrid');
        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('HeatInputPeriod').load();
    },

    closeWindow: function (btn) {
        btn.up('workWinterConditionInfoWindow').close();
    },

    onCopyWorkWinterPeriod: function (btn) {
       var distributionWindow = Ext.create('B4.view.workwintercondition.CopyWindow', {
            renderTo: B4.getBody().getActiveTab().getEl()
        });
        distributionWindow.show();
   },

    onSaveInfo: function (btn) {
        var me = this,
            window = btn.up('workWinterConditionInfoWindow'),
            recordGrid = window.down('workWinterConditionInfoGrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];
        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Сохранение', window);

        B4.Ajax.request({
            url: B4.Url.action('SaveChangedInfo', 'WorkWinterCondition'),
            method: 'POST',
            timeout: 5 * 60 * 1000, // 5 минут,
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
    },

    onCopy: function (btn) {
        var me = this,
            view = me.getMainView(),
            window = btn.up('workwinterConditionCopyWindow'),
            store = view.getStore(),
            info = me.getMainInfo(),
            hipId = info.myId,
            municipality = info.municipalityId,
            copyyear = window.down('[name=Year]').getValue(),
            copymonth = window.down('[name=Month]').getValue();
      
        B4.Ajax.request({
            url: B4.Url.action('CopyPeriodWorkWinterCondition', 'WorkWinterCondition'),
            method: 'POST',
            timeout: 5 * 60 * 1000, // 5 минут,
             params: {
                    copyyear: copyyear,
                    copymonth: copymonth,
                    municipality: municipality,
                    hipId: hipId
                }           
           
        }).next(function (response) {
            var obj = Ext.JSON.decode(response.responseText);
            
            if (Ext.isEmpty(obj.message))
                Ext.Msg.alert('Успешно!', 'Данные успешно сохранены');
            else
                Ext.Msg.alert('Ошибка' , obj.message);
            store.removeAll();
            info.getStore().load();
            me.unmask();
            window.close();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || e);
        });
    },      

    closeCopyWindow: function (btn) {
        btn.up('workwinterConditionCopyWindow').close();
    }
});