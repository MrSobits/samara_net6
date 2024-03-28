Ext.define('B4.controller.dict.TypeInformationNpa', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.TypeInformationNpa'],
    stores: ['dict.TypeInformationNpa'],
    views: [
        'dict.typeinformationnpa.Grid',
        'dict.typeinformationnpa.EditWindow'
    ],

    mainView: 'dict.typeinformationnpa.Grid',
    mainViewSelector: 'typeinformationnpaGrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'typeinformationnpatGridWindowAspect',
            gridSelector: 'typeinformationnpaGrid',
            modelName: 'dict.TypeInformationNpa',
            editFormSelector: '#typeInformationNpaEditWindow',
            editWindowView: 'dict.typeinformationnpa.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.TypeInformationNpa.Create',
                    applyTo: 'b4addbutton',
                    selector: 'typeinformationnpaGrid'
                },
                {
                    name: 'Gkh.Dictionaries.TypeInformationNpa.Edit',
                    applyTo: 'b4savebutton',
                    selector: '#typeInformationNpaEditWindow'
                },
                {
                    name: 'Gkh.Dictionaries.TypeInformationNpa.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'typeinformationnpaGrid',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                }
            ]
        }
    ],

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('typeinformationnpaGrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});