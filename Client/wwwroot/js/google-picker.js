// Scope to use to access user's Drive items.
const scope = ['https://www.googleapis.com/auth/drive.file'];

let pickerApiLoaded = false;
let oauthToken = null;

function initPicker() {};

function loadPicker (clientId, appId, developerKey) {
    gapi.load('auth', { 'callback': onAuthApiLoad(clientId) });
    gapi.load('picker', { 'callback': onPickerApiLoad(appId, developerKey) });
};

function onAuthApiLoad (clientId) {
    window.gapi.auth.authorize(
        {
            'client_id': clientId,
            'scope': scope,
            'immediate': false
        },
        handleAuthResult);
}

function handleAuthResult(authResult) {
    if (authResult && !authResult.error) {
        oauthToken = authResult.access_token;
        if (oauthToken) {
            const accessToken = document.getElementById("Submission_GDriveOAuthToken");
            accessToken.value = oauthToken;
        }
        createPicker();
    }
};

function onPickerApiLoad(appId, developerKey) {
    pickerApiLoaded = true;
    createPicker(appId, developerKey);
};

// Create and render a Picker object for searching images.
function createPicker(appId, developerKey) {
    if (pickerApiLoaded && oauthToken) {
        var view = new google.picker.View(google.picker.ViewId.DOCS);
        view.setMimeTypes("image/png,image/jpeg,image/jpg");
        var picker = new google.picker.PickerBuilder()
            .enableFeature(google.picker.Feature.NAV_HIDDEN)
            .enableFeature(google.picker.Feature.MULTISELECT_ENABLED)
            .setAppId(appId)
            .setOAuthToken(oauthToken)
            .addView(view)
            .addView(new google.picker.DocsUploadView())
            .setDeveloperKey(developerKey)
            .setCallback(pickerCallback)
            .build();
        picker.setVisible(true);
    }
};

// A simple callback implementation.
function pickerCallback(data) {
    if (data.action === google.picker.Action.PICKED) {
        //var fileId = data.docs[0].id;
        handleFiles(data.docs);
    }
};

function handleFiles(docs) {
    for (let i = 0; i < docs.length; i++) {
        //call dot net method to add file
        DotNet.invokeMethod("StoryForce.Client", "AddToFileList", docs[i].name, docs[i].id, oauthToken);
    }
};
