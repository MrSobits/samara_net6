Ext.define('B4.controller.MobileAppAccountComparsion', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    licenseActionId: null,
  
    models: [
        'MobileAppAccountComparsion'
    ],
    stores: [
        'MobileAppAccountComparsion'
    ],
    views: [
        'mobileappaccountcomparsion.Grid',
        'mobileappaccountcomparsion.EditWindow',
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'mobileappaccountcomparsionGridAspect',
            gridSelector: 'mobileappaccountcomparsiongrid',
            editFormSelector: '#mobileappaccountcomparsionEditWindow',
            storeName: 'MobileAppAccountComparsion',
            modelName: 'MobileAppAccountComparsion',
            editWindowView: 'mobileappaccountcomparsion.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowClose'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onChangeCheckbox: function (field, newValue) {
                debugger;
                this.controller.params.showCloseAppeals = newValue;
                this.controller.getStore('MobileAppAccountComparsion').load();
            },
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    if (rec.getId()) {
                        asp.controller.mask('Отметка о прочтении', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('TrySetOpenRecord', 'PersonAccountOperation', {
                            docId: rec.getId()
                        })).next(function (response) {
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    var me = this;
                }
            }
        },
   
    ],

    mainView: 'mobileappaccountcomparsion.Grid',
    mainViewSelector: 'mobileappaccountcomparsiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mobileappaccountcomparsiongrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this;
        me.params = {};
        this.getStore('MobileAppAccountComparsion').on('beforeload', this.onBeforeLoadDoc, this);
        this.callParent(arguments);
    },

    onBeforeLoadDoc: function (store, operation) {
        debugger;
        if (this.params) {
            operation.params.showCloseAppeals = this.params.showCloseAppeals;
        }
    },

    index: function () {
        this.params = {};
        var view = this.getMainView() || Ext.widget('mobileappaccountcomparsiongrid');
        this.params.showCloseAppeals = view.down('#cbShowClose').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('MobileAppAccountComparsion').load();
    }
});