Ext.define('B4.controller.skap.Skap', {
    extend: 'B4.base.Controller',
    mixins: { context: 'B4.mixins.Context' },

    views: ['skap.Skap'],
    mainView: 'skap.Skap',
    mainViewSelector: 'skapskap',

    refs: [
        {
            ref: 'mainView',
            selector: 'skapskap'
        }
    ],

    init: function () {
        var me = this;        
        me.callParent(arguments);
    },
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('skapskap');

        me.bindContext(view);
        me.application.deployView(view);
    }       
});