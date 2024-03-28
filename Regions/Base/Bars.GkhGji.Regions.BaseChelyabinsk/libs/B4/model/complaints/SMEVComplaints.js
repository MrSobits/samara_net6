Ext.define('B4.model.complaints.SMEVComplaints', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaints'
    },
    fields: [
        { name: 'Inspector'},
        { name: 'State' },
        { name: 'ComplaintId', defaultValue: null},
        { name: 'Number' },
        { name: 'CommentInfo' },
        { name: 'EsiaOid' },
        { name: 'RequesterName' },
        { name: 'RequesterRole', defaultValue: 0 },
        { name: 'RequesterContragent', defaultValue: null },
        { name: 'RequesterFIO' },
        { name: 'IdentityDocumentType', defaultValue: 2 },
        { name: 'DocSeries' },
        { name: 'DocNumber' },
        { name: 'INNFiz' },
        { name: 'ComplaintState' },
        { name: 'SNILS' }, 
        { name: 'BirthDate' },
        { name: 'BirthAddress' },
        { name: 'Gender', defaultValue: 0 },
        { name: 'Nationality' },
        { name: 'RegAddess' },
        { name: 'Email' },
        { name: 'MobilePhone' },
        { name: 'Ogrnip' },
        { name: 'LegalFullName' },
        { name: 'Ogrn' },
        { name: 'SMEVComplaintsDecision' },
        { name: 'Inn' }, 
        { name: 'LegalAddress' },
        { name: 'WorkingPosition' },
        { name: 'RevokeFlag' },
        { name: 'ComplaintDate' },
        { name: 'Okato' },
        { name: 'AppealNumber' },
        { name: 'TypeAppealDecision' },
        { name: 'LifeEvent' },
        { name: 'RequestDate' },
        { name: 'Answer' },
        { name: 'DecisionReason' },
        { name: 'MessageId' },
        { name: 'Request_ID' },
        { name: 'Entry_ID' },
        { name: 'RequestDate' },
        { name: 'FileInfo' }
    ]
});