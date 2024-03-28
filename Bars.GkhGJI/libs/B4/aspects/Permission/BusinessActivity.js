Ext.define('B4.aspects.permission.BusinessActivity', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.businessactivityperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования
        { name: 'GkhGji.BusinessActivity.Edit', applyTo: 'b4savebutton', selector: '#businessActivityEditWindow' },


    //поля панели редактирования
        { name: 'GkhGji.BusinessActivity.Field.Contragent_Edit', applyTo: '#sfContragent', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.OrganizationFormName_Edit', applyTo: '#cbOrganizationFormName', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.Ogrn_Edit', applyTo: '#tfOgrn', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.Inn_Edit', applyTo: '#tfInn', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.MailingAddress_Edit', applyTo: '#tfMailingAddress', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.TypeKindActivity_Edit', applyTo: '#cbTypeKindActivity', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.IncomingNotificationNum_Edit', applyTo: '#tfIncomingNotificationNum', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.DateBegin_Edit', applyTo: '#dfDateBegin', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.DateRegistration_Edit', applyTo: '#dfDateRegistration', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.IsNotBuisnes_Edit', applyTo: '#chbNotBuisnes', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.AcceptedOrganization_Edit', applyTo: '#tfAcceptedOrganization', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.RegNum_Edit', applyTo: '#tfRegNum', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.IsOriginal_Edit', applyTo: '#chbIsOriginal', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.File_Edit', applyTo: '#ffFile', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Field.DateNotif_Edit', applyTo: '#dfDateNotif', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Register.ServiceJuridal.View', applyTo: '#serviceJuridicalGjiGrid', selector: '#businessActivityEditWindow' },
        { name: 'GkhGji.BusinessActivity.Register.ServiceJuridal.Create', applyTo: 'b4addbutton', selector: '#serviceJuridicalGjiGrid' },
        { name: 'GkhGji.BusinessActivity.Register.ServiceJuridal.Delete', applyTo: 'b4deletecolumn', selector: '#serviceJuridicalGjiGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.BusinessActivity.Field.Registering', applyTo: '#btnState', selector: '#businessActivityEditWindow' }
    ]
});