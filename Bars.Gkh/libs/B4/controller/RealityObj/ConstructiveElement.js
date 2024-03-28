Ext.define('B4.controller.realityobj.ConstructiveElement', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.permission.realityobj.ConstructiveElement'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.ConstructiveElement',
        'dict.ConstructiveElement'
    ],

    stores: [
        'realityobj.ConstructiveElement',
        'dict.ConstructiveElementForSelect',
        'dict.ConstructiveElementForSelected'
    ],

    views: [
        'realityobj.ConstructiveElementGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjconstructiveelementgrid'
        }
    ],
    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'constructiveelementperm',
            name: 'constructiveElementPerm'
        },
        {
            //Аспект взаимодействия таблицы конструктивных элементов по дому с формой массового выбора конструктивных элементов
            //При добавлении открывается форма массового выбора конструктивных элементов. После выбора список получается через подписку 
            //на событие getdata идет добавление записей в сторе

            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'realityObjConstructiveElementAspect',
            gridSelector: 'realityobjconstructiveelementgrid',
            storeName: 'realityobj.ConstructiveElement',
            modelName: 'realityobj.ConstructiveElement',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityObjConstructiveElementMultiSelectWindow',
            storeSelect: 'dict.ConstructiveElementForSelect',
            storeSelected: 'dict.ConstructiveElementForSelected',
            titleSelectWindow: 'Выбор конструктивных элементов',
            titleGridSelect: 'Конструктивные элементы для отбора',
            titleGridSelected: 'Выбранные конструктивные элементы',
            columnsGridSelect: [
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, sortable: false },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (me, records) {
                    var store = me.controller.getStore(me.storeName);

                    records.each(function (rec) {
                        if (rec.get('Id')) {
                            var recordConstrElem = me.controller.getModel('realityobj.ConstructiveElement').create();
                            recordConstrElem.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                            recordConstrElem.set('ConstructiveElement', rec.get('Id'));
                            recordConstrElem.set('ConstructiveElementName', rec.get('Name'));
                            recordConstrElem.set('ConstructiveElementGroup', rec.get('Group'));

                            store.insert(0, recordConstrElem);
                        }
                    });
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('realityobj.ConstructiveElement').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjconstructiveelementgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.ConstructiveElement').load();
        me.getAspect('constructiveElementPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});