Ext.define('B4.controller.dict.AccountOperation', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.dict.accountoperation.Grid',
        'B4.view.dict.accountoperation.EditWindow',
        'B4.store.dict.AccountOperation',
        'B4.model.dict.AccountOperation',

        'B4.aspects.GridEditWindow',
        'B4.form.SelectField',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    stores: [
        'dict.AccountOperation'
    ],

    models: [
        'dict.AccountOperation'
    ],

    views: [
        'dict.accountoperation.Grid',
        'dict.accountoperation.EditWindow'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'accountOperationGridWindowAspect',
            gridSelector: 'accountoperationpanel',
            editFormSelector: 'accountoperationwindow',
            storeName: 'dict.AccountOperation',
            modelName: 'dict.AccountOperation',
            editWindowView: 'dict.accountoperation.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.AccountOperation.Create', applyTo: 'b4addbutton', selector: 'accountoperationpanel' },
                { name: 'Ovrhl.Dictionaries.AccountOperation.Edit', applyTo: 'b4savebutton', selector: 'accountoperationwindow' },
                {
                    name: 'Ovrhl.Dictionaries.AccountOperation.Delete', applyTo: 'b4deletecolumn', selector: 'accountoperationpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'accountoperationpanel' }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('accountoperationpanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});