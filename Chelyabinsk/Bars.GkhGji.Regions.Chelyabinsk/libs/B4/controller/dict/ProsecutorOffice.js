Ext.define('B4.controller.dict.ProsecutorOffice', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['dict.ProsecutorOffice'],
    stores: ['dict.ProsecutorOffice'],

    views: ['dict.prosecutoroffice.Grid'],

    mainView: 'dict.prosecutoroffice.Grid',
    mainViewSelector: 'prosecutorofficegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'prosecutorofficegrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'inlinegridpermissionaspect',
        //    gridSelector: 'regionCodeGrid',
        //    permissionPrefix: 'GkhGji.Dict.RegionCode'
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'prosecutЩrofficeGridAspect',
            storeName: 'dict.ProsecutorOffice',
            modelName: 'dict.ProsecutorOffice',
            gridSelector: 'prosecutorofficegrid'
        }
    ],

    init: function () {
        this.control({

            'prosecutorofficegrid #btnGetProsecutorOffice': { click: { fn: this.sendGetPODictRequest, scope: this } }           

        });

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('prosecutorofficegrid');
      
        this.bindContext(view);
        this.application.deployView(view);
        
        this.getStore('dict.ProsecutorOffice').load();
    },

    sendGetPODictRequest: function (btn) {
        var me = this;
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        var result = B4.Ajax.request(B4.Url.action('SendAskProsecOfficesRequest', 'GISERPExecute', {
            
        }))
            .next(function (response) {
                me.unmask();
                var data = Ext.decode(response.responseText);
                Ext.Msg.alert('Сообщение', data.data);

                return true;
            }).error(function () {
                me.unmask();

            });
    }
});