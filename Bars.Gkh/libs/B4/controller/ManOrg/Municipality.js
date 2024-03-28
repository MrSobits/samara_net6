Ext.define('B4.controller.manorg.Municipality', {
    extend: 'B4.controller.MenuItemController',
    params: {},
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'manorg.Municipality'
    ],

    stores: [
        'manorg.Municipality',
        'dict.SettlementWithMoForSelect',
        'dict.SettlementWithMoForSelected'
    ],

    views: [
        'manorg.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'manorg.MunicipalityGrid',
    mainViewSelector: 'municipalitygrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.manorg.Navigation',

    aspects: [
        {
            /* 
            Аспект взаимодействия таблицы МО домов с массовой формой выбора МО
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'manorgMuAspect',
            gridSelector: 'municipalitygrid',
            storeName: 'manorg.Municipality',
            modelName: 'dict.SettlementWithMo',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#manorgMuMultiSelectWindow',
            storeSelect: 'dict.SettlementWithMoForSelect',
            storeSelected: 'dict.SettlementWithMoForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'ParentMo', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'ParentMo', flex: 1, sortable: false },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipalities', 'ManagingOrgMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                manorgId: asp.controller.params.id
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            },
            onBeforeLoad: function(store, operation) {
                operation.params.manorg = true;
            }
        }
    ],

    init: function () {
        this.getStore('manorg.Municipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.manorgId = this.params.id;
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('municipalitygrid');

        me.bindContext(view);
        me.setContextValue(view, 'manorgId', id);
        me.application.deployView(view, 'manorgId_info');
        
        me.params.id = id;
        me.getStore('manorg.Municipality').load();
    }
});