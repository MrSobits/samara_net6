Ext.define('B4.controller.TechnicalCustomer', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['TechnicalCustomer'],
    stores: ['TechnicalCustomer'],
    views: [
        'technicalcustomer.Grid',
        'technicalcustomer.EditWindow'
    ],

    mainView: 'technicalcustomer.Grid',
    mainViewSelector: 'technicalcustomerGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'technicalcustomerGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.TechnicalCustomer.Create',
                    applyTo: 'b4addbutton',
                    selector:
                        'technicalcustomerGrid'
                },
                {
                    name: 'Gkh.Orgs.TechnicalCustomer.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'technicalcustomerwindow'
                },
                {
                    name: 'Gkh.Orgs.TechnicalCustomer.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'technicalcustomerwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.TechnicalCustomer.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'technicalcustomerGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'technicalcustomerGridWindowAspect',
            gridSelector: 'technicalcustomerGrid',
            editFormSelector: 'technicalcustomerwindow',
            storeName: 'TechnicalCustomer',
            modelName: 'TechnicalCustomer',
            editWindowView: 'technicalcustomer.EditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=Contragent]'] = {
                    'change': { fn: this.onChangeContragent, scope: this }
                };
            },
            onChangeContragent: function (sf, newValue) {
                var win = sf.up('technicalcustomerwindow'),
                    orgForm = win.down('[name=OrganizationForm]');

                if (newValue) {
                    orgForm.setValue(newValue.OrganizationForm);
                } else {
                    orgForm.setValue(null);
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('technicalcustomerGrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});