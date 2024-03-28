Ext.define('B4.controller.dict.CrPeriod', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.crperiod.Grid',
        'B4.view.dict.crperiod.EditWindow',
        'B4.store.dict.CrPeriod',
        'B4.model.dict.CrPeriod',

        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    stores: [
        'dict.CrPeriod'
    ],

    models: [
        'dict.CrPeriod'
    ],

    views: [
        'dict.crperiod.Grid',
        'dict.crperiod.EditWindow'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'crperiodGridWindowAspect',
            gridSelector: 'crperiodpanel',
            editFormSelector: 'crperiodwindow',
            storeName: 'dict.CrPeriod',
            modelName: 'dict.CrPeriod',
            editWindowView: 'dict.crperiod.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.CrPeriod.Create', applyTo: 'b4addbutton', selector: 'crperiodpanel' },
                { name: 'Ovrhl.Dictionaries.CrPeriod.Edit', applyTo: 'b4savebutton', selector: 'crperiodwindow' },
                {
                    name: 'Ovrhl.Dictionaries.CrPeriod.Delete', applyTo: 'b4deletecolumn', selector: 'crperiodpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'crperiodpanel' }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('crperiodpanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});