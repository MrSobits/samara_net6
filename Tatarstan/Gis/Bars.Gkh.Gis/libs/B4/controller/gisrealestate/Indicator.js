Ext.define('B4.controller.gisrealestate.Indicator', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
      'indicator.Grid',
      'gisrealestatetype.IndicatorGridEditWindow'
    ],

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    mainView: 'indicator.Grid',
    mainViewSelector: 'indicatorgrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'indicatorGridWindowAspect',
            gridSelector: 'indicatorgrid',
            editFormSelector: 'indicatorgrideditwindow',
            modelName: 'gisrealestate.IndicatorServiceComparison',
            editWindowView: 'gisrealestatetype.IndicatorGridEditWindow'
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('indicatorgrid');
        this.bindContext(view);
        this.application.deployView(view);
    }
});