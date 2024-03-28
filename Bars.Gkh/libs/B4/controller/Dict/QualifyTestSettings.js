Ext.define('B4.controller.dict.QualifyTestSettings', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.qtestsettings.QualifyTestSettings'],
    stores: ['dict.qtestsettings.QualifyTestSettings'],
    views: ['dict.qtestsettings.Grid', 'dict.qtestsettings.EditWindow'],

    mainView: 'dict.qtestsettings.Grid',
    mainViewSelector: 'qtestsettingsgrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'qtestsettingsgrid'
        }
    ],

    aspects: [
       
        {
            xtype: 'grideditwindowaspect',
            name: 'qtestsettingsGridWindowAspect',
            gridSelector: 'qtestsettingsgrid',
            editFormSelector: '#qtestsettingsEditWindow',
            storeName: 'dict.qtestsettings.QualifyTestSettings',
            modelName: 'dict.qtestsettings.QualifyTestSettings',
            editWindowView: 'dict.qtestsettings.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('qtestsettingsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.qtestsettings.QualifyTestSettings').load();
    }
});