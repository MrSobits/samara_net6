Ext.define('B4.controller.dict.RegionCodeMVD', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RegionCodeMVD'],
    stores: ['dict.RegionCodeMVD'],

    views: ['dict.regioncode.Grid'],

    mainView: 'dict.regioncode.Grid',
    mainViewSelector: 'regioncodegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'regioncodegrid'
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
            name: 'regionCodeGridAspect',
            storeName: 'dict.RegionCodeMVD',
            modelName: 'dict.RegionCodeMVD',
            gridSelector: 'regioncodegrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('regioncodegrid');
      
        this.bindContext(view);
        this.application.deployView(view);
        
        this.getStore('dict.RegionCodeMVD').load();
    }
});