Ext.define('B4.aspects.permission.dict.QualificationMember', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.qualificationmemberperm',

    permissions: [
        { name: 'GkhCr.Dict.QualMember.Create', applyTo: 'b4addbutton', selector: '#qualificationMemberGrid' },
        { name: 'GkhCr.Dict.QualMember.Edit', applyTo: 'b4savebutton', selector: '#qualificationMemberEditWindow' },
        { name: 'GkhCr.Dict.QualMember.Delete', applyTo: 'b4deletecolumn', selector: '#qualificationMemberGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});