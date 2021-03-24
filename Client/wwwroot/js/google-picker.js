// Scope to use to access user's Drive items.
const scope = ['https://www.googleapis.com/auth/drive.file'];

let pickerApiLoaded = false;
let oauthToken = null;

const clientId = "819781231719-kmlgq9anqg49a2dltgsr2ld0t5e62p6b.apps.googleusercontent.com";
const appId = "819781231719";
const developerKey = "AIzaSyCc-MEb_bdMG0_8MmHMO5EDI_H5R0vDPEs";

function initPicker() {};

function loadPicker () {
    gapi.load('auth', { 'callback': onAuthApiLoad });
    gapi.load('picker', { 'callback': onPickerApiLoad });
};

function onAuthApiLoad () {
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

function onPickerApiLoad() {
    pickerApiLoaded = true;
    createPicker();
};

// Create and render a Picker object for searching images.
function createPicker() {
    if (pickerApiLoaded && oauthToken) {
        var picker = new google.picker.PickerBuilder()
            .enableFeature(google.picker.Feature.NAV_HIDDEN)
            .enableFeature(google.picker.Feature.MULTISELECT_ENABLED)
            .setAppId(appId)
            .setOAuthToken(oauthToken)
            .addViewGroup(
                new google.picker.ViewGroup(google.picker.ViewId.DOCS)
                    .addView(google.picker.ViewId.DOCUMENTS)
                    .addView(google.picker.ViewId.DOCS_IMAGES)
                    .addView(google.picker.ViewId.PDFS)
                    .addView(google.picker.ViewId.PRESENTATIONS)
                    .addView(google.picker.ViewId.SPREADSHEETS)
                    .addView(new google.picker.DocsUploadView())
            )
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
        DotNet.invokeMethod("StoryForce.Client", "AddToFileList", docs[i].name, docs[i].id, docs[i].sizeBytes, oauthToken);
    }
};
