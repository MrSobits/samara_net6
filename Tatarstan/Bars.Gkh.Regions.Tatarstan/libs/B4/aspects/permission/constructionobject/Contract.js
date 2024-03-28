Ext.define('B4.aspects.permission.constructionobject.Contract', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectcontractpermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Create',
            applyTo: 'b4addbutton',
            selector: 'constructionobjectcontractgrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Number_Edit',
            applyTo: '[name=Number]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Number_View',
            applyTo: '[name=Number]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Name_Edit',
            applyTo: '[name=Name]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Name_View',
            applyTo: '[name=Name]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Type_Edit',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Type_View',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Contragent_Edit',
            applyTo: '[name=Contragent]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Contragent_View',
            applyTo: '[name=Contragent]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Date_Edit',
            applyTo: '[name=Date]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Date_View',
            applyTo: '[name=Date]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.File_Edit',
            applyTo: '[name=File]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.File_View',
            applyTo: '[name=File]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Sum_Edit',
            applyTo: '[name=Sum]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Sum_View',
            applyTo: '[name=Sum]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStart_Edit',
            applyTo: '[name=DateStart]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStart_View',
            applyTo: '[name=DateStart]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEnd_Edit',
            applyTo: '[name=DateEnd]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEnd_View',
            applyTo: '[name=DateEnd]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStartWork_Edit',
            applyTo: '[name=DateStartWork]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateStartWork_View',
            applyTo: '[name=DateStartWork]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEndWork_Edit',
            applyTo: '[name=DateEndWork]',
            selector: 'constructobjcontracteditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.DateEndWork_View',
            applyTo: '[name=DateEndWork]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Smr',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Smr', filterFn: function(item) { return item.get('Name') != 'Smr'; } });
                } else {
                    store.filters.removeAtKey('Smr');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Psd',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow',
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
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Supervision',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Supervision', filterFn: function(item) { return item.get('Name') != 'Supervision'; } });
                } else {
                    store.filters.removeAtKey('Supervision');
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Contracts.Fields.Types.Additional',
            applyTo: '[name=Type]',
            selector: 'constructobjcontracteditwindow',
            applyBy: function(component, allowed) {
                var store = component.getStore();
                if (!allowed) {
                    store.filter({ id: 'Additional', filterFn: function(item) { return item.get('Name') != 'Additional'; } });
                } else {
                    store.filters.removeAtKey('Additional');
                }
            }
        }
    ]
});