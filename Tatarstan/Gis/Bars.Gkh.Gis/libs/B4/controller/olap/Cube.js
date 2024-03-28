Ext.define('B4.controller.olap.Cube', {
    extend: 'B4.base.Controller',
    mixins: { context: 'B4.mixins.Context' },

    views: ['olap.Cube'],
    mainView: 'olap.Cube',
    mainViewSelector: 'olapcube',

    refs: [
        {
            ref: 'mainView',
            selector: 'olapcube'
        }
    ],

    init: function () {
        var me = this;        
        //me.control({
        //    'olapcube uxiframe': {
        //        'afterrender': function (iframe) {
        //            iframe.up('panel').setLoading(true);
        //        }
        //    }
        //});

        me.callParent(arguments);
    },
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('olapcube');

        me.bindContext(view);
        me.application.deployView(view);
    }       
});