Ext.define('B4.controller.report.PrescriptionViolationRemoval', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PrescriptionViolationRemovalPanel',
    mainViewSelector: '#prescriptionViolationRemovalPanel',

    requires: [
        'B4.ux.button.Update'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: '#prescriptionViolationRemovalPanel #sfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#prescriptionViolationRemovalPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#prescriptionViolationRemovalPanel #dfDateEnd'
        }
    ],
    
    validateParams: function () {
        var mcpField = this.getMunicipalityField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        
        return (mcpField && mcpField.isValid() 
            && dateStartField && dateStartField.isValid() 
            && dateEndField && dateEndField.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();

        return {
            municipalityId: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});