Ext.define('B4.aspects.permission.manorg.ContractTat', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgcontracttatperm',

    permissions: [
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_Edit',
            applyTo: 'contractownersadditionservicegrid',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_View',
            applyTo: 'contractownersadditionservicegrid',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_Edit',
            applyTo: 'contractownerscommunalservicegrid',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_View',
            applyTo: 'contractownerscommunalservicegrid',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_Edit',
            applyTo: 'contractownersworkservicegrid',
            selector: '#manorgContractOwnersEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_View',
            applyTo: 'contractownersworkservicegrid',
            selector: '#manorgContractOwnersEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_Edit',
            applyTo: 'contractjsktsjadditionservicegrid',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_View',
            applyTo: 'contractjsktsjadditionservicegrid',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_Edit',
            applyTo: 'contractjsktsjcommunalservicegrid',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_View',
            applyTo: 'contractjsktsjcommunalservicegrid',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_Edit',
            applyTo: 'contractjsktsjworkservicegrid',
            selector: '#jskTsjContractEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_View',
            applyTo: 'contractjsktsjworkservicegrid',
            selector: '#jskTsjContractEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_Edit',
            applyTo: 'transferadditionservicegrid',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_View',
            applyTo: 'transferadditionservicegrid',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_Edit',
            applyTo: 'transfercommunalservicegrid',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_View',
            applyTo: 'transfercommunalservicegrid',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_Edit',
            applyTo: 'transferworkservicegrid',
            selector: '#manorgTransferEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Payment.WorkService_View',
            applyTo: 'transferworkservicegrid',
            selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_Edit',
            applyTo: 'relationadditionservicegrid',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Addition_View',
            applyTo: 'relationadditionservicegrid',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_Edit',
            applyTo: 'relationcommunalservicegrid',
            selector: '#manOrgContractRelationEditWindow'
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Services.Communal_View',
            applyTo: 'relationcommunalservicegrid',
            selector: '#manOrgContractRelationEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },


       
        { name: 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Edit', applyTo: 'b4savebutton', selector: 'relationrditwindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Delete', applyTo: 'b4deletecolumn', selector: 'contractrelationgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.View', applyTo: 'tabpanel tab[text=Передача управления]', selector: 'jsktsjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.Orgs.Managing.Register.Contract.ControlTransfer.Create', applyTo: 'b4addbutton', selector: 'contractrelationgrid' },

        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.SendDuUstav_View', applyTo: 'button[itemId=btnSendUstav]', selector: 'jsktsjeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.SendDuUstav_View', applyTo: 'button[itemId=btnSendDu]', selector: 'ownerseditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.Orgs.Managing.Register.Contract.Field.SendDuUstav_View', applyTo: 'button[itemId=btnSendDu]', selector: '#manorgTransferEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});