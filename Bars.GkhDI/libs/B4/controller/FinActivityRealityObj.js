Ext.define('B4.controller.FinActivityRealityObj', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid'
    ],

    models:
    [
        'DisclosureInfoRealityObj',
        'finactivity.RealityObjCommunalService'
    ],
    
    stores: [
        'finactivity.RealityObjCommunalService'
    ],
    
    views:
    [
        'finactivity.RealityObjEditPanel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'finactivity.RealityObjEditPanel',
    mainViewSelector: '#finActivityRealityObjEditPanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'finActivityRealityObjEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: '#finActivityRealityObjEditPanel',
            saveRecord: function (rec) {
                var me = this;
                if (rec.get('RealityObject')) {
                    rec.set('RealityObject', rec.get('RealityObject').Id);
                }

                if (me.fireEvent('beforesave', me, rec) !== false) {
                    me.saveRecordHasUpload(rec);
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'finActivityRealityObjCommunalServiceGridAspect',
            gridSelector: '#finActivityRealityObjCommunalServiceGrid',
            storeName: 'finactivity.RealityObjCommunalService',
            modelName: 'finactivity.RealityObjCommunalService',
            saveButtonSelector: '#finActivityRealityObjEditPanel b4savebutton',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) {

                    rec.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                    rec.set('Id', 0);
                    records.push(rec.data);
                });

                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'FinActivityRealityObjCommunalService', {
                    records: Ext.JSON.encode(records),
                    disclosureInfoRealityObjId: asp.controller.params.disclosureInfoRealityObjId
                })).next(function () {
                    asp.updateGrid();
                    asp.controller.unmask();
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        }
    ],
    
    init: function () {

        this.getStore('finactivity.RealityObjCommunalService').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('finActivityRealityObjEditPanelAspect').setData(this.params.disclosureInfoRealityObjId);
        }
        
        this.getStore('finactivity.RealityObjCommunalService').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
        }
    }
});
