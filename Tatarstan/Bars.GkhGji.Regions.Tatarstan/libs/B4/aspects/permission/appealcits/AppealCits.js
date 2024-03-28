Ext.define('B4.aspects.permission.appealcits.AppealCits', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.appealcitspermissionaspect',

    permissions: [
        {
            name: 'GkhGji.Inspection.WarningInspection.Create',
            applyTo: '#btnCreateWarningInspection',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        { name: 'GkhGji.AppealCitizens.Create', applyTo: 'b4addbutton', selector: '#appealCitsGrid' },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.View',
            applyTo: '#menuFiltersBtn',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowSoprAppeals',
            applyTo: '[fieldParam=showSoprAppeals]',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowProcessedAppeals',
            applyTo: '[fieldParam=showProcessedAppeals]',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowNotProcessedAppeals',
            applyTo: '[fieldParam=showNotProcessedAppeals]',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowInWorkAppeals',
            applyTo: '[fieldParam=showInWorkAppeals]',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizens.ShowAppealFilters.ShowClosedAppeals',
            applyTo: '[fieldParam=showClosedAppeals]',
            selector: '#appealCitsGrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.ActionIsolated.View',
            applyTo: 'appealcitsactionisolatedgrid',
            selector: '#appealCitizensTabPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                }
                else {
                    component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.MotivatedPresentation.View',
            applyTo: 'motivatedpresentationappealcitsgrid',
            selector: '#appealCitizensTabPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                }
                else {
                    component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.SoprInformation.View',
            applyTo: 'appealcitssoprinformationpanel',
            selector: '#appealCitizensTabPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                }
                else {
                    component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.SoprInformation.Delete',
            applyTo: '#appealCitsContragentGrid b4deletecolumn',
            selector: '#appealCitsSoprInformationPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.SoprInformation.Delete',
            applyTo: '#appealCitsSoprInformationGrid b4deletecolumn',
            selector: '#appealCitsSoprInformationPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.SoprInformation.CreateAppeal',
            applyTo: '#appealCitsContragentGrid actioncolumn[name=createAppeal]',
            selector: '#appealCitsSoprInformationPanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.CreateActionIsolated',
            applyTo: '#btnCreateActionIsolated',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.CreateMotivatedPresentation',
            applyTo: '#btnCreateMotivatedPresentation',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizens.CreateWarningInspection',
            applyTo: '#btnCreateWarningInspection',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizens.WarningInspection.View',
            applyTo: 'appealCitsWarningInspectionGrid',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.tab.show();
                }
                else {
                    component.tab.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.Field.IsIdentityVerified_View',
            applyTo: '[name=IsIdentityVerified]',
            selector: '#appealCitsEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                }
                else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.AppealCitizensState.Field.IsIdentityVerified_Edit',
            applyTo: '[name=IsIdentityVerified]',
            selector: '#appealCitsEditWindow'
        }
    ]
});