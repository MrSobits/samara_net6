Ext.define('B4.view.wizard.WizardFinishStepFrame', {
    extend: 'B4.view.wizard.WizardBaseStepFrame',
    stepId: 'finish',
    title: 'Результаты работы мастера',
    wizard: undefined,

    layout: 'border',

    defaults: {
        bodyStyle: 'padding:15px',
        margins: '5 5 5 5'
    },

    items: [
        {
            itemId: 'finishStepImage',
            region: 'west',
            width: 150,
            baseCls: 'icon_apply'
        },
        {
            region: 'center',
            layout: 'border',
            defaults: {
                bodyStyle: 'padding:15px',
                margins: '5 5 5 5'
            },
            items: [
                {
                    region: 'center',
                    html: '',
                    itemId: 'finishDescription'
                }
            ]
        }
    ],

    init: function () {
        var me = this;
        me.description = 'Работа мастера была завершена.' + '<br><br>';

        var finishStepImageEl = me.wizard.down('#finishStepImage');
        var finishDescriptionEl = me.wizard.down('#finishDescription');

        if (me.wizard.result) {

            me.description += me.wizard.result.message;

            if (me.wizard.result.state === 'success') {
                finishStepImageEl.removeCls('icon_error');
                finishStepImageEl.removeCls('icon_warning');
                finishStepImageEl.addCls('icon_apply');
            }
            else if (me.wizard.result.state === 'warning') {
                finishStepImageEl.removeCls('icon_apply');
                finishStepImageEl.removeCls('icon_error');
                finishStepImageEl.addCls('icon_warning');
            }
            else {
                finishStepImageEl.removeCls('icon_apply');
                finishStepImageEl.removeCls('icon_warning');
                finishStepImageEl.addCls('icon_error');
            }

            finishDescriptionEl.update(me.description);
        }

        return true;
    },

    allowBackward: function () {
        return false;
    },

    allowForward: function () {
        return false;
    }
});