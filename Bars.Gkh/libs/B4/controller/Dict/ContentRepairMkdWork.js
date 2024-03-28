Ext.define('B4.controller.dict.ContentRepairMkdWork', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.ContentRepairMkdWork'],
    stores: ['dict.ContentRepairMkdWork'],
    views: [
        'dict.ContentRepairMkdWork.EditWindow',
        'dict.ContentRepairMkdWork.Grid'
    ],
    mixins: {
        context: 'B4.mixins.Context'
    },
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'contentRepairMkdWorkGrid',
            permissionPrefix: 'Gkh.Dictionaries.ContentRepairMkdWork'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'organizationWorkGridWindowAspect',
            gridSelector: 'contentRepairMkdWorkGrid',
            editFormSelector: '#contentRepairMkdWorkEditWindow',
            storeName: 'dict.ContentRepairMkdWork',
            modelName: 'dict.ContentRepairMkdWork',
            editWindowView: 'dict.ContentRepairMkdWork.EditWindow'
        }
    ],

    mainView: 'dict.ContentRepairMkdWork.Grid',
    mainViewSelector: 'contentRepairMkdWorkGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'contentRepairMkdWorkGrid'
        }],

    index: function () {
        var view = this.getMainView() || Ext.widget('contentRepairMkdWorkGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ContentRepairMkdWork').load();
    }
});
