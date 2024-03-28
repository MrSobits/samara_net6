Ext.define('B4.controller.emergencyobj.Documents', {
    /* 
    * Контроллер формы редактирования Документов аварийного дома
    */
    extend: 'B4.base.Controller',
    views: ['emergencyobj.DocumentsPanel'],

    params: null,
    requires: [
        'B4.aspects.GkhEditPanel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    models: ['emergencyobj.Documents'],

    mainView: 'emergencyobj.DocumentsPanel',
    mainViewSelector: '#emergencyObjDocumentsPanel',

    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'emergencyObjDocumentsPanelAspect',
            editPanelSelector: '#emergencyObjDocumentsPanel',
            modelName: 'emergencyobj.Documents',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.EmergencyObject = this.controller.params.getId();
                    }
                }
            }
        }
    ],

    onLaunch: function () {
        var me = this,
            aspect = me.getAspect('emergencyObjDocumentsPanelAspect');

        me.mask('Загрузка', me.getMainComponent());
        B4.Ajax.request(B4.Url.action('GetDocumentsIdByEmergencyObject', 'EmergencyObjectDocuments', {
                emergencyObjectId: me.params.get('Id')
            })).next(function(response) {
                var obj = Ext.JSON.decode(response.responseText);
                aspect.setData(obj.documentsId);
                me.unmask();
                return true;
            }, me)
            .error(function() {
                Ext.Msg.alert('Сообщение', 'Произошла ошибка');
                me.unmask();
            }, me);
    }
});