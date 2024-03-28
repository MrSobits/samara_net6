Ext.define('B4.view.prescription.MenuButton', {
    extend: 'Ext.button.Button',
    requires: [
        'B4.DisposalTextValues'
    ],
    
    alias: 'widget.prescriptionmenubutton',
    
    iconCls: 'icon-accept',
    itemId: 'btnCreateDocument',
    text: 'Сформировать',
    menu: [
        {
            text: 'Протокол',
            textAlign: 'left',
            itemId: 'btnCreatePrescriptionToProtocol',
            actionName: 'createPrescriptionToProtocol'
        },
        {
            text: B4.DisposalTextValues.getSubjectiveForPrescriptionCase(),
            textAlign: 'left',
            itemId: 'btnCreatePrescriptionToDisposal',
            actionName: 'createPrescriptionToDisposal'
        }
    ]
});
