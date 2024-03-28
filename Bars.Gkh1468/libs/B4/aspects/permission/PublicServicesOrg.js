Ext.define('B4.aspects.permission.PublicServicesOrg', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.publicserorgperm',

    permissions: [

        { name: 'Gkh1468.Orgs.PublicServiceOrg.Create', applyTo: 'b4addbutton', selector: 'publicservorgGrid' },
        { name: 'Gkh1468.Orgs.PublicServiceOrg.Edit', applyTo: 'b4savebutton', selector: 'publicservorgeditpanel' },
        {
            name: 'Gkh1468.Orgs.PublicServiceOrg.Delete', applyTo: 'b4deletecolumn', selector: 'publicservorgGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        /*Пермишены Модуль ЖКХ1468 -> Поставщики ресурсов -> «Жилые дома» */
        { name: 'Gkh1468.Orgs.PublicServiceOrg.RealtyObject.Edit', applyTo: 'b4addbutton', selector: 'publicServOrgRoGrid' },
        { name: 'Gkh1468.Orgs.PublicServiceOrg.RealtyObject.Edit', applyTo: 'b4deletecolumn', selector: 'publicServOrgRoGrid' },
        
        /*Пермишены Модуль ЖКХ1468 -> Поставщики ресурсов -> «Договора с жилыми домами» */
        { name: 'Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.Edit', applyTo: 'b4addbutton', selector: 'publicservorgcontractgrid' },
        { name: 'Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.Edit', applyTo: 'b4deletecolumn', selector: 'publicservorgcontractgrid' },
        { name: 'Gkh1468.Orgs.PublicServiceOrg.ContractsWithRealObj.Edit', applyTo: 'b4savebutton[name=pubServOrg]', selector: 'publicservorgcontracteditwindow' }
    ]
});