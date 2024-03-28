Ext.define('B4.view.wizard.export.votingProtocol.ExportVotingProtocolaWizard', {
    extend: 'B4.view.wizard.preparedata.Wizard',

    getParametersStepFrames: function () {
        return [Ext.create('B4.view.wizard.export.votingProtocol.VotingProtocolParametersStepFrame', { wizard: this })];
    }
});