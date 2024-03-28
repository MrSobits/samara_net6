Ext.define('B4.controller.contragent.Bank', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.EntityChangeLog'
    ],

    models: [
        'contragent.Bank'
    ],

    stores: [
        'contragent.Bank'
    ],

    views: [
        'contragent.BankPanel',
        'contragent.BankEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentbankpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Create', applyTo: 'b4addbutton', selector: 'contragentBankGrid' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Edit', applyTo: 'b4savebutton', selector: '#contragentBankEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Name_Edit', applyTo: '[name=Name]', selector: '#contragentBankEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Name_View', applyTo: '[name=Name]', selector: '#contragentBankEditWindow',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.CreditOrg_Edit', applyTo: '[name=CreditOrg]', selector: '#contragentBankEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.CreditOrg_View', applyTo: '[name=CreditOrg]', selector: '#contragentBankEditWindow',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.File_Edit', applyTo: '[name=File]', selector: '#contragentBankEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.File_View', applyTo: '[name=File]', selector: '#contragentBankEditWindow',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Delete', applyTo: 'b4deletecolumn', selector: 'contragentBankGrid',
                    applyBy: function(component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Bank.ChangeLog_View',
                    applyTo: 'entitychangeloggrid',
                    selector: 'contragentbankpanel',
                    applyBy: function (component, allowed) {
                        var tabPanel = component.ownerCt;
                        if (allowed) {
                            tabPanel.showTab(component);
                        } else {
                            tabPanel.hideTab(component);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentBankGridWindowAspect',
            gridSelector: 'contragentBankGrid',
            editFormSelector: '#contragentBankEditWindow',
            storeName: 'contragent.Bank',
            modelName: 'contragent.Bank',
            editWindowView: 'contragent.BankEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.Contragent = asp.controller.getContextValue(asp.controller.getMainComponent(), 'contragentId');
                    }
                    if (record.get('CreditOrg')) {
                        record.set('CreditOrg', record.get('CreditOrg').Id);
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = me.getModel(record);

                id ? model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                }) : this.setFormData(new model({ Id: 0 }));
            }
        },
        {
            xtype: 'entitychangelogaspect',
            gridSelector: 'contragentbankpanel entitychangeloggrid',
            entityType: 'Bars.Gkh.Overhaul.Entities.ContragentBankCreditOrg',
            inheritEntityChangeLogCode: 'ContragentBankCreditOrg',
            getEntityId: function() {
                var asp = this,
                    me = asp.controller;
                return me.getContextValue(me.getMainView(), 'contragentId');
            }
        }
    ],

    init: function () {
        this.getStore('contragent.Bank').on('beforeload', this.onBeforeLoad, this);

        this.control({
            '#contragentBankEditWindow [name="CreditOrg"]': {
                change: this.onCrOrgChange
            }
        });

        this.callParent(arguments);
    },

    onCrOrgChange: function (field, record, oldValue) {
        if (oldValue) {
            var win = field.up();

            if (record) {
                win.down('[name="CorrAccount"]').setValue(record.CorrAccount);
                win.down('[name="Bik"]').setValue(record.Bik);
                win.down('[name="Okpo"]').setValue(record.Okpo);
            } else {
                win.down('[name="CorrAccount"]').setValue();
                win.down('[name="Bik"]').setValue();
                win.down('[name="Okpo"]').setValue();
            }
        }
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentbankpanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getStore('contragent.Bank').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});