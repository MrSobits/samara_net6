Ext.define('B4.controller.dict.CriteriaForActualizeVersion', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.criteriaforactualizeversion.Grid',
        'B4.view.dict.criteriaforactualizeversion.EditWindow',
        'B4.store.dict.CriteriaForActualizeVersion',
        'B4.model.dict.CriteriaForActualizeVersion',

        'B4.aspects.GridEditWindow',
        'B4.form.SelectField',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    stores: [
        'dict.CriteriaForActualizeVersion'
    ],

    models: [
        'dict.CriteriaForActualizeVersion'
    ],

    views: [
        'dict.criteriaforactualizeversion.Grid',
        'dict.criteriaforactualizeversion.EditWindow'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'criteriaForActualizeVersionGridWindowAspect',
            gridSelector: 'criteriaforactualizeversionpanel',
            editFormSelector: 'criteriaforactualizeversionwindow',
            storeName: 'dict.CriteriaForActualizeVersion',
            modelName: 'dict.CriteriaForActualizeVersion',
            editWindowView: 'dict.criteriaforactualizeversion.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.CriteriaForActualizeVersion.Create', applyTo: 'b4addbutton', selector: 'criteriaforactualizeversionpanel' },
                { name: 'Ovrhl.Dictionaries.CriteriaForActualizeVersion.Edit', applyTo: 'b4savebutton', selector: 'criteriaforactualizeversionwindow' },
                {
                    name: 'Ovrhl.Dictionaries.CriteriaForActualizeVersion.Delete', applyTo: 'b4deletecolumn', selector: 'criteriaforactualizeversionpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'criteriaforactualizeversionpanel' }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('criteriaforactualizeversionpanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});