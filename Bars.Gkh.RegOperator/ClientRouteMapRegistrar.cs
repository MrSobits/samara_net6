using Bars.B4;

namespace Bars.Gkh.RegOperator
{
    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("paymentdoc_snapshot", "B4.controller.regop.report.PaymentDocumentSnapshotController", requiredPermission: "GkhRegOp.Accounts.PaymentDocumentSnapshot.View"));
            map.AddRoute(new ClientRoute("bank_statement", "B4.controller.regop.BankStatement", requiredPermission: "GkhRegOp.Accounts.BankOperations.View"));
            map.AddRoute(new ClientRoute("bank_acc_statement", "B4.controller.BankAccountStatement", requiredPermission: "GkhRegOp.Settings.BankAccountStatement.View"));
            map.AddRoute(new ClientRoute("comparison_paying_agents", "B4.controller.regop.ComparisonPayingAgents", requiredPermission: "GkhRegOp.Settings.ComparisonPayingAgents.View"));
            map.AddRoute(new ClientRoute("regop_debtor", "B4.controller.regop.personal_account.Debtor", requiredPermission: "GkhRegOp.PersonalAccountOwner.Debtor.View"));
            map.AddRoute(new ClientRoute("creditorgservicecondition", "B4.controller.CreditOrgServiceCondition", requiredPermission: "Gkh.Orgs.CreditorgServiceCondition.View"));
            map.AddRoute(new ClientRoute("regop_personal_account", "B4.controller.regop.personal_account.BasePersonalAccount", requiredPermission: "GkhRegOp.Accounts.BasePersonalAccount.View"));
            map.AddRoute(new ClientRoute("regop_personal_account/onlyroom/{roomid}", "B4.controller.regop.personal_account.BasePersonalAccount", requiredPermission: "GkhRegOp.Accounts.BasePersonalAccount.View"));
            map.AddRoute(new ClientRoute("regop_registry_account", "B4.controller.regop.RegistryAccount", requiredPermission: "GkhRegOp.Accounts.RegistryAccount.View"));
            map.AddRoute(new ClientRoute("regop_cproc", "B4.controller.regop.cproc.ComputingProcess", requiredPermission: "GkhRegOp.Processes.ComputingProcess.View"));
            map.AddRoute(new ClientRoute("regop_special_account", "B4.controller.regop.SpecialAccount", requiredPermission: "GkhRegOp.Accounts.SpecialAccount.View"));
            map.AddRoute(new ClientRoute("regop_dict_location_code", "B4.controller.regop.LocationCode", requiredPermission: "GkhRegOp.Dictionaries.RegopDictLocationCode"));
            map.AddRoute(new ClientRoute("regop_suspense_account", "B4.controller.SuspenseAccount", requiredPermission: "GkhRegOp.Accounts.SuspenseAccount.View"));
            map.AddRoute(new ClientRoute("statuspaymentdocumenthouses", "B4.controller.regop.statusPDH.StatusPaymentDocumentHouses", requiredPermission: "GkhRegOp.Accounts.StatusPaymentDocumentHouses.View"));

            map.AddRoute(new ClientRoute("regop_personal_acc_owner", "B4.controller.regop.owner.PersonalAccountOwner", requiredPermission: "GkhRegOp.PersonalAccountOwner.PersonalAccountOwner.View"));
            map.AddRoute(new ClientRoute("regop_personal_acc_owner/onlyroom/{roomid}", "B4.controller.regop.owner.PersonalAccountOwner", requiredPermission: "GkhRegOp.PersonalAccountOwner.PersonalAccountOwner.View"));
            map.AddRoute(new ClientRoute("regop_personal_acc_owner/{id}/{ownerType}", "B4.controller.regop.owner.PersonalAccountOwner", "edit", requiredPermission: "GkhRegOp.PersonalAccountOwner.PersonalAccountOwner.View"));
            map.AddRoute(new ClientRoute("charges_period", "B4.controller.regop.ChargePeriod", requiredPermission: "GkhRegOp.Settings.ChargePeriod.View"));
            map.AddRoute(new ClientRoute("unaccepted_charges", "B4.controller.regop.UnacceptedCharges", requiredPermission: "GkhRegOp.Accounts.UnacceptedCharge.View"));
            map.AddRoute(new ClientRoute("unaccepted_payment", "B4.controller.regop.UnacceptedPayment", requiredPermission: "GkhRegOp.Accounts.UnacceptedPayment.View"));

            map.AddRoute(new ClientRoute("paymentpenalties", "B4.controller.dict.PaymentPenalties"));
            map.AddRoute(new ClientRoute("paymentsource", "B4.controller.config.PaymentSourceTree"));
            map.AddRoute(new ClientRoute("privilegedcategory", "B4.controller.dict.PrivilegedCategory", requiredPermission: "GkhRegOp.Dictionaries.PrivilegedCategory.View"));
            map.AddRoute(new ClientRoute("loan_manage", "B4.controller.regop.loan.Manage", requiredPermission: "GkhRegOp.Loans.Manage.View"));
            map.AddRoute(new ClientRoute("loans", "B4.controller.regop.loan.Loan", requiredPermission: "GkhRegOp.Loans.Loan.View"));
            map.AddRoute(new ClientRoute("tariffbyperiodforclaimwork", "B4.controller.dict.TariffByPeriodForClaimWork", requiredPermission: "GkhRegOp.Dictionaries.TariffByPeriodForClaimWork.View"));

            map.AddRoute(new ClientRoute("personal_acc_details/{id}", "B4.controller.regop.personal_account.Details", "show"));

            map.AddRoute(new ClientRoute("bank_doc_import", "B4.controller.regop.BankDocumentImport", requiredPermission: "GkhRegOp.Settings.BankDocumentImport.View"));
            map.AddRoute(new ClientRoute("notdefined_payment", "B4.controller.regop.NotDefinedPayment", requiredPermission: "GkhRegOp.Settings.BankDocumentImport.View"));
            map.AddRoute(new ClientRoute("ownersimport", "B4.controller.import.OwnersImport", requiredPermission: "Import.OwnersImport"));
            map.AddRoute(new ClientRoute("roomimport", "B4.controller.import.RoomImport", requiredPermission: "Import.RoomImport.View"));
            map.AddRoute(new ClientRoute("ownerroomimport", "B4.controller.import.OwnerRoomImport", requiredPermission: "Import.OwnerRoomImport"));
            map.AddRoute(new ClientRoute("decisionprotocolimport", "B4.controller.import.DecisionProtocolImport", requiredPermission: "Import.DecisionProtocolImport"));
            map.AddRoute(new ClientRoute("offlinepaymentchargeimport", "B4.controller.import.OfflinePaymentAndChargeImport", requiredPermission: "Import.PersonalAccountChargePaymentImportXml"));
            map.AddRoute(new ClientRoute("ownerdecisionprotocolimport", "B4.controller.import.OwnerDecisionProtocolImport", requiredPermission: "Import.OwnerDecisionProtocolImport"));
            map.AddRoute(new ClientRoute("suspenseaccountimport", "B4.controller.import.SuspenseAccount", requiredPermission: "GkhRegOp.Accounts.SuspenseAccount.View"));
            map.AddRoute(new ClientRoute("calculationprotocol", "B4.controller.regop.CalculationProtocol", requiredPermission: "GkhRegOp.Accounts.CalculationProtocol.View"));
            map.AddRoute(new ClientRoute("chargestoclosedperiodimport", "B4.controller.import.ChargesToClosedPeriodsImport", requiredPermission: "Import.ChargesToClosedPeriods.View"));
            map.AddRoute(new ClientRoute("warninginchargestoclosedperiodsimport/{Id}", "B4.controller.import.WarningInChargesToClosedPeriodsImport", "show", requiredPermission: "Import.ChargesToClosedPeriods.Warnings.View"));
            map.AddRoute(new ClientRoute("paymentstoclosedperiodimport", "B4.controller.import.PaymentsToClosedPeriodsImport", requiredPermission: "Import.PaymentsToClosedPeriods.View"));
            map.AddRoute(new ClientRoute("warninginpaymentstoclosedperiodsimport/{Id}", "B4.controller.import.WarningInPaymentsToClosedPeriodsImport", "show", requiredPermission: "Import.PaymentsToClosedPeriods.Warnings.View"));
            map.AddRoute(new ClientRoute("debtorclaimworkimport", "B4.controller.import.DebtorClaimWorkImport", requiredPermission: "Import.DebtorClaimWorkImport.View"));
            map.AddRoute(new ClientRoute("personalaccountdigitalreceiptimport", "B4.controller.import.PersonalAccountDigitalReceiptImport", requiredPermission: "Import.PersonalAccountDigitalReceiptImport.View"));

            map.AddRoute(new ClientRoute("chesimport", "B4.controller.import.ChesImport", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}", "B4.controller.import.chesimport.Navigation", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/sums", "B4.controller.import.chesimport.Sums", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/comparing", "B4.controller.import.chesimport.Comparing", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/compared", "B4.controller.import.chesimport.Compared", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/analysis", "B4.controller.import.chesimport.Analysis", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/saldocheck", "B4.controller.import.chesimport.SaldoCheck", requiredPermission: "Import.ChesImport.View"));
            map.AddRoute(new ClientRoute("chesimport_detail/{id}/payments/{day}", "B4.controller.import.chesimport.Payments", requiredPermission: "Import.ChesImport.View"));

            map.AddRoute(new ClientRoute("personalaccountimport", "B4.controller.import.PersonalAccount"));
            map.AddRoute(new ClientRoute("socialsupportimport", "B4.controller.import.SocialSupport", requiredPermission: "Import.SocialSupportImport"));
            map.AddRoute(new ClientRoute("regoperator", "B4.controller.RegOperator", requiredPermission: "GkhRegOp.FormationRegionalFund.RegOperator.View"));
            map.AddRoute(new ClientRoute("fundformationcontract", "B4.controller.FundFormationContract", requiredPermission: "GkhRegOp.FormationRegionalFund.FundFormationContract.View"));
            map.AddRoute(new ClientRoute("transferfunds", "B4.controller.TransferFunds", requiredPermission: "GkhRf.TransferFunds.View"));
            map.AddRoute(new ClientRoute("paymentdocinfo", "B4.controller.dict.PaymentDocInfo", requiredPermission: "GkhRegOp.Settings.PaymentDocInfo.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/realtychargeaccount", "B4.controller.regop.realty.RealtyChargeAccount", requiredPermission: "Gkh.RealityObject.Register.Accounts.RealtyChargeAccount.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/realtypaymentaccount", "B4.controller.regop.realty.RealtyPaymentAccount", requiredPermission: "Gkh.RealityObject.Register.Accounts.RealtyPaymentAccount.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/realtysupplieraccount", "B4.controller.regop.realty.RealtySupplierAccount", requiredPermission: "Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/realtysubsidyaccount", "B4.controller.regop.realty.RealtySubsidyAccount", requiredPermission: "Gkh.RealityObject.Register.Accounts.SubsidyAccount.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/loan", "B4.controller.regop.realty.Loan", requiredPermission: "Gkh.RealityObject.Register.Loan.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/deliveryagent", "B4.controller.realityobj.DeliveryAgent", requiredPermission: "Gkh.Orgs.DeliveryAgent.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/cashpaymentcenter", "B4.controller.realityobj.CashPaymentCenter", requiredPermission: "Gkh.Orgs.CashPaymentCenter.View"));

            map.AddRoute(new ClientRoute("deliveryagentedit/{id}", "B4.controller.deliveryagent.Navigation", requiredPermission: "Gkh.Orgs.DeliveryAgent.View"));
            map.AddRoute(new ClientRoute("deliveryagentedit/{id}/edit", "B4.controller.deliveryagent.Edit", requiredPermission: "Gkh.Orgs.DeliveryAgent.Edit"));
            map.AddRoute(new ClientRoute("deliveryagentedit/{id}/municipality", "B4.controller.deliveryagent.Municipality", requiredPermission: "Gkh.Orgs.DeliveryAgent.Municipality.View"));
            map.AddRoute(new ClientRoute("deliveryagentedit/{id}/realobj", "B4.controller.deliveryagent.RealityObject", requiredPermission: "Gkh.Orgs.DeliveryAgent.RealityObject.View"));
            map.AddRoute(new ClientRoute("deliveryagent", "B4.controller.DeliveryAgent", requiredPermission: "Gkh.Orgs.DeliveryAgent.View"));
            map.AddRoute(new ClientRoute("federalstandardfeecr", "B4.controller.dict.FederalStandardFeeCr", requiredPermission: "GkhRegOp.Settings.FederalStandardFeeCr.View"));

            map.AddRoute(new ClientRoute("cashpaymentcenteredit/{id}", "B4.controller.cashpaymentcenter.Navigation", requiredPermission: "Gkh.Orgs.CashPaymentCenter.View"));
            map.AddRoute(new ClientRoute("cashpaymentcenteredit/{id}/edit", "B4.controller.cashpaymentcenter.Edit", requiredPermission: "Gkh.Orgs.CashPaymentCenter.Edit"));
            map.AddRoute(new ClientRoute("cashpaymentcenteredit/{id}/municipality", "B4.controller.cashpaymentcenter.Municipality", requiredPermission: "Gkh.Orgs.CashPaymentCenter.Municipality.View"));
            map.AddRoute(new ClientRoute("cashpaymentcenteredit/{id}/realobj", "B4.controller.cashpaymentcenter.RealityObject", requiredPermission: "Gkh.Orgs.CashPaymentCenter.RealityObject.View"));
            map.AddRoute(new ClientRoute("cashpaymentcenteredit/{id}/manorg", "B4.controller.cashpaymentcenter.ManOrg", requiredPermission: "Gkh.Orgs.CashPaymentCenter.ManOrg.View"));
            map.AddRoute(new ClientRoute("cashpaymentcenter", "B4.controller.CashPaymentCenter", requiredPermission: "Gkh.Orgs.CashPaymentCenter.View"));

            map.AddRoute(new ClientRoute("calcaccountcredit", "B4.controller.calcaccount.Credit", requiredPermission: "GkhRegOp.Loans.AccountCredit.View"));
            map.AddRoute(new ClientRoute("calcaccountoverdraft", "B4.controller.calcaccount.Overdraft", requiredPermission: "GkhRegOp.Loans.Overdraft.View"));
            map.AddRoute(new ClientRoute("personal_calculation_protocol/{periodId}/{accountId}", "B4.controller.regop.CalculationProtocol", "show", requiredPermission: "GkhRegOp.Accounts.CalculationProtocol.View"));
            map.AddRoute(new ClientRoute("controltransitaccount", "B4.controller.transitaccount.ControlTransitAccount", requiredPermission: "GkhRegOp.Accounts.ControlTransitAccount.View"));
            map.AddRoute(new ClientRoute("paymentdocuments", "B4.controller.regop.paymentdocument.PaymentDocumentsController", requiredPermission: "GkhRegOp.Accounts.PaymentDocuments.View"));
            map.AddRoute(new ClientRoute("paymentdocumentlogs", "B4.controller.regop.paymentdocument.PaymentDocumentLogsController", requiredPermission: "Tasks.PaymentDocumentLogsView"));
            map.AddRoute(new ClientRoute("crpayments", "B4.controller.calcaccount.PaymentCrSpecAccNotRegop", requiredPermission: "GkhRegOp.Accounts.PaymentCrSpecAccNotRegop.View"));
            map.AddRoute(new ClientRoute("regopservicelog", "B4.controller.RegopServiceLog", requiredPermission: "GkhRegOp.Settings.RegopServiceLog.View"));

            map.AddRoute(new ClientRoute("persaccdistribution/{id}&{code}&{sum}&{src}&{inn}", "B4.controller.distribution.PersonalAccount"));
            map.AddRoute(new ClientRoute("work_act_distribution/{id}&{code}&{sum}&{src}&{inn}", "B4.controller.distribution.PerformedWorkAct", requiredPermission: "GkhRegOp.Accounts.BankOperations.Distributions.PerformedWorkActsDistribution"));
            map.AddRoute(new ClientRoute("realtyaccdistribution/{id}&{code}&{sum}&{src}&{inn}", "B4.controller.distribution.RealtyAccount"));
            map.AddRoute(new ClientRoute("transferctrdistribution/{id}&{code}&{sum}&{src}&{inn}", "B4.controller.distribution.TransferContractor"));
            map.AddRoute(new ClientRoute("refundtransferctrdistribution/{id}&{code}&{sum}&{src}&{inn}", "B4.controller.distribution.RefundTransferContractor"));

            map.AddRoute(new ClientRoute("fstownimportsettings", "B4.controller.administration.fsTownImportSettings", requiredPermission: "Import.FsGorodImportInfoSettings.View"));
            map.AddRoute(new ClientRoute("transferctr", "B4.controller.TransferCtr", requiredPermission: "GkhRf.TransferCtr.View"));
            map.AddRoute(new ClientRoute("programcrtype", "B4.controller.dict.ProgramCrType", requiredPermission: "GkhRegOp.Dictionaries.ProgramCrType.View"));

            map.AddRoute(new ClientRoute("legalclaimwork", "B4.controller.claimwork.LegalClaimWork", requiredPermission: "Clw.ClaimWork.Legal.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/legaledit", "B4.controller.claimwork.EditLegalClaimWork", requiredPermission: "Clw.ClaimWork.Legal.Update"));

            map.AddRoute(new ClientRoute("individualclaimwork", "B4.controller.claimwork.IndividualClaimWork", requiredPermission: "Clw.ClaimWork.Individual.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/individualedit", "B4.controller.claimwork.EditIndividualClaimWork", requiredPermission: "Clw.ClaimWork.Individual.Update"));         

            map.AddRoute(new ClientRoute("jurjournal/debtor", "B4.controller.claimwork.JurJournalDebtor", requiredPermission: "Clw.JurJournal.View"));
            map.AddRoute(new ClientRoute("subsidyincome", "B4.controller.SubsidyIncome", requiredPermission: "GkhRegOp.Settings.SubsidyIncome.View"));
            map.AddRoute(new ClientRoute("persaccbenefits", "B4.controller.PersonalAccountBenefits", requiredPermission: "GkhRegOp.PersonalAccountOwner.Benefits.View"));
            map.AddRoute(new ClientRoute("bank_doc_import_details/{id}", "B4.controller.regop.BankDocumentImportDetail", "show"));
            map.AddRoute(new ClientRoute("bank_doc_import_details/{id}/onlynotdefined", "B4.controller.regop.BankDocumentImportDetail", "show"));

            map.AddRoute(new ClientRoute("emailnewsletter", "B4.controller.EmailNewsletterController", requiredPermission: "GkhRegOp.PersonalAccountOwner.EmailNewsletter.View"));

            map.AddRoute(new ClientRoute("persaccgroup", "B4.controller.dict.PersAccGroup", requiredPermission: "GkhRegOp.Settings.PersAccGroup.View"));
            map.AddRoute(new ClientRoute("paydoctemplate", "B4.controller.regop.report.PaymentDocumentTemplate", requiredPermission: "GkhRegOp.Settings.PayDocTemplate.View"));

            map.AddRoute(new ClientRoute("cp_checks", "B4.controller.regop.period.CloseCheck", requiredPermission: "GkhRegOp.Settings.PeriodChecks.View"));
            map.AddRoute(new ClientRoute("cp_checking", "B4.controller.regop.period.CloseChecking", requiredPermission: "GkhRegOp.AccountingMonth.PeriodChecking.View"));

            map.AddRoute(new ClientRoute("saldo_refresh", "B4.controller.SaldoRefresh", requiredPermission: "GkhRegOp.Accounts.SaldoRefresh.View"));

            map.AddRoute(new ClientRoute("mobileappaccountcomparsion", "B4.controller.MobileAppAccountComparsion", requiredPermission: "GkhRegOp.Accounts.MobileAppAccountComparsion.View"));

            map.AddRoute(new ClientRoute("unconfirmed_payments", "B4.controller.UnconfirmedPayments", requiredPermission: "GkhRegOp.Accounts.UnconfirmedPayments.View"));

            map.AddRoute(new ClientRoute("sberbankpaymentdoc", "B4.controller.regop.paymentdocument.SberbankPaymentDoc", requiredPermission: "GkhRegOp.Accounts.SberbankPaymentDoc.View"));
        }
    }
}