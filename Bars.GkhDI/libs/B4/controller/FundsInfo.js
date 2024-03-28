Ext.define('B4.controller.FundsInfo', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models:
    [
        'DisclosureInfo',
        'FundsInfo'
    ],
    stores: ['FundsInfo'],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    views: ['fundsinfo.EditPanel'],

    mainView: 'fundsinfo.EditPanel',
    mainViewSelector: '#fundsInfoEditPanel',

    aspects: [
    {
        xtype: 'gkhstatepermissionaspect',
        editFormAspectName: 'fundsInfoEditPanelAspect',
        permissions: [
            { name: 'GkhDi.Disinfo.FundsInfo.FundsInfoField', applyTo: '#cbFundsInfo', selector: '#fundsInfoEditPanel' },
            { name: 'GkhDi.Disinfo.FundsInfo.Add', applyTo: '#addFundsButton', selector: '#fundsInfoEditPanel' },
            { name: 'GkhDi.Disinfo.FundsInfo.Edit', applyTo: 'b4savebutton', selector: '#fundsInfoEditPanel' }
        ]
    },
    {
        xtype: 'gkheditpanel',
        name: 'fundsInfoEditPanelAspect',
        modelName: 'DisclosureInfo',
        editPanelSelector: '#fundsInfoEditPanel',
        otherActions: function (actions) {
            actions[this.editPanelSelector + ' b4savebutton'] = { 'click': { fn: this.onSaveBtnClick, scope: this} };
            actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this} };
            actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this } };
            actions[this.editPanelSelector + ' b4filefield'] = { 'fileclear': { fn: this.onClearFileTrigger, scope: this } };
            actions[this.editPanelSelector + ' #cbFundsInfo'] = { 'change': { fn: this.setDisableGrid, scope: this } };
        },
        onSaveBtnClick: function () {
            //вызываем стандартное сохранение + сохранение из аспекта грида
            this.saveRequestHandler();
            this.controller.getAspect('fundsInfoInlineGridAspect').save();
        },
        onAddBtnClick: function () {
            this.controller.getAspect('fundsInfoInlineGridAspect').addRecord();
        },
        onUpdateBtnClick: function () {
            this.setData(this.controller.params.disclosureInfoId);
            if (Ext.ComponentQuery.query('#cbFundsInfo')[0].value == 10) {
                this.controller.getAspect('fundsInfoInlineGridAspect').updateGrid();
            }
        },
        onClearFileTrigger: function (fld) {
            // при сохранении панели вызывается метод updateRecord
            // он восстанавливает Id файла и тем самым файл не удаляется с сервера
            // здесь мы сбрасываем начальное состояние fileField'a
            fld.resetOriginalValue();
        },

        setDisableGrid: function (field) {
            var grid = Ext.ComponentQuery.query('#fundsInfoGrid')[0];
            var editPanel = Ext.ComponentQuery.query('#fundsInfoEditPanel')[0];
            var addFundsButton = Ext.ComponentQuery.query('#addFundsButton')[0];

            if (field.getValue() != 10) {
                grid.setDisabled(true);
                grid.getStore().removeAll();
                addFundsButton.hide();
            }
            else {
                grid.setDisabled(false);
                grid.getStore().load();
                addFundsButton.show();
            }
            editPanel.doLayout();

            this.controlFileField(field.getValue());
        },

        afterSetPanelData: function (aspect, rec, panel) {

            panel.setDisabled(false);
            this.controlFileField(rec.get('FundsInfo'));
        },

        controlFileField: function (fundsInfo) {
            var documentWithoutFunds = this.getPanel().down('#ffDocumentWithoutFunds');
            if (fundsInfo == 20) {
                documentWithoutFunds.show();
            } else {
                documentWithoutFunds.hide();
            }
        }
    },
    {
        xtype: 'inlinegridaspect',
        name: 'fundsInfoInlineGridAspect',
        storeName: 'FundsInfo',
        modelName: 'FundsInfo',
        gridSelector: '#fundsInfoGrid',
        listeners: {
            beforesave: function (asp, store) {
                store.each(function (record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                });
                return true;
            }
        }
    }],

    init: function () {
        var actions = {}
        this.getStore('FundsInfo').on('beforeload', this.onBeforeLoad, this);

        actions[this.mainViewSelector] = { 'activate': { fn: this.onMainViewAfterRender, scope: this } };

        this.callParent(arguments);
        this.control(actions);
    },

    onMainViewAfterRender: function () {
        var me = this,
            asp = me.getAspect('fundsInfoEditPanelAspect');

        if (asp) {
            me.mask('Загрузка');
            asp.getModel().load(me.params.disclosureInfoId, {
                success: function(rec) {
                    this.setPanelData(rec);
                    me.unmask();
                },
                scope: asp
            });
        }
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('fundsInfoEditPanelAspect').setData(this.params.disclosureInfoId);
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    }
});