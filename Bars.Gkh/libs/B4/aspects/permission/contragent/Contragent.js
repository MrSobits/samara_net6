Ext.define('B4.aspects.permission.contragent.Contragent', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.contragentperm',

    permissions: [
        { name: 'Gkh.Orgs.Contragent.Edit', applyTo: 'b4savebutton', selector: 'contragentEditPanel' },
        {
            name: 'Gkh.Orgs.Contragent.ChangeLog_View',
            applyTo: 'entitychangeloggrid',
            selector: 'contragentgeneralinfopanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    tabPanel.hideTab(component);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.AdditionRole_View',
            applyTo: 'contragentAdditionRoleGrid',
            selector: 'contragentgeneralinfopanel',
            applyBy: function (component, allowed) {
                var tabPanel = component.ownerCt;
                if (allowed) {
                    tabPanel.showTab(component);
                } else {
                    tabPanel.hideTab(component);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Edit',
            applyTo: 'textfield[name=ProviderCode]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                component.setReadOnly(allowed);
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_View',
            applyTo: 'textfield[name=ProviderCode]',
            selector: 'contragentEditPanel',
            applyBy: function(component, allowed) {
                var fieldSet = component.up('fieldset'),
                    button = fieldSet.down('button[action=GenerateProviderCode]');

                component.setVisible(allowed);

                if (button.isVisible() || allowed) {
                    fieldSet.show();
                } else {
                    fieldSet.hide();
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Register.Provider.ProviderCode_Generate',
            applyTo: 'button[action=GenerateProviderCode]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                var fieldSet = component.up('fieldset'),
                    textField = fieldSet.down('textfield[name=ProviderCode]');

                component.setVisible(allowed);

                if (textField.isVisible() || allowed) {
                    fieldSet.show();
                } else {
                    fieldSet.hide();
                }
            }
        },

        {
            name: 'Gkh.Orgs.Contragent.Field.FrguRegNumber_View',
            applyTo: 'textfield[name=FrguRegNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.FrguRegNumber_Edit',
            applyTo: 'textfield[name=FrguRegNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.FrguOrgNumber_View',
            applyTo: 'textfield[name=FrguOrgNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.FrguOrgNumber_Edit',
            applyTo: 'textfield[name=FrguOrgNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.FrguServiceNumber_View',
            applyTo: 'textfield[name=FrguServiceNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.FrguServiceNumber_Edit',
            applyTo: 'textfield[name=FrguServiceNumber]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.AddressCoords_View',
            applyTo: 'textfield[name=Coords]',
            selector: '#fiasSelectAddressWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.AddressCoords_Edit',
            applyTo: 'textfield[name=Coords]',
            selector: '#fiasSelectAddressWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.ActivityStage_View',
            applyTo: 'activitystagegrid',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.ActivityStage_Edit',
            applyTo: 'b4addbutton',
            selector: 'contragentEditPanel activitystagegrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.ActivityStage_Edit',
            applyTo: 'b4savebutton',
            selector: 'activitystageeditwincontragent',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },

        {
            name: 'Gkh.Orgs.Contragent.Field.TimeZoneType_View',
            applyTo: '[name=TimeZoneType]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.TimeZoneType_Edit',
            applyTo: '[name=TimeZoneType]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },

        {
            name: 'Gkh.Orgs.Contragent.Field.Okogu_View',
            applyTo: '[name=Okogu]',
            selector: 'contragentEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.Okogu_Edit',
            applyTo: '[name=Okogu]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.Okfs_View',
            applyTo: '[name=Okfs]',
            selector: 'contragentEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.Okfs_Edit',
            applyTo: '[name=Okfs]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.MainRole_View',
            applyTo: '[name=MainRole]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.MainRole_Edit',
            applyTo: '[name=MainRole]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.ReceiveNotifications_View',
            applyTo: '[name=ReceiveNotifications]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.Orgs.Contragent.Field.ReceiveNotifications_Edit',
            applyTo: '[name=ReceiveNotifications]',
            selector: 'contragentEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setDisabled(!allowed);
                }
            }
        }
    ]
});