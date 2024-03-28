Ext.define('B4.controller.AlphaBi', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    views: ['AlphaBi'],

    refs: [
        {
            ref: 'mainView',
            selector: 'rms-alphabi-frame'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('rms-alphabi-frame');
        me.bindContext(view);
        me.application.deployView(view);
    }
});