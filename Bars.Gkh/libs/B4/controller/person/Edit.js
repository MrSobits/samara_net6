Ext.define('B4.controller.person.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.StateContextButton',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditCtxWindow',
        'B4.view.person.QualificationGrid',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.BackForward',
        'B4.aspects.StateContextMenu',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.enums.QualificationDocumentType',
        'B4.view.person.DetailsEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'Person',
        'person.QualificationCertificate',
        'person.RequestToExam'
    ],

    stores: [
        'person.QExamQuestion'
    ],
    
    requestId: null,
    questionId: null,
    questionCount: 0,
    qualifyTestId: null,
    
    views: [
        'person.EditPanel',
        'person.QualificationGrid',
        'person.QualificationEditWindow',
        'person.ExamWindow',
        'person.RequestToExamEditWindow',
        'person.RequestToExamGrid',
        'person.QualificationDocumentGrid',
        'person.QualificationDocumentEditWindow',
        'person.TechnicalMistakeGrid',
        'person.AnswersGrid'
    ],

    mainView: 'person.EditPanel',
    mainViewSelector: 'personEditPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'personEditPanel'
        },
        {
            ref: 'qualGrid',
            selector: 'personqualificationgrid'
        },
        {
            ref: 'qualEditWindow',
            selector: 'personqualificationeditwindow'
        }
    ],
    
    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.Edit', applyTo: 'b4savebutton', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.RequestToExamView', applyTo: 'personrequesttoexamgrid', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {

                        if (component) {
                            if (allowed) {
                                component.tab.up('.tabpanel').setActiveTab(0);
                                component.tab.show();
                            } else {
                                component.tab.up('.tabpanel').setActiveTab(1);
                                component.tab.hide();
                            }
                        }

                    }
                },
                { name: 'Gkh.Person.RequestToExamCreate', applyTo: 'b4addbutton', selector: 'personrequesttoexamgrid' },
                { name: 'Gkh.Person.Qualification.Create', applyTo: 'b4addbutton', selector: 'personqualificationgrid' },
                {
                    name: 'Gkh.Person.Qualification.Field.RequestToExam_View', applyTo: 'gridcolumn[dataIndex=RequestToExamName]', selector: 'personqualificationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                
                // Fields
                { name: 'Gkh.Person.Field.Surname_Edit', applyTo: '[name=Surname]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Surname_View', applyTo: '[name=Surname]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Name_Edit', applyTo: '[name=Name]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Name_View', applyTo: '[name=Name]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Patronymic_Edit', applyTo: '[name=Patronymic]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Patronymic_View', applyTo: '[name=Patronymic]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Email_Edit', applyTo: '[name=Email]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Email_View', applyTo: '[name=Email]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Phone_Edit', applyTo: '[name=Phone]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Phone_View', applyTo: '[name=Phone]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.AddressReg_Edit', applyTo: '[name=AddressReg]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.AddressReg_View', applyTo: '[name=AddressReg]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.AddressLive_Edit', applyTo: '[name=AddressLive]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.AddressLive_View', applyTo: '[name=AddressLive]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.AddressBirth_Edit', applyTo: '[name=AddressBirth]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.AddressBirth_View', applyTo: '[name=AddressBirth]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Birthdate_Edit', applyTo: '[name=Birthdate]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Birthdate_View', applyTo: '[name=Birthdate]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.Inn_Edit', applyTo: '[name=Inn]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.Inn_View', applyTo: '[name=Inn]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.TypeIdentityDocument_Edit', applyTo: '[name=TypeIdentityDocument]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.TypeIdentityDocument_View', applyTo: '[name=TypeIdentityDocument]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.IdIssuedDate_Edit', applyTo: '[name=IdIssuedDate]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.IdIssuedDate_View', applyTo: '[name=IdIssuedDate]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.IdSerial_Edit', applyTo: '[name=IdSerial]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.IdSerial_View', applyTo: '[name=IdSerial]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.IdNumber_Edit', applyTo: '[name=IdNumber]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.IdNumber_View', applyTo: '[name=IdNumber]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.Field.IdIssuedBy_Edit', applyTo: '[name=IdIssuedBy]', selector: 'personEditPanel' },
                {
                    name: 'Gkh.Person.Field.IdIssuedBy_View', applyTo: '[name=IdIssuedBy]', selector: 'personEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personqualificationStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.Qualification.Edit', applyTo: 'b4savebutton[name=main]', selector: 'personqualificationeditwindow' },
                { name: 'Gkh.Person.Qualification.TechnicalMistace.View', applyTo: 'tab[title=Информация о тех. ошибках]', selector: 'personqualificationeditwindow tabpanel' },
                {
                    name: 'Gkh.Person.Qualification.Field.RequestToExam_View', applyTo: '[name=RequestToExam]', selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }

                    }
                },

                {
                    name: 'Gkh.Person.Qualification.Field.QualificationDocument_View',
                    applyTo: '[name=File]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Field.QualificationDocument_Edit',
                    applyTo: '[name=File]',
                    selector: 'personqualificationeditwindow'
                },

                {
                    name: 'Gkh.Person.Qualification.Field.QualificationNotification_View',
                    applyTo: '[name=FileNotificationOfExamResults]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Field.QualificationNotification_Edit',
                    applyTo: '[name=FileNotificationOfExamResults]',
                    selector: 'personqualificationeditwindow'
                },

                { name: 'Gkh.Person.Qualification.Field.RecieveDate_Edit', applyTo: '[name=RecieveDate]', selector: 'personqualificationeditwindow' },
                {
                    name: 'Gkh.Person.Qualification.Field.RecieveDate_View',
                    applyTo: '[name=RecieveDate]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Field.IsFromAnotherRegion_View',
                    applyTo: '[name=IsFromAnotherRegion]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Field.IsFromAnotherRegion_Edit',
                    applyTo: '[name=IsFromAnotherRegion]',
                    selector: 'personqualificationeditwindow'
                },
                {
                    name: 'Gkh.Person.Qualification.Field.RegionCode_View',
                    applyTo: '[name=RegionCode]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Field.RegionCode_Edit',
                    applyTo: '[name=RegionCode]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        var checkBox,
                            checkBoxNotAllowed = false;

                        if (component) {
                            checkBox = component.up().down('[name=IsFromAnotherRegion]');
                            if (checkBox) {
                                checkBoxNotAllowed = !checkBox.getValue();
                            }
                            component.setDisabled(!allowed || checkBoxNotAllowed);
                            component.manualDisabled = !allowed;
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Duplicate.View',
                    applyTo: 'qualificationdocumentgrid[name=Duplicate]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        var tabPanel,
                            permArray;

                        if (component.tab) {
                            tabPanel = component.tab.up('.tabpanel');

                            if (allowed) {
                                tabPanel.setActiveTab(0);
                                component.tab.show();
                            } else {
                                tabPanel.setActiveTab(1);
                                component.tab.hide();
                            }
                        }

                        if (tabPanel) {
                            permArray = tabPanel.permArray || [];

                            if (permArray[0] === null) {
                                permArray[0] = allowed;
                            } else {
                                permArray[0] = null;
                            }

                            tabPanel.permArray = permArray;
                        }
                    }
                },
                {
                    name: 'Gkh.Person.Qualification.Duplicate.View',
                    applyTo: 'qualificationdocumentgrid[name=Renew]',
                    selector: 'personqualificationeditwindow',
                    applyBy: function (component, allowed) {
                        var tabPanel,
                            permArray;

                        if (component.tab) {
                            tabPanel = component.tab.up('.tabpanel');

                            if (allowed) {
                                tabPanel.setActiveTab(0);
                                component.tab.show();
                            } else {
                                tabPanel.setActiveTab(1);
                                component.tab.hide();
                            }
                        }

                        if (tabPanel) {
                            permArray = tabPanel.permArray || [];

                            if (permArray[1] === null) {
                                permArray[1] = allowed;
                            } else {
                                permArray[1] = null;
                            }

                            tabPanel.permArray = permArray;
                        }
                    }
                },

                { name: 'Gkh.Person.Qualification.Duplicate.Edit', applyTo: 'b4savebutton', selector: 'qualificationdocumentgrid[name=Duplicate]' },
                { name: 'Gkh.Person.Qualification.Duplicate.Create', applyTo: 'b4addbutton', selector: 'qualificationdocumentgrid[name=Duplicate]' },
                {
                    name: 'Gkh.Person.Qualification.Duplicate.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'qualificationdocumentgrid[name=Duplicate]',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },

                { name: 'Gkh.Person.Qualification.Renew.Edit', applyTo: 'b4savebutton', selector: 'qualificationdocumentgrid[name=Renew]' },
                { name: 'Gkh.Person.Qualification.Renew.Create', applyTo: 'b4addbutton', selector: 'qualificationdocumentgrid[name=Renew]' },
                {
                    name: 'Gkh.Person.Qualification.Renew.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'qualificationdocumentgrid[name=Renew]',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },

                { name: 'Gkh.Person.Qualification.TechnicalMistake.Edit', applyTo: 'b4savebutton', selector: 'technicalmistakegrid' },
                { name: 'Gkh.Person.Qualification.TechnicalMistake.Create', applyTo: 'b4addbutton', selector: 'technicalmistakegrid' },
                {
                    name: 'Gkh.Person.Qualification.TechnicalMistake.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'technicalmistakegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ],

            listeners: {
                apply: function(asp, allowed, options) {
                    var controller = asp.controller,
                        tabPanel = controller.getAspect('personQualificationAspect').getForm().down('tabpanel[name=DublicatePanel]'),
                        allInitialized = true,
                        anyVisible = false;

                    if (tabPanel) {
                        Ext.Array.each(tabPanel.permArray,
                            function (element) {
                                if (element !== null) {
                                    anyVisible = anyVisible | element;
                                } else {
                                    allInitialized = false;
                                }
                            });

                        if (allInitialized) {
                            tabPanel.disabledByPermissions = !anyVisible;
                        }
                    }
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'deleteQualificationStatePerm',
            permissions: [
                { name: 'Gkh.Person.Qualification.Delete' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'personRequestToExamAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Gkh.Person.RequestToExam.Edit', applyTo: 'b4savebutton', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.RequestSupplyMethod_View', applyTo: '[name=RequestSupplyMethod]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.RequestSupplyMethod_Edit', applyTo: '[name=RequestSupplyMethod]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.RequestNum_View', applyTo: '[name=RequestNum]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.RequestNum_Edit', applyTo: '[name=RequestNum]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.RequestDate_View', applyTo: '[name=RequestDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.RequestDate_Edit', applyTo: '[name=RequestDate]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.RequestTime_View', applyTo: '[name=RequestTime]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.RequestTime_Edit', applyTo: '[name=RequestTime]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.RequestFile_View', applyTo: '[name=RequestFile]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.RequestFile_Edit', applyTo: '[name=RequestFile]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_View', applyTo: '[name=PersonalDataConsentFile]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.PersonalDataConsentFile_Edit', applyTo: '[name=PersonalDataConsentFile]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.NotificationNum_View', applyTo: '[name=NotificationNum]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.NotificationNum_Edit', applyTo: '[name=NotificationNum]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.NotificationDate_View', applyTo: '[name=NotificationDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.NotificationDate_Edit', applyTo: '[name=NotificationDate]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.IsDenied_View', applyTo: '[name=IsDenied]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.IsDenied_Edit', applyTo: '[name=IsDenied]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ExamDate_View', applyTo: '[name=ExamDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ExamDate_Edit', applyTo: '[name=ExamDate]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ExamTime_View', applyTo: '[name=ExamTime]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ExamTime_Edit', applyTo: '[name=ExamTime]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_View', applyTo: '[name=CorrectAnswersPercent]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.CorrectAnswersPercent_Edit', applyTo: '[name=CorrectAnswersPercent]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ProtocolNum_View', applyTo: '[name=ProtocolNum]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ProtocolNum_Edit', applyTo: '[name=ProtocolNum]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ProtocolDate_View', applyTo: '[name=ProtocolDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ProtocolDate_Edit', applyTo: '[name=ProtocolDate]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ProtocolFile_View', applyTo: '[name=ProtocolFile]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ProtocolFile_Edit', applyTo: '[name=ProtocolFile]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ResultNotificationNum_View', applyTo: '[name=ResultNotificationNum]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ResultNotificationNum_Edit', applyTo: '[name=ResultNotificationNum]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.ResultNotificationDate_View', applyTo: '[name=ResultNotificationDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.ResultNotificationDate_Edit', applyTo: '[name=ResultNotificationDate]', selector: 'personrequesttoexameditwin' },
                {
                    name: 'Gkh.Person.RequestToExam.Field.MailingDate_View', applyTo: '[name=MailingDate]', selector: 'personrequesttoexameditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'Gkh.Person.RequestToExam.Field.MailingDate_Edit', applyTo: '[name=MailingDate]', selector: 'personrequesttoexameditwin' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'deleteRequestToExamStatePerm',
            permissions: [
                { name: 'Gkh.Person.RequestToExam.Delete' }
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.PersonRegisterGji.Field.Email_Rqrd', applyTo: '#tfEmail', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.Number_Rqrd', applyTo: '[name=Number]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.BlankNumber_Rqrd', applyTo: '[name=BlankNumber]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.RecieveDate_Rqrd', applyTo: '[name=RecieveDate]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.IssuedDate_Rqrd', applyTo: '[name=IssuedDate]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.FileIssueApplication_Rqrd', applyTo: '[name=FileIssueApplication]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.ApplicationDate_Rqrd', applyTo: '[name=ApplicationDate]', selector: 'personqualificationeditwindow' },
                { name: 'GkhGji.PersonRegisterGji.Qualification.Field.FileNotificationOfExamResults_Rqrd', applyTo: '[name=FileNotificationOfExamResults]', selector: 'personqualificationeditwindow' },

                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.RequestSupplyMethod_Rqrd', applyTo: '[name=RequestSupplyMethod]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.RequestNum_Rqrd', applyTo: '[name=RequestNum]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.RequestDate_Rqrd', applyTo: '[name=RequestDate]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.RequestTime_Rqrd', applyTo: '[name=RequestTime]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.RequestFile_Rqrd', applyTo: '[name=RequestFile]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.PersonalDataConsentFile_Rqrd', applyTo: '[name=PersonalDataConsentFile]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.NotificationNum_Rqrd', applyTo: '[name=NotificationNum]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.NotificationDate_Rqrd', applyTo: '[name=NotificationDate]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.IsDenied_Rqrd', applyTo: '[name=IsDenied]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ExamDate_Rqrd', applyTo: '[name=ExamDate]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ExamTime_Rqrd', applyTo: '[name=ExamTime]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.CorrectAnswersPercent_Rqrd', applyTo: '[name=CorrectAnswersPercent]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ProtocolDate_Rqrd', applyTo: '[name=ProtocolDate]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ProtocolFile_Rqrd', applyTo: '[name=ProtocolFile]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ResultNotificationNum_Rqrd', applyTo: '[name=ResultNotificationNum]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.ResultNotificationDate_Rqrd', applyTo: '[name=ResultNotificationDate]', selector: 'personrequesttoexameditwin' },
                { name: 'GkhGji.PersonRegisterGji.RequestToExam.Field.MailingDate_Rqrd', applyTo: '[name=MailingDate]', selector: 'personrequesttoexameditwin' },

                { name: 'GkhGji.PersonRegisterGji.Field.Inn_Rqrd', applyTo: '[name=Inn]', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Field.TypeIdentityDocument_Rqrd', applyTo: '[name=TypeIdentityDocument]', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdIssuedDate_Rqrd', applyTo: '[name=IdIssuedDate]', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdSerial_Rqrd', applyTo: '[name=IdSerial]', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdNumber_Rqrd', applyTo: '[name=IdNumber]', selector: 'personEditPanel' },
                { name: 'GkhGji.PersonRegisterGji.Field.IdIssuedBy_Rqrd', applyTo: '[name=IdIssuedBy]', selector: 'personEditPanel' }
            ]
        },
        {
            /*
             * Вешаем аспект смены статуса в карточке редактирования
             */
            xtype: 'statecontextbuttonaspect',
            name: 'personStateButtonAspect',
            stateButtonSelector: 'personEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                }
            }
        },
        {
            //Аспект кнопки печати акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'personPrintAspect',
            buttonSelector: 'personEditPanel #btnPrint',
            codeForm: 'Person',
            getUserParams: function (reportId) {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { PersonId: me.controller.getContextValue(view, 'personId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'backforwardaspect',
            panelSelector: 'personEditPanel',
            backForwardController: 'Person'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'requestToExamStateTransferAspect',
            gridSelector: 'personrequesttoexamgrid',
            menuSelector: 'personrequesttoexamgridStateMenu',
            stateType: 'gkh_person_request_exam'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'qualificationregistryStateTransferAspect',
            gridSelector: 'personqualificationregistrygrid',
            menuSelector: 'personqualificationregistrygridStateMenu',
            stateType: 'gkh_person_qc'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'personqualificationgridStateTransferAspect',
            gridSelector: 'personqualificationgrid',
            menuSelector: 'personqualificationgridStateMenu',
            stateType: 'gkh_person_qc'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'persononlinetestPrintAspect',
            buttonSelector: 'personrequesttoexameditwin #btnPrint',
            codeForm: 'PersonOnlineTest',
            getUserParams: function () {
                var param = { Id: this.controller.qualifyTestId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'statecontextbuttonaspect',
            name: 'requestToExamStateButtonAspect',
            stateButtonSelector: 'personrequesttoexameditwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    asp.setStateData(entityId, newState);
                    var editWindowAspect = asp.controller.getAspect('personRequestToExamAspect');
                    var model = asp.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'personEditPanelAspect',
            editPanelSelector: 'personEditPanel',
            modelName: 'Person',
            otherActions: function (actions) {

                actions['personEditPanel #btnMVD'] = { 'click': { fn: this.sendRequest, scope: this } };
            },
            listeners: {
                savesuccess: function (asp, rec) {
                    asp.setData(rec.getId());
                },
                aftersetpaneldata: function (asp, rec, panel) {
                    var me = this,
                        gridQual = panel.down('personqualificationgrid'),
                        gridRequestExam = panel.down('personrequesttoexamgrid'),
                        storeQual = gridQual.getStore(),
                        storeRequestExam = gridRequestExam.getStore();
                    
                    storeQual.clearFilter(true);
                    storeQual.filter('personId', rec.getId());
                    
                    storeRequestExam.clearFilter(true);
                    storeRequestExam.filter('personId', rec.getId());
                    
                    me.controller.getAspect('personPrintAspect').loadReportStore();
                    me.controller.getAspect('personStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    me.controller.getAspect('personStatePermAspect').setPermissionsByRecord({ getId: function () { return rec.get('Id'); } });
                }
            },
            sendRequest: function (record) {
                var me = this;
                var personId = me.controller.getContextValue(me.controller.getMainView(), 'personId');
                
                me.mask('Обмен данными с МВД', me.controller.getMainView());
                var result = B4.Ajax.request(B4.Url.action('CreateMVDRequest', 'SMEVMVDExecute', {
                        personId: personId
                    }
                    )).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        //var grid = form.down('gisgmpfileinfogrid'),
                        //    store = grid.getStore();
                        //store.filter('GisGmp', taskId);

                        me.unmask();

                        return true;
                    })
                        .error(function (resp) {
                            Ext.Msg.alert('Ошибка', resp.message);
                            me.unmask();
                        });
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'personQualificationAspect',
            gridSelector: 'personqualificationgrid',
            modelName: 'person.QualificationCertificate',
            editFormSelector: 'personqualificationeditwindow',
            editWindowView: 'person.QualificationEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.Person = me.controller.getCurrent();
                    }
                },
                aftersetformdata: function (me, record) {
                    var personId = me.controller.getContextValue(me.controller.getMainView(), 'personId'),
                        form = this.getForm(),
                        duplicateSet = form.down('[type=duplicateSet]'),
                        technicalmistakeGrid = form.down('technicalmistakegrid'),
                        duplicateGrid = duplicateSet.down('qualificationdocumentgrid[name=Duplicate]'),
                        renewGrid = duplicateSet.down('qualificationdocumentgrid[name=Renew]'),
                        hasValueCb = form.down('[name=HasDuplicate]');

                    me.controller.getAspect('personqualificationStatePermAspect').setPermissionsByRecord({ getId: function () { return personId; } });

                    if (!record.phantom) {
                        duplicateGrid.getStore().filter([
                            { property: 'qualificationId', value: record.getId() },
                            { property: 'typeDocument', value: B4.enums.QualificationDocumentType.Duplicate }
                        ]);

                        renewGrid.getStore().filter([
                            { property: 'qualificationId', value: record.getId() },
                            { property: 'typeDocument', value: B4.enums.QualificationDocumentType.Renew }
                        ]);

                        technicalmistakeGrid.getStore().filter('qualificationId', record.getId());
                    } else {
                        if (technicalmistakeGrid.tab) {
                            technicalmistakeGrid.tab.setDisabled(true);
                        }
                    }

                    me.onDuplicateChange(hasValueCb, hasValueCb.getValue());
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' combobox[name=TypeCancelation]'] = { 'change': { fn: me.onChangeTypeCancelation, scope: me } };
                actions[me.editFormSelector + ' checkbox[name=HasDuplicate]'] = { 'change': { fn: me.onDuplicateChange, scope: me } };
                actions[me.editFormSelector + ' checkbox[name=HasCancelled]'] = { 'change': { fn: me.onCancelChange, scope: me } };
                actions[me.editFormSelector + ' checkbox[name=HasRenewed]'] = { 'change': { fn: me.onRenewChange, scope: me } };
                actions[me.editFormSelector + ' [name=RequestToExam]'] = { 'beforeload': { fn: me.beforeLoadRequestToExam, scope: me } };
                actions[me.editFormSelector + ' checkboxfield[name=IsFromAnotherRegion]'] = { 'change': { fn: this.onChangeIsFromAnotherRegion, scope: this } };

                // удаляем стандартный селектор и заменяем на более точный
                delete actions[me.editFormSelector + ' b4savebutton'];
                actions[me.editFormSelector + ' b4savebutton[name=main]'] = { 'click': { fn: this.saveRequestHandler, scope: me } };
            },
            deleteRecord: function (rec) {
                // проверка удаления записи Квал Аттестата происходит по статусу Должностного лицв. Внимание
                var me = this,
                    personId = me.controller.getContextValue(me.controller.getMainView(), 'personId'),
                    modelPerson = me.controller.getModel('Person');

                modelPerson.load(personId, {
                    success: function(record) {
                        
                        me.controller.getAspect('deleteQualificationStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);
                            if (grants && grants[0]) {
                                grants = grants[0];
                            }
                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе должностного лица запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(rec);
                                        var qualRec = new model({ Id: rec.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        qualRec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }
                        }, me);


                    },
                    scope: me
                });
                
            },
            onChangeTypeCancelation: function (cmb, newValue) {
                var me = this,
                    form = cmb.up('personqualificationeditwindow'),
                    cancelDate = form.down('datefield[name=CancelationDate]');
                
                if (newValue > 0) {
                    cancelDate.allowBlank = false;
                }
                else {
                    cancelDate.allowBlank = true;
                }

                cancelDate.validate();
            },
            onDuplicateChange: function(cmb, newValue) {
                var form = cmb.up('personqualificationeditwindow'),
                    duplicateSet = form.down('[type=duplicateSet]');

                duplicateSet.setDisabled(!newValue || form.getRecord().phantom || !!duplicateSet.disabledByPermissions);

                form.getForm().isValid();
            },
            onCancelChange: function(cmb, newValue) {
                var form = cmb.up('personqualificationeditwindow'),
                    fieldset = form.down('[type=cancelledSet]'),
                    typeCancelationField= fieldset.down('[name=TypeCancelation]'),
                    cancelationDateField= fieldset.down('[name=CancelationDate]'),
                    cancelNumberField= fieldset.down('[name=CancelNumber]'),
                    cancelProtocolDateField= fieldset.down('[name=CancelProtocolDate]');

                fieldset.setDisabled(!newValue);
                typeCancelationField.allowBlank = !newValue;
                cancelationDateField.allowBlank = !newValue;
                cancelNumberField.allowBlank = !newValue;
                cancelProtocolDateField.allowBlank = !newValue;
                
                form.getForm().isValid();
            },
            onRenewChange: function(cmb, newValue) {
                var form = cmb.up('personqualificationeditwindow'),
                    fieldset = form.down('[type=renewSet]'),
                    courtNameField = fieldset.down('[name=CourtName]'),
                    courtActNumberField = fieldset.down('[name=CourtActNumber]'),
                    courtActDateField = fieldset.down('[name=CourtActDate]');

                fieldset.setDisabled(!newValue);
                courtNameField.allowBlank = !newValue;
                courtActNumberField.allowBlank = !newValue;
                courtActDateField.allowBlank = !newValue;
                
                form.getForm().isValid();
            },
            beforeLoadRequestToExam: function (fld, operation) {
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.personId = this.controller.getCurrent();
            },
            onChangeIsFromAnotherRegion: function (cmp, value) {
                var regionCodeComponent = cmp.up().down('[name=RegionCode]'),
                    isDisabled = regionCodeComponent.manualDisabled || !value;

                if (regionCodeComponent) {
                    regionCodeComponent.setDisabled(isDisabled);

                    if (!regionCodeComponent.manualDisabled && isDisabled) {
                        regionCodeComponent.setValue('');
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'personRequestToExamAspect',
            gridSelector: 'personrequesttoexamgrid',
            modelName: 'person.RequestToExam',
            editFormSelector: 'personrequesttoexameditwin',
            editWindowView: 'person.RequestToExamEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                //   typeKNDDictId = record.getId();
                //   asp.controller.setTypeKNDDictId(typeKNDDictId);
            },
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.Person = me.controller.getCurrent();
                    }
                },
                aftersetformdata: function (me, record) {
                    var form = me.getForm(),
                        numField = form.down('[name=RequestNum]'),
                        addCertButton = form.down('[action=AddCert]');
                    var grid = form.down('qexamanswersgrid'),
                        store = grid.getStore();
                    requestId = record.getId();
                    store.filter('personRequestToExam', record.getId());
                    me.controller.qualifyTestId = record.getId();
                    addCertButton.setDisabled(record.phantom || record.get('HasCert') === true);
                    numField.allowBlank = record.phantom;
                    numField.setReadOnly(record.phantom);
                    numField.validate();
                    me.controller.getAspect('persononlinetestPrintAspect').loadReportStore();
                    me.controller.getAspect('requestToExamStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            },
            otherActions: function (actions) {
                var me = this;

                actions[me.editFormSelector + ' b4addbutton[action=AddCert]'] = { 'click': { fn: me.addCertificate, scope: me } };
                actions[me.editFormSelector + ' button[action=TakeExam]'] = { 'click': { fn: me.takeExam, scope: me } };

                actions['personexamwindow button[action=nextQuestion]'] = { 'click': { fn: me.nextQuestion, scope: me } };
                actions['personexamwindow button[action=closeExamForm]'] = { 'click': { fn: me.closeExamForm, scope: me } };
                //actions['personexamwindow radiogroup[name=radGroup]'] = { 'change': { fn: me.activateNextButton, scope: me } };
                
            },
            addCertificate: function (btn) {
                var me = this,
                    form = me.getForm(),
                    record = form.getRecord(),
                    model = me.controller.getModel('person.QualificationCertificate'),
                    certificate;

                certificate = new model({ Id: 0 });
                certificate.set('Person', me.controller.getCurrent());
                certificate.set('RequestToExam', record.getId());
                
                me.mask('Добавление квалификационного аттестата', form);
                
                certificate.save()
                    .next(function () {
                        me.unmask();
                        btn.setDisabled(true);
                        Ext.Msg.alert('Успешно!', 'Квалификационный сертификат успешно сохранен');
                    }, this)
                    .error(function (result) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);

            },

            //showBigDescription: function (field) {
            //    var currentDescriptonText = field.getValue();
            //    win = Ext.create('B4.view.person.DetailsEditWindow');
            //    var valuefield = win.down('#dfDescription');
            //    var closebutton = win.down('b4closebutton');
            //    var savebutton = win.down('b4savebutton');
            //    valuefield.setValue(currentDescriptonText);
            //    win.show();
            //    closebutton.addListener('click', function () {
            //        win.close();
            //    });
            //    savebutton.addListener('click', function () {
            //        currentDescriptonText = valuefield.getValue();
            //        field.setValue(currentDescriptonText);
            //        win.close();
            //    });
            //},

            closeExamForm: function (btn) {
                var window = btn.up('personexamwindow');
                if (window.task) window.task.destroy();
                window.destroy();
                Ext.Msg.alert('Внимание!', 'Не забудьте сохранить результаты экзамена. Кнопка Сохранить на панели заявки');

            },
            activateNextButton: function (radiogroup) {
                var window = radiogroup.up('personexamwindow');
                var nextbtn = window.down('button[action = nextQuestion]');
                nextbtn.setDisabled(false);
            },
            updateClock: function (startTime,window) {
                var clockEl = Ext.fly('clockEl');
                var date = new Date();
                var hour1 = 1000 * 60 * 60;
                var time = startTime - date + hour1 * 2+30000;
                if (time < 0) {
                    clockEl.dom.innerHTML = 'Время вышло';
                    var closeBtn = window.down('button[action = closeExamForm]');
                    var btn = window.down('button[action = nextQuestion]');
                    btn.hide();
                    closeBtn.show();
                    qnLabel.setText('');
                    window.down('label[name = questionLabel]').setText('');
                }
                var hour = Math.floor(time/hour1);
                var minutes = Math.floor((time%hour1)/(60*1000));
                if( minutes < 10 ){
                    minutes = ('0' + minutes).slice(-2);
                }
                if(clockEl) {
                    clockEl.dom.innerHTML =
                        Ext.String.format('Оставшееся время<br>{0}:{1}',//" | {2}",
                            hour, minutes
                           // Ext.Date.format(date,"H:i"),
                           // Ext.Date.format(startTime,"H:i")
                            
                        );
                }
            },
            takeExam: function (btn) {
                var examWindow = Ext.create('B4.view.person.ExamWindow');
                examWindow.step = 1;
                examWindow.requestId = requestId;
                
                this.createList();
                examWindow.show();
                this.clockUpdate(examWindow);               
            },
            clockUpdate: function(examWindow) {
                if (examWindow.task) {
                    examWindow.task.destroy();
                }

                examWindow.task = Ext.TaskManager.newTask({
                    scope: this,
                    args: [
                        startTime = new Date(),
                        window = examWindow
                    ],
                    run: this.updateClock,
                    interval: 1000
                });
                
                examWindow.task.start();
            },
            nextQuestion: function (btn) {
               
                var me = this;//step = step + 1;
                var window = btn.up('personexamwindow');
                if (window.step > 1) {
                    var radGrp = window.down('[name = radGroup]');
                    var ans = radGrp.getChecked();
                    if (ans.length ==0) {
                        Ext.Msg.alert('Ошибка!','Выберите один из вариантов');
                    }
                }
                //btn.setDisabled(true);
                if (window.step == 1) {
                    //query возвращает массивы с одним элементом, берем первый ([0])
                  //  Ext.ComponentQuery.query('[name = questionField]')[0].show();
                    Ext.ComponentQuery.query('[name = helpField]')[0].hide();
                 //   Ext.ComponentQuery.query('[name = radGroup]')[0].show();
                    Ext.ComponentQuery.query('[name = infoField]')[0].hide();
                //    window.down('label[name = questionLabel]').setText('Вопрос №');
                    this.getNext(window);
                }
                if (window.step > 1) {
                    this.saveAndGetNext(window,btn);
                }
                
                var qnLabel = Ext.ComponentQuery.query('[name = questionNumberLabel]')[0];
                qnLabel.setText(window.step + ' из ' + this.controller.questionCount);

                if (window.step > this.controller.questionCount) {
                    var closeBtn = window.down('button[action = closeExamForm]');
                    btn.hide();
                    closeBtn.show();
                    qnLabel.setText('');
                    window.down('label[name = questionLabel]').setText('');
                }
                
                
                
                window.step++;
              //  btn.setDisabled(false);
            },
            saveAndGetNext: function (window,btn) {
                var radGrp = window.down('[name = radGroup]');
                var ans = radGrp.getChecked()[0].itemId;
                var qst = questionId;
             //   btn.setDisabled(true);
                B4.Ajax.request({
                    url: B4.Url.action('SaveAndGetNextQuestion', 'PersonRequestToExam'),
                    params: {
                        questionId: qst || 0,
                        answerId: ans || 0,
                        requestToExamId: requestId
                    }
                               
                }).next(function (resp) {
                    var tryDecoded;
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }

                    var qText = window.down('[name = questionField]');
                    var radGrp = window.down('[name = radGroup]');
                    radGrp.removeAll();

                  //  btn.setDisabled(false);

                    if (tryDecoded.data.question != null && tryDecoded.data.answers != []) {
                        var respQuestion = tryDecoded.data.question;
                        questionId = respQuestion.Id;

                        qText.setText(respQuestion.QuestionText);

                        var respAnswers = tryDecoded.data.answers;
                        respAnswers.forEach(function (item) {
                            radGrp.add(
                                {
                                    boxLabel: item.AnswerText,
                                    itemId: item.Id,
                                    name: 'radioGroupItems'
                                  
                                });
                        });
                    }
                    else {
                        qText.setText('Экзамен окончен');
                    }
                }).error(function (err) {
                    //btn.setDisabled(false);
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },
            getNext: function (window) {
                //window.mask();
                //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('GetNextQuestion', 'PersonRequestToExam'),
                    params: {
                        requestToExamId: requestId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }

                    var qText = window.down('[name = questionField]');
                    var radGrp = window.down('[name = radGroup]');
                    radGrp.removeAll();

                    if (tryDecoded.data.question != null && tryDecoded.data.answers != []) {

                        Ext.ComponentQuery.query('[name = questionField]')[0].show();
                     
                        Ext.ComponentQuery.query('[name = radGroup]')[0].show();
                       
                        window.down('label[name = questionLabel]').setText('Вопрос №');

                        var respQuestion = tryDecoded.data.question;
                        questionId = respQuestion.Id;

                        qText.setText(respQuestion.QuestionText);

                        var respAnswers = tryDecoded.data.answers;
                        respAnswers.forEach(function(item) {
                            radGrp.add(
                                {
                                    boxLabel: item.AnswerText,
                                    itemId: item.Id,
                                    name: 'radioGroupItems'
                                });
                        });
                    }
                    else {
                        qText.setText('Экзамен окончен');
                    } 
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                    });
                //window.unmask();
            },
            saveAnswer: function (window) {
                window.mask();
                var radGrp = window.down('[name = radGroup]');
                var ans = radGrp.getChecked()[0].itemId;
                var qst = questionId;
                //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('SaveAnswer', 'PersonRequestToExam'),
                    params: {
                        questionId: qst,
                        answerId: ans
            }
                }).next(function (resp) {
                    var tryDecoded;

                    //asp.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }
                    
                }).error(function (err) {
                    //asp.unmask();
                    Ext.Msg.alert('Ошибка', err.message);
                    });
                window.unmask();
            },
            createList: function () {
                //asp.getMainComponent().mask('Получение выписки', asp.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('ListQuestionsForExam', 'PersonRequestToExam'),
                    params: {
                        requestToExamId: requestId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                        this.controller.questionCount = tryDecoded.data.questionCount;

                    } catch (e) {
                        tryDecoded = {};
                    }

                    var id = resp.data ? resp.data : tryDecoded.data;
                    if (tryDecoded.success == false) {
                        throw new Error(tryDecoded.message);
                    }
                    
                    }).error(function (err) {
                    Ext.Msg.alert('Ошибка', err.message);
                });
            },
            deleteRecord: function (record) {
                var me = this;
                if (record.getId()) {
                    me.controller.getAspect('deleteRequestToExamStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);
                            if (grants && grants[0]) {
                                grants = grants[0];
                            }
                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(record);
                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }
                        }, this);
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'qualificationdocumentDuplicateGridAspect',
            modelName: 'person.QualificationDocument',
            gridSelector: 'qualificationdocumentgrid[name=Duplicate]',
            editFormSelector: 'qualificationdocumenteditwindow[type=Duplicate]',
            editWindowView: 'person.QualificationDocumentEditWindow',
            getForm: function() {
                var me = this,
                    editWindow;

                if (me.editFormSelector) {
                    editWindow = me.componentQuery(me.editFormSelector);

                    if (editWindow && !editWindow.getBox().width) {
                        editWindow = editWindow.destroy();
                    }

                    if (!editWindow) {

                        editWindow = me.controller.getView(me.editWindowView).create(
                        {
                            constrain: true,
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            closeAction: 'destroy',
                            ctxKey: me.controller.getCurrentContextKey(),
                            type: 'Duplicate'
                        });

                        editWindow.show();
                    }

                    return editWindow;
                }
            },
            listeners: {
                getdata: function (me, record) {
                    var window = me.getGrid().up('window'),
                        form = window ? window.getForm() : null,
                        rec = form ? form.getRecord() : null,
                        id = rec ? rec.getId() : 0;

                    if (!record.data.Id) {
                        record.data.QualificationCertificate = id;
                        record.data.DocumentType = B4.enums.QualificationDocumentType.Duplicate;
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'qualificationdocumentRenewGridAspect',
            modelName: 'person.QualificationDocument',
            gridSelector: 'qualificationdocumentgrid[name=Renew]',
            editFormSelector: 'qualificationdocumenteditwindow[type=Renew]',
            editWindowView: 'person.QualificationDocumentEditWindow',
            getForm: function () {
                var me = this,
                    editWindow;

                if (me.editFormSelector) {
                    editWindow = me.componentQuery(me.editFormSelector);

                    if (editWindow && !editWindow.getBox().width) {
                        editWindow = editWindow.destroy();
                    }

                    if (!editWindow) {

                        editWindow = me.controller.getView(me.editWindowView).create(
                        {
                            constrain: true,
                            renderTo: B4.getBody().getActiveTab().getEl(),
                            closeAction: 'destroy',
                            ctxKey: me.controller.getCurrentContextKey(),
                            type: 'Renew'
                        });

                        editWindow.show();
                    }

                    return editWindow;
                }
            },
            listeners: {
                getdata: function (me, record) {
                    var window = me.getGrid().up('window'),
                        form = window ? window.getForm() : null,
                        rec = form ? form.getRecord() : null,
                        id = rec ? rec.getId() : 0;

                    if (!record.data.Id) {
                        record.data.QualificationCertificate = id;
                        record.data.DocumentType = B4.enums.QualificationDocumentType.Renew;
                    }
                }
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'technicalmistakegridAspect',
            modelName: 'person.TechnicalMistake',
            gridSelector: 'technicalmistakegrid',
            listeners: {
                beforeaddrecord: function (asp, rec) {
                    var window = asp.getGrid().up('window'),
                        form = window ? window.getForm() : null,
                        record = form ? form.getRecord() : null,
                        id = record ? record.getId() : 0;

                    rec.set('QualificationCertificate', id);
                },
                beforesave: function(asp, store) {
                    var modifRecords = store.getModifiedRecords();

                    // маленькая костыль, все свойства, которые являются объектами превращаем в Id
                    // иначе при сохранении отправляется весь объект и NHibernate пытается его изменить
                    Ext.Array.each(modifRecords, function (rec) {

                        for (var property in rec.data) {
                            if (rec.data[property]
                                    && typeof rec.data[property] === 'object'
                                    && rec.data[property].Id) {
                                rec.data[property] = rec.data[property].Id;
                            }
                        }
                    });
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('personEditPanel');

        me.bindContext(view);
        view.params = {};
        view.params.personId = id;
        me.setContextValue(view, 'personId', id);
        me.application.deployView(view, 'person_info');

        me.getAspect('personEditPanelAspect').setData(id);
    },

    init: function () {
        var me = this,
            actions = {
               // 'personexamwindow radiogroup[name=radGroup]' : { 'change': { fn: me.activateNextButton, scope: me } }
            };

        this.control(actions);

        this.callParent(arguments);
    },

    
    hasChanges: function () {
        return this.getMainComponent().getForm().isDirty();
    },

    getCurrent: function () {

        var me = this;
        return me.getContextValue(me.getMainComponent(), 'personId');
    },
    
    editRequest: function (id, requestId, qsId) {
        if (qsId == 0) {
            var me = this,
                aspect = me.getAspect('personRequestToExamAspect'),
                model = me.getModel('person.RequestToExam'),
                view = me.getMainView() || Ext.widget('personEditPanel');

            me.bindContext(view);
            me.setContextValue(view, 'personId', id);
            me.application.deployView(view, 'person_info');

            me.getAspect('personEditPanelAspect').setData(id);

            requestId ? model.load(requestId, {
                success: function (rec) {
                    aspect.setFormData(rec);
                },
                scope: aspect
            }) : aspect.setFormData(new model({ Id: 0 }));

            aspect.getForm().getForm().isValid();
        }
        else {
                  var me = this,
            aspect = me.getAspect('personQualificationAspect'),
            model = me.getModel('person.QualificationCertificate'),
            view = me.getMainView() || Ext.widget('personEditPanel');
        me.bindContext(view);
        me.setContextValue(view, 'personId', id);
        me.application.deployView(view, 'person_info');

        me.getAspect('personEditPanelAspect').setData(id);

        requestId ? model.load(requestId, {
            success: function (rec) {
                aspect.setFormData(rec);
            },
            scope: aspect
        }) : aspect.setFormData(new model({ Id: 0 }));

        aspect.getForm().getForm().isValid();
        }
    },

    //editSertificate: function (id, personId, poh) {
    //    var me = this,
    //        aspect = me.getAspect('personQualificationAspect'),
    //        model = me.getModel('person.QualificationCertificate'),
    //        view = me.getMainView() || Ext.widget('personEditPanel');
    //    me.bindContext(view);
    //    me.setContextValue(view, 'personId', id);
    //    me.application.deployView(view, 'person_info');

    //    me.getAspect('personEditPanelAspect').setData(id);

    //    qualifyId ? model.load(qualifyId, {
    //        success: function (rec) {
    //            aspect.setFormData(rec);
    //        },
    //        scope: aspect
    //    }) : aspect.setFormData(new model({ Id: 0 }));

    //    aspect.getForm().getForm().isValid();
    //}
});