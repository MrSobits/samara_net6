Ext.define('B4.aspects.permission.ConfirmContribution', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.confirmcontribperm',

    permissions: [
        { name: 'GkhRf.TransferRf.Create', applyTo: 'b4addbutton', selector: 'transferRfGrid' },
        { name: 'GkhRf.TransferRf.Edit', applyTo: 'b4savebutton', selector: '#transferRfEditWindow' },
        {
            name: 'GkhRf.TransferRf.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'transferRfGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.RegOperator.ConfirmContribution.EditData.ConfirmContributionDoc',
            applyTo: '#transferRfRecObjSaveButton',
            selector: '#transferRfRecordEditWindow'
        }
    ]
});