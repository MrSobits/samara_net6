Ext.define('B4.controller.InfoAboutReductionPayment', {
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.infoaboutreductionpayment.State',
        
        'B4.enums.KindServiceDi'
    ],

    models:
    [
        'DisclosureInfoRealityObj',
        'InfoAboutReductionPayment'
    ],
    stores:
    [
        'InfoAboutReductionPayment',
        'service.ServiceSelect',
        'service.ServiceSelected'
    ],
    
    views:
    [
        'infoaboutreductionpayment.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],
    
    mainView: 'infoaboutreductionpayment.EditPanel',
    mainViewSelector: '#infoAboutReductionPaymentEditPanel',

    aspects: [
    {
        xtype: 'infoaboutreductionpaymentstateperm',
        name: 'infoAboutReductionPaymentPermissionAspect'
    },
    {
        xtype: 'gkheditpanel',
        name: 'infoAboutReductionPaymentEditPanelAspect',
        modelName: 'DisclosureInfoRealityObj',
        editPanelSelector: '#infoAboutReductionPaymentEditPanel',

        listeners: {
            beforesave: function (asp, record) {
                record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
            }
        },
        otherActions: function (actions) {
            actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this } };
            actions[this.editPanelSelector + ' #infoAboutReductionPaymentSaveButton'] = { 'click': { fn: this.onSaveBtnClick, scope: this } };
            actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this } };
            actions[this.editPanelSelector + ' #cbReductionPayment'] = { 'change': { fn: this.changeReductionPayment, scope: this } };
        },
        onAddBtnClick: function () {
            this.controller.getAspect('infoAboutReductionPaymentGridAspect').gridAction(null, 'add');
        },
        onSaveBtnClick: function () {
            this.saveRequestHandler();
            this.controller.getAspect('infoAboutReductionPaymentGridAspect').gridAction(null, 'save');
        },
        onUpdateBtnClick: function () {
            this.setData(this.controller.params.disclosureInfoRealityObjId);
            if (Ext.ComponentQuery.query('#cbReductionPayment')[0].value == 10) {
                this.controller.getAspect('infoAboutReductionPaymentGridAspect').updateGrid();
            }
        },

        changeReductionPayment: function (field, newValue, oldValue) {
            //При первом заходе не сохраняем
            if (oldValue !== undefined) {
                this.saveRequestHandler();
            }

            this.setDisableGrid(field);
        },

        setDisableGrid: function (field) {
            var grid = Ext.ComponentQuery.query('#infoAboutReductionPaymentGrid')[0];
            var editPanel = Ext.ComponentQuery.query('#infoAboutReductionPaymentEditPanel')[0];
            var addInfoAboutReductionPaymentButton = Ext.ComponentQuery.query('#addInfoAboutReductionPaymentButton')[0];

            if (field.getValue() != 10) {
                grid.setDisabled(true);
                grid.getStore().removeAll();
                addInfoAboutReductionPaymentButton.hide();
            }
            else {
                grid.setDisabled(false);
                grid.getStore().load();
                addInfoAboutReductionPaymentButton.show();
            }
            editPanel.doLayout();
        }
    },
    {
        xtype: 'gkhinlinegridmultiselectwindowaspect',
        name: 'infoAboutReductionPaymentGridAspect',
        gridSelector: '#infoAboutReductionPaymentGrid',
        saveButtonSelector: '#infoAboutReductionPaymentGrid #infoAboutReductionPaymentSaveButton',
        storeName: 'InfoAboutReductionPayment',
        modelName: 'InfoAboutReductionPayment',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#infoAboutReductionPaymentMultiSelectWindow',
        storeSelect: 'service.ServiceSelect',
        storeSelected: 'service.ServiceSelect',
        titleSelectWindow: 'Выбор услуг',
        titleGridSelect: 'Услуги',
        titleGridSelected: 'Выбранные услуги',
        onBeforeLoad: function (store, operation) {
            operation.params.disclosureInfoRealityObjId = this.controller.params.disclosureInfoRealityObjId;
        },
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
            { header: 'Вид', xtype: 'gridcolumn', dataIndex: 'KindServiceDi', flex: 1, renderer: function (val) { return B4.enums.KindServiceDi.displayRenderer(val); } }
        ],
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
        ],

        listeners: {
            //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида без сохранения                
            getdata: function (asp, records) {

                var repairListIds = [];

                var st = asp.controller.getStore(asp.storeName);
                st.each(function (rec) {
                    repairListIds.push(rec.get('TemplateService'));
                });

                records.each(function (rec) {
                    if (Ext.Array.indexOf(repairListIds, rec.get('Id')) == -1) {
                        //создаем рекорд модели InfoAboutReductionPayment
                        var recordInfoAboutReductionPayment = asp.controller.getModel(asp.modelName).create();
                        recordInfoAboutReductionPayment.set('BaseService', rec.get('Id'));
                        recordInfoAboutReductionPayment.set('BaseServiceName', rec.get('Name'));
                        recordInfoAboutReductionPayment.set('BaseServiceKindServiceDi', rec.get('KindServiceDi'));
                        recordInfoAboutReductionPayment.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);

                        st.insert(0, recordInfoAboutReductionPayment);
                    }
                });
                return true;
            }
        }
    },
    {
        xtype: 'gkhpermissionaspect',
        permissions: [
               { name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Field.OrderNum_View', applyTo: '#dcOrderNum', selector: '#infoAboutReductionPaymentGrid' },
               { name: 'GkhDi.DisinfoRealObj.InfoAboutReductionPayment.Field.OrderDate_View', applyTo: '#dcOrderDate', selector: '#infoAboutReductionPaymentGrid' }
        ],
        applyBy: function(component, allowed) {
            if (allowed) {
                component.show();
            } else {
                component.hide();
            }
        }
    }
    ],
    
    init: function () {
        this.getStore('InfoAboutReductionPayment').on('beforeload', this.onBeforeLoad, this);
        
        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('infoAboutReductionPaymentEditPanelAspect').setData(this.params.disclosureInfoRealityObjId);
        }
        
        if (this.params) {
            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('infoAboutReductionPaymentPermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation) {
        operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
    }
});
