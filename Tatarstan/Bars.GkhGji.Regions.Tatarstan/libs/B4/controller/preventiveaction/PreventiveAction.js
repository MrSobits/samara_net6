Ext.define('B4.controller.preventiveaction.PreventiveAction', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.PreventiveAction',
        'B4.aspects.ButtonDataExport',
        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType'
    ],

    views: [
        'preventiveaction.PreventiveActionGrid',
        'preventiveaction.AddWindow'
    ],
    
    models: [
        'preventiveaction.PreventiveAction'
    ],
    
    stores: [
        'preventiveaction.PreventiveAction'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'preventiveaction.PreventiveActionGrid',
    mainViewSelector: 'preventiveactiongrid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'preventiveActionGridButtonExportAspect',
            gridSelector: 'preventiveactiongrid',
            buttonSelector: 'preventiveactiongrid #btnExport',
            controllerName: 'PreventiveAction',
            actionName: 'Export'
        },
        {
            xtype: 'preventiveactionpermissions',
            name: 'preventiveActionPermissionAspect'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'preventiveActionAspect',
            gridSelector: 'preventiveactiongrid',
            editFormSelector: '#preventiveactionaddwindow',
            storeName: 'preventiveaction.PreventiveAction',
            modelName: 'preventiveaction.PreventiveAction',
            editWindowView: 'B4.view.preventiveaction.AddWindow',
            controllerEditName: 'B4.controller.preventiveaction.Navigation',
            // Переопределяем для нужного сообщения при удалении  
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить мероприятие со всеми сформированными документами?', function (result) {
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
            updateGrid: function () {
                this.controller.updateGrid();
            },
        }
    ],

    init: function(){
        var me = this;

        me.control({
            'preventiveactiongrid  [name=Plan], preventiveactiongrid [name=ActionType], preventiveactiongrid [name=ControlledOrganization], preventiveactiongrid [name=ShowClosed]': {
                change: me.onSelectFieldChanged
            },
            'preventiveactiongrid  [name=PeriodStart], preventiveactiongrid  [name=PeriodEnd]': {
                change: me.onPeriodLimitChange
            },
            '#preventiveactionaddwindow [name=ActionType]': {
                change: me.onAddWindowActionTypeChange
            },
            'preventiveactiongrid b4updatebutton': {
                'click': { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },
    
    index: function(){
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector),
            store = view.getStore();

        me.bindContext(view);
        me.application.deployView(view);
        
        store.load();
    },

    onSelectFieldChanged: function(cmp, newValue, oldValue){
        var me = this,
            store = me.getMainView().getStore();
        
        if(newValue === oldValue){
            return;
        }
        
        me.applyStoreParam(cmp.name, newValue?.Id ?? newValue);
        
        store.load();
    },

    updateGrid: function () {
        this.getMainView().getStore().load();
    },
    
    applyStoreParam: function(paramName, value){
        var me = this,
            store = me.getMainView().getStore();
        
        store.getProxy().setExtraParam(paramName, value);
    },

    onPeriodLimitChange: function (field, newValue, oldValue)
    {
        if(!newValue){
            return;
        }

        var me = this,
            secondPeriodLimitFieldName = field.name === 'PeriodEnd' ? 'PeriodStart' : 'PeriodEnd',
            secondPeriodLimitField = field.up().down('[name=' + secondPeriodLimitFieldName + ']'),
            periodLimitValues = [],
            store = me.getMainView().getStore();

        periodLimitValues[secondPeriodLimitFieldName] = secondPeriodLimitField.getValue();
        periodLimitValues[field.name] = newValue;

        if(periodLimitValues[secondPeriodLimitFieldName]){
            if(periodLimitValues['PeriodEnd'] < periodLimitValues['PeriodStart']){
                Ext.Msg.alert('Некорректный период!', 'Левая граница периода не может быть больше правой границы');
                field.setValue(oldValue);
                return;
            }
        }
        
        me.applyStoreParam(field.name, newValue);
        store.load();
    },

    onAddWindowActionTypeChange: function(cmp, newValue){
        var visitTypeCombo = cmp.up('panel').down('[name=VisitType]');
        
        if(newValue === B4.enums.PreventiveActionType.Visit){
            visitTypeCombo.show();
            visitTypeCombo.setDisabled(false);
        } else {
            visitTypeCombo.hide();
            visitTypeCombo.setDisabled(true);
        }
    }
});