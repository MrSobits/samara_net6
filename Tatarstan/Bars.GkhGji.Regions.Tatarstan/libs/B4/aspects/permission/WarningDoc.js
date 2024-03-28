Ext.define('B4.aspects.permission.WarningDoc', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.warningdocperm',

    permissions: [
        // Поля
        // Основание предостережения - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.BaseWarning_View',
            applyTo: '[name=BaseWarning]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Основание предостережения - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.BaseWarning_Edit',
            applyTo: '[name=BaseWarning]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Срок принятия мер о соблюдении требований - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.TakingDate_View',
            applyTo: '[name=TakingDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Срок принятия мер о соблюдении требований - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.TakingDate_Edit',
            applyTo: '[name=TakingDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Документ основания - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.File_View',
            applyTo: '[name=File]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Документ основания - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.File_Edit',
            applyTo: '[name=File]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Результат предостережения - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.ResultText_View',
            applyTo: '[name=ResultText]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Результат предостережения - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.ResultText_Edit',
            applyTo: '[name=ResultText]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Должностные лица
        // Должностное лицо, вынесшее распоряжение - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Official.Autor_View',
            applyTo: '[name=Autor]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Должностное лицо, вынесшее распоряжение - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Official.Autor_Edit',
            applyTo: '[name=Autor]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Ответственный за исполнение - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Official.Executant_View',
            applyTo: '[name=Executant]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Ответственный за исполнение - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Official.Executant_Edit',
            applyTo: '[name=Executant]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Уведомление о направлении предостережения
        // Дата документа - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDate_View',
            applyTo: '[name=NcOutDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Дата документа - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDate_Edit',
            applyTo: '[name=NcOutDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Номер документа - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNum_View',
            applyTo: '[name=NcOutNum]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Номер документа - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNum_Edit',
            applyTo: '[name=NcOutNum]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Дата исходящего пиьма  - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDateLatter_View',
            applyTo: '[name=NcOutDateLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // "Дата исходящего пиьма  - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutDateLatter_Edit',
            applyTo: '[name=NcOutDateLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Номер исходящего письма  - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNumLatter_View',
            applyTo: '[name=NcOutNumLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Номер исходящего письма  - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutNumLatter_Edit',
            applyTo: '[name=NcOutNumLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Уведомление отправлено - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutSent_View',
            applyTo: '[name=NcOutSent]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Уведомление отправлено - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Output.NcOutSent_Edit',
            applyTo: '[name=NcOutSent]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Уведомление об устранении нарушений
        // Дата документа - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDate_View',
            applyTo: '[name=NcInDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Дата документа - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDate_Edit',
            applyTo: '[name=NcInDate]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Номер документа - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNum_View',
            applyTo: '[name=NcInNum]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Номер документа - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNum_Edit',
            applyTo: '[name=NcInNum]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Дата исходящего пиьма  - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDateLatter_View',
            applyTo: '[name=NcInDateLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Дата исходящего пиьма  - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInDateLatter_Edit',
            applyTo: '[name=NcInDateLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Номер исходящего письма  - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNumLatter_View',
            applyTo: '[name=NcInNumLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Номер исходящего письма  - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInNumLatter_Edit',
            applyTo: '[name=NcInNumLatter]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Уведомление отправлено - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInRecived_View',
            applyTo: '[name=NcInRecived]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        //Уведомление отправлено - Редактирование
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.NcInRecived_Edit',
            applyTo: '[name=NcInRecived]',
            selector: '#warningdoceditpanel warningdocrequisitepanel'
        },
        // Получено возражение - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.ObjectionReceived_Edit',
            applyTo: '[name=ObjectionReceived]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setReadOnly(!allowed);
                }
            }
        },
        // Получено возражение - Просмотр
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Field.Input.ObjectionReceived_View',
            applyTo: '[name=ObjectionReceived]',
            selector: '#warningdoceditpanel warningdocrequisitepanel',
            applyBy: function(component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Реестры
        // Основание для предостережения
        // Создание записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.WarningBasis.Create',
            applyTo: 'b4addbutton',
            selector: '#warningdoceditpanel warningdocbasisgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Удаление записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.WarningBasis.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#warningdoceditpanel warningdocbasisgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Нарушение требований
        // Создание записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Violations.Create',
            applyTo: 'b4addbutton',
            selector: '#warningdoceditpanel warningdocviolationsgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Удаление записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Violations.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#warningdoceditpanel warningdocviolationsgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Редактирование записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Violations.Edit',
            applyTo: 'b4editcolumn',
            selector: '#warningdoceditpanel warningdocviolationsgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Документы
        // Создание записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Annex.Create',
            applyTo: 'b4addbutton',
            selector: '#warningdoceditpanel warningdocannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Удаление записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Annex.Delete',
            applyTo: 'b4deletecolumn',
            selector: '#warningdoceditpanel warningdocannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        },
        // Редактирование записей
        {
            name: 'GkhGji.DocumentsGji.WarningInspection.Register.Annex.Edit',
            applyTo: 'b4editcolumn',
            selector: '#warningdoceditpanel warningdocannexgrid',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            }
        }
    ]
});