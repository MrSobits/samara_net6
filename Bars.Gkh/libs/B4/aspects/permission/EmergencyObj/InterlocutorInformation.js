Ext.define('B4.aspects.permission.emergencyobj.InterlocutorInformation', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.emergencyobjinterlocutorinformation',

    permissions: [
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.ApartmentArea_Edit', applyTo: '[name=ApartmentArea]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.PropertyType_Edit', applyTo: '[name=PropertyType]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.AvailabilityMinorsAndIncapacitatedProprietors_Edit', applyTo: '[name=AvailabilityMinorsAndIncapacitatedProprietors]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateDemolitionIssuing_Edit', applyTo: '[name=DateDemolitionIssuing]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateDemolitionReceipt_Edit', applyTo: '[name=DateDemolitionReceipt]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateNotification_Edit', applyTo: '[name=DateNotification]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateNotificationReceipt_Edit', applyTo: '[name=DateNotificationReceipt]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAgreement_Edit', applyTo: '[name=DateAgreement]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAgreementRefusal_Edit', applyTo: '[name=DateAgreementRefusal]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateOfReferralClaimCourt_Edit', applyTo: '[name=DateOfReferralClaimCourt]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateOfDecisionByTheCourt_Edit', applyTo: '[name=DateOfDecisionByTheCourt]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.ConsiderationResultClaim_Edit', applyTo: '[name=ConsiderationResultClaim]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAppeal_Edit', applyTo: '[name=DateAppeal]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.DateAppealDecision_Edit', applyTo: '[name=DateAppealDecision]', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Interlocutor.AppealResult_Edit', applyTo: '[name=AppealResult]', selector: '#emergencyObjInterlocutorInformationEditWindow' },

        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Create', applyTo: 'b4addbutton', selector: '#emergencyObjInterlocutorInformationGrid' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Edit', applyTo: 'b4savebutton', selector: '#emergencyObjInterlocutorInformationEditWindow' },
        { name: 'Gkh.EmergencyObject.InterlocutorInformation.Delete', applyTo: 'b4deletecolumn', selector: '#emergencyObjInterlocutorInformationGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});