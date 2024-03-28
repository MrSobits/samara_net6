Ext.define('B4.controller.RegopServiceLog', {
    extend: 'B4.base.Controller',

    requires: [],
    mixins: {
        context: 'B4.mixins.Context'
    },
    views: ['regoprservicelog.Grid'],

    mainView: 'regoprservicelog.Grid',
    mainViewSelector: 'importLogGrid',

    refs: [{
        ref: 'mainView',
        selector: 'regoprserviceloggrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('regoprserviceloggrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});