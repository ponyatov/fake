const vscode = require('vscode');

function hello() {  //
    vscode.window.showInformationMessage('flang/hello');
}

async function activate(context) {
    vscode.window.showInformationMessage('flang/activate');
    // context.subscriptions.push(
    //     vscode.commands.registerCommand('dponyatov.flang.hello', hello));
}

function deactivate() {
    vscode.window.showInformationMessage('flang/deactivate');
}

module.exports = {
    activate,
    deactivate,
    hello
}
