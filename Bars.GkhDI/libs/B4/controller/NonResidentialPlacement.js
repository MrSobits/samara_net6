Ext.define('B4.controller.NonResidentialPlacement', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.nonresidentialplacement.State',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: [
        'DisclosureInfoRealityObj',
        'NonResidentialPlacement',
        'nonresidentialplacement.MeteringDevice'
    ],

    stores: [
        'NonResidentialPlacement',
        'nonresidentialplacement.MeteringDevice'
    ],

    views: [
        'nonresidentialplacement.EditPanel',
        'nonresidentialplacement.EditWindow',
        'nonresidentialplacement.MeteringDeviceEditWindow'
    ],

    editWindowSelector: '#nonResidentialPlacementEditWindow',
    
    mainView: 'nonresidentialplacement.EditPanel',
    mainViewSelector: '#nonResidentialPlacementEditPanel',

    aspects: [
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo.Fields.StartDate', applyTo: '[name=DateStart]', selector: 'nonresidentialplacementeditwindow' },
                { name: 'Gkh.DisclosureInfo.RealityObject.AreaNotLivingPremisesInfo.Fields.EndDate', applyTo: '[name=DateEnd]', selector: 'nonresidentialplacementeditwindow' }
            ]
        },
        {
            xtype: 'nonresidentialplacementstateperm',
            name: 'nonResidentialPlacementPermissionAspect'
        },
        {
            xtype: 'gkheditpanel',
            name: 'nonResidentialPlacementEditPanelAspect',
            modelName: 'DisclosureInfoRealityObj',
            editPanelSelector: '#nonResidentialPlacementEditPanel',

            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfo', asp.controller.params.disclosureInfoId);
                }
            },
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' b4addbutton'] = { 'click': { fn: this.onAddBtnClick, scope: this} };
                actions[this.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: this.onUpdateBtnClick, scope: this} };
                actions[this.editPanelSelector + ' #cbNonResidentialPlacement'] = { 'change': { fn: this.changeNonResidentialPlacement, scope: this } };
            },
            onAddBtnClick: function () {
                this.controller.getAspect('nonResidentialPlacementGridWindowAspect').editRecord();
            },
            onUpdateBtnClick: function () {
                this.setData(this.controller.params.disclosureInfoRealityObjId);
                if (Ext.ComponentQuery.query('#cbNonResidentialPlacement')[0].value == 10) {
                    this.controller.getAspect('nonResidentialPlacementGridWindowAspect').updateGrid();
                }
            },
            
            changeNonResidentialPlacement: function (field, newValue, oldValue) {
                // условие необходимо для срабатывания сохранения только при изменении значения комбобокса непосредственно пользователем
                if ( oldValue != null && newValue != null ) {
                    this.saveRequestHandler();
                }

                this.setDisableGrid(field);
            },

            setDisableGrid: function (field) {
                var grid = Ext.ComponentQuery.query('#nonResidentialPlacementGrid')[0];
                var editPanel = Ext.ComponentQuery.query('#nonResidentialPlacementEditPanel')[0];
                var addNonResidentialPlacementButton = Ext.ComponentQuery.query('#addNonResidentialPlacementButton')[0];

                if (field.getValue() != 10) {
                    grid.setDisabled(true);
                    grid.getStore().removeAll();
                    addNonResidentialPlacementButton.hide();
                }
                else {
                    grid.setDisabled(false);
                    grid.getStore().load();
                    addNonResidentialPlacementButton.show();
                }
                editPanel.doLayout();
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'nonResidentialPlacementGridWindowAspect',
            gridSelector: '#nonResidentialPlacementGrid',
            editFormSelector: '#nonResidentialPlacementEditWindow',
            storeName: 'NonResidentialPlacement',
            modelName: 'NonResidentialPlacement',
            editWindowView: 'nonresidentialplacement.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            listeners: {
                beforesave: function (asp, record) {
                    record.set('DisclosureInfoRealityObj', asp.controller.params.disclosureInfoRealityObjId);
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'nonResidentialPlacementMeteringDeviceGridWindowAspect',
            gridSelector: '#nonResidentialPlacementMeteringDeviceGrid',
            editFormSelector: '#nonResidentialPlacementMeteringDeviceEditWindow',
            storeName: 'nonresidentialplacement.MeteringDevice',
            modelName: 'nonresidentialplacement.MeteringDevice',
            editWindowView: 'nonresidentialplacement.MeteringDeviceEditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    record.set('NonResidentialPlacement', asp.controller.params.nonResidentialPlacementId);
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.getStore('NonResidentialPlacement').on('beforeload', this.onBeforeLoad, this, 'NonResidentialPlacement');
        this.getStore('nonresidentialplacement.MeteringDevice').on('beforeload', this.onBeforeLoad, this, 'NonResidentialPlacementMeteringDevice');

        this.callParent(arguments);
    },

    onLaunch: function () {
        
        if (this.params) {
            
            var aspect = this.getAspect('nonResidentialPlacementEditPanelAspect');
            var combo = aspect.getPanel().down("#cbNonResidentialPlacement");
            
            // обнуляем значение комбобокса. необходимо, если при открытом окне редактирования жилого дома открывается на редактирование ещё один дом.
            combo.setValue(null);
            
            aspect.setData(this.params.disclosureInfoRealityObjId);

            var me = this;
            me.params.getId = function () { return me.params.disclosureInfoId; };
            this.getAspect('nonResidentialPlacementPermissionAspect').setPermissionsByRecord(this.params);
        }
    },

    onBeforeLoad: function (store, operation, type) {
        if (this.params) {
            if (type == 'NonResidentialPlacement') {
                operation.params.disclosureInfoRealityObjId = this.params.disclosureInfoRealityObjId;
            }
            if (type == 'NonResidentialPlacementMeteringDevice') {
                operation.params.nonResidentialPlacementId = this.params.nonResidentialPlacementId;
            }
        }
    },

    setCurrentId: function (record) {
        this.params.nonResidentialPlacementId = record.getId();
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];

        editWindow.down('.tabpanel').setActiveTab(0);

        var store = this.getStore('nonresidentialplacement.MeteringDevice');
        store.removeAll();

        if (record.getId() > 0) {
            editWindow.down('#nonResidentialPlacementMeteringDeviceGrid').setDisabled(false);
            store.load();
        } else {
            editWindow.down('#nonResidentialPlacementMeteringDeviceGrid').setDisabled(true);
        }
    }
});
