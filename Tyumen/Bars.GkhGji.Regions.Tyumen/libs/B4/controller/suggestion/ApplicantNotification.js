Ext.define('B4.controller.suggestion.ApplicantNotification', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'suggestion.ApplicantNotification'
    ],

    models: [
        'suggestion.ApplicantNotification'
    ],

    views: [
        'suggestion.applicantnotification.Grid',
        'suggestion.applicantnotification.EditWindow'
    ],

    mainView: 'suggestion.applicantnotification.Grid',
    mainViewSelector: 'applicantnotificationpanel',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'applicantNotificationGridWindowAspect',
            gridSelector: 'applicantnotificationpanel',
            editFormSelector: 'applicantnotificationwindow',
            modelName: 'suggestion.ApplicantNotification',
            editWindowView: 'suggestion.applicantnotification.EditWindow',
            deleteRecord: function (record) {
                var me = this;

                if (!record.getId()) {
                    return;
                }

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result != 'yes') {
                        return;
                    }

                    var model = me.getModel(record);

                    var rec = new model({ Id: record.getId() });
                    me.mask('Удаление', me.controller.getMainComponent());
                    rec.destroy()
                        .next(function () {
                            me.unmask();
                            me.fireEvent('deletesuccess', me);
                            me.updateGrid();
                            
                        }, me)
                        .error(function (result) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        }, me);
                }, me);
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.ApplicantNotification.Create', applyTo: 'b4addbutton', selector: 'applicantnotificationpanel' },
                { name: 'Gkh.Dictionaries.Suggestion.ApplicantNotification.Edit', applyTo: 'b4savebutton', selector: 'applicantnotificationwindow' },
                {
                    name: 'Gkh.Dictionaries.Suggestion.ApplicantNotification.Delete', applyTo: 'b4deletecolumn', selector: 'applicantnotificationpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        }
    ],
    refs: [
        { ref: 'mainView', selector: 'applicantnotificationpanel' }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('applicantnotificationpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('suggestion.ApplicantNotification').load();
    },

    init: function () {
        var me = this;

        me.callParent(arguments);
    }
});