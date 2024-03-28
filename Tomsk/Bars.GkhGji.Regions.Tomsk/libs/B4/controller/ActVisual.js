Ext.define('B4.controller.ActVisual', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.enums.YesNoNotSet'
    ],

    models: ['ActVisual'],

    stores: [
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.FrameVerification'
    ],

    views: ['actvisual.EditPanel'],

    mainView: 'actvisual.EditPanel',
    mainViewSelector: '#actvisualpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Аспект формирвоания документов для Акта виз-го осмотра
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'actVisualCreateButtonAspect',
            buttonSelector: '#actvisualpanel gjidocumentcreatebutton',
            containerSelector: '#actvisualpanel',
            typeDocument: 100 // Тип документа Акт виз-го осмотра
        },
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'actVisualEditPanelAspect',
            permissions: [
                {
                    name: 'GkhGji.DocumentsGji.ActVisual.Delete', applyTo: '#btnDelete', selector: '#actvisualpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                },
                {
                    name: 'GkhGji.DocumentsGji.ActVisual.Field.DocumentNumber_Edit', applyTo: 'textfield[name="DocumentNumber"]', selector: '#actvisualpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) component.setReadOnly(false);
                            else component.setReadOnly(true);
                        }
                    }
                },
                { name: 'GkhGji.DocumentsGji.ActVisual.Field.DocumentDate_Edit', applyTo: 'datefield[name="DocumentDate"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Field.DocumentTime_Edit', applyTo: 'timefield[name="Time"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Details.Inspectors_Edit', applyTo: 'gkhtriggerfield[name="Inspectors"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Details.Address_Edit', applyTo: 'b4selectfield[name="RealityObject"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Details.Flat_Edit', applyTo: 'textfield[name="Flat"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Details.CheckArea_Edit', applyTo: 'b4selectfield[name="FrameVerification"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.CheckResult.CheckResult_Edit', applyTo: 'textarea[name="InspectionResult"]', selector: '#actvisualpanel' },
                { name: 'GkhGji.DocumentsGji.ActVisual.Conclusion.Conclusion_Edit', applyTo: 'textarea[name="Conclusion"]', selector: '#actvisualpanel' }
            ]
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actvisualstatebuttonaspect',
            stateButtonSelector: '#actvisualpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель

                    var editPanelAspect = asp.controller.getAspect('actVisualEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            //Аспект кнопки печати акта проверки
            xtype: 'gkhbuttonprintaspect',
            name: 'actVisualPrintAspect',
            buttonSelector: '#actvisualpanel #btnPrint',
            codeForm: 'ActVisual',
            displayField: 'Description',
            getUserParams: function (reportId) {
                var me = this,
                    param = { DocumentId: me.controller.params.documentId };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /* 
            * Аспект  для Акта визуального осмотра (в нем идет сохранение основных сведений + формирование дочерних документов)
            */
            xtype: 'gjidocumentaspect',
            name: 'actVisualEditPanelAspect',
            editPanelSelector: '#actvisualpanel',
            modelName: 'ActVisual',
            otherActions: function(actions) {
                actions[this.editPanelSelector + ' textfield[name=Time]'] = {change: {fn: function(cmp, value) {
                            if (!cmp.isValid() || Ext.isEmpty(value)) {
                                return;
                            }
                        }
                    }
                };
            },
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this,
                    tfTime = panel.down('[name=Time]'),
                    hour = rec.get('Hour'),
                    minute = rec.get('Minute'),
                    trfInspectors = panel.down('triggerfield[name=Inspectors]'),
                    hourStr,
                    callbackUnMask,
                    minuteStr;

                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                tfTime.setValue(null);

                hourStr = '00';
                if (hour > 0 && hour < 10) {
                    hourStr = '0' + Ext.util.Format.number(hour, '0');
                } else if (hour > 0) {
                    hourStr = Ext.util.Format.number(hour, '00');
                }
                
                minuteStr = '00';
                if (minute > 0 && minute < 10) {
                    minuteStr = '0' + Ext.util.Format.number(minute, '0');
                }
                else if (minute > 0) {
                    minuteStr = Ext.util.Format.number(minute, '00');
                }
                
                tfTime.setValue(hourStr + ':' + minuteStr);

                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber')) {
                    panel.setTitle('Акт визуального осмотра ' + rec.get('DocumentNumber'));
                } else {
                    panel.setTitle('Акт визуального осмотра');
                }

                B4.Ajax.request(B4.Url.action('GetInfo', 'ActVisual', {
                    documentId: rec.getId()
                })).next(function(response) {
                    var obj = Ext.decode(response.responseText);
                    trfInspectors.setValue(obj.data.inspectorIds);
                    trfInspectors.updateDisplayedText(obj.data.inspectorFio);
                });

                //Передаем аспекту смены статуса необходимые параметры
                asp.controller.getAspect('actvisualstatebuttonaspect').setStateData(rec.get('Id'), rec.get('State'));
                asp.disableButtons(false);
                
                //загружаем стор отчетов
                me.controller.getAspect('actVisualPrintAspect').loadReportStore();
                
                // обновляем кнопку Сформирвоать
                me.controller.getAspect('actVisualCreateButtonAspect').setData(rec.get('Id'));
            },
            
            disableButtons: function (value) {
                //получаем все батон-группы
                var me = this,
                    groups = Ext.ComponentQuery.query(me.editPanelSelector + ' buttongroup'),
                    idx = 0;
                
                //теперь пробегаем по массиву groups и активируем их
                while (true) {
                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            listeners: {
                beforesave: function (asp, rec) {
                    var panel = asp.getPanel(),
                        tfTime = panel.down('textfield[name=Time]'),
                        value = tfTime.getValue(),
                        hour,
                        minute;
                    
                    if (value) {
                        hour = value.getHours();
                        minute = value.getMinutes();
                        
                        if (hour > 23 || minute > 59) {
                            B4.QuickMsg.msg('Предупреждение', 'Введено неверное время', 'warning');
                            return false;
                        }

                        rec.set('Hour', hour);
                        rec.set('Minute', minute);
                    }

                    return true;
                }
            }
            
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actvisualinspectoraspect',
            fieldSelector: '#actvisualpanel gkhtriggerfield[name=Inspectors]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actvisualinspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для выбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    Ext.Array.each(records.items, function(item) {
                        recordIds.push(item.get('Id'));
                    });

                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        B4.QuickMsg.msg('Успешно', 'Инспекторы успешно сохранены', 'success');
                        return true;
                    }).error(function (e) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка', e.message || 'Во время сохранения инспекторов произошла ошибка');
                    });

                    return true;
                }
            }
        }
    ],

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('actVisualEditPanelAspect').setData(me.params.documentId);
        }
    }
});