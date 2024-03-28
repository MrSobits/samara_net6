Ext.define('B4.controller.dict.EGRNApplicantType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.EGRNApplicantType'],
    stores: ['dict.EGRNApplicantType'],

    views: ['dict.egrnapplicanttype.Grid'],

    mainView: 'dict.egrnapplicanttype.Grid',
    mainViewSelector: 'egrnapplicanttypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'egrnapplicanttypegrid'
        }
    ],

    aspects: [
        //{
        //    xtype: 'inlinegridpermissionaspect',
        //    gridSelector: 'regionCodeGrid',
        //    permissionPrefix: 'GkhGji.Dict.RegionCode'
        //},
        {
            xtype: 'gkhinlinegridaspect',
            name: 'egrnapplicanttypeGridAspect',
            storeName: 'dict.EGRNApplicantType',
            modelName: 'dict.EGRNApplicantType',
            gridSelector: 'egrnapplicanttypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('egrnapplicanttypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.EGRNApplicantType').load();
    }
});