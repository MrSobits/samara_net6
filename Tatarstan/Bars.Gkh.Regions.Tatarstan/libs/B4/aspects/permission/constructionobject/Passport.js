Ext.define('B4.aspects.permission.constructionobject.Passport', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectpassportpermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Edit',
            applyTo: 'b4savebutton',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.FiasAddress_Edit',
            applyTo: '[name=FiasAddress]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.FiasAddress_View',
            applyTo: '[name=FiasAddress]',
            selector: 'constructionobjeditpanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettlementProgram_Edit',
            applyTo: '[name=ResettlementProgram]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettlementProgram_View',
            applyTo: '[name=ResettlementProgram]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.Description_Edit',
            applyTo: '[name=Description]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.Description_View',
            applyTo: '[name=Description]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumSmr_Edit',
            applyTo: '[name=SumSmr]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumSmr_View',
            applyTo: '[name=SumSmr]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumDevolopmentPsd_Edit',
            applyTo: '[name=SumDevolopmentPsd]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.SumDevolopmentPsd_View',
            applyTo: '[name=SumDevolopmentPsd]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateEndBuilder_Edit',
            applyTo: '[name=DateEndBuilder]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateEndBuilder_View',
            applyTo: '[name=DateEndBuilder]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStartWork_Edit',
            applyTo: '[name=DateStartWork]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStartWork_View',
            applyTo: '[name=DateStartWork]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStopWork_Edit',
            applyTo: '[name=DateStopWork]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateStopWork_View',
            applyTo: '[name=DateStopWork]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateResumeWork_Edit',
            applyTo: '[name=DateResumeWork]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateResumeWork_View',
            applyTo: '[name=DateResumeWork]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ReasonStopWork_Edit',
            applyTo: '[name=ReasonStopWork]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ReasonStopWork_View',
            applyTo: '[name=ReasonStopWork]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateCommissioning_Edit',
            applyTo: '[name=DateCommissioning]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.DateCommissioning_View',
            applyTo: '[name=DateCommissioning]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.LimitOnHouse_Edit',
            applyTo: '[name=LimitOnHouse]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.LimitOnHouse_View',
            applyTo: '[name=LimitOnHouse]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TotalArea_Edit',
            applyTo: '[name=TotalArea]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TotalArea_View',
            applyTo: '[name=TotalArea]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberApartments_Edit',
            applyTo: '[name=NumberApartments]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberApartments_View',
            applyTo: '[name=NumberApartments]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettleProgNumberApartments_Edit',
            applyTo: '[name=ResettleProgNumberApartments]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.ResettleProgNumberApartments_View',
            applyTo: '[name=ResettleProgNumberApartments]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberFloors_Edit',
            applyTo: '[name=NumberFloors]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberFloors_View',
            applyTo: '[name=NumberFloors]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberEntrances_Edit',
            applyTo: '[name=NumberEntrances]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberEntrances_View',
            applyTo: '[name=NumberEntrances]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberLifts_Edit',
            applyTo: '[name=NumberLifts]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.NumberLifts_View',
            applyTo: '[name=NumberLifts]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.WallMaterial_Edit',
            applyTo: '[name=WallMaterial]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.WallMaterial_View',
            applyTo: '[name=WallMaterial]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.RoofingMaterial_Edit',
            applyTo: '[name=RoofingMaterial]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.RoofingMaterial_View',
            applyTo: '[name=RoofingMaterial]',
            selector: 'constructionobjeditpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TypeRoof_Edit',
            applyTo: '[name=TypeRoof]',
            selector: 'constructionobjeditpanel'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Passport.Fields.TypeRoof_View',
            applyTo: '[name=TypeRoof]',
            selector: 'constructionobjeditpanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});