﻿function ShowDeletionModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('deletionModal')).show();
}

function HideDeletionModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('deletionModal')).hide();
}

function ShowAcceptModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('acceptionModal')).show();
}

function HideAcceptModal() {
    bootstrap.Modal.getOrCreateInstance(document.getElementById('acceptionModal')).hide();
}