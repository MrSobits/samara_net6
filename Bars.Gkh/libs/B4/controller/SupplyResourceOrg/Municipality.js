Ext.define('B4.controller.supplyresourceorg.Municipality', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'supplyresourceorg.Municipality',
        'SupplyResourceOrg'
    ],

    stores: [
        'supplyresourceorg.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'supplyresourceorg.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'supplyresourceorg.MunicipalityGrid',
    mainViewSelector: 'supplyresorgmugrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /* 
            Аспект взаимодействия таблицы МО домов с массовой формой выбора МО
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'supplyResOrgMuAspect',
            gridSelector: 'supplyresorgmugrid',
            storeName: 'supplyresourceorg.Municipality',
            modelName: 'supplyresourceorg.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#supplyResOrgMuMultiSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
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
                            url: B4.Url.action('AddMunicipalities', 'SupplyResourceOrgMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                supplyResOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'supplyresorgId')
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
            }
        }
    ],

    init: function () {
        this.getStore('supplyresourceorg.Municipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgmugrid');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        view.getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'supplyresorgId');
    }
});