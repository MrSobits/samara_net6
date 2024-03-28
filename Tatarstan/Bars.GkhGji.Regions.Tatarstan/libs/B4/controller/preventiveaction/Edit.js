Ext.define('B4.controller.preventiveaction.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        
        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',
        'B4.enums.YesNo',
        
        'B4.Ajax',
        'B4.Url'
    ],
    
    models: [
        'preventiveaction.PreventiveAction'
    ],
    
    stores: [
        'preventiveaction.PreventiveAction',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],
    
    views: [
        'preventiveaction.EditPanel'
    ],
    
    mainView: 'preventiveaction.EditPanel',
    mainViewSelector: '#preventiveActionEditPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects: [
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'preventiveactionCreateButtonAspect',
            buttonSelector: '#preventiveActionEditPanel gjidocumentcreatebutton',
            containerSelector: '#preventiveActionEditPanel',
            typeDocument: 190
        },
        {
            xtype: 'statebuttonaspect',
            name: 'preventiveactionStateButtonAspect',
            stateButtonSelector: '#preventiveActionEditPanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    asp.controller.getAspect('preventiveActionEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#preventiveActionNavigationPanel').getComponent('preventiveActionMenuTree').getStore().load();
                }
            }
        },
        {
            xtype: 'gjidocumentaspect',
            name: 'preventiveActionEditPanelAspect',
            editPanelSelector: '#preventiveActionEditPanel',
            modelName: 'preventiveaction.PreventiveAction',
            onSaveSuccess: function (asp, rec) {
                //исключение изменения заголовка панели
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                var visitType = rec.get('VisitType'),
                    sentToErknmComboBox = panel.down('[name=SentToErknm]'),
                    erknmContainer = panel.down('#erknmContainer'),
                    erknmId = rec.get('ErknmId');
                
                if(visitType === B4.enums.PreventiveActionVisitType.Preventive){
                    erknmContainer.show();
                    sentToErknmComboBox.show();
                } else {
                    erknmContainer.hide();
                    sentToErknmComboBox.hide();
                }
                
                this.disableButtons(false);
                this.updateFieldValue(panel, rec);

                this.controller.getAspect('preventiveactionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                this.controller.getAspect('preventiveactionCreateButtonAspect').setData(rec.get('Id'));
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            updateFieldValue: function (panel, rec) {
                var fieldInspectors = panel.down('#trigfInspectors');

                fieldInspectors.updateDisplayedText(rec.data.Inspectors);
                fieldInspectors.setValue(rec.data.InspectorIds);
            },
            btnDeleteClick: function() {
                var me = this,
                    panel = this.getPanel(),
                    record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить мероприятие со всеми дочерними документами?', function(result) {
                    if (result == 'yes') {
                        this.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function() {
                                //Обновляем дерево меню
                                var tree = me.getTreePanel();
                                tree.getStore().load();

                                Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                                panel.close();
                                this.unmask();
                            }, this)
                            .error(function(result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                this.unmask();
                            }, this);

                    }
                }, this);
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'preventiveActionInspectorMultiSelectWindowAspect',
            fieldSelector: '#preventiveActionEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#preventiveActionInspectorsSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('AddInspectors', 'DocumentGjiInspector'),
                        method: 'POST',
                        params: {
                            inspectorIds: (recordIds),
                            documentId: asp.controller.params.documentId
                        }
                    }).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                    }).error(function () {
                        asp.controller.unmask();
                        return false;
                    });
                }
            }
        }
    ],
    
    init: function () {
        var me = this;
        
        me.control({
            '#preventiveActionEditPanel [name=ActionType]': {
               change: me.onActionTypeChange
           }
        });
        
        me.callParent(arguments);
    },
    
    onLaunch: function () {
        var me = this;
        if (me.params) {
            me.getAspect('preventiveActionEditPanelAspect').setData(me.params.documentId);
        }
    },

    onActionTypeChange: function(cmp, newValue){
        var visitTypeCombo = cmp.up('panel').down('[name=VisitType]');
        
        if(newValue === B4.enums.PreventiveActionType.Visit){
            visitTypeCombo.show();
        } else {
            visitTypeCombo.hide();
        }
    }
});