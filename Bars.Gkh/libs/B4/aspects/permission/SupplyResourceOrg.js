Ext.define('B4.aspects.permission.SupplyResourceOrg', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.supplyresorgperm',

    permissions: [
        
        { name: 'Gkh.Orgs.SupplyResource.Create', applyTo: 'b4addbutton', selector: 'supplyResOrgGrid' },
        { name: 'Gkh.Orgs.SupplyResource.Edit', applyTo: 'b4savebutton', selector: 'supplyresorgaddwindow' },
        {
            name: 'Gkh.Orgs.SupplyResource.Delete', applyTo: 'b4deletecolumn', selector: 'supplyResOrgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.Orgs.SupplyResource.Create', applyTo: 'b4addbutton', selector: '#supplyResOrgGrid' },
        { name: 'Gkh.Orgs.SupplyResource.Create', applyTo: 'b4addbutton', selector: 'supplyresorgdocumentationgrid' },
        { name: 'Gkh.Orgs.SupplyResource.Create', applyTo: 'b4addbutton', selector: 'supplyresorgservicegrid' },

        { name: 'Gkh.Orgs.SupplyResource.Edit', applyTo: 'b4savebutton', selector: 'supplyresorgeditpanel' },
        { name: 'Gkh.Orgs.SupplyResource.Edit', applyTo: 'b4savebutton', selector: 'supplyresorgdocumentationeditwindow' },
        { name: 'Gkh.Orgs.SupplyResource.Edit', applyTo: 'b4savebutton', selector: 'supplyresorgserviceeditwindow' },

        { name: 'Gkh.Orgs.SupplyResource.Delete', applyTo: 'b4deletecolumn', selector: '#supplyResOrgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.SupplyResource.Delete', applyTo: 'b4deletecolumn', selector: 'supplyresorgdocumentationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.SupplyResource.Delete', applyTo: 'b4deletecolumn', selector: 'supplyresorgservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        /*Пермишены Модуль ЖКХ -> Участники процесса -> Поставщики коммунальных услуг» -> «Жилые дома» */
        { name: 'Gkh.Orgs.SupplyResource.RealtyObject.Edit', applyTo: 'b4addbutton', selector: 'supplyresorgrogrid' },
        { name: 'Gkh.Orgs.SupplyResource.RealtyObject.Edit', applyTo: 'b4deletecolumn', selector: 'supplyresorgrogrid' },
        
        /*Пермишены Модуль ЖКХ -> Участники процесса -> Поставщики коммунальных услуг» -> «Договора с жилыми домами» */
        { name: 'Gkh.Orgs.SupplyResource.ContractsWithRealObj.Edit', applyTo: 'b4addbutton', selector: 'supplyresorgcontractgrid' },
        { name: 'Gkh.Orgs.SupplyResource.ContractsWithRealObj.Edit', applyTo: 'b4deletecolumn', selector: 'supplyresorgcontractgrid' },
        { name: 'Gkh.Orgs.SupplyResource.ContractsWithRealObj.Edit', applyTo: 'b4savebutton', selector: 'supplyresorgcontracteditwindow' }
    ]
});