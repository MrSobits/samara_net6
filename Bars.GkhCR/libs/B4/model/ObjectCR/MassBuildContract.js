Ext.define('B4.model.objectcr.MassBuildContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MassBuildContract'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProgramCr' },
        { name: 'Inspector' },
        { name: 'InspectorName' },
        { name: 'Builder' },
        { name: 'BuilderName' },
        { name: 'TypeContractBuild', defaultValue: 10 },
        { name: 'BuilderInn' },
        { name: 'RoMunicipality' },
        { name: 'RoSettlement' },
        { name: 'RoAddress' },
        { name: 'TypeContractBuild', defaultValue: 10},
        { name: 'DateEndWork' },
        { name: 'DateInGjiRegister' },
        { name: 'DocumentDateFrom' },
        { name: 'ProtocolDateFrom' },
        { name: 'DateCancelReg' },
        { name: 'DateAcceptOnReg' },
        { name: 'DocumentName' },
        { name: 'ProtocolName' },
        { name: 'DocumentNum' },
        { name: 'ProtocolNum' },
        { name: 'Description' },
        { name: 'BudgetMo', defaultValue: null },
        { name: 'BudgetSubject', defaultValue: null },
        { name: 'OwnerMeans', defaultValue: null },
        { name: 'FundMeans', defaultValue: null },
        { name: 'Sum' },
        { name: 'StartSum' },
        { name: 'DateStartWork' },
        { name: 'State', defaultValue: null },
        { name: 'DocumentFile', defaultValue: null },
        { name: 'ProtocolFile', defaultValue: null },
        { name: 'IsClaimWorking', defaultValue: null },
        { name: 'Text'},
        { name: 'TypeWork' },
        { name: 'UsedInExport', defaultValue: 20 },
        { name: 'Contragent' },
        { name: 'TerminationDate', defaultValue: null },
        { name: 'TerminationDocumentFile', defaultValue: null },
        { name: 'TerminationReason', defaultValue: null },
        { name: 'GuaranteePeriod', defaultValue: null },
        { name: 'UrlResultTrading', defaultValue: null },
        { name: 'TerminationDocumentNumber', defaultValue: null },
        { name: 'TerminationDictReason', defaultValue: null }
    ]
});