//Todo �������� ������ ��������� � ��� �������� ��������� ����� ������ ����� ����� �������� NsoDisposal ������� subclass
Ext.define('B4.model.Disposal', {
    extend: 'B4.model.DocumentGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'Disposal'
    },
    fields: [
        { name: 'Inspection', defaultValue: null },
        { name: 'IssuedDisposal', defaultValue: null },
        { name: 'ResponsibleExecution', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'TypeDisposal', defaultValue: 10 },
        { name: 'TypeAgreementProsecutor', defaultValue: 10 },
        { name: 'TypeAgreementResult', defaultValue: 10 },
        { name: 'KindCheck', defaultValue: null },
        { name: 'ActCheckGeneralId' },
        { name: 'ParentDocumentsList' },
        { name: 'TypeDocumentGji', defaultValue: 10 },
        { name: 'IsActCheckExist' },
        { name: 'RealityObjectCount' },
        { name: 'TypeSurveyNames' },
        { name: 'TypeBase' },
        { name: 'KindKNDGJI' },
        { name: 'ContragentName' },
        { name: 'MunicipalityNames' },
        { name: 'InspectorNames' },
        { name: 'InspectionId', defaultValue: null },
        { name: 'InspectionType', defaultValue: null },
        { name: 'Description' },
        { name: 'ObjectVisitStart' },
        { name: 'ObjectVisitEnd' },
        { name: 'OutInspector', defaultValue: false },
        { name: 'HasChildrenActCheck', defaultValue: false },
        { name: 'HasActSurvey', defaultValue: false },
        { name: 'TimeVisitStart' },
        { name: 'TimeVisitEnd' },
        { name: 'DateStatement' },
        { name: 'TimeStatement' },
        { name: 'PoliticAuthority' },
        { name: 'NcNum' },
        { name: 'NcDate' },
        { name: 'NcNumLatter' },
        { name: 'NcDateLatter' },
        { name: 'MotivatedRequestNumber' },
        { name: 'MotivatedRequestDate' },
        { name: 'NcObtained', defaultValue: 20 },
        { name: 'NcSent', defaultValue: 20 },
        { name: 'PeriodCorrect' },
        { name: 'NoticeDateProtocol' },
        { name: 'NoticeTimeProtocol' },
        { name: 'NoticePlaceCreation' },
        { name: 'NoticeDescription' },
        { name: 'FactViols' },
        { name: 'FactViolIds' },
        { name: 'ProsecutorDecNumber' },
        { name: 'ProsecutorDecDate' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'Stage' },
        { name: 'ProcAprooveFile' },
        { name: 'ProcAprooveDate' },
        { name: 'FioProcAproove' },
        { name: 'PositionProcAproove' },
        { name: 'ProcAprooveNum' },
        { name: 'CountDays' },
        { name: 'CountHours' }
    ]
});