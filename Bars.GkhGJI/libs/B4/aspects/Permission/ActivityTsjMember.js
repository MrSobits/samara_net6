Ext.define('B4.aspects.permission.ActivityTsjMember', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.activitytsjmemberstateperm',

    permissions: [
        { name: 'GkhGji.ActivityTsj.Register.Member.Field.File', applyTo: '#ffStatuteFile', selector: '#activityTsjMemberEditWindow' }
    ]
});