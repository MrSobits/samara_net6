/**
* контроллер реестра настройки проверки лимитов по заявке на перечисление средств
*/
Ext.define('B4.controller.LimitCheck', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],
    
    models: [
        'LimitCheck',
        'limitcheck.FinSource'
    ],
    
    stores: [
        'LimitCheck',
        'limitcheck.FinSource',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],
    
    views: [
        'limitcheck.Grid',
        'limitcheck.EditWindow',
        'limitcheck.FinSourceGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    
    aspects: [
        {
            /* Аспект взаимодействия грида и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'limitcheckGridEditWindowAspect',
            gridSelector: 'limitCheckRfGrid',
            storeName: 'LimitCheck',
            modelName: 'LimitCheck',
            editWindowView: 'limitcheck.EditWindow',
            editFormSelector: '#limitCheckRfEditWindow',
            onSaveSuccess: function(asp, rec) {
                asp.controller.setCurrentId(rec.get('Id'));
            },
            listeners: {
                aftersetformdata: function (asp, rec) {
                    asp.controller.setCurrentId(rec.get('Id'));
                }
            }
        },
        {
            /* Аспект множественного выбора источников финансирования
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'limitcheckfinsourcemultiselect',
            modelName: 'limitcheck.FinSource',
            storeName: 'limitcheck.FinSource',
            gridSelector: '#limitcheckfinsourcegrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#limitcheckfinSourceMultiSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор источников финансирования',
            titleGridSelect: 'Разрезы финансирования для отбора',
            titleGridSelected: 'Выбранные разрезы финансирования',
            listeners: {
                getdata: function (asp, records) {
                    var objectIds = [];
                    Ext.each(records.items, function (rec) {
                        objectIds.push(rec.get('Id'));
                    });

                    if (objectIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        B4.Ajax.request(B4.Url.action('AddFinSources', 'LimitCheckFinSource', {
                            objectIds: objectIds,
                            limitCheckId: asp.controller.limitCheckId
                        })).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать разрезы финансирования');
                        return false;
                    }
                }
            }
        }
    ],
    
    mainView: 'limitcheck.Grid',
    mainViewSelector: 'limitCheckRfGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'EditWindow',
            selector: '#limitCheckRfEditWindow'
        },
        {
            ref: 'mainView',
            selector: 'limitCheckRfGrid'
        }
    ],

    //идентификатор редактируемой записи
    limitCheckId: null,

    init: function () {
        this.getStore('limitcheck.FinSource').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('limitCheckRfGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('LimitCheck').load();
    },
    
    setCurrentId: function(id) {
        this.limitCheckId = id;

        if (id) {
            this.getStore('limitcheck.FinSource').load();
        } else {
            this.getStore('limitcheck.FinSource').removeAll();
        }
        this.getEditWindow().down('#limitcheckfinsourcegrid').setDisabled(!id);
    },
    
    onBeforeLoad: function(store, operation) {
        operation.params.limitCheckId = this.limitCheckId;
    }
});