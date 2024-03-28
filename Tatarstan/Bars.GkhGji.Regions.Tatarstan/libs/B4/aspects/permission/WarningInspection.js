Ext.define('B4.aspects.permission.WarningInspection', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.warninginspectionperm',

    permissions: [
        {
            name: 'GkhGji.Inspection.WarningInspection.Edit',
            applyTo: 'b4savebutton',
            selector: '#warninginspectionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Edit',
            applyTo: 'gjidocumentcreatebutton',
            selector: '#warninginspectionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Registry.RealityObjects.Create',
            applyTo: 'b4addbutton',
            selector: '#warninginspectionEditPanel realityobjectgjigrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Registry.RealityObjects.View',
            applyTo: 'realityobjectgjigrid',
            selector: 'tabpanel[name=inspectionTabPanel]',
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
            name: 'GkhGji.Inspection.WarningInspection.Registry.RealityObjects.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#warninginspectionEditPanel realityobjectgjigrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Registry.Contragents.Create',
            applyTo: 'b4addbutton',
            selector: '#warninginspectionEditPanel inspectiongjicontragentgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Registry.Contragents.View',
            applyTo: 'inspectiongjicontragentgrid ',
            selector: 'tabpanel[name=inspectionTabPanel]',
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
            name: 'GkhGji.Inspection.WarningInspection.Registry.Contragents.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#warninginspectionEditPanel inspectiongjicontragentgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Date_View',
            applyTo: '[name=Date]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Date_Edit',
            applyTo: '[name=Date]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.InspectionNumber_View',
            applyTo: '[name=InspectionNumber]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.InspectionNumber_Edit',
            applyTo: '[name=InspectionNumber]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.TypeJurPerson_View',
            applyTo: '[name=TypeJurPerson]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.TypeJurPerson_Edit',
            applyTo: '[name=TypeJurPerson]',
            selector: '#warninginspectionEditPanel',
            applyBy: function (component, allowed) {
                var me = this;
                if (component) {
                    component.setDisabled(!allowed || me.controller.disabledFields['TypeJurPerson']);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.PersonInspection_View',
            applyTo: '[name=PersonInspection]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.PersonInspection_Edit',
            applyTo: '[name=PersonInspection]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Contragent_View',
            applyTo: '[name=Contragent]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Contragent_Edit',
            applyTo: '[name=Contragent]',
            selector: '#warninginspectionEditPanel',
            applyBy: function (component, allowed) {
                var me = this;
                if (component) {
                    component.setDisabled(!allowed || me.controller.disabledFields['Contragent']);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.PhysicalPerson_View',
            applyTo: '[name=PhysicalPerson]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.PhysicalPerson_Edit',
            applyTo: '[name=PhysicalPerson]',
            selector: '#warninginspectionEditPanel',
            applyBy: function (component, allowed) {
                var me = this;
                if (component) {
                    component.setDisabled(!allowed || me.controller.disabledFields['PhysicalPerson']);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumber_View',
            applyTo: '[name=RegistrationNumber]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumber_Edit',
            applyTo: '[name=RegistrationNumber]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate_View',
            applyTo: '[name=RegistrationNumberDate]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.RegistrationNumberDate_Edit',
            applyTo: '[name=RegistrationNumberDate]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.CheckDayCount_View',
            applyTo: '[name=CheckDayCount]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.CheckDayCount_Edit',
            applyTo: '[name=CheckDayCount]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.CheckDate_View',
            applyTo: '[name=CheckDate]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.CheckDate_Edit',
            applyTo: '[name=CheckDate]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Inspectors_View',
            applyTo: '[name=Inspectors]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.Inspectors_Edit',
            applyTo: '[name=Inspectors]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.InspectionBasis_View',
            applyTo: '[name=InspectionBasis]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.InspectionBasis_Edit',
            applyTo: '[name=InspectionBasis]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.SourceFormType_View',
            applyTo: '[name=SourceFormType]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.SourceFormType_Edit',
            applyTo: '[name=SourceFormType]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentName_View',
            applyTo: '[name=DocumentName]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentName_Edit',
            applyTo: '[name=DocumentName]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentNumber_View',
            applyTo: '[name=DocumentNumber]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentNumber_Edit',
            applyTo: '[name=DocumentNumber]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentDate_View',
            applyTo: '[name=DocumentDate]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.File_View',
            applyTo: '[name=File]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.File_Edit',
            applyTo: '[name=File]',
            selector: '#warninginspectionEditPanel'
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.ControlType_View',
            applyTo: '[name=WarningInspectionControlType]',
            selector: '#warninginspectionEditPanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.Inspection.WarningInspection.Field.ControlType_Edit',
            applyTo: '[name=WarningInspectionControlType]',
            selector: '#warninginspectionEditPanel'
        }
    ]
});