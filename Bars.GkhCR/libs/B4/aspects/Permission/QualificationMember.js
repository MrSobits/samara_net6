Ext.define('B4.aspects.permission.QualificationMember', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.qualificationmemberperm',

    permissions: [
        { name: 'GkhCr.QualificationMember.Create', applyTo: 'b4addbutton', selector: '#qualificationMemberGrid' },
        { name: 'GkhCr.QualificationMember.Edit', applyTo: 'b4savebutton', selector: '#qualificationMemberEditWindow' },
        { name: 'GkhCr.QualificationMember.Delete', applyTo: 'b4deletecolumn', selector: '#qualificationMemberGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});