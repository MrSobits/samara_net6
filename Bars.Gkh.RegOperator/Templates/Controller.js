Ext.define('B4.controller.regop.personal_account.PersonalAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.view.regop.personal_account.PersonalAccountGrid'
    ],

    stores: ['regop.personal_account.PersonalAccount'],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: ''
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('');
        this.bindContext(view);
        this.application.deployView(view);
    }
});

