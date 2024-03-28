Ext.define('B4.aspects.permission.manorg.Contract', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgcontractperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Contract.Edit', applyTo: 'b4savebutton', selector: '#manorgContractOwnersEditWindow' },
        { name: 'Gkh.Orgs.Managing.Register.Contract.Create', applyTo: '#btnAddJskTsj', selector: 'manorgContractGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Contract.Create', applyTo: '#btnAddManOrgOwners', selector: 'manorgContractGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Contract.Create', applyTo: '#btnAddManOrgJskTsj', selector: '#manOrgContractRelationGrid' },

        //jskTsjContractEditWindow
        { name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_Edit', applyTo: 'combobox[name=ContractStopReason]', selector: '#jskTsjContractEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_View', applyTo: 'combobox[name=ContractStopReason]', selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_Edit',
            applyTo: '[name=ContractFoundation]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_View',
            applyTo: '[name=ContractFoundation]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit',
            applyTo: '[name=DocumentNumber]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View',
            applyTo: '[name=DocumentNumber]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View',
            applyTo: '[name=DocumentDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit',
            applyTo: '[name=StartDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View',
            applyTo: '[name=StartDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit',
            applyTo: '[name=PlannedEndDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View',
            applyTo: '[name=PlannedEndDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit',
            applyTo: '[name=EndDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View',
            applyTo: '[name=EndDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit',
            applyTo: '[name=FileInfo]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View',
            applyTo: '[name=FileInfo]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit',
            applyTo: '[name=Note]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_View',
            applyTo: '[name=Note]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit',
            applyTo: '[name=TerminateReason]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View',
            applyTo: '[name=TerminateReason]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_Edit',
            applyTo: '[name=TerminationDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_View',
            applyTo: '[name=TerminationDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_Edit',
            applyTo: '[name=TerminationFile]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_View',
            applyTo: '[name=TerminationFile]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (this.controller.localPermission['InputMeteringDeviceValuesBeginDate']) {
                    return;
                }
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function(component, allowed) {
                if (this.controller.localPermission['InputMeteringDeviceValuesEndDate']) {
                    return;
                }
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_View',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_Edit',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function(component, allowed) {
                if (this.controller.localPermission['DrawingPaymentDocumentDate']) {
                    return;
                }
                component.setDisabled(!allowed);
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_View',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_Edit',
            applyTo: '[name=DocDateMonth]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_View',
            applyTo: '[name=DocDateMonth]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit',
            applyTo: '[name=ProtocolNumber]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View',
            applyTo: '[name=ProtocolNumber]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit',
            applyTo: '[name=ProtocolDate]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View',
            applyTo: '[name=ProtocolDate]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Delete', applyTo: 'b4deletecolumn', selector: 'manorgContractGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.View', applyTo: 'tabpanel tab[text=Сведения о плате]', selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_Edit',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_View',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_Edit',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_View',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyReqiredPaymentAmount_Edit',
            applyTo: '[name=CompanyReqiredPaymentAmount]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyReqiredPaymentAmount_View',
            applyTo: '[name=CompanyReqiredPaymentAmount]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolFile_Edit',
            applyTo: '[name=CompanyPaymentProtocolFile]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolFile_View',
            applyTo: '[name=CompanyPaymentProtocolFile]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolDescription_Edit',
            applyTo: '[name=CompanyPaymentProtocolDescription]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.CompanyPaymentProtocolDescription_View',
            applyTo: '[name=CompanyPaymentProtocolDescription]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentAmount_Edit',
            applyTo: '[name=ReqiredPaymentAmount]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentAmount_Edit',
            applyTo: '[name=ReqiredPaymentAmount]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolFile_Edit',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolFile_View',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolDescription_Edit',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.ReqiredPaymentProtocolDescription_View',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        //manorgContractOwnersEditWindow
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_View',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_Edit',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_View',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_Edit',
            applyTo: '[name=DocDateMonth]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_View',
            applyTo: '[name=DocDateMonth]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit',
            applyTo: '[name=ProtocolDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View',
            applyTo: '[name=ProtocolDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit',
            applyTo: '[name=DocumentNumber]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View',
            applyTo: '[name=DocumentNumber]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View',
            applyTo: '[name=DocumentDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit',
            applyTo: '[name=StartDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View',
            applyTo: '[name=StartDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit',
            applyTo: '[name=EndDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View',
            applyTo: '[name=EndDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit',
            applyTo: '[name=FileInfo]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View',
            applyTo: '[name=FileInfo]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit',
            applyTo: '[name=Note]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_View',
            applyTo: '[name=Note]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceRegister_Edit',
            applyTo: '[name=DateLicenceRegister]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceRegister_View',
            applyTo: '[name=DateLicenceRegister]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceDelete_Edit',
            applyTo: '[name=DateLicenceDelete]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DateLicenceDelete_View',
            applyTo: '[name=DateLicenceDelete]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.RegisterReason_Edit',
            applyTo: '[name=RegisterReason]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.RegisterReason_View',
            applyTo: '[name=RegisterReason]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DeleteReason_Edit',
            applyTo: '[name=DeleteReason]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DeleteReason_View',
            applyTo: '[name=DeleteReason]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit',
            applyTo: '[name=TerminateReason]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View',
            applyTo: '[name=TerminateReason]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_Edit',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_View',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.RealityObjectId_Edit',
            applyTo: '[name=RealityObjectId]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.triggerCell.show();
                } else {
                    component.triggerCell.hide();
                }
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_Edit',
            applyTo: '[name=ContractFoundation]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_View',
            applyTo: '[name=ContractFoundation]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_Edit',
            applyTo: '[name=TerminationDate]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_View',
            applyTo: '[name=TerminationDate]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_Edit',
            applyTo: '[name=TerminationFile]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_View',
            applyTo: '[name=TerminationFile]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.View', applyTo: 'tabpanel tab[text=Сведения о плате]', selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_Edit',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_View',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_Edit',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_View',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },




        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_Edit',
            applyTo: '[name=PaymentAmount]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_View',
            applyTo: '[name=PaymentAmount]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_Edit',
            applyTo: '[name=SetPaymentsFoundation]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_View',
            applyTo: '[name=SetPaymentsFoundation]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_Edit',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_View',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_Edit',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_View',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_Edit',
            applyTo: '[name=RevocationReason]',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_View',
            applyTo: '[name=RevocationReason]',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        //manorgTransferEditWindow
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_Edit',
            applyTo: '[name=ContractFoundation]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_View',
            applyTo: '[name=ContractFoundation]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_View',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_Edit',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_View',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_Edit',
            applyTo: '[name=DocDateMonth]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_View',
            applyTo: '[name=DocDateMonth]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit',
            applyTo: '[name=ProtocolDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View',
            applyTo: '[name=ProtocolDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit',
            applyTo: '[name=DocumentNumber]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View',
            applyTo: '[name=DocumentNumber]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View',
            applyTo: '[name=DocumentDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit',
            applyTo: '[name=StartDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View',
            applyTo: '[name=StartDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit',
            applyTo: '[name=EndDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View',
            applyTo: '[name=EndDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit',
            applyTo: '[name=FileInfo]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View',
            applyTo: '[name=FileInfo]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit',
            applyTo: '[name=Note]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_View',
            applyTo: '[name=Note]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit',
            applyTo: '[name=TerminateReason]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View',
            applyTo: '[name=TerminateReason]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_Edit',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_View',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_Edit',
            applyTo: '[name=TerminationDate]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_View',
            applyTo: '[name=TerminationDate]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_Edit',
            applyTo: '[name=TerminationFile]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_View',
            applyTo: '[name=TerminationFile]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.View', applyTo: 'tabpanel tab[text=Сведения о плате]', selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_Edit',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.StartDatePaymentPeriod_View',
            applyTo: '[name=StartDatePaymentPeriod]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_Edit',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.EndDatePaymentPeriod_View',
            applyTo: '[name=EndDatePaymentPeriod]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_Edit',
            applyTo: '[name=PaymentAmount]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentAmount_View',
            applyTo: '[name=PaymentAmount]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_Edit',
            applyTo: '[name=SetPaymentsFoundation]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.SetPaymentsFoundation_View',
            applyTo: '[name=SetPaymentsFoundation]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_Edit',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolFile_View',
            applyTo: '[name=PaymentProtocolFile]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_Edit',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.PaymentProtocolDescription_View',
            applyTo: '[name=PaymentProtocolDescription]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_Edit',
            applyTo: '[name=RevocationReason]',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.RevocationReason_View',
            applyTo: '[name=RevocationReason]',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        //manOrgContractRelationEditWindow
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesBeginDate_View',
            applyTo: '[name=InputMeteringDeviceValuesBeginDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_Edit',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.InputMeteringDeviceValuesEndDate_View',
            applyTo: '[name=InputMeteringDeviceValuesEndDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_Edit',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DrawingPaymentDocumentDate_View',
            applyTo: '[name=DrawingPaymentDocumentDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_Edit',
            applyTo: '[name=DocDateMonth]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocDateMonth_View',
            applyTo: '[name=DocDateMonth]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_Edit',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolNumber_View',
            applyTo: '[name=ProtocolNumber]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_Edit',
            applyTo: '[name=ProtocolDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolDate_View',
            applyTo: '[name=ProtocolDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_Edit',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ProtocolFileInfo_View',
            applyTo: '[name=ProtocolFileInfo]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_Edit',
            applyTo: '[name=DocumentNumber]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentNumber_View',
            applyTo: '[name=DocumentNumber]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_Edit',
            applyTo: '[name=DocumentDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.DocumentDate_View',
            applyTo: '[name=DocumentDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_Edit',
            applyTo: '[name=StartDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.StartDate_View',
            applyTo: '[name=StartDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_Edit',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.PlannedEndDate_View',
            applyTo: '[name=PlannedEndDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_Edit',
            applyTo: '[name=EndDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.EndDate_View',
            applyTo: '[name=EndDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_Edit',
            applyTo: '[name=FileInfo]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFileInfo_View',
            applyTo: '[name=FileInfo]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_Edit',
            applyTo: '[name=Note]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.Note_View',
            applyTo: '[name=Note]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_Edit',
            applyTo: '[name=TerminateReason]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminateReason_View',
            applyTo: '[name=TerminateReason]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_Edit',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractStopReason_View',
            applyTo: 'combobox[name=ContractStopReason]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_Edit',
            applyTo: '[name=ContractFoundation]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.ContractFoundation_View',
            applyTo: '[name=ContractFoundation]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_Edit',
            applyTo: '[name=TerminationDate]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationDate_View',
            applyTo: '[name=TerminationDate]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_Edit',
            applyTo: '[name=TerminationFile]',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.TerminationFile_View',
            applyTo: '[name=TerminationFile]',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});