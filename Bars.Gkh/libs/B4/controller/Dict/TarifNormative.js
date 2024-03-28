Ext.define('B4.controller.dict.TarifNormative', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax', 'B4.Url',
    ],

    models: ['cscalculation.TarifNormative',
        'dict.Municipality'
    ],
    stores: ['cscalculation.TarifNormative',
        'dict.MunicipalityForSelect',
        'dict.SettlementWithMoForSelect',
        'dict.SettlementWithMoForSelected',
        'dict.MunicipalityForSelected'
    ],
    views: [
        'dict.tarifnormative.Grid',
        'dict.tarifnormative.EditWindow',
        'dict.tarifnormative.MultiSelectWindowMunicipality'
    ],
    mainView: 'dict.tarifnormative.Grid',
    mainViewSelector: 'tarifnormativegrid',
    calcId: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'tarifnormativegrid'
        },
        {
            ref: 'tarifnormativeEditWindow',
            selector: 'tarifnormativeEditWindow'
        }
    ],
    mkdlicrequestId: null,

    aspects: [        
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'tarifnormativegridAspect',
        //    gridSelector: 'tarifnormativegrid',
        //    editFormSelector: '#tarifnormativeEditWindow',
        //    storeName: 'cscalculation.TarifNormative',
        //    modelName: 'cscalculation.TarifNormative',
        //    editWindowView: 'dict.tarifnormative.EditWindow'
        //},        
        {
            /* 
               Аспект взаимодействия таблицы Исполнителей и грида с массовым доабавлением
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'tarifnormativegridWindowAspect',
            gridSelector: 'tarifnormativegrid',
            storeName: 'cscalculation.TarifNormative',
            modelName: 'cscalculation.TarifNormative',
            multiSelectWindow: 'dict.tarifnormative.MultiSelectWindowMunicipality',
            multiSelectWindowSelector: '#tarifnormativemultiSelectWindowMunicipality',
            editFormSelector: '#tarifnormativeEditWindow',
            editWindowView: 'dict.tarifnormative.EditWindow',
            storeSelect: 'dict.SettlementWithMoForSelect',
            storeSelected: 'dict.SettlementWithMoForSelected',
            titleSelectWindow: 'Выбор районов',
            titleGridSelect: 'Муниципальные районы',
            titleGridSelected: 'Выбранные районы',
            columnsGridSelect: [
                { header: 'Район', xtype: 'gridcolumn', dataIndex: 'ParentMo', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Район', xtype: 'gridcolumn', dataIndex: 'ParentMo', flex: 1 },
                { header: 'Образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            listeners: {
                beforesave: function (aspect, rec) { //Перекрываем для поддержки загрузки файла
                    var win = Ext.ComponentQuery.query('#tarifnormativeEditWindow')[0];
                    var frm = win.getForm();
                    win.mask('Сохранение', frm);
                    frm.submit({
                        url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                        params: {
                            records: Ext.encode([rec.getData()])
                        },
                        success: function (form, action) {
                            win.unmask();
                            aspect.updateGrid();

                            win.close();
                        },
                        failure: function (form, action) {
                            win.unmask();
                            win.fireEvent('savefailure', rec, action.result.message);
                            Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                        }
                    });

                    return false;
                },
                getdata: function (asp, records) {
                    var recordIds = [];
                    debugger;
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });
                    debugger;
                    var tfName = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #tfName')[0];
                    if (!tfName.allowBlank && !tfName.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать наименование');
                        return false;
                    }

                    var tfCode = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #tfCode')[0];
                    if (!tfCode.allowBlank && !tfCode.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать код тарифа/норматива');
                        return false;
                    }

                    var tfValue = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #tfValue')[0];
                    if (!tfValue.allowBlank && !tfValue.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать значение');
                        return false;
                    }                  

                    var tfUnitMeasure = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #tfUnitMeasure')[0];
                    if (!tfUnitMeasure.allowBlank && !tfUnitMeasure.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать единицу измерения');
                        return false;
                    }

                    var dfDateFrom = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #dfDateFrom')[0];
                    if (!dfDateFrom.allowBlank && !dfDateFrom.value) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо указать дату начала');
                        return false;
                    }

                    var dfDateTo = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #dfDateTo')[0];

                    var sfCategoryCSMKD = Ext.ComponentQuery.query('#tarifnormativemultiSelectWindowMunicipality #sfCategoryCSMKD')[0];

                    if (recordIds[0] <= 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать район');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddTarifs', 'CSCalculationOperations', {
                        municipalityIds: Ext.encode(recordIds),
                        tfName: tfName.value,
                        tfCode: tfCode.value,
                        tfValue: tfValue.value,
                        tfUnitMeasure: tfUnitMeasure.value,
                        dfDateFrom: dfDateFrom.value,
                        dfDateTo: dfDateTo.value,
                        categoryId: sfCategoryCSMKD.value ? sfCategoryCSMKD.value.Id : 0
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранено!', 'Тарифы/нормативы сохранены успешно');
                        return true;
                    }).error(function (result) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка', result.message ? result.message : 'Произошла ошибка');
                    });

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    this.controller.getAspect('appcitsExecutantStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                },
                panelrendered: function (asp, prm) {
                    var me = this,
                        autoPerformanceDate = Gkh.config.HousingInspection.SettingTheVerification.AutoPerformanceDate;

                    if (autoPerformanceDate) {
                        var performanceDateValue = me.controller.getStore('AppealCits').getById(me.controller.appealCitizensId).get('CheckTime'),
                            performanceDateEl = prm.window.down('#dfPerformanceDate');

                        performanceDateEl.setValue(performanceDateValue);
                        performanceDateEl.setDisabled(true);
                    }
                },
            }
        }
        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('tarifnormativegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('cscalculation.TarifNormative').load();
    },

    init: function () {
        var me = this;
        me.callParent(arguments);
    }
});