Ext.define('B4.controller.InfoAboutPaymentHousing', {
    extend: 'B4.base.Controller',
 views: [ 'infoaboutpaymenthousing.Grid' ], 

    requires:
    [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.infoaboutpaymenthousing.State'
    ],

    models:
    [
        'InfoAboutPaymentHousing'
    ],
    stores:
    [
        'InfoAboutPaymentHousing'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    mainView: 'infoaboutpaymenthousing.Grid',
    mainViewSelector: '#infoAboutPaymentHousingGrid',

    aspects: [
        {
            xtype: 'infoaboutpaymenthousingstateperm',
            name: 'infoAboutPaymentHousingPermissionAspect'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'infoAboutPaymentHousingGridAspect',
            storeName: 'InfoAboutPaymentHousing',
            modelName: 'InfoAboutPaymentHousing',
            gridSelector: '#infoAboutPaymentHousingGrid',
            
            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) {
                    records.push({ Id: rec.getId(), GeneralAccrual: rec.get('GeneralAccrual'), Collection: rec.get('Collection') });
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('SaveInfoAboutPaymentHousing', 'InfoAboutPaymentHousing', {
                    records: Ext.JSON.encode(records),
                    disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                })).next(function () {
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение', 'Сохранено успешно', 'success');
                    asp.controller.unmask();
                    return true;
                }).error(function() {
                    B4.QuickMsg.msg('Сохранение', 'Не удалось сохранить данные', 'error');
                    asp.controller.unmask();
                    return false;
                });
            }
        }
    ],
    
    init: function () {
        this.getStore('InfoAboutPaymentHousing').on('beforeload', this.onBeforeLoad, this);
        
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('InfoAboutPaymentHousing').load();
        
        if (this.params) {
            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('infoAboutPaymentHousingPermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
    }
});
