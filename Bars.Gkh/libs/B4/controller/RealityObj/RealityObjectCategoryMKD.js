Ext.define('B4.controller.realityobj.RealityObjectCategoryMKD', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
    },

    models: [
        'realityobj.RealityObjectCategoryMKD'
    ],

    stores: [
        'realityobj.RealityObjectCategoryMKD',
        'cscalculation.CategoryCSMKDForSelect',
        'cscalculation.CategoryCSMKDForSelected'
    ],

    views: [
        'realityobj.CategoryGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjcategorygrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'realityobjcategorygridAspect',
            gridSelector: 'realityobjcategorygrid',
            storeName: 'realityobj.RealityObjectCategoryMKD',
            modelName: 'realityobj.RealityObjectCategoryMKD',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityobjcategoryMultiSelectWindow',
            storeSelect: 'cscalculation.CategoryCSMKDForSelect',
            storeSelected: 'cscalculation.CategoryCSMKDForSelected',
            titleSelectWindow: 'Выбор категорий',
            titleGridSelect: 'Категории для отбора',
            titleGridSelected: 'Выбранные категории',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeCategoryCS', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Тип', xtype: 'gridcolumn', dataIndex: 'TypeCategoryCS', flex: 1, filter: { xtype: 'textfield' } }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (item) {
                        recordIds.push(item.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddCategoryes', 'CSCalculationOperations'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                realityObjectId: asp.controller.getContextValue(asp.controller.getMainComponent(), 'realityObjectId')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать категории МКД');
                        return false;
                    }
                    return true;
                }
            }
        }       
    ],

    init: function () {
        var me = this;
        
        me.getStore('realityobj.RealityObjectCategoryMKD').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjcategorygrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.RealityObjectCategoryMKD').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});