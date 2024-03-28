Ext.define('B4.controller.StateTransferRule', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.GridEditWindow'],

    models: ['StateTransferRule'],
    stores: ['StateTransferRule'],
    views: [
        'StateTransfer.RuleGrid',
        'StateTransfer.RuleEditWindow'
    ],

    mainView: 'StateTransfer.RuleGrid',
    mainViewSelector: '#stateTransferRuleGrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'stateTransferRuleGridWindowAspect',
            gridSelector: '#stateTransferRuleGrid',
            editFormSelector: '#stateTransferRuleEditWindow',
            storeName: 'StateTransferRule',
            modelName: 'StateTransferRule',
            editWindowView: 'StateTransfer.RuleEditWindow',
            otherActions: function(actions) {

                actions[this.editFormSelector + ' #stateTransferObject'] = {
                    'change': { fn: this.stateTransferObjectChange, scope: this }
                };
                actions[this.editFormSelector + ' #stateTransferRule'] = {
                    'beforeload': { fn: this.stateTransferRuleBeforeLoad, scope: this }
                };
                actions[this.editFormSelector + ' #stateTransfer'] = {
                    'beforeload': { fn: this.stateTransferBeforeLoad, scope: this },
                    'change': { fn: this.stateTransferChange, scope: this }
                };
                actions[this.editFormSelector + ' #stateTransferRole'] = {
                    'change': { fn: this.stateTransferRoleChange, scope: this }
                };
            },
            getRecordBeforeSave: function(record) {
                return record;
            },

            stateTransferRuleBeforeLoad: function(field, operation) {
                var field = this.getForm().down('#stateTransfer');
                var ruleField = this.getForm().down('#stateTransferRule');
                var value = field.getValue();
                if (value) {
                    operation.params = operation.params || {};
                    operation.params.stateTransferId = value;
                    ruleField.setDisabled(false);
                }
            },

            stateTransferChange: function (field, operation) {
                var field = this.getForm().down('#stateTransfer');
                var ruleField = this.getForm().down('#stateTransferRule');
                var value = field.getValue();
                if (value) {
                    operation.params = operation.params || {};
                    operation.params.stateTransferId = value;
                    ruleField.setDisabled(false);
                }
            },

            stateTransferBeforeLoad: function(field, operation) {
                var objectField = this.getForm().down('#stateTransferObject');
                var roleField = this.getForm().down('#stateTransferRole');
                var transferField = this.getForm().down('#stateTransfer')
                var objectValue = objectField.getValue();
                var roleValue = roleField.getValue();
                if (objectValue || roleValue) {
                    operation.params = operation.params || {};
                    operation.params.statefulEntityId = objectValue;
                    operation.params.roleId = roleValue;
                }
                if (objectValue && roleValue) {
                    transferField.setDisabled(false);
                }

            },

            stateTransferObjectChange: function(field, newValue, oldValue) {
                var stateTransferRuleField = this.getForm().down('#stateTransferRule');
                var stateTransferField = this.getForm().down('#stateTransfer');
                var roleField = this.getForm().down('#stateTransferRole');
                var objectField = this.getForm().down('#stateTransferObject');
                var objectValue = objectField.getValue();
                var roleValue = roleField.getValue();
                if (oldValue) {
                    stateTransferRuleField.setValue(null);
                    stateTransferField.setValue(null);
                }
                if (objectValue && roleValue) {
                    stateTransferField.setDisabled(false);
                }
            },

            stateTransferRoleChange: function(field, newValue, oldValue) {
                var stateTransferField = this.getForm().down('#stateTransfer');
                var roleField = this.getForm().down('#stateTransferRole');
                var objectField = this.getForm().down('#stateTransferObject');
                var objectValue = objectField.getValue();
                var roleValue = roleField.getValue();

                if (oldValue) {
                    stateTransferField.setValue(null);
                }
                if (objectValue && roleValue) {
                    stateTransferField.setDisabled(false);
                }
            },
        }
    ]
});