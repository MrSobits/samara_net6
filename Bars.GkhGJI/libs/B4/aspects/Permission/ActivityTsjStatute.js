Ext.define('B4.aspects.permission.ActivityTsjStatute', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.activitytsjstatutestateperm',

    permissions: [
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.StatuteApprovalDate', applyTo: '#dfStatuteApprovalDate', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.StatuteProvisionDate', applyTo: '#dfStatuteProvisionDate', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.StatuteFile', applyTo: '#ffStatuteFile', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.TypeConclusion', applyTo: '#cbxTypeConclusion', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.ConclusionNum', applyTo: '#tfConclusionNum', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.ConclusionDate', applyTo: '#dfConclusionDate', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.ConclusionFile', applyTo: '#ffConclusionFile', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Register.Statute.Field.ConclusionDescription', applyTo: '#taConclusionDescription', selector: '#activityTsjStatuteEditWindow' }
    ]
});