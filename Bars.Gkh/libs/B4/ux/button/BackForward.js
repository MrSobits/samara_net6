Ext.define('B4.ux.button.BackForward', {
    extend: 'Ext.container.ButtonGroup',

    alias: 'widget.backforwardbutton',

    items: [{
        direction: 'back',
        icon: B4.Url.content('content/img/icons/arrow_left.png')
    }, {
        direction: 'forward',
        icon: B4.Url.content('content/img/icons/arrow_right.png')
    }]
});