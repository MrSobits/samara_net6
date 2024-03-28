Ext.define('B4.controller.activitytsj.RealObj', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax', 'B4.Url'
    ],

    models: ['activitytsj.RealObj'],

    mainView: 'activitytsj.RealObjGrid',
    mainViewSelector: '#activityTsjRealObjGrid',
    views: [
        'activitytsj.RealObjGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            //Аспект взаимодействия таблицы списка домов деятельности ТСЖ с массовой формой выбора жилых домов
           //При добавлении открывается форма массового выбора домов. После выбора список получается через подписку 
           //на событие getdata идет добавление записей в сторе
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'activityTsjRealObjAspect',
            gridSelector: '#activityTsjRealObjGrid',
            storeName: 'activitytsj.RealObj',
            modelName: 'activitytsj.RealObj',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#activityTsjRealObjMultiSelectWindow',
            storeSelect: 'activitytsj.RealityObjectForSelect',
            storeSelected: 'activitytsj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, sortable: false }
            ],

            onBeforeLoad: function(store, operation) {
                operation.params.realityObjIds = this.controller.realityObjIds;
            },

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида списка домов
                getdata: function(asp, records) {
                    var recordIds = [];
                    records.each(function(rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ActivityTsjRealObj', {
                            objectIds: recordIds,
                            activityTSJ: asp.controller.params.get('Id')
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        this.getStore('activitytsj.RealObj').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('activitytsj.RealObj').load();
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.activityTSJ = this.params.get('Id');
        }
    }
});