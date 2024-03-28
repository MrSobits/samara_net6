Ext.define('B4.aspects.permission.constructionobject.TypeWork', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjecttypeworkpermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Create',
            applyTo: 'b4addbutton',
            selector: 'constructionobjtypeworkgrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.YearBuilding_Edit',
            applyTo: '[name=YearBuilding]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.YearBuilding_View',
            applyTo: '[name=YearBuilding]',
            selector: 'constructionobjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasPsd_Edit',
            applyTo: '[name=HasPsd]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasPsd_View',
            applyTo: '[name=HasPsd]',
            selector: 'constructionobjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasExpertise_Edit',
            applyTo: '[name=HasExpertise]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.HasExpertise_View',
            applyTo: '[name=HasExpertise]',
            selector: 'constructionobjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Work_Edit',
            applyTo: '[name=Work]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Work_View',
            applyTo: '[name=Work]',
            selector: 'constructionobjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.TypeWork_View',
            applyTo: '[name=TypeWork]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.UnitMeasureName_View',
            applyTo: '[name=UnitMeasureName]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Volume_Edit',
            applyTo: '[name=Volume]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Volume_View',
            applyTo: '[name=Volume]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Sum_Edit',
            applyTo: '[name=Sum]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Sum_View',
            applyTo: '[name=Sum]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Deadline_Edit',
            applyTo: '[name=Deadline]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Deadline_View',
            applyTo: '[name=Deadline]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Description_Edit',
            applyTo: '[name=Description]',
            selector: 'constructionobjeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.TypeWorks.Fields.Description_View',
            applyTo: '[name=Description]',
            selector: 'constructionobjeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});