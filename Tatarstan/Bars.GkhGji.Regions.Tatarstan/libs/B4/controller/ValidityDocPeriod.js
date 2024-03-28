Ext.define('B4.controller.ValidityDocPeriod', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    models: [
        'B4.model.ValidityDocPeriod'
    ],

    stores: [
        'B4.store.ValidityDocPeriod'
    ],

    views: [
        'B4.view.validitydocperiod.Grid',
        'B4.view.validitydocperiod.EditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'validitydocperiod.Grid',
    mainViewSelector: 'validitydocperiodgrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'periodsValidityDocGridWindowAspect',
            gridSelector: 'validitydocperiodgrid',
            editFormSelector: 'validitydocperiodeditwindow',
            modelName: 'ValidityDocPeriod',
            editWindowView: 'validitydocperiod.EditWindow'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        view.getStore().load();
    }
});