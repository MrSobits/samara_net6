Ext.define('B4.controller.regop.personal_account.Details',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.EditRecord',
        'B4.aspects.regop.AccountGridEdit',
        'B4.controller.regop.owner.PersonalAccountOwner',
        'B4.aspects.FieldRequirementAspect',
        'B4.enums.TypeTransferSource',
        'B4.enums.YesNo',
        'B4.enums.regop.PersonalAccountOwnerType'
    ],

    views: [
        'regop.personal_account.PersonalAccountCardPanel',
        'regop.personal_account.PersonalAccountOperationWindow',
        'regop.personal_account.PersonalAccountOperationDocumentWindow',
        'regop.personal_account.PersonalAccountHistoryGrid',
        'regop.personal_account.PersonalAccountFieldDetailsWindow',
        'regop.personal_account.privilegedcategory.EditWindow',
        'regop.personal_account.persaccgroup.Grid',
        'regop.personal_account.persaccgroup.EditWindow',
        'regop.personal_account.persaccgroup.AddGroupWindow',
        'regop.personal_account.persaccgroup.AddGroupGrid',
        'regop.personal_account.PersonalAccountPaymentGrid',
        'regop.personal_account.BankStatPaymentWindow',
        'regop.personal_account.BankImportPaymentWindow',
        'regop.personal_account.ChoicesDebtorWindow',
        'regop.personal_account.ownerinformation.Grid',
        'regop.personal_account.ownerinformation.EditWindow',
        'regop.personal_account.PersonalAccountPerfWorkWindow'
    ],
    stores: [
        'regop.personal_account.PeriodSummaryInfo',
        'regop.personal_account.PersonalAccountOperationDetails',
        'regop.personal_account.PersonalAccountHistory',
        'regop.personal_account.BasePersonalAccount',
        'regop.personal_account.PrivilegedCategory',
        'regop.personal_account.OwnerInformation',
        'regop.personal_account.PaymentsInfo',
        'regop.personal_account.ChoicesDebtor',
        'regop.personal_account.PersonalAccountGroup'
    ],

    models: [
        'regop.owner.IndividualAccountOwner',
        'regop.owner.LegalAccountOwner',
        'regop.owner.PersonalAccountOwner',
        'regop.personal_account.BasePersonalAccount',
        'regop.personal_account.PrivilegedCategory',
        'regop.personal_account.OwnerInformation',
        'regop.personal_account.ChoicesDebtor',
        'regop.personal_account.PersAccGroup'
    ],

    mainView: 'regop.personal_account.PersonalAccountCardPanel',
    mainViewSelector: 'personalaccountcardpanel',

    refs: [
        {
            ref: 'closeDateContainer',
            selector: 'paowneraccountaddwin container[name=CloseDateContainer]'
        },
        {
            ref: 'persAccWin',
            selector: 'paowneraccountaddwin'
        },
        {
            ref: 'mainView',
            selector: 'personalaccountcardpanel'
        },
        {
            ref: 'wizard',
            selector: '#itemwizard'
        },
        {
            ref: 'historyGrid',
            selector: 'paccounthistorygrid'
        },
        {
            ref: 'groupGrid',
            selector: 'paccountgroupgrid'
        },
        {
            ref: 'groupAddGrid',
            selector: 'paccountaddgroupgrid'
        },
        {
            ref: 'accountHistoryGrid',
            selector: 'paowneraccountaddwin paowneraccounthistorygrid'
        },
        {
            ref: 'editPanel',
            selector: 'personalaccounteditpanel'
        },
        {
            ref: 'personalAccountGrid',
            selector: 'paownereditwin paowneraccountgrid'
        },
        {
            ref: 'ownerIdField',
            selector: 'paownereditwin hiddenfield[name=Id]'
        },
        {
            ref: 'ownerTypeCombo',
            selector: 'paownereditwin b4enumcombo[name=OwnerType]'
        },
        {
            ref: 'ownerInfoGrid',
            selector: 'paccountownerinformationgrid'
        },
        {
            ref: 'ownerInfoEditWindow',
            selector: 'paccountownerinformationeditwindow'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    accId: null,

    claimWorkInfo: {},

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'detailsPerm',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            },
            permissions: [
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.EntityLogLightGrid_View',
                    applyTo: 'paccounthistorygrid',
                    selector: 'personalaccountcardpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.PaymentsInfo_View',
                    applyTo: 'papaymentgrid',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.PrivilegedCategory.View',
                    applyTo: 'paccountprivilegedcategorygrid',
                    selector: 'personalaccountcardpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.OwnerInformation_View',
                    applyTo: 'paccountownerinformationgrid',
                    selector: 'personalaccountcardpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.BanRecalc_View',
                    applyTo: 'paccountbanrecalcgrid',
                    selector: 'personalaccountcardpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                { name: 'GkhRegOp.PersonalAccount.Tab.PrivilegedCategory.Create', applyTo: 'b4addbutton', selector: 'paccountprivilegedcategorygrid' },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.PrivilegedCategory.Edit',
                    applyTo: 'b4editcolumn',
                    selector: 'paccountprivilegedcategorygrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.PrivilegedCategory.Delete',
                    applyTo: 'b4deletecolumn',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.Groups.View',
                    applyTo: 'paccountgroupgrid',
                    selector: 'personalaccountcardpanel',
                    applyBy: function (component, allowed) {
                        if (component && component.tab) {
                            component.tab.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.Groups.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'paccountgroupgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.Groups.Create',
                    applyTo: 'b4addbutton',
                    selector: 'paccountgroupgrid'
                },
                {
                    name: 'GkhRegOp.Settings.PersAccGroup.Create',
                    applyTo: 'b4addbutton',
                    selector: 'persaccgroupeditwindow'
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.ChargePeriodAdvanced_View',
                    applyTo: '[dataIndex=ChargedByBaseTariff]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.ChargePeriodAdvanced_View',
                    applyTo: '[dataIndex=TariffPayment]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.ChargePeriodAdvanced_View',
                    applyTo: '[dataIndex=Recalc]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Protocol_View',
                    applyTo: 'actioncolumn[type=Protocol]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.SaldoInFromServ_View',
                    applyTo: '[dataIndex=SaldoInFromServ]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.SaldoChangeFromServ_View',
                    applyTo: '[dataIndex=SaldoChangeFromServ]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.SaldoOutFromServ_View',
                    applyTo: '[dataIndex=SaldoOutFromServ]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.PerformedWorkCharged_View',
                    applyTo: '[dataIndex=PerformedWorkCharged]',
                    selector: 'paoperationgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                { name: 'GkhRegOp.PersonalAccount.Registry.Saldo_Change', applyTo: 'gridcolumn[dataIndex=SaldoChange]', selector: 'paoperationgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.CurrTariffDebt_View', applyTo: 'gridcolumn[dataIndex=CurrTariffDebt]', selector: 'paoperationgrid' },
                { name: 'GkhRegOp.PersonalAccount.Registry.OverdueTariffDebt_View', applyTo: 'gridcolumn[dataIndex=OverdueTariffDebt]', selector: 'paoperationgrid' },
                { name: 'GkhRegOp.PersonalAccount.Field.ReportPersonalAccount_View', applyTo: '#btnReportPersonalAccount', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentDoc_View', applyTo: '#sfPaymentDoc', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.Fio_View', applyTo: '#txFio', selector: 'personalaccounteditpanel' },
                {
                    name: 'GkhRegOp.PersonalAccount.Field.ServiceType_View',
                    applyTo: 'b4combobox[name=ServiceType]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Field.ServiceType_Edit',
                    applyTo: 'b4combobox[name=ServiceType]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setReadOnly(!allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Registry.Field.PrivilegedCategoryPercent_View',
                    applyTo: '[name=PrivilegedCategoryPercent]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                { name: 'GkhRegOp.PersonalAccount.Field.AreaShare_View', applyTo: '#txAreaShare', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PersAccNumExternalSystems_View', applyTo: '#btnPersAccNumExternalSystems', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ChangeDate_View', applyTo: '#btnOpenDate', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ChangeDate_View', applyTo: '#btnCloseDate', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ChangeDate_View', applyTo: 'changevalbtn[name=btnRoomAddress]', selector: 'personalaccounteditpanel'},

                //первая строка сумм
                { name: 'GkhRegOp.PersonalAccount.Field.ChargedTariff_View', applyTo: '[name=ChargedBaseTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ChargedOwnerDecision_View', applyTo: '[name=ChargedDecisionTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ChargedPenalty_View', applyTo: '[name=ChargedPenalty]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.TotalCharge_View', applyTo: '[name=TotalCharge]', selector: 'personalaccounteditpanel' },

                //вторая строка сумм
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentTariff_View', applyTo: '[name=PaymentBaseTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentOwnerDecision_View', applyTo: '[name=PaymentDecisionTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentPenalty_View', applyTo: '[name=PaymentPenalty]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentTotal_View', applyTo: '[name=TotalPayment]', selector: 'personalaccounteditpanel' },

                //третья строка сумм
                { name: 'GkhRegOp.PersonalAccount.Field.PaymentDebt_View', applyTo: '[name=DebtBaseTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.ContributionsInArrearsTariff_View', applyTo: '[name=DebtDecisionTariff]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.PenaltyDebt_View', applyTo: '[name=DebtPenalty]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.TotalDebt_View', applyTo: '[name=TotalDebt]', selector: 'personalaccounteditpanel' },
                { name: 'GkhRegOp.PersonalAccount.Field.CashPayCenter_View', applyTo: '[name=CashPayCenter]', selector: 'personalaccounteditpanel' },

                { name: 'GkhRegOp.PersonalAccount.Field.PerfWorkChargeBalance_View', applyTo: '[name=PerfWorkChargeBalanceFieldSet]', selector: 'personalaccounteditpanel' },

                {
                    name: 'GkhRegOp.PersonalAccount.Field.Restructuring_View',
                    applyTo: '[name=Restructuring]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                            component.allowed = allowed;
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Field.Amicable_Agreement_View',
                    applyTo: '[name=AmicableAgreement]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                            component.allowed = allowed;
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Field.Pir_View',
                    applyTo: '[name=Pir]',
                    selector: 'personalaccounteditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                            component.allowed = allowed;
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccount.Tab.PaymentsInfo.UserLogin_View',
                    applyTo: '[dataIndex=UserLogin]',
                    selector: 'papaymentgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
            ]
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'redirectPerm',
            permissions: [
                {
                    name: 'GkhRegOp.Accounts.BankOperations.View'
                }
            ],
            loadPermissions: function () {
                var me = this;
                return B4.Ajax.request({
                    url: B4.Url.action('/Permission/GetPermissions'),
                    params: {
                        permissions: Ext.encode(me.collectPermissions())
                    }
                });
            }
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhRegOp.PersonalAccountOwner.Field.BirthDate_Rqrd', applyTo: 'datefield[name=BirthDate]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentityType_Rqrd', applyTo: 'b4enumcombo[name=IdentityType]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentitySerial_Rqrd', applyTo: 'textfield[name=IdentitySerial]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentityNumber_Rqrd', applyTo: 'textfield[name=IdentityNumber]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Patronymic_Rqrd', applyTo: 'textfield[name=SecondName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.CreateDate_Rqrd', applyTo: '#dfOpenDate', selector: 'paowneraccountaddwin [section="add"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.CreateDate_Rqrd', applyTo: '#dfOpenDate', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractNumber_Rqrd', applyTo: '#tfContractNumber', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractDate_Rqrd', applyTo: '#dfContractDate', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractDocument_Rqrd', applyTo: '#ffContractDocument', selector: 'paowneraccountaddwin [section="edit"]' }
            ],
            viewSelector: 'paownereditwin'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'PersonalAccountOperationDocumentViewFormAspect',
            gridSelector: 'paoperationwin gridpanel',
            editFormSelector: 'paoperationdocwin',
            editWindowView: 'regop.personal_account.PersonalAccountOperationDocumentWindow',
            storeName: 'regop.personal_account.PersonalAccountOperationDetails',
            editRecord: function (record) {
                this.setFormData(record);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'PersonalAccountPrivilegedCategoryGridWindowAspect',
            gridSelector: 'paccountprivilegedcategorygrid',
            editFormSelector: 'paccountprivilegedcategoryeditwindow',
            modelName: 'regop.personal_account.PrivilegedCategory',
            editWindowView: 'regop.personal_account.privilegedcategory.EditWindow',
            listeners: {
                getdata: function (asp, rec) {
                    rec.set('PersonalAccount', asp.controller.accId);
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'PersonalAccountGroupGridWindowAspect',
            gridSelector: 'paccountgroupgrid',
            editFormSelector: 'persaccgroupeditwindow',
            modelName: 'regop.personal_account.PersAccGroup',
            editWindowView: 'regop.personal_account.persaccgroup.EditWindow',
            editRecord: null,
            listeners: {
                getdata: function (asp, rec) {
                    rec.set('PersonalAccount', asp.controller.accId);
                }
            },
            otherActions: function (actions) {
                // открытие формы включения в группу
                actions[this.gridSelector + ' b4addbutton'] = {
                    'click': function () {
                        var me = this,
                            win = me.getCmpInContext('persaccgroupeditwindow');

                        if (!win) {
                            win = Ext.create('B4.view.regop.personal_account.persaccgroup.EditWindow', {
                                closeAction: 'destroy',
                                ctxKey: me.getCurrentContextKey(),
                                modal: true
                            });
                        }

                        win.show();
                    }
                }

                // исключение ЛС из группы
                actions[this.gridSelector + ' b4deletecolumn'] = {
                    'click': function (a, b, t, y, r, record) {
                        var me = this;

                        if (record.get('IsSystem') == B4.enums.YesNo.Yes) {
                            Ext.Msg.alert('Ошибка удаления', 'Нельзя исключать лицевые счета из системных групп');
                            return;
                        }

                        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите исключить лицевой счёт из гуппы?', function (result) {
                            if (result == 'yes') {
                                me.mask('Удаление', B4.getBody());
                                B4.Ajax.request(
                                    {
                                        url: B4.Url.action('RemovePersonalAccountFromGroups', 'PersonAccountGroup'),
                                        timeout: 5 * 60 * 1000, // 5 минут
                                        method: 'POST',
                                        params: {
                                            'accId': me.accId,
                                            'groupIds': [record.get('Id')]
                                        }
                                    }
                                )
                                    .next(function () {
                                        me.getGroupGrid().getStore().load();
                                        me.unmask();
                                    }, this)
                                    .error(function (result) {
                                        Ext.Msg.alert('Ошибка удаления!', result.message);
                                        me.unmask();
                                    }, this);
                            }

                        }, me);
                    }
                }

                // обработка включения ЛС в выбранные группы
                actions['paccountaddgroupgrid button[itemId=btnAddToGroup]'] = {
                    'click': function () {
                        var me = this,
                            grid = me.getCmpInContext('paccountaddgroupgrid'),
                            groupsGrid = me.getCmpInContext('persaccgroupeditwindow'),
                            store = me.getCmpInContext('paccountgroupgrid').getStore(),
                            sm = grid.getSelectionModel(),
                            selected = Ext.Array.map(sm.getSelection(), function (el) {
                                return el.get('Id');
                            });

                        if (selected.length === 0) {
                            Ext.Msg.alert('Ошибка при выполнении операции', 'Необходимо выбрать хотя бы одну грппу');
                            return;
                        }

                        me.mask('Включение в группу', B4.getBody().getActiveTab());
                        B4.Ajax.request(
                            {
                                url: B4.Url.action('AddPersonalAccountToGroups', 'PersonAccountGroup'),
                                timeout: 5 * 60 * 1000, // 5 минут
                                method: 'POST',
                                params: {
                                    'accId': me.accId,
                                    'groupIds': selected
                                }
                            }
                        ).next(function (resp) {
                            me.unmask();
                            groupsGrid.close();
                            store.load();
                        }).error(function (resp) {
                            var response = Ext.isEmpty(resp.responseText) ? resp : Ext.JSON.decode(resp.responseText);
                            Ext.Msg.alert('Ошибка при выполнении операции', response.message || response.Message);
                            me.unmask();
                        });
                    }
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'PersonalAccountAddGroupWindowAspect',
            gridSelector: 'paccountaddgroupgrid',
            editFormSelector: 'persaccgroupaddwindow',
            modelName: 'regop.personal_account.PersAccGroup',
            editWindowView: 'regop.personal_account.persaccgroup.AddGroupWindow',

            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model = me.getModel(null);

                if (!id) {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'PersonalAccountPaymentsFormAspect',
            gridSelector: 'papaymentgrid',
            editRecord: function (record) {
                var asp = this,
                    sourceType = record.get('Source');

                if (sourceType != B4.enums.TypeTransferSource.BankAccountStatement &&
                    sourceType != B4.enums.TypeTransferSource.BankDocumentImport) {
                    Ext.Msg.alert('Ошибка', 'Редактирование не возможно из-за отсутствия связи с документом оплаты.');
                    return;
                }

                if (sourceType == B4.enums.TypeTransferSource.BankAccountStatement) {
                    asp.editFormSelector = 'pabankstatpaymenteditwin';
                    asp.editWindowView = 'regop.personal_account.BankStatPaymentWindow';
                } else {
                    asp.editFormSelector = 'pabankimportpaymenteditwin';
                    asp.editWindowView = 'regop.personal_account.BankImportPaymentWindow';
                }

                asp.getDistrCode(record);

                if (Gkh.config.RegOperator.GeneralConfig.IsPersonalAccountPaymentSingleField) {
                    asp.getForm().down('[name=AllInfo]').update(record.data);
                }
            },
            getDistrCode: function (rec) {
                var asp = this,
                    form = asp.getForm(),
                    field = form.down('[name=DistributionCode]'),
                    store,
                    distr,
                    distrCode = rec.get('DistributionCode'),
                    sourceType = rec.get('Source');

                if (sourceType == B4.enums.TypeTransferSource.BankAccountStatement) {

                    form.hide();

                    field.setValue(null);

                    store = field.getStore();

                    store.load();

                    store.on('load', function (store, records) {
                        Ext.each(distrCode.split(','), function (code) {

                            Ext.each(records, function (rec) {
                                if (rec.get('Code') == code) {
                                    if (distr) {
                                        distr = Ext.String.format('{0}, {1}', distr, rec.get('Name'));
                                    } else {
                                        distr = rec.get('Name');
                                    }
                                }
                            });
                        });

                        asp.setFormData(rec);

                        field.setValue(distr);
                        rec.set('DistributionCodeText', distr);

                        form.show();
                    });
                } else {
                    asp.setFormData(rec);
                }
            }
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'PersonalAccountOwnerInfoGridWindowAspect',
            gridSelector: 'paccountownerinformationgrid',
            editFormSelector: 'paccountownerinformationeditwindow',
            modelName: 'regop.personal_account.OwnerInformation',
            editWindowView: 'B4.view.regop.personal_account.ownerinformation.EditWindow',
            listeners: {
                getdata: function (asp, rec) {
                    rec.set('BasePersonalAccount', asp.controller.accId);
                },
                beforesave: function (asp, rec) {
                    var endDate = rec.get('EndDate');
                    if (endDate) {
                        if (rec.get('StartDate') > endDate) {
                            Ext.Msg.alert('Ошибка!', 'Дата начала документа должна быть меньше даты окончания документа');
                            return false;
                        }
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'PersonalAccountBanRecalcGridWindowAspect',
            gridSelector: 'paccountbanrecalcgrid',
            modelName: 'regop.personal_account.BanRecalc',
            listeners: {
                getdata: function (asp, rec) {
                    rec.set('PersonalAccount', asp.controller.accId);
                }
            },
            editRecord: function () { },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите снять запрет перерасчета за выбранный период?', function (result) {
                    if (result == 'yes') {
                        var model = this.getModel(record);

                        var rec = new model({ Id: record.getId() });
                        me.mask('Удаление', B4.getBody());
                        rec.destroy()
                            .next(function () {
                                this.fireEvent('deletesuccess', this);
                                me.updateGrid();
                                me.unmask();
                            }, this)
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                me.unmask();
                            }, this);
                    }
                }, me);
            },
        }
    ],

    init: function () {
        var me = this,
            events = {
                'personalaccountcardpanel paoperationgrid b4editcolumn': {
                    'click': me.onEditRow
                },
                'paoperationgrid [name=OpenDate]': { change: { fn: me.filterDates, scope: me } },
                'paoperationgrid [name=CloseDate]': { change: { fn: me.filterDates, scope: me } },
                'paoperationgrid': {
                    render: function (grid) {
                        grid.getStore().on('beforeload', me.onOperationStoreBeforeLoad, me);
                    }
                },
                'paccountprivilegedcategorygrid': {
                    render: function (grid) {
                        grid.getStore().on('beforeload', me.onPrivilegedCategoryBeforeLoad, me);
                        grid.getStore().load();
                    }
                },
                'personalaccounteditpanel [action=redirecttoowner]': { click: { fn: me.goToOwner, scope: me } },
                'personalaccounteditpanel [action=redirecttoroom]': { click: { fn: me.goToRoom, scope: me } },
                'personalaccounteditpanel #btnReportPersonalAccount': { click: { fn: me.reportPersonalAccount, scope: me } },
                'personalaccounteditpanel b4selectfield': {
                    change: {
                        fn: me.getPaymentDocument,
                        scope: me
                    }
                },
                'personalaccounteditpanel changevalbtn': {
                    beforevaluesave: {
                        fn: function (btn, params) {
                            params.entityId = me.getMainView().params.Id;
                        },
                        scope: me
                    },
                    beforeshowwindow: {
                        fn: function () {
                            var record = me.getMainView().getForm().getRecord(),
                                state = record.get('State');

                            if (state && state.FinalState) {
                                Ext.Msg.alert('Внимание', 'Для ЛС в данном статусе невозможна смена значения данного параметра.');
                                return false;
                            }

                            return true;
                        },
                        scope: me
                    },
                    valuesaved: function (btn, value) {
                        var field = me.getEditPanel().down(btn.valueFieldSelector);

                        if (field.xtype == 'datefield') {
                            value = Ext.Date.parse(value, 'd.m.Y');
                        }

                        if (field) {
                            field.setValue(value);
                        }
                    }
                },
                'paccounthistorygrid': {
                    'show': function (grid) {
                        grid.getStore().on('beforeload', me.onHistoryBeforeLoad, me);
                        grid.getStore().load();
                    }
                },

                'paccountgroupgrid': {
                    'render': function (grid) {
                        grid.getStore().on('beforeload', me.onAccountGroupBeforeLoad, me);
                        grid.getStore().load();
                    }
                },

                'persaccgroupeditwindow paccountaddgroupgrid': {
                    'render': function (grid) {
                        var store = grid.getStore();
                        store.on('beforeload', function (st, operation) {
                            operation.params.isSystem = B4.enums.YesNo.No;
                        });

                        store.load();
                    }
                },

                'paownereditwin paowneraccountgrid': {
                    render: {
                        fn: me.onPersonalAccoumtGridRender,
                        scope: me
                    }
                },

                'personalaccounteditpanel [hasDetails=true]': {
                    focus: me.showDetails
                },

                'personalaccounteditpanel [name=PerfWorkChargeBalance]': {
                    focus: me.showPerfWorkDetails
                },

                'paccountownerinformationgrid': {
                    'render': function (grid) {
                        grid.getStore().on('beforeload', me.onAccountOwnerInfoBeforeLoad, me);
                        grid.getStore().load();
                    }
                },

                'papaymentgrid': {
                    'afterrender': function (grid) {
                        grid.getStore().on('beforeload', function (store, operation) {
                            operation.params.accId = me.accId;
                        });
                        grid.getStore().load();
                    },
                    'rowaction': {
                        fn: me.onPaymentGridRowAction,
                        scope: me
                    }
                },

                'paccountbanrecalcgrid': {
                    'render': function (grid) {
                        grid.getStore().on('beforeload', me.onAccountBanRecalcBeforeLoad, me);
                        grid.getStore().load();
                    }
                }

            };

        if (Gkh.config.ClaimWork.Enabled === true) {
            events['personalaccountcardpanel button[action=restruct]'] = { 'click': me.gotoClaimWork };
            events['personalaccountcardpanel button[action=amicagr]'] = { 'click': me.gotoClaimWork };
            events['personalaccountcardpanel button[action=pir]'] = { 'click': me.gotoClaimWork };
        }

        me.control(events);

        me.callParent(arguments);
    },

    showPerfWorkDetails: function () {
        var me = this,
            win = Ext.widget('paoperatioperfworknwin', {
                listeners: {
                    show: function () {
                        Ext.select('.x-mask').addListener('click', function () {
                            win.close();
                        });
                    }
                }
            }),
            store = win.down('gridpanel').getStore();

        store.on('beforeload',
            function (store, operation) {
                operation.params.accId = me.getMainView().params.Id;
            },
            me);

        store.load();
        win.show();
    },

    showDetails: function (fld) {
        var me = this,
            win = Ext.widget('personalaccountfielddetailswindow', {
                listeners: {
                    show: function () {
                        Ext.select('.x-mask').addListener('click', function () {
                            win.close();
                        });
                    }
                }
            }),
            store = win.down('gridpanel').getStore();

        store.on('beforeload',
            function (store, operation) {
                operation.params.fieldName = fld.name;
                operation.params.accId = me.getMainView().params.Id;
            },
            me);

        store.on('load', function () { win.show(); });
        store.load();
    },

    filterDates: function (fld) {
        var grid = fld.up('paoperationgrid'),
            store = grid.getStore(),
            startFld = grid.down('[name=OpenDate]'),
            endFld = grid.down('[name=CloseDate]');

        store.clearFilter(true);
        store.filter([{ property: 'OpenDate', value: startFld.getValue() }, { property: 'CloseDate', value: endFld.getValue() }]);
    },

    onEditRow: function (p1, p2, p3, p4, p5, rec) {
        var me = this,
            //panel = me.getCmpInContext('personalaccountcardpanel'),
            win = me.getCmpInContext('paoperationwin'),
            store,
            activeTab;

        if (!win) {

            activeTab = B4.getBody().getActiveTab();

            win = Ext.create('B4.view.regop.personal_account.PersonalAccountOperationWindow', {
                constrain: true,
                //renderTo: activeTab.getEl(),
                closeAction: 'destroy',
                title: 'Операции за период ' + rec.get('Period'),
                ctxKey: me.getCurrentContextKey(),
                modal: true
            });

            activeTab.add(win);
        }

        store = win.down('grid').getStore();

        store.clearFilter(true);
        store.filter('periodSummaryId', rec.get('Id'));

        win.show();
    },

    onOperationStoreBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView();

        if (view.params) {
            operation.params.accountId = view.params.Id;
        }
    },

    show: function (id) {
        var me = this,
            view,
            params = {
                Id: id
            },
            model = me.getModel('regop.personal_account.BasePersonalAccount');

        me.accId = id;

        view = me.getMainView() || Ext.widget('personalaccountcardpanel');
        me.bindContext(view);
        me.application.deployView(view);

        view.params = params;

        me.mask();

        model.load(id, {
            success: function (rec) {
                me.unmask();

                view.loadRecord(rec);
                params.ownerId = rec.get('AccountOwner');
                params.ownerType = rec.get('OwnerType');
                params.realityObjectId = rec.get('RealityObject').Id;
                params.realityObjectRoomId = rec.get('Room').Id;

                    me.initClaimWorkIds(id, params.ownerId);
                view.down('paoperationgrid').getStore().load();
            },
            failure: function () {
                me.unmask();
            }
        });
    },

    goToOwner: function () {
        var me = this;

        var items = B4.getBody().items;
        var index = items.findIndexBy(function (tab) {
            return tab.urlToken != null && tab.urlToken.indexOf('regop_personal_acc_owner') === 0;
        });

        if (index != -1) {
            B4.getBody().remove(items.items[index], true);
        }

        var innerParams = me.getMainView().params;
        me.application.getRouter().redirectTo('regop_personal_acc_owner/' + innerParams.ownerId + '/' + innerParams.ownerType);
    },

    goToRoom: function () {
        var me = this;

        var items = B4.getBody().items;
        var index = items.findIndexBy(function (tab) {
            return tab.urlToken != null && tab.urlToken.indexOf('realityobjectedit') === 0;
        });

        if (index != -1) {
            B4.getBody().remove(items.items[index], true);
        }

        var innerParams = me.getMainView().params;
        me.application.getRouter().redirectTo('realityobjectedit/' + innerParams.realityObjectId + '/roomedit/' + innerParams.realityObjectRoomId);
        return;
    },

    gotoClaimWork: function (button) {
        var me = this,
            items = B4.getBody().items,
            index = items.findIndexBy(function(tab) {
                return tab.urlToken != null && tab.urlToken.indexOf('realityobjectedit') === 0;
            }),
            ownerType = me.getMainView().params.ownerType,
            claimWorkId = me.claimWorkInfo.ClaimWorkId,
            restructDebtId = me.claimWorkInfo.RestructDebtId,
            restructDebtAmicAgrId = me.claimWorkInfo.RestructDebtAmicAgrId,
            urlType,
            document;

        if (ownerType == B4.enums.regop.PersonalAccountOwnerType.Individual) {
            urlType = 'Individual';
        } else {
            urlType = 'Legal';
        }

        if (index != -1) {
            B4.getBody().remove(items.items[index], true);
        }

        switch (button.action) {
            case 'pir':
                document = Ext.String.format('{0}edit', urlType);
                break;
            case 'restruct':
                if (restructDebtId) {
                    document = Ext.String.format('{0}/restructdebt', restructDebtId);
                } else {
                    return;
                }
                break;
            case 'amicagr':
                if (restructDebtAmicAgrId) {
                    document = Ext.String.format('{0}/restructdebtamicagr', restructDebtAmicAgrId);
                } else {
                    return;
                }
                break;
            default:
                return;
        }

        if (claimWorkId) {
            me.application.getRouter()
                .redirectTo(Ext.String.format('claimwork/{0}ClaimWork/{1}/{2}', urlType, claimWorkId, document));
        }
    },

    reportPersonalAccount: function () {
        var me = this,
            grid = me.getMainView();

        var _params = {
            accId: grid.params.Id,
            reportId: 'PersonalAccountReport'
        };
        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('PersonalAccountReport', 'BasePersonalAccount'),
            params: _params,
            timeout: 300000
        }).next(function (resp) {
            var tryDecoded;

            me.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err.message || err);
        });
    },

    getPaymentDocument: function (win, val) {
        if (!val) {
            return;
        }
        var me = this,
            params = me.getMainView().params,
            isPeriodClosed = val.IsClosed,
            isAvailablePrintingInOpenPeriod = Gkh.config.RegOperator.PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.PrintingInOpenPeriod;

        if (!params) {
            return;
        }

        // Если выгрузка происходит по открытому периоду - должна быть включена галочка (в единых настройках приложения), разрешающая это делать
        if (!isPeriodClosed && !isAvailablePrintingInOpenPeriod) {
            Ext.Msg.alert('Период не закрыт', 'Выгрузить платежный документ можно только для закрытого периода');
            return false;
        }

        var _params = {
            reportId: 'PaymentDocument',
            periodId: val.Id,
            reportPerAccount: true,
            accountId: params.Id
        };
        me.mask("Обработка...");
        B4.Ajax.request({
            url: B4.Url.action('ExportPaymentDocuments', 'BasePersonalAccount'),
            params: _params,
            timeout: 300000
        }).next(function (resp) {
            var tryDecoded;

            me.unmask();
            try {
                tryDecoded = Ext.JSON.decode(resp.responseText);
            } catch (e) {
                tryDecoded = {};
            }

            var id = resp.data ? resp.data : tryDecoded.data;

            if (id > 0) {
                Ext.DomHelper.append(document.body, {
                    tag: 'iframe',
                    id: 'downloadIframe',
                    frameBorder: 0,
                    width: 0,
                    height: 0,
                    css: 'display:none;visibility:hidden;height:0px;',
                    src: B4.Url.action('Download', 'FileUpload', { Id: id })
                });
            }
        }).error(function (err) {
            me.unmask();
            Ext.Msg.alert('Ошибка', err.message || err.message || err);
        });
    },

    onHistoryBeforeLoad: function (store) {
        var me = this,
            params = me.getMainView().params;

        if (params) {
            Ext.apply(store.getProxy().extraParams, {
                id: params.Id
            });
        }
    },

    onPaymentGridRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'redirect':
                this.onPaymentRedirect(record);
                break;
        }
    },

    onPaymentRedirect: function (record) {
        var me = this,
            docId = record.get('DocumentId'),
            source = record.get('Source'),
            permissionAspect;

        switch (source) {
            case B4.enums.TypeTransferSource.BankAccountStatement:

                permissionAspect = me.getAspect('redirectPerm');
                permissionAspect.loadPermissions(record)
                    .next(function (response) {
                        var grants = Ext.decode(response.responseText);

                        if (grants && grants[0]) {
                            grants = grants[0];
                        }

                        if (grants[0] == 0) {
                            Ext.Msg.alert('Сообщение', 'Отсутствуют права доступа для просмотра данной информации');
                            return;
                        }

                        me.application.getRouter().forward('bank_statement', true, { Id: docId });
                    });
                break;
            case B4.enums.TypeTransferSource.BankDocumentImport:
                Ext.History.add(Ext.String.format('bank_doc_import_details/{0}', docId));
                break;
            default:
                Ext.Msg.alert('Внимание!', 'Просмотр недоступен из-за отсутствия связи с документом');
                break;
        }
    },

    onPrivilegedCategoryBeforeLoad: function (store, operation) {
        operation.params.accId = this.accId;
    },

    onAccountGroupBeforeLoad: function (store, operation) {
        operation.params.accId = this.accId;
    },

    onAccountOwnerInfoBeforeLoad: function (store, operation) {
        operation.params.accId = this.accId;
    },

    onAccountBanRecalcBeforeLoad: function (store, operation) {
        operation.params.accId = this.accId;
    },

    onBeforePersonalAccountStoreLoad: function (store, operation) {
        operation.params.fromOwner = true;
        operation.params.ownerId = this.getOwnerIdField().getValue();
    },

    onPersonalAccoumtGridRender: function (grid) {
        grid.getStore().on('beforeload', this.onBeforePersonalAccountStoreLoad, this);
    },

    getWindowClaimWork: function (url, btn, check) {
        var me = this,
            choosegDebtorWindow,
            ownerType = me.getMainView().params.ownerType,
            urlType;

        if (ownerType == B4.enums.regop.PersonalAccountOwnerType.Individual) {
            urlType = 'individual';
        } else {
            urlType = 'legal';
        }

        me.check = check;

        switch (btn.name) {
            case 'Pir':
                choosegDebtorWindow = me.getView('regop.personal_account.ChoicesDebtorWindow').create();
                break;
        }

        choosegDebtorWindow.url = url;
        choosegDebtorWindow.down('gridpanel').getStore().on('beforeload', this.onBeforeChoosingDebtorStoreLoad, this);
        choosegDebtorWindow.down('gridpanel').getStore().load();
        choosegDebtorWindow.down('gridpanel').getStore().on('load', function (store, records, successful, eOpts) {
            if (records.length == 1) {
                Ext.History.add(Ext.String.format(url, urlType, records[0].data.Id, urlType));
            } else {
                choosegDebtorWindow.show();
            }
        }, this);

    },

    initClaimWorkIds: function(id, ownerId) {
        var me = this;

        B4.Ajax.request(B4.Url.action('CheckClaimWork', 'DebtorClaimworkRegoperator', {
            id: id,
            ownerId: ownerId
        })).next(function (resp) {
            var data = Ext.decode(resp.responseText).data || {};
            me.claimWorkInfo = data;

            me.setVisibleClaimWorkButtons();
        }).error(function () {
            me.claimWorkInfo = {};
            me.setVisibleClaimWorkButtons();
        });
    },

    setVisibleClaimWorkButtons: function() {
        var me = this,
            view = me.getMainView(),
            pir = view.down('button[action=pir]'),
            restruct = view.down('button[action=restruct]'),
            amicagr = view.down('button[action=amicagr]');

        view.down('buttongroup[name=ClaimWorkButtons]').setVisible(me.claimWorkInfo.ClaimWorkId);

        var setButtonVisible = function (button, allowed) {
            button.setVisible(allowed);
        }

        setButtonVisible(pir, me.claimWorkInfo.ClaimWorkId);
        setButtonVisible(restruct, me.claimWorkInfo.RestructDebtId);
        setButtonVisible(amicagr, me.claimWorkInfo.RestructDebtAmicAgrId);

    },

    onBeforeChoosingDebtorStoreLoad: function (store, operation) {
        operation.params.Id = this.getMainView().params.ownerId;
        operation.params.checkRestructDebtAmicableAgreement = this.check;
    }
});