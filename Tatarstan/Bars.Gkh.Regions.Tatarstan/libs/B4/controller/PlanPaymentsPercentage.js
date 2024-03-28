Ext.define('B4.controller.PlanPaymentsPercentage',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: ['PlanPaymentsPercentage'],

    models: ['PlanPaymentsPercentage'],

    views: ['planpaymentspercentage.Grid', 'planpaymentspercentage.EditWindow'],

    mainView: 'planpaymentspercentage.Grid',
    mainViewSelector: 'planpaymentspercentagegrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'planPaymentsPercentageEditAspect',
            gridSelector: 'planpaymentspercentagegrid',
            editFormSelector: 'planpaymentspercentagewindow',
            storeName: 'PlanPaymentsPercentage',
            modelName: 'PlanPaymentsPercentage',
            editWindowView: 'planpaymentspercentage.EditWindow'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.PlanPaymentsPercentage.Create',
                    applyTo: 'b4addbutton',
                    selector: 'planpaymentspercentagegrid'
                },
                {
                    name: 'Gkh.Dictionaries.PlanPaymentsPercentage.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'planpaymentspercentagewindow'
                },
                {
                    name: 'Gkh.Dictionaries.PlanPaymentsPercentage.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'planpaymentspercentagegrid',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        }
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'planpaymentspercentagegrid'
        },
        {
            ref: 'planpaymentspercentageEditWin',
            selector: 'planpaymentspercentagewindow'
        }
    ],

    index: function() {
        var view = this.getMainView() || Ext.widget('planpaymentspercentagegrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    }
});

