Ext.define('B4.model.SpecialAccountDecisionNotice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountDecisionNotice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'State' },
        { name: 'SpecialAccountDecision' },
        { name: 'NoticeNumber' },
        { name: 'NoticeDate' },
        { name: 'File' },
        { name: 'RegDate' },
        { name: 'GjiNumber' },
        { name: 'HasOriginal' },
        { name: 'HasCopyProtocol' },
        { name: 'HasCopyCertificate' },
        { name: 'ContragentOkato' },
        { name: 'ContragentOktmo' },
        { name: 'ContragentOgrn' },
        { name: 'ContragentKpp' },
        { name: 'ContragentInn' },
        { name: 'ContragentMailingAddress' },
        { name: 'ContragentName' },
        { name: 'DecTypeOrganization' },
        { name: 'ProtocolDate' },
        { name: 'DecOpenDate' },
        { name: 'MunicipalityName' },
        { name: 'Address' },
        { name: 'MethodFormFund' },
        { name: 'RealityObject' }
    ]
});