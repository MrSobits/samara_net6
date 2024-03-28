Ext.define('B4.controller.repairobject.Edit', {
    extend: 'B4.base.Controller',
    
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['RepairObject'],
    stores: ['repairobject.NavigationMenu'],
    views: ['repairobject.EditPanel'],

    mainView: 'repairobject.EditPanel',
    mainViewSelector: '#repairObjEditPanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRepair.RepairObject.RepairObjectViewEdit.Edit',
                    applyTo: 'b4savebutton',
                    selector: '#repairObjEditPanel'
                },
                {
                    name: 'GkhRepair.RepairObject.RepairObjectViewEdit.Edit',
                    applyTo: 'btnState',
                    selector: '#repairObjEditPanel'
                },
                {
                    name: 'GkhRepair.RepairObject.RepairObjectViewEdit.Field.Document',
                    applyTo: '#documentfield',
                    selector: '#repairObjEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhRepair.RepairObject.RepairObjectViewEdit.Field.Comment',
                    applyTo: '#commentfield',
                    selector: '#repairObjEditPanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'statebuttonaspect',
            name: 'repairObjStateButtonAspect',
            stateButtonSelector: '#repairObjEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('repairObjEditPanelAspect').setData(entityId);
                    asp.controller.getStore('repairobject.NavigationMenu').load();
                }
            }
        },
        {
            xtype: 'gkheditpanel',
            name: 'repairObjEditPanelAspect',
            editPanelSelector: '#repairObjEditPanel',
            modelName: 'RepairObject',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('repairObjStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                }
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {
            this.getAspect('repairObjEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});