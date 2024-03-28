Ext.define('B4.aspects.permission.TaskDisposal', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.taskdisposalperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNum_View', applyTo: '#nfDocumentNum', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

       //поля панели редактирования "Номер документа с результатом согласования"
       {
           name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_Edit', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#taskdisposalEditPanel',
           applyBy: function (component, allowed) {
               component.setDisabled(!allowed);
               component.allowedEdit = allowed;
               this.controller.onChangeTypeAgreement();
           }
       },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_View', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },

        //поля панели редактирования "Дата документа с результатом согласования:"
       {
           name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_Edit', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#taskdisposalEditPanel',
           applyBy: function (component, allowed) {
               component.setDisabled(!allowed);
               component.allowedEdit = allowed;
               this.controller.onChangeTypeAgreement();
           }
       },
       //поля панели редактирования "Дата документа с результатом согласования:"
       {
           name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_View', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#taskdisposalEditPanel',
           applyBy: function (component, allowed) {
               component.setVisible(allowed);

           }
       },

        //Просмотр Уведомления о проверке в табе Реквизиты
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.Notice_Fieldset_View', applyTo: '[name=noticeFieldset]', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else component.hide();
                }
            }
        },

        //Редактирование Уведомления о проверке в табе Реквизиты
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.Notice_Fieldset_Edit', applyTo: '[name=noticeFieldset]', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.query('field').forEach(function (c) {
                        c.setReadOnly && c.setReadOnly(!allowed);
                    });
                }
            }
        },

        //Просмотр Уведомления о проверке в закладке Приказа
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Notice.View', applyTo: '#tabDisposalNoticeOfInspection', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.tab.show();
                    else component.tab.hide();
                }
            }
        },

        //Редактирование Уведомления о проверке в закладке Приказа
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Notice.Edit', applyTo: '#tabDisposalNoticeOfInspection', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.query('field').forEach(function (c) {
                        c.setReadOnly && c.setReadOnly(!allowed);
                    });
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumber_Edit', applyTo: '#tfDocumentNumber', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNum_Edit', applyTo: '#nfDocumentNum', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentYear_View', applyTo: '#nfDocumentYear', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.LiteralNum_Edit', applyTo: '#nfLiteralNum', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.LiteralNum_View', applyTo: '#nfLiteralNum', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentSubNum_Edit', applyTo: '#nfDocumentSubNum', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentSubNum_View', applyTo: '#nfDocumentSubNum', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitStart_Edit', applyTo: '#dfObjectVisitStart', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitStart_View', applyTo: '#dfObjectVisitStart', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitEnd_Edit', applyTo: '#dfObjectVisitEnd', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.ObjectVisitEnd_View', applyTo: '#dfObjectVisitEnd', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.OutInspector_Edit', applyTo: '#cbOutInspector', selector: '#taskdisposalEditPanel' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.OutInspector_View', applyTo: '#cbOutInspector', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentYear_Edit', applyTo: '#nfDocumentYear', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.ResponsibleExecution_Edit', applyTo: '#sflResponsibleExecution', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DateStart_Edit', applyTo: '#dfDateStart', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DateEnd_Edit', applyTo: '#dfDateEnd', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.TypeAgreementProsecutor_Edit', applyTo: '#cbTypeAgreementProsecutor', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.TypeAgreementResult_Edit', applyTo: '#cbTypeAgreementResult', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.IssuedDisposal_Edit', applyTo: '#sfIssuredDisposal', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.Inspectors_Edit', applyTo: '#trigFInspectors', selector: '#taskdisposalEditPanel' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Field.KindCheck_Edit', applyTo: '#cbTypeCheck', selector: '#taskdisposalEditPanel' },

        //DisposalTypeSurvey
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Register.TypeSurvey.Create', applyTo: 'b4addbutton', selector: '#disposalTypeSurveyGrid' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.TypeSurvey.Delete', applyTo: 'b4deletecolumn', selector: '#disposalTypeSurveyGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DisposalAnnex
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Create', applyTo: 'b4addbutton', selector: '#disposalAnnexGrid' },
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Edit', applyTo: 'b4savebutton', selector: '#disposalAnnexEditWindow' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Annex.Delete', applyTo: 'b4deletecolumn', selector: '#disposalAnnexGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DisposalExpert
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Expert.Create', applyTo: 'b4addbutton', selector: '#disposalExpertGrid' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.Expert.Delete', applyTo: 'b4deletecolumn', selector: '#disposalExpertGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //DisposalProvidedDocument
        { name: 'GkhGji.DocumentsGji.TaskDisposal.Register.ProvidedDoc.Create', applyTo: 'b4addbutton', selector: '#disposalProvidedDocGrid' },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.ProvidedDoc.Delete', applyTo: 'b4deletecolumn', selector: '#disposalProvidedDocGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.View',
            applyTo: '[name=SubjectVerificationGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsubjectverificationgrid',
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SubjectVerificationGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsubjectverificationgrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.View',
            applyTo: '[name=SurveyPurposeGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsurveypurposegrid'
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyPurposeGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsurveypurposegrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.View',
            applyTo: '[name=SurveyObjectiveGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalsurveyobjectivegrid'
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.SurveyObjectiveGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalsurveyobjectivegrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.View',
            applyTo: '[name=InspFoundationCheckGrid]',
            selector: '#disposalTabPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.tab.setVisible(allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.Create',
            applyTo: 'b4addbutton',
            selector: 'disposalinspfoundationcheckgrid'
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Register.InspFoundationCheckGrid.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'disposalinspfoundationcheckgrid',
            applyBy: this.setVisible
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_Edit', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.readOnly = !allowed;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentNumberWithResultAgreement_View', applyTo: '#cbDocumentNumberWithResultAgreement', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        //поля панели редактирования
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_Edit', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.readOnly = !allowed;
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskDisposal.Field.DocumentDateWithResultAgreement_View', applyTo: '#cbDocumentDateWithResultAgreement', selector: '#taskdisposalEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    ]
});