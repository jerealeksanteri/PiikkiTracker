function ShowDeletionModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('deletionModal')).show();
}

function HideDeletionModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('deletionModal')).hide();
}