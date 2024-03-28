Ext.define('B4.controller.publicservorg.Municipality', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'publicservorg.Municipality',
        'PublicServiceOrg'
    ],

    stores: [
        'publicservorg.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'publicservorg.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'publicservorg.MunicipalityGrid',
    mainViewSelector: 'publicservorgmunicipalitygrid',

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
            name: 'publicServOrgMuAspect',
            gridSelector: 'publicservorgmunicipalitygrid',
            storeName: 'publicservorg.Municipality',
            modelName: 'publicservorg.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#publicServOrgMuMultiSelectWindow',
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
                            url: B4.Url.action('AddMunicipalities', 'PublicServiceOrgMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                supplyResOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'publicservorgid')
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
        this.getStore('publicservorg.Municipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('publicservorgmunicipalitygrid');

        me.bindContext(view);
        me.setContextValue(view, 'publicservorgid', id);
        me.application.deployView(view, 'public_servorg');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'publicservorgid');
    }
});