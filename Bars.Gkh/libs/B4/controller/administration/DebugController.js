Ext.define('B4.controller.administration.DebugController', {
    extend: 'B4.base.Controller',
    requires: [
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: '',
    mainViewSelector: '',

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function (viewName) {
        var me = this,
            view;
        Ext.syncRequire(viewName);
        view = Ext.create(viewName);

        me.bindContext(view);
        me.application.deployView(view);
    },
});