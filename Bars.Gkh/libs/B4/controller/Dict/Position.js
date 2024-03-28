Ext.define('B4.controller.dict.Position', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.Position'],
    stores: ['dict.Position'],
    views: [
        'dict.position.Grid',
        'dict.position.EditWindow'
    ],

    mainView: 'dict.position.Grid',
    mainViewSelector: 'positionGrid',

    mixins: { context: 'B4.mixins.Context' },

    refs: [{
        ref: 'mainView',
        selector: 'positionGrid'
    }],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Dictionaries.Position.Create', applyTo: 'b4addbutton', selector: 'positionGrid' },
                { name: 'Gkh.Dictionaries.Position.Edit', applyTo: 'b4savebutton', selector: '#positionEditWindow' },
                { name: 'Gkh.Dictionaries.Position.Delete', applyTo: 'b4deletecolumn', selector: 'positionGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'positionGridWindowAspect',
            gridSelector: 'positionGrid',
            editFormSelector: '#positionEditWindow',
            storeName: 'dict.Position',
            modelName: 'dict.Position',
            editWindowView: 'dict.position.EditWindow'
        }
    ],

    index: function () {
        var view = this.getMainView();
        if (!view) {
            view = Ext.widget('positionGrid');
            this.bindContext(view);
            this.application.deployView(view);
            this.getStore('dict.Position').load();
        }
    }
});