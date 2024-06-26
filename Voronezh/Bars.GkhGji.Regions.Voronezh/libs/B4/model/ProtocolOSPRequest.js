﻿Ext.define('B4.model.ProtocolOSPRequest', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolOSPRequest'
    },
    fields: [
        { name: 'Id' },
        { name: 'FIO' },
        { name: 'State' },
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'RoFiasGuid' },
        { name: 'UserEsiaGuid' },
        { name: 'Date' },
        { name: 'Room' },
        { name: 'DateFrom' },
        { name: 'DateTo' },
        { name: 'CadastralNumber' },
        { name: 'AttorneyDate' },
        { name: 'AttorneyNumber' },
        { name: 'AttorneyFio' },
        { name: 'ResolutionContent' },
        { name: 'FileInfo' },
        { name: 'GjiId' },
        { name: 'Approved', defaultValue: 30 },
        { name: 'OwnerProtocolType' },

        { name: 'PhoneNumber' },
        { name: 'ProtocolNum' },
        { name: 'ProtocolDate' },
        { name: 'Inspector' },
        { name: 'ApplicantType' },
        { name: 'Note' },
        { name: 'DocDate' },
        { name: 'DocNumber' },
        { name: 'RequestNumber' },
        { name: 'ProtocolFile' },
        { name: 'AttorneyFile' },
        { name: 'Email' },
    ]
});