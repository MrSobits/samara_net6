Ext.define('B4.model.specialobjectcr.qualification.VoiceMember', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialVoiceMember'
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