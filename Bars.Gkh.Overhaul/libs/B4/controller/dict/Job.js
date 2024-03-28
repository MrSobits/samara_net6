Ext.define('B4.controller.dict.Job', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.view.dict.job.EditWindow',
        'B4.view.dict.job.Grid',
        'B4.store.dict.Job',
        'B4.model.dict.Job',

        'B4.aspects.GridEditWindow',
        'B4.form.SelectField',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    stores: [
        'dict.Job'
    ],
    
    models: [
        'dict.Job'
    ],
    
    views: [
        'dict.job.EditWindow',
        'dict.job.Grid'
    ],
    
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'jobGridAspect',
            gridSelector: 'jobpanel',
            editFormSelector: 'jobwindow',
            storeName: 'dict.Job',
            modelName: 'dict.Job',
            editWindowView: 'dict.job.EditWindow',
            onSaveSuccess: function(asp, rec) {
                this.closeWindowHandler(this.getForm());
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.Dictionaries.Job.Create', applyTo: 'b4addbutton', selector: 'jobpanel' },
                { name: 'Ovrhl.Dictionaries.Job.Edit', applyTo: 'b4savebutton', selector: 'jobwindow' },
                { name: 'Ovrhl.Dictionaries.Job.Delete', applyTo: 'b4deletecolumn', selector: 'jobpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'jobpanel' }
    ],

    index: function() {
        var view = this.getMainPanel() || Ext.widget('jobpanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});