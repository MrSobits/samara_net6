Ext.define('B4.controller.OtherService', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.otherservice.State',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'otherservice.OtherService',
        'otherservice.ProviderOtherService',
        'otherservice.TariffForConsumersOtherService'
    ],
    stores:[
            'otherservice.OtherService',
            'otherservice.ProviderOtherService',
            'otherservice.TariffForConsumersOtherService'
    ],
    views: [
        'otherservice.Grid',
        'otherservice.editwindow.EditWindow',
        'otherservice.editwindow.ProviderGrid',
        'otherservice.editwindow.TariffGrid',
        'otherservice.editwindow.ProviderGridEditWindow'
    ],

    mainView: 'otherservice.Grid',
    mainViewSelector: 'otherservicegrid',

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'otherServiceAspect',
            storeName: 'otherservice.OtherService',
            modelName: 'otherservice.OtherService',
            gridSelector: 'otherservicegrid',
            listeners: {
                beforesave: function(asp, store) {
                    store.each(function(rec) {
                        if (!rec.get('Id')) {
                            rec.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                        }
                    });
                    return true;
                }
            }
        },
        {
            xtype: 'otherservicestateperm',
            name: 'otherServicePermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'otherServiceGridWindowAspect',
            gridSelector: 'otherservicegrid',
            modelName: 'otherservice.OtherService',
            storeName: 'otherservice.OtherService',
            editFormSelector: 'otherserviceeditwindow',
            editWindowView: 'otherservice.editwindow.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.params.otherServiceId = record.getId();
                    asp.controller.getStore('otherservice.TariffForConsumersOtherService').load();
                    asp.controller.getStore('otherservice.ProviderOtherService').load();
                }
            },

            otherActions: function (actions) {
                actions['otherserviceeditwindow [name=changeProviderButton]'] = { 'click': { fn: this.changeProvider, scope: this, asp: 'providerServiceGridWindowAspect' } };
            },

            changeProvider: function (btn, event, providerAspect) {
                var aspect = this.controller.getAspect(providerAspect.asp);
                aspect.editRecord(null);
            },
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'tariffForConsumersGridAspect',
            storeName: 'otherservice.TariffForConsumersOtherService',
            modelName: 'otherservice.TariffForConsumersOtherService',
            gridSelector: '#tariffGrid',
            saveButtonSelector: '#tariffGrid #tariffSaveButton',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    rec.set('DateStart', new Date());
                    rec.set('OtherService', asp.controller.params.otherServiceId);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'providerServiceGridWindowAspect',
            gridSelector: '#providerGrid',
            editFormSelector: 'providergrideditwindow',
            storeName: 'otherservice.ProviderOtherService',
            modelName: 'otherservice.ProviderOtherService',
            editWindowView: 'otherservice.editwindow.ProviderGridEditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    record.set('OtherService', asp.controller.params.otherServiceId);
                    return true;
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setVisibleForNumberContract(asp.controller.params.year, form);
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('otherservice.OtherService').on('beforeload', this.onBeforeServiceLoad, this);
        me.getStore('otherservice.ProviderOtherService').on('beforeload', this.onBeforeLoad, this);
        me.getStore('otherservice.TariffForConsumersOtherService').on('beforeload', this.onBeforeLoad, this);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        me.getStore('otherservice.OtherService').load();
        
        me.params.getId = function () { return me.params.disclosureInfoId; };
        me.getAspect('otherServicePermissionAspect').setPermissionsByRecord(me.params);
    },

    onBeforeServiceLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.otherServiceId = this.params.otherServiceId
        }
    },

    setVisibleForNumberContract: function (year, form) {
        var isYearGte2015 = false;
        try {
            var yearFromParams = parseInt(year.substring(0, 4));
            if (yearFromParams >= 2015) {
                isYearGte2015 = true;
            }
        } catch (e) {
        }
        // NumberContract
        // 2015 и более - показать
        // 2014 и менее - спрятать
        var tfNumberContract = form.down("textfield[name=NumberContract]");
        if (tfNumberContract) {
            tfNumberContract.setVisible(isYearGte2015);
            tfNumberContract.allowBlank = !isYearGte2015;
        }
    },
});