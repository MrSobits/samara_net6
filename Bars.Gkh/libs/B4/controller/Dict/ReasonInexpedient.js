Ext.define('B4.controller.dict.ReasonInexpedient', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: '#reasonInexpedientGrid',
            permissionPrefix: 'Gkh.Dictionaries.ReasonInexpedient'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'reasonInexpedientGridAspect',
            storeName: 'dict.ReasonInexpedient',
            modelName: 'dict.ReasonInexpedient',
            gridSelector: '#reasonInexpedientGrid'
        }
    ],

    models: ['dict.ReasonInexpedient'],
    stores: ['dict.ReasonInexpedient'],
    views: ['dict.reasoninexpedient.Grid'],
    
    mainView: 'dict.reasoninexpedient.Grid',
    mainViewSelector: 'reasonInexpedientGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'reasonInexpedientGrid'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('reasonInexpedientGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ReasonInexpedient').load();
    }
});