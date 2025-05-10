const vscode = require('vscode');

function hello() {
    vscode.window.showInformationMessage('F/hello');
}

async function activate(context) {
    vscode.window.showInformationMessage('F/activate');
}

function deactivate() {
    vscode.window.showInformationMessage('F/deactivate');
}

module.exports = {
    activate,
    deactivate,
    hello,
};
