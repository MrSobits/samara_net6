Ext.define('B4.controller.servorg.Service', {
    extend: 'B4.base.Controller',

    params: null,
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: ['servorg.Service'],
    stores: [
        'servorg.Service',
        'dict.TypeServiceForSelect',
        'dict.TypeServiceForSelected'
    ],
    views: [
        'servorg.ServiceGrid',
        'servorg.ServiceEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'servorg.ServiceGrid',
    mainViewSelector: 'servorgservicegrid',

    aspects: [
        {
            //Аспект взаимодействия таблицы списка услуг поставщиков с массовой формой выбора услуг
            //При добавлении открывается форма массового выбора услуг. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'servorgServiceAspect',
            gridSelector: 'servorgservicegrid',
            storeName: 'servorg.Service',
            modelName: 'servorg.Service',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#servorgServiceMultiSelectWindow',
            storeSelect: 'dict.TypeServiceForSelect',
            storeSelected: 'dict.TypeServiceForSelected',
            titleSelectWindow: 'Выбор типов услуг',
            titleGridSelect: 'Типы услуг',
            titleGridSelected: 'Типы услуг',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида списка типов услуг
                getdata: function(asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function(item) {
                        recordIds.push(item.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddTypeServiceObjects', 'ServiceOrgService'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                servorgId: asp.controller.getContextValue(asp.controller.getMainView(), 'servorgId')
                            }
                        }).next(function() {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать типы услуг');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        this.getStore('servorg.Service').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgservicegrid');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        view.getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.servorgId = this.getContextValue(this.getMainView(), 'servorgId');
    }
});