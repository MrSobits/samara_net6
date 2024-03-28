Ext.define('B4.view.persaccbenefits.HistoryGrid', {
    extend: 'B4.view.entityloglight.EntityLogLightGrid',

    requires: [
        'B4.store.EntityLogLight'
    ],

    title: 'История изменений',

    alias: 'widget.persaccbenefitshistorygrid',
    
    entityId: null,

    initComponent: function () {
        var me = this;

        me.callParent(arguments);
        me.store.on('beforeload', function (s, operation) {
            if (this.entityId) {
                operation.params = operation.params || {};
                operation.params.parameters = ['pa_benefits_sum'];
                operation.params.className = 'PersonalAccountBenefits',
                operation.params.entityId = me.entityId;
                return true;
            } else {
                return false;
            }
        }, me);
    }
});