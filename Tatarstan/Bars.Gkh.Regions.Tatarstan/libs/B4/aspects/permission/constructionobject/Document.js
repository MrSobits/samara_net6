Ext.define('B4.aspects.permission.constructionobject.Document', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectdocumentpermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Create',
            applyTo: 'b4addbutton',
            selector: 'constructionobjectdocumentgrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Name_Edit',
            applyTo: '[name=Name]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Name_View',
            applyTo: '[name=Name]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Number_Edit',
            applyTo: '[name=Number]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Number_View',
            applyTo: '[name=Number]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Type_Edit',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Type_View',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Date_Edit',
            applyTo: '[name=Date]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Date_View',
            applyTo: '[name=Date]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.File_Edit',
            applyTo: '[name=File]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.File_View',
            applyTo: '[name=File]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Contragent_Edit',
            applyTo: '[name=Contragent]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Contragent_View',
            applyTo: '[name=Contragent]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Description_Edit',
            applyTo: '[name=Description]',
            selector: 'constructobjdocumenteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.Description_View',
            applyTo: '[name=Description]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.MatchingSheet',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'MatchingSheet', filterFn: function(item) { return item.get('Name') != 'MatchingSheet'; } });
                } else {
                    store.filters.removeAtKey('MatchingSheet');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.Psd',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Psd', filterFn: function(item) { return item.get('Name') != 'Psd'; } });
                } else {
                    store.filters.removeAtKey('Psd');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.PsdExpertise',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'PsdExpertise', filterFn: function(item) { return item.get('Name') != 'PsdExpertise'; } });
                } else {
                    store.filters.removeAtKey('PsdExpertise');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.CommissioningAct',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'CommissioningAct', filterFn: function(item) { return item.get('Name') != 'CommissioningAct'; } });
                } else {
                    store.filters.removeAtKey('CommissioningAct');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Documents.Fields.DocumentTypes.Other',
            applyTo: '[name=Type]',
            selector: 'constructobjdocumenteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Other', filterFn: function(item) { return item.get('Name') != 'Other'; } });
                } else {
                    store.filters.removeAtKey('Other');
                }
            }
        }
    ]
});