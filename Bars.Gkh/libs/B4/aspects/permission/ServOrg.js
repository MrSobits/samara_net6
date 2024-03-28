Ext.define('B4.aspects.permission.ServOrg', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.servorgperm',

    permissions: [
        
        { name: 'Gkh.Orgs.Serv.Create', applyTo: 'b4addbutton', selector: 'servorgGrid' },
        { name: 'Gkh.Orgs.Serv.Edit', applyTo: 'b4savebutton', selector: '#servorgAddWindow' },
        {
            name: 'Gkh.Orgs.Serv.Delete', applyTo: 'b4deletecolumn', selector: 'servorgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.Orgs.Serv.Create', applyTo: 'b4addbutton', selector: '#servorgGrid' },
        { name: 'Gkh.Orgs.Serv.Create', applyTo: 'b4addbutton', selector: 'servorgdocumentationgrid' },
        { name: 'Gkh.Orgs.Serv.Create', applyTo: 'b4addbutton', selector: 'servorgservicegrid' },

        { name: 'Gkh.Orgs.Serv.Edit', applyTo: 'b4savebutton', selector: 'servorgeditpanel' },
        { name: 'Gkh.Orgs.Serv.Edit', applyTo: 'b4savebutton', selector: 'servorgdocumentationeditwindow' },
        { name: 'Gkh.Orgs.Serv.Edit', applyTo: 'b4savebutton', selector: 'servorgactivitypanel' },

        { name: 'Gkh.Orgs.Serv.Delete', applyTo: 'b4deletecolumn', selector: '#servorgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Serv.Delete', applyTo: 'b4deletecolumn', selector: 'servorgdocumentationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Serv.Delete', applyTo: 'b4deletecolumn', selector: 'servorgservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
    
        /*Пермишены Модуль ЖКХ -> Участники процесса -> Поставщики жилищных услуг» -> «Жилые дома» */
        { name: 'Gkh.Orgs.Serv.RealtyObject.Edit', applyTo: 'b4addbutton', selector: 'servorgrogrid' },
        { name: 'Gkh.Orgs.Serv.RealtyObject.Edit', applyTo: 'b4deletecolumn', selector: 'servorgrogrid' },
        
        /*Пермишены Модуль ЖКХ -> Участники процесса -> Поставщики жилищных услуг» -> «Договора с жилыми домами» */
        { name: 'Gkh.Orgs.Serv.RealityObjectContract.Edit', applyTo: 'b4addbutton', selector: 'servorgrocontractgrid' },
        { name: 'Gkh.Orgs.Serv.RealityObjectContract.Edit', applyTo: 'b4deletecolumn', selector: 'servorgrocontractgrid' },
        { name: 'Gkh.Orgs.Serv.RealityObjectContract.Edit', applyTo: 'b4savebutton', selector: 'servorgrocontracteditwindow' }
    ]
});