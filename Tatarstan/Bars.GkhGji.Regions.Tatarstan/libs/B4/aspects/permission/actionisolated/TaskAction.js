Ext.define('B4.aspects.permission.actionisolated.TaskAction', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.actionisolatedtaskperm',

    permissions: [
        // Вкладка "Задание/Реквизиты"
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.AppealCits_Edit',
            applyTo: '#sfAppealCits',
            selector: '#taskactionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setHideTrigger(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.PlannedAction_Edit',
            applyTo: '#tfPlannedActions',
            selector: '#taskactionEditPanel',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setHideTrigger(!allowed);
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Requisites.Fields.PlannedAction_View',
            applyTo: '#tfPlannedActions',
            selector: '#taskactionEditPanel',
            applyBy: function (component, allowed) {
                var view = this.controller.getMainView(),
                    kindAction = view.down('b4enumcombo[name=KindAction]').getValue(),
                    isVisible = kindAction != B4.enums.KindAction.Survey ? false : allowed;
                this.setVisible(component, isVisible, isVisible && component.permissionRequired);
            }
        },

        // Вкладка "Задание/Дома"
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Houses.Create',
            applyTo: 'b4addbutton',
            selector: '#taskActionIsolatedHouseGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Houses.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskActionIsolatedHouseGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        // Вкладка "Задание/Предмет мероприятия"
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Item.Create',
            applyTo: 'b4addbutton',
            selector: '#taskActionIsolatedItemGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Item.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskActionIsolatedItemGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#taskActionIsolatedAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskActionIsolatedAnnexGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Purposes.Create',
            applyTo: 'b4addbutton',
            selector: '#taskActionIsolatedSurveyPurposeGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.Purposes.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskActionIsolatedSurveyPurposeGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.ArticleLaw.Create',
            applyTo: 'b4addbutton',
            selector: '#taskActionIsolatedArticleLawGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'GkhGji.DocumentsGji.TaskActionIsolated.Register.ArticleLaw.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#taskActionIsolatedArticleLawGrid',
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