Ext.define('B4.controller.dict.WorkRealityObjectOutdoor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.workrealityobjectoutdoor.Grid',
        'B4.view.dict.workrealityobjectoutdoor.EditWindow',
        'B4.store.dict.WorkRealityObjectOutdoor',
        'B4.model.dict.WorkRealityObjectOutdoor',

        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.WorkRealityObjectOutdoor',
        'B4.aspects.fieldrequirement.WorkRealityObjectOutdoor'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    stores: [
        'dict.WorkRealityObjectOutdoor'
    ],

    models: [
        'dict.WorkRealityObjectOutdoor'
    ],

    views: [
        'dict.workrealityobjectoutdoor.Grid',
        'dict.workrealityobjectoutdoor.EditWindow'
    ],

    mainView: 'dict.workrealityobjectoutdoor.Grid',
    mainViewSelector: 'workrealityobjectoutdoorpanel',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'workrealityobjectoutdoorGridWindowAspect',
            gridSelector: 'workrealityobjectoutdoorpanel',
            editFormSelector: 'workrealityobjectoutdoorwindow',
            modelName: 'dict.WorkRealityObjectOutdoor',
            editWindowView: 'dict.workrealityobjectoutdoor.EditWindow'
        },
        {
            xtype: 'workoutdoorpermission'
        },
        {
            xtype: 'workrealityobjectoutdoorfieldrequirement'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});