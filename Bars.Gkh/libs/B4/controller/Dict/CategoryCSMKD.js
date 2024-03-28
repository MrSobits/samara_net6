Ext.define('B4.controller.dict.CategoryCSMKD', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'       
    ],

    models: ['cscalculation.CategoryCSMKD'],
    stores: ['cscalculation.CategoryCSMKD'],
    views: [
        'dict.categorycsmkd.Grid',
        'dict.categorycsmkd.EditWindow'
    ],
    mainView: 'dict.categorycsmkd.Grid',
    mainViewSelector: 'categorycsmkdgrid',
    calcId: null,
    refs: [
        {
            ref: 'mainView',
            selector: 'categorycsmkdgrid'
        },
        {
            ref: 'categorycsmkdEditWindow',
            selector: 'categorycsmkdEditWindow'
        }
    ],
    mkdlicrequestId: null,

    aspects: [

        
        {
            xtype: 'grideditwindowaspect',
            name: 'categorycsmkdgridAspect',
            gridSelector: 'categorycsmkdgrid',
            editFormSelector: '#categorycsmkdEditWindow',
            storeName: 'cscalculation.CategoryCSMKD',
            modelName: 'cscalculation.CategoryCSMKD',
            editWindowView: 'dict.categorycsmkd.EditWindow'
        },        
        
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    index: function () {

        this.params = {};
        var view = this.getMainView() || Ext.widget('categorycsmkdgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('cscalculation.CategoryCSMKD').load();
    },

    init: function () {
        var me = this;
        me.callParent(arguments);
    }
});