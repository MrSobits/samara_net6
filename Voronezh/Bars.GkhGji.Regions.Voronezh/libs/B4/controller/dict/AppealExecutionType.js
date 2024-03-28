Ext.define('B4.controller.dict.AppealExecutionType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.AppealExecutionType'],
    stores: ['dict.AppealExecutionType'],

    views: ['dict.appealexecutiontype.Grid'],

    mainView: 'dict.appealexecutiontype.Grid',
    mainViewSelector: 'appealexecutiontypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealexecutiontypegrid'
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
            name: 'appealexecutiontypegridaspect',
            storeName: 'dict.AppealExecutionType',
            modelName: 'dict.AppealExecutionType',
            gridSelector: 'appealexecutiontypegrid'
        }
    ],

    index: function () {
        debugger;
        var view = this.getMainView() || Ext.widget('appealexecutiontypegrid');
        debugger;
        this.bindContext(view);
        this.application.deployView(view);
        debugger;
        this.getStore('dict.AppealExecutionType').load();
    }
});