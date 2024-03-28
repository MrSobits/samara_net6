Ext.define('B4.controller.al.Kp60Reports', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    views: ['al.Kp60Reports'],

    refs: [
        {
            ref: 'mainView',
            selector: 'kp60reportsframe'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('kp60reportsframe');
        me.bindContext(view);
        me.application.deployView(view);
    }
});