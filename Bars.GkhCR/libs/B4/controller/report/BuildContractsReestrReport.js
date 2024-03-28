Ext.define('B4.controller.report.BuildContractsReestrReport', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.BuildContractsReestrReportPanel',
    mainViewSelector: '#buildContractsReestrReportPanel',

    refs: [
        {
            ref: 'ProgramCrSelectField',
            selector: '#buildContractsReestrReportPanel #sfProgramCr'
        },
        {
            ref: 'MunicipalitySelectField',
            selector: '#buildContractsReestrReportPanel #sfMunicipality'
        },
        {
            ref: 'ContractsFilterCheckboxField',
            selector: '#buildContractsReestrReportPanel #cbContractsFilter'
        }
    ],
    
    validateParams: function () {
        var prCrId = this.getProgramCrSelectField();
        return prCrId && prCrId.isValid();
    },

    init: function () {
        this.control({
            '#buildContractsReestrReportPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField = this.getProgramCrSelectField();
        var mcpField = this.getMunicipalitySelectField();
        var filterField = this.getContractsFilterCheckboxField();

        return {
            programCrId: programmField.getValue(),
            municipalityIds: Ext.encode(mcpField.getValue()),
            contractsFilter: filterField.getValue()
        };
    }
});