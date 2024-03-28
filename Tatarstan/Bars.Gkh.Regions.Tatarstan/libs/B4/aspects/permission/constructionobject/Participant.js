Ext.define('B4.aspects.permission.constructionobject.Participant', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectparticipantpermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Create',
            applyTo: 'b4addbutton',
            selector: 'constructionobjectparticipantgrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerType_Edit',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerType_View',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ParticipantType_Edit',
            applyTo: '[name=ParticipantType]',
            selector: 'constructobjparticipanteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ParticipantType_View',
            applyTo: '[name=ParticipantType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Contragent_Edit',
            applyTo: '[name=Contragent]',
            selector: 'constructobjparticipanteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Contragent_View',
            applyTo: '[name=Contragent]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Description_Edit',
            applyTo: '[name=Description]',
            selector: 'constructobjparticipanteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.Description_View',
            applyTo: '[name=Description]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentInn_View',
            applyTo: '[name=ContragentInn]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentContactName_View',
            applyTo: '[name=ContragentContactName]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.ContragentContactPhone_View',
            applyTo: '[name=ContragentContactPhone]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Gzhf',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Gzhf', filterFn: function(item) { return item.get('Name') != 'Gzhf'; } });
                } else {
                    store.filters.removeAtKey('Gzhf');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Ispolkom',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Ispolkom', filterFn: function(item) { return item.get('Name') != 'Ispolkom'; } });
                } else {
                    store.filters.removeAtKey('Ispolkom');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Minstroj',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Minstroj', filterFn: function(item) { return item.get('Name') != 'Minstroj'; } });
                } else {
                    store.filters.removeAtKey('Minstroj');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.Gisu',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Gisu', filterFn: function(item) { return item.get('Name') != 'Gisu'; } });
                } else {
                    store.filters.removeAtKey('Gisu');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.TechnicalSupervisionOrganization',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'TechnicalSupervisionOrganization', filterFn: function(item) { return item.get('Name') != 'TechnicalSupervisionOrganization'; } });
                } else {
                    store.filters.removeAtKey('TechnicalSupervisionOrganization');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Participants.Fields.CustomerTypes.PsdDevelopers',
            applyTo: '[name=CustomerType]',
            selector: 'constructobjparticipanteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'PsdDevelopers', filterFn: function(item) { return item.get('Name') != 'PsdDevelopers'; } });
                } else {
                    store.filters.removeAtKey('PsdDevelopers');
                }
            }
        }
    ]
});