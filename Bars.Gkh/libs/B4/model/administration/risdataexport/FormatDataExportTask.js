Ext.define('B4.model.administration.risdataexport.FormatDataExportTask', {
    extend: 'B4.model.SchedulableTask',
    fields: [
        { name: 'Login' },
        { name: 'TriggerName' },
        { name: 'CreateDate' },
        { name: 'EntityGroupCodeList', useNull: false, defaultValue: [] },

        { name: 'BaseParams' },
        { name: 'DisplayParams', useNull: true },
        { name: 'MainContragent' },
        { name: 'MunicipalityList', defaultValue: [] },
        { name: 'ContragentList', defaultValue: [] },
        { name: 'UseIncremental', defaultValue: false },
        { name: 'StartEditDate', useNull: true },
        { name: 'EndEditDate', useNull: true },
        { name: 'MaxFileSize', useNull: true },
        { name: 'IsSeparateArch', defaultValue: false },
        { name: 'WithoutAttachment', defaultValue: false },
        { name: 'NoEmptyMandatoryFields', defaultValue: false },
        { name: 'OnlyExistsFiles', defaultValue: false },
        { name: 'AuditType', useNull: false, defaultValue: 0 },
        { name: 'DisposalStartDate', useNull: true },
        { name: 'DisposalEndDate', useNull: true },
        { name: 'ChargePeriod', useNull: true },
        { name: 'ProgramVersionMunicipalityList', defaultValue: [] },
        { name: 'ProgramCrList', defaultValue: [] },
        { name: 'ObjectCrMunicipalityList', defaultValue: [] }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'FormatDataExportTask'
    },
});