Ext.define('B4.aspects.FormPanel', {
    extend: 'B4.base.Aspect',

    alias: 'widget.formpanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    requires: ['B4.QuickMsg'],

    formPanelSelector: null,

    modelName: null,

    controller: null,

    objectId: 0,

    init: function (controller) {
        var me = this, actions = {};

        actions[me.formPanelSelector] = {
            'render': {
                fn: me.loadRecord,
                scope: me
            }
        };

        controller.control(actions);

        me.callParent(arguments);
    },

    getFormPanel: function () {
        return this.componentQuery(this.formPanelSelector);
    },

    loadRecord: function() {
        var me = this,
            model = me.controller.getModel(me.modelName), objId;

        if (!me.onBeforeLoadRecord(me)) {
            return;
        };

        if (Ext.isFunction(me.objectId)) {
            objId = me.objectId();
        } else {
            objId = me.objectId;
        }

        model.load(objId, {
            scope: me,
            failure: function (rec, operation) {
                me.onLoadRecordFailure(me, operation.error);
            },
            success: function (rec) {
                if (rec) {
                    me.getFormPanel().loadRecord(rec);
                    me.afterLoadRecord(me, rec);
                }
            }
        });

    },

    onBeforeLoadRecord: function(asp) {
        return true;
    },
    
    afterLoadRecord: function() {
    },

    onLoadRecordFailure: function(asp, message) {
    }
});