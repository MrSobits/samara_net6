Ext.define('B4.controller.dict.TypeNormativeAct', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['dict.TypeNormativeAct'],
    stores: ['dict.TypeNormativeAct'],
    views: [
        'dict.typenormativeact.Grid',
        'dict.typenormativeact.EditWindow'
    ],

    mainView: 'dict.typenormativeact.Grid',
    mainViewSelector: 'typenormativeact',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'typenormativeactGridWindowAspect',
            gridSelector: 'typenormativeactGrid',
            modelName: 'dict.TypeNormativeAct',
            editFormSelector: '#typenormativeactEditWindow',
            editWindowView: 'dict.typenormativeact.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.TypeNormativeAct.Create',
                    applyTo: 'b4addbutton',
                    selector: 'typenormativeactGrid'
                },
                {
                    name: 'Gkh.Dictionaries.TypeNormativeAct.Edit',
                    applyTo: 'b4savebutton',
                    selector: '#typenormativeactEditWindow'
                },
                {
                    name: 'Gkh.Dictionaries.TypeNormativeAct.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'typenormativeactGrid',
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
            view = this.getMainView() || Ext.widget('typenormativeactGrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});