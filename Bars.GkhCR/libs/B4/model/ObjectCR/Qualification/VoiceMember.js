Ext.define('B4.model.objectcr.qualification.VoiceMember', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VoiceMember'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Qualification', defaultValue: null },
        { name: 'QualificationMember', defaultValue: null },
        { name: 'MemberName' },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'Reason' },
        { name: 'TypeAcceptQualification', defaultValue: 10 }
    ]
});