// Этот "псевдо"-аспект сделан для того чтобы не переопределять в регионе контроллер чисто из за этого аспекта


Ext.define('B4.aspects.regop.PersAccImportAspect', {
    extend: 'B4.aspects.GkhButtonImportAspect',

    alias: 'widget.persaccimportaspect',

    requires: [
        'B4.view.import.PersonalAccountImportWindow',
        'B4.view.regop.personal_account.PersonalAccountGrid',
        'B4.enums.BenefitsCategoryImportIdentificationType'
    ],

    name: 'personalAccImportAspect',
    buttonSelector: 'paccountgrid #btnImport',
    codeImport: 'PersonalAccountImport',
    windowImportView: 'import.PersonalAccountImportWindow',
    windowImportSelector: 'personalaccountimportwin',
    maxFileSize: 2097152000, //в байтах 2000 мб

    listeners: {
        aftercreatewindow: function (window, importId) {
            var replaceDataChkBox = window.down('[name=replaceData]'),
                identifTypeCombo = window.down('[name=IdentificationType]');
            if (importId === 'Bars.Gkh.RegOperator.Imports.BenefitsCategoryImport') {
                replaceDataChkBox.setVisible(true);
                identifTypeCombo.setVisible(false);
            } else if (importId === 'Bars.Gkh.RegOperator.Imports.BenefitsCategoryImportVersion2') {
                replaceDataChkBox.setVisible(true);
                identifTypeCombo.setVisible(true);
            } else {
                replaceDataChkBox.setVisible(false);
                identifTypeCombo.setVisible(false);
            }
        }
    }
});