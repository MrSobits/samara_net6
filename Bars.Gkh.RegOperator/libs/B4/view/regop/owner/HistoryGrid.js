Ext.define('B4.view.regop.owner.HistoryGrid', {
    extend: 'B4.view.entityloglight.EntityLogLightGrid',

    requires: [
        'B4.store.EntityLogLight'
    ],

    title: 'История изменений',

    alias: 'widget.paowneraccounthistorygrid',
    
    entityId: null,

    initComponent: function () {
        var me = this;

        me.callParent(arguments);
        me.store.on('beforeload', function (s, operation) {
            if (this.entityId) {
                operation.params = operation.params || {};
                operation.params.parameters = ['room_area', 'room_ownership_type', 'room_num', 'room_chamber_num'];
                operation.params.className = 'BasePersonalAccount',
                operation.params.entityId = me.entityId;
                return true;
            } else {
                return false;
            }
        }, me);
    }
});