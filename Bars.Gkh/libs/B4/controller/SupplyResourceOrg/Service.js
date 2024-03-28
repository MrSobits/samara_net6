Ext.define('B4.controller.supplyresourceorg.Service', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhGridPermissionAspect'
    ],

    models: [
        'supplyresourceorg.Service'
    ],
    stores: [
        'supplyresourceorg.Service',
        'dict.TypeServiceForSelect',
        'dict.TypeServiceForSelected'
    ],
    views: [
        'supplyresourceorg.ServiceGrid',
        'supplyresourceorg.ServiceEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'supplyresourceorg.ServiceGrid',
    mainViewSelector: 'supplyresorgservicegrid',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            //Аспект взаимодействия таблицы списка услуг поставщиков с массовой формой выбора услуг
            //При добавлении открывается форма массового выбора услуг. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'supplyResOrgServiceAspect',
            gridSelector: 'supplyresorgservicegrid',
            storeName: 'supplyresourceorg.Service',
            modelName: 'supplyresourceorg.Service',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#supplyResOrgServiceMultiSelectWindow',
            storeSelect: 'dict.TypeServiceForSelect',
            storeSelected: 'dict.TypeServiceForSelected',
            titleSelectWindow: 'Выбор типов услуг',
            titleGridSelect: 'Типы услуг',
            titleGridSelected: 'Типы услуг',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида списка типов услуг
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddTypeServiceObjects', 'SupplyResourceOrgService', {
                            objectIds: Ext.encode(recordIds),
                            supplyResOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'supplyresorgId')
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать типы услуг');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('supplyresourceorg.Service').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgservicegrid');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        view.getStore().load();
    },
    
    onBeforeLoad: function(store, operation) {
        operation.params.supplyResOrgId = this.getContextValue(this.getMainView(), 'supplyresorgId');
    }
});