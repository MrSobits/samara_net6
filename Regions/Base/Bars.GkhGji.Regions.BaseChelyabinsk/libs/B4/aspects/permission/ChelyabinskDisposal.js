Ext.define('B4.aspects.permission.ChelyabinskDisposal', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.nsodisposalperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.PeriodCorrect_View', applyTo: '[name=PeriodCorrect]', selector: '#disposalEditPanel',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.FactViols_View', applyTo: '[name=FactViols]', selector: '#disposalEditPanel',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.ProsecutorDecDate_View', applyTo: '[name=ProsecutorDecDate]', selector: '#disposalEditPanel',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Field.ProsecutorDecNumber_View', applyTo: '[name=ProsecutorDecNumber]', selector: '#disposalEditPanel',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveySubject.Create', applyTo: 'b4addbutton', selector: 'disposalsubjectverificationgrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveySubject.Delete', applyTo: 'b4deletebutton', selector: 'disposalsubjectverificationgrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyPurpose.Create', applyTo: 'b4addbutton', selector: 'disposalsurveypurposegrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyPurpose.Delete', applyTo: 'b4deletecolumn', selector: 'disposalsurveypurposegrid',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyObjective.Create', applyTo: 'b4addbutton', selector: 'disposalsurveyobjectivegrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.SurveyObjective.Delete', applyTo: 'b4deletecolumn', selector: 'disposalsurveyobjectivegrid',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },

        { name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundation.Create', applyTo: 'b4addbutton', selector: 'disposalinspfoundationgrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundation.Delete', applyTo: 'b4deletecolumn', selector: 'disposalinspfoundationgrid',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheck.Create', applyTo: 'b4addbutton', selector: 'disposalinspfoundationcheckgrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheck.Delete', applyTo: 'b4deletecolumn', selector: 'disposalinspfoundationcheckgrid',
            applyBy: function (component, allowed) {
                this.setVisible(component, allowed);
            }
        },
        { name: 'GkhGji.DocumentsGji.Disposal.Register.AdminRegulation.Create', applyTo: 'b4addbutton', selector: 'disposaladminregulationgrid' },
        {
            name: 'GkhGji.DocumentsGji.Disposal.Register.AdminRegulation.Delete', applyTo: 'b4deletecolumn', selector: 'disposaladminregulationgrid',
            applyBy: function(component, allowed) {
                this.setVisible(component, allowed);
            }
        }
    ]
});