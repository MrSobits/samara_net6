Ext.define('B4.controller.supplyresourceorg.RealtyObject', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'supplyresourceorg.RealtyObject',
        'SupplyResourceOrg'
    ],

    stores: [
        'supplyresourceorg.RealtyObjForSelect',
        'supplyresourceorg.RealtyObjForSelected',
        'supplyresourceorg.RealtyObject'
    ],

    views: [
        'supplyresourceorg.RealtyObjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'supplyresourceorg.RealtyObjectGrid',
    mainViewSelector: 'supplyresorgrogrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект взаимодействия таблицы жилых домов с массовой формой выбора жилых домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'supplyResOrgRoAspect',
            gridSelector: 'supplyresorgrogrid',
            storeName: 'supplyresourceorg.RealtyObject',
            modelName: 'supplyresourceorg.RealtyObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#supplyResOrgRoMultiSelectWindow',
            storeSelect: 'supplyresourceorg.RealtyObjForSelect',
            storeSelected: 'supplyresourceorg.RealtyObjForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные жилые дома',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
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
                            url: B4.Url.action('AddRealtyObjects', 'SupplyResourceOrgRealtyObject'),
                            method: 'POST',
                            params: {
                                roIds: Ext.encode(recordIds),
                                supplyResOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'supplyresorgId')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Жилые дома сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать жилые дома');
                        return false;
                    }
                    return true;
                }
            },

            onBeforeLoad: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                    operation.params.supplyResOrgId = this.controller.getContextValue(this.controller.getMainView(), 'supplyresorgId');
            }
        }
    ],

    init: function () {
        this.getStore('supplyresourceorg.RealtyObject').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgrogrid');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        view.getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'supplyresorgId');
        if (this.params) {
            operation.params.contragentId = this.params.get('ContragentId');
        }
    }
});