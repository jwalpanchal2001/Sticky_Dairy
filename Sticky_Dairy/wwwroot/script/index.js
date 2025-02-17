


$(document).ready(function () {
    $('#Password, #confirm_Password').on('input', function () {
        let password = $('#Password').val();
        let confirmPassword = $('#confirm_Password').val();
        let errorMessageDiv = $('#error-message');

        if (password !== confirmPassword) {
            errorMessageDiv.text("Passwords do not match. Please try again.");
        } else {
            errorMessageDiv.text(""); // Clear the error message if passwords match
        }
    });

    $('#EmailAddress').on('input', function () {
        let email = $(this).val();
        let emailErrorDiv = $('#email-error');
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailPattern.test(email)) {
            emailErrorDiv.text("Please enter a valid email address.");
        } else {
            emailErrorDiv.text(""); // Clear the error message if email is valid
        }
    });

    $("#submitbtn").click(function (event) {
        event.preventDefault(); // Prevent the form from submitting

        let password = $('#Password').val();
        let confirmPassword = $('#confirm_Password').val();
        let errorMessageDiv = $('#password-error');
        let email = $('#EmailAddress').val();
        let emailErrorDiv = $('#email-error');
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (password !== confirmPassword) {
            errorMessageDiv.text("Passwords do not match. Please try again.");
        } else {
            errorMessageDiv.text(""); // Clear the error message if passwords match
        }

        if (!emailPattern.test(email)) {
            emailErrorDiv.text("Please enter a valid email address.");
        } else {
            emailErrorDiv.text(""); // Clear the error message if email is valid
        }

        if (password === confirmPassword && emailPattern.test(email)) {
            // If passwords match and email is valid, submit the form
            $("form").submit();
        }
    });

   



    $('[data-bs-toggle="modal"]').on('click', function () {

        console.log("index model part has been called....");
        var noteId = $(this).data('id');
        var title = $(this).data('title');
        var content = $(this).data('content');
        var reminder = $(this).data('reminder');
        var attachments = $(this).data('attachments'); // This will be the JSON string for attachments

        // Set the values in the modal form
        $('[name="Title"]').val(title);
        $('[name="Content"]').val(content);
        $('[name="ReminderAt"]').val(reminder);

        // If there are attachments, display them in the modal
        if (attachments) {
            var attachmentsList = JSON.parse(attachments);
            var attachmentsHtml = '';
            attachmentsList.forEach(function (attachment) {
                attachmentsHtml += `<li><a href="${attachment.FilePath}" target="_blank" download="${attachment.FileName}">📄 ${attachment.FileName}</a></li>`;
            });
            $('#attachmentsList').html(attachmentsHtml); // Assuming you have a <ul id="attachmentsList"> in the modal
        }

        // Store the noteId in the hidden input field
        $('[name="NoteId"]').val(noteId);
    });


    
 
});