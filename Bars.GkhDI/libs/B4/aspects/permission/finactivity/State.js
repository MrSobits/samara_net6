Ext.define('B4.aspects.permission.finactivity.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.finactstateperm',

    permissions: [
        // Общие сведения
        { name: 'GkhDi.Disinfo.FinActivity.GeneralData.Fields.TaxSystem', applyTo: '#sflTaxSystem', selector: '#finActivityEditPanel' },
        { name: 'GkhDi.Disinfo.FinActivity.GeneralData.Audit.Add', applyTo: 'b4addbutton', selector: '#finActivityAuditGrid' },
        { name: 'GkhDi.Disinfo.FinActivity.GeneralData.Audit.Edit', applyTo: 'b4savebutton', selector: '#finActivityAuditEditWindow' },
        {
            name: 'GkhDi.Disinfo.FinActivity.GeneralData.Audit.Delete', applyTo: 'b4deletecolumn', selector: '#finActivityAuditGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
    
        // Документы
        { name: 'GkhDi.Disinfo.FinActivity.Docs.Save', applyTo: '#saveDocsButton', selector: '#finActivityEditPanel' },
        { name: 'GkhDi.Disinfo.FinActivity.Docs.ByYear.Add', applyTo: 'b4addbutton', selector: '#finActivityDocByYearGrid' },
        { name: 'GkhDi.Disinfo.FinActivity.Docs.ByYear.Edit', applyTo: 'b4savebutton', selector: '#finActivityDocByYearEditWindow' },
        {
            name: 'GkhDi.Disinfo.FinActivity.Docs.ByYear.Delete', applyTo: 'b4deletecolumn', selector: '#finActivityDocByYearGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        // Управление по домам
        {
            name: 'GkhDi.Disinfo.FinActivity.ManagRealityObj.PresentedToRepayColumn', applyTo: '#colPresentedToRepay', selector: '#finActivityManagRealityObjGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.editor = 'gkhdecimalfield';
                } else {
                    component.editor = null;
                }
            }
        },
        {
            name: 'GkhDi.Disinfo.FinActivity.ManagRealityObj.ReceivedProvidedServiceColumn', applyTo: '#colReceivedProvidedService', selector: '#finActivityManagRealityObjGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.editor = 'gkhdecimalfield';
                } else {
                    component.editor = null;
                }
            }
        },
        {
            name: 'GkhDi.Disinfo.FinActivity.ManagRealityObj.SumDebtColumn', applyTo: '#colSumDebt', selector: '#finActivityManagRealityObjGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.editor = 'gkhdecimalfield';
                } else {
                    component.editor = null;
                }
            }
        }
    ]
});