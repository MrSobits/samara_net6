Ext.define('B4.controller.ConfirmContribution', {
    confContribId: null,
    confContribRecordId: null,
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.ConfirmContribution',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport'
    ],

    models: [
        'ConfirmContribution',
        'confirmcontribution.Record',
        'confirmcontribution.RealityObject'
    ],

    stores: [
        'ConfirmContribution',
        'confirmcontribution.Record',
        'confirmcontribution.RealityObject'
    ],

    views: [
        'confirmcontribution.EditWindow',
        'confirmcontribution.RecordEditWindow',
        'confirmcontribution.Grid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'confirmcontribution.Grid',
    mainViewSelector: 'confirmContribGrid',

    editWindowSelector: 'confContribRecordEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'confirmContribGrid'
        }
    ],

    aspects: [
        {
            /*
            * Аспект взаимодействия основного грида и формы редактирования "Поступление взносов на КР"
            */
            xtype: 'grideditwindowaspect',
            name: 'confContribGridEditWindowAspect',
            gridSelector: 'confirmContribGrid',
            editFormSelector: 'confContribEditWindow',
            storeName: 'ConfirmContribution',
            modelName: 'ConfirmContribution',
            editWindowView: 'confirmcontribution.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record);
                }
            }
        },
        {
            /*
            * Аспект взаимодействия грида "Сведения о поступлении взносов" и его формы редактирования записи
            */
            xtype: 'grideditwindowaspect',
            name: 'confContribRecordGridEditWindowAspect',
            gridSelector: 'confirmContribRecordGrid',
            editFormSelector: 'confContribRecordEditWindow',
            storeName: 'confirmcontribution.Record',
            modelName: 'confirmcontribution.Record',
            editWindowView: 'confirmcontribution.RecordEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                beforesave: function (asp, rec) {
                    var id = rec.getId();
                    if (id == 0) {
                        rec.set('ConfirmContribution', asp.controller.getConfContribId());
                    }
                }
            }
        },
        {
            /*
            * Аспект экспорта в excel основного грида
            */
            xtype: 'b4buttondataexportaspect',
            name: 'ConfirmContribButtonExportAspect',
            gridSelector: 'confirmContribGrid',
            buttonSelector: 'confirmContribGrid #btnconfirmContribExport',
            controllerName: 'ConfirmContribution',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'confContribRecordEditWindow b4selectfield[name=RealityObject]': {
                beforeload: { fn: me.onBeforeLoad, scope: me }
            }
        });

        me.getStore('confirmcontribution.Record').on('beforeload', me.onBeforeLoad, me);

        // Устанавливаем 1-й день месяца как default значение для формата даты M Y
        Ext.Date.defaults.d = 1;
        
        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('confirmContribGrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ConfirmContribution').load();
    },

    getConfContribId: function() {
        return this.getCmpInContext('confContribEditWindow').getForm().getRecord().getId();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            recId = me.getConfContribId();

        if (recId) {
            operation.params.confContribId = recId;
        }
    },

    setCurrentId: function (record) {
        var editWindow,
            store,
            gridSelector,
            id,
            manOrgSelect,
            me = this;
        me.confContribId = id = record.getId();
        editWindow = Ext.ComponentQuery.query('confContribEditWindow')[0];
        store = me.getStore('confirmcontribution.Record');
        manOrgSelect = editWindow.down('b4selectfield');
        gridSelector = 'confirmContribRecordGrid';

        if (id > 0) {
            editWindow.down(gridSelector).setDisabled(false);
            manOrgSelect.setReadOnly(true);
        } else {
            editWindow.down(gridSelector).setDisabled(true);
            manOrgSelect.setReadOnly(false);
        }
        store.load();
    }
});