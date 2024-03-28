Ext.define('B4.controller.publicservorg.RealtyObject', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'publicservorg.RealtyObject',
        'PublicServiceOrg'
    ],

    stores: [
        'publicservorg.RealtyObjForSelect',
        'publicservorg.RealtyObjForSelected',
        'publicservorg.RealtyObject'
    ],

    views: [
        'publicservorg.RealtyObjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'publicservorg.RealtyObjectGrid',
    mainViewSelector: 'publicServOrgRoGrid',

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
            name: 'publicServOrgRoAspect',
            gridSelector: 'publicServOrgRoGrid',
            storeName: 'publicservorg.RealtyObject',
            modelName: 'publicservorg.RealtyObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#publicServOrgRoMultiSelectWindow',
            storeSelect: 'publicservorg.RealtyObjForSelect',
            storeSelected: 'publicservorg.RealtyObjForSelected',
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
                            url: B4.Url.action('AddRealtyObjects', 'PublicServiceOrgRealtyObject'),
                            method: 'POST',
                            params: {
                                roIds: Ext.encode(recordIds),
                                publicServOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'publicservorgid')
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
                if (this.controller.params) {
                    operation.params.publicSupplyResOrgId = this.controller.getContextValue(this.controller.getMainView(), 'publicservorgid');
                }
            }
        }
    ],

    init: function () {
        this.getStore('publicservorg.RealtyObject').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('publicServOrgRoGrid');

        me.bindContext(view);
        me.setContextValue(view, 'publicservorgid', id);
        me.application.deployView(view, 'public_servorg');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.publicServOrgId = this.getContextValue(this.getMainView(), 'publicservorgid');
        if (this.params) {
            operation.params.contragentId = this.params.get('ContragentId');
        }
    }
});