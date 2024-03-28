Ext.define('B4.controller.dict.ReportingPeriod', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'reportingPeriodGrid',
            permissionPrefix: 'GkhDi.Reforma.Dictionaries.ReportingPeriod'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'reportingPeriodGridAspect',
            storeName: 'dict.ReportingPeriod',
            modelName: 'dict.ReportingPeriod',
            gridSelector: 'reportingPeriodGrid'
        }
    ],

    models: ['dict.ReportingPeriod'],
    stores: ['dict.ReportingPeriod'],
    views: ['dict.reportingperiod.Grid'],

    mainView: 'dict.reportingperiod.Grid',
    mainViewSelector: 'reportingPeriodGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'reportingPeriodGrid'
        }
    ],

    init: function() {
        this.control({
            'reportingPeriodGrid [name=Is_988]': {
                'beforecheckchange': {
                    fn: this.onBeforeIs988Change,
                    scope: this
                }
            }
        });

        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainView() || Ext.widget('reportingPeriodGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ReportingPeriod').load();
    },

    onBeforeIs988Change: function () {
        //не даем чекать, только просмотр
        return false;
    }
});