Ext.define('B4.controller.servorg.Municipality', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'servorg.Municipality'
    ],

    stores: [
        'servorg.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'servorg.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'servorg.MunicipalityGrid',
    mainViewSelector: 'servorgmugrid',

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
            name: 'servorgMuAspect',
            gridSelector: 'servorgmugrid',
            storeName: 'servorg.Municipality',
            modelName: 'servorg.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#servorgMuMultiSelectWindow',
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
                            url: B4.Url.action('AddMunicipalities', 'ServiceOrgMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                servorgId: asp.controller.getContextValue(asp.controller.getMainView(), 'servorgId')
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
        this.getStore('servorg.Municipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgmugrid');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.servorgId = this.getContextValue(this.getMainView(), 'servorgId');
    }
});